﻿using common.libs;
using smash.plugin;
using smash.plugins.proxy;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace smash.plugins.hijack;

public sealed class HijackController : IController
{
    private static readonly string SystemDriver = $"{Environment.SystemDirectory}\\drivers\\netfilter2.sys";
    public const string NFDriver = "nfdriver.sys";
    public const string Name = "netfilter2";
    private readonly HijackConfig hijackConfig;
    private readonly ProxyConfig proxyConfig;
    private readonly HijackEventHandler hijackEventHandler;

    public HijackController(HijackConfig hijackConfig, ProxyConfig proxyConfig)
    {
        this.hijackConfig = hijackConfig;
        this.proxyConfig = proxyConfig;
        hijackEventHandler = new HijackEventHandler(hijackConfig);
    }

    public bool Validate(out string error)
    {
        error = string.Empty;

        return true;
    }
    public bool Start()
    {
        //初始化一些数据
        hijackConfig.ParseProcesss();
        Stop();

        //检查安装驱动
        CheckDriver();
        //给驱动获取进程权限
        NFAPI.nf_adjustProcessPriviledges();

        //初始化驱动
        NF_STATUS nF_STATUS = NFAPI.nf_init(Name, hijackEventHandler);
        if (nF_STATUS != NF_STATUS.NF_STATUS_SUCCESS)
        {
            throw new Exception($"{Name} start failed.{nF_STATUS}");
        }
        DefaultRule();

        return true;
    }
    public void Stop()
    {
        try
        {
            NFAPI.nf_deleteRules();
            NFAPI.nf_free();
        }
        catch (Exception)
        {
        }
    }

    private void DefaultRule()
    {
        List<NF_RULE> rules = new List<NF_RULE>();

        Filter53(rules);
        FilterIPV6Lan(rules);
        FilterIPV4Lan(rules);
        FilterWan(rules);

        NFAPI.nf_setRules(rules.ToArray());
    }
    private void Filter53(List<NF_RULE> rules)
    {
        rules.AddRange(new NF_RULE[] {
            //TCP 53
            new NF_RULE
            {
                direction = (byte)NF_DIRECTION.NF_D_OUT,
                filteringFlag = (uint)NF_FILTERING_FLAG.NF_INDICATE_CONNECT_REQUESTS,
                protocol = (int)ProtocolType.Tcp,
                remotePort = BinaryPrimitives.ReverseEndianness((ushort)53),
                ip_family = (ushort)AddressFamily.InterNetwork
            },
            new NF_RULE
            {
                direction = (byte)NF_DIRECTION.NF_D_OUT,
                filteringFlag = (uint)NF_FILTERING_FLAG.NF_INDICATE_CONNECT_REQUESTS,
                protocol = (int)ProtocolType.Tcp,
                remotePort = BinaryPrimitives.ReverseEndianness((ushort)53),
                ip_family = (ushort)AddressFamily.InterNetworkV6
            },
             //UDP 53
            new NF_RULE
            {
                direction = (byte)NF_DIRECTION.NF_D_OUT,
                filteringFlag = (uint)NF_FILTERING_FLAG.NF_FILTER,
                protocol = (int)ProtocolType.Udp,
                remotePort = BinaryPrimitives.ReverseEndianness((ushort)53),
                ip_family = (ushort)AddressFamily.InterNetwork
            },
            new NF_RULE
            {
                direction = (byte)NF_DIRECTION.NF_D_OUT,
                filteringFlag = (uint)NF_FILTERING_FLAG.NF_FILTER,
                protocol = (int)ProtocolType.Udp,
                remotePort = BinaryPrimitives.ReverseEndianness((ushort)53),
                ip_family = (ushort)AddressFamily.InterNetworkV6
            }
        });
    }
    private void FilterIPV6Lan(List<NF_RULE> rules)
    {
        rules.AddRange(new NF_RULE[]
        {
            //IPV6 环回 ::1/128
            new NF_RULE
            {
                direction = (byte)NF_DIRECTION.NF_D_OUT,
                filteringFlag = (uint)NF_FILTERING_FLAG.NF_ALLOW,
                ip_family = (ushort)AddressFamily.InterNetworkV6,
                remoteIpAddress = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                remoteIpAddressMask = new byte[] { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 },
            },
            //IPV6 组播 FF00::/8
            new NF_RULE
            {
                direction = (byte)NF_DIRECTION.NF_D_OUT,
                filteringFlag = (uint)NF_FILTERING_FLAG.NF_ALLOW,
                ip_family = (ushort)AddressFamily.InterNetworkV6,
                remoteIpAddress = new byte[] { 255, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                remoteIpAddressMask = new byte[] { 255, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            },
            //本地链路 FE80::/10
            new NF_RULE
            {
                direction = (byte)NF_DIRECTION.NF_D_OUT,
                filteringFlag = (uint)NF_FILTERING_FLAG.NF_ALLOW,
                ip_family = (ushort)AddressFamily.InterNetworkV6,
                remoteIpAddress = new byte[] { 0xFE, 0x80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                remoteIpAddressMask = new byte[] { 255, 192, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            },
            //本地站点 FEC0::/10
            new NF_RULE
            {
                direction = (byte)NF_DIRECTION.NF_D_OUT,
                filteringFlag = (uint)NF_FILTERING_FLAG.NF_ALLOW,
                ip_family = (ushort)AddressFamily.InterNetworkV6,
                remoteIpAddress = new byte[] { 0xFE, 0xC0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                remoteIpAddressMask = new byte[] { 255, 192, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            }
        });
    }
    private void FilterIPV4Lan(List<NF_RULE> rules)
    {
        foreach (string item in hijackConfig.IntranetIpv4s)
        {
            string[] arr = item.Split('/');
            uint ip = BinaryPrimitives.ReadUInt32LittleEndian(IPAddress.Parse(arr[0]).GetAddressBytes());
            uint mask = BinaryPrimitives.ReverseEndianness(0xffffffff << 32 - byte.Parse(arr[1]));
            byte[] ipBytes = new byte[16];
            byte[] maskBytes = new byte[16];
            BitConverter.GetBytes(ip).AsSpan().CopyTo(ipBytes);
            BitConverter.GetBytes(mask).AsSpan().CopyTo(maskBytes);
            rules.Add(new NF_RULE
            {
                filteringFlag = (uint)NF_FILTERING_FLAG.NF_ALLOW,
                ip_family = (ushort)AddressFamily.InterNetwork,
                remoteIpAddress = BitConverter.GetBytes(ip),
                remoteIpAddressMask = BitConverter.GetBytes(mask),

            });
        }
    }
    private void FilterWan(List<NF_RULE> rules)
    {
        rules.AddRange(new List<NF_RULE> { 
            //TCP
            new NF_RULE
            {
                direction = (byte)NF_DIRECTION.NF_D_OUT,
                filteringFlag = (uint)NF_FILTERING_FLAG.NF_INDICATE_CONNECT_REQUESTS,
                protocol = (int)ProtocolType.Tcp,
                ip_family = (ushort)AddressFamily.InterNetwork
            },
            new NF_RULE
            {
                direction = (byte)NF_DIRECTION.NF_D_OUT,
                filteringFlag = (uint)NF_FILTERING_FLAG.NF_INDICATE_CONNECT_REQUESTS,
                protocol = (int)ProtocolType.Tcp,
                ip_family = (ushort)AddressFamily.InterNetworkV6
            },
            //UDP
             new NF_RULE
            {
                direction = (byte)NF_DIRECTION.NF_D_OUT,
                filteringFlag = (uint)NF_FILTERING_FLAG.NF_FILTER,
                protocol = (int)ProtocolType.Udp,
                ip_family = (ushort)AddressFamily.InterNetwork
            },
            new NF_RULE
            {
                direction = (byte)NF_DIRECTION.NF_D_OUT,
                filteringFlag = (uint)NF_FILTERING_FLAG.NF_FILTER,
                protocol = (int)ProtocolType.Udp,
                ip_family = (ushort)AddressFamily.InterNetworkV6
            },

        });
    }

    private string GetFileVersion(string file)
    {
        if (File.Exists(file))
            return FileVersionInfo.GetVersionInfo(file).FileVersion ?? "";

        return "";
    }
    private void CheckDriver()
    {
        var binFileVersion = GetFileVersion(NFDriver);
        var systemFileVersion = GetFileVersion(SystemDriver);

        if (File.Exists(SystemDriver) == false)
        {
            // Install
            InstallDriver();
            return;
        }

        var reinstall = false;
        if (Version.TryParse(binFileVersion, out var binResult) && Version.TryParse(systemFileVersion, out var systemResult))
        {
            if (binResult.CompareTo(systemResult) > 0)
                // Update
                reinstall = true;
            else if (systemResult.Major != binResult.Major)
                // Downgrade when Major version different (may have breaking changes)
                reinstall = true;
        }
        else
        {
            // Parse File versionName to Version failed
            if (!systemFileVersion.Equals(binFileVersion))
                // versionNames are different, Reinstall
                reinstall = true;
        }

        if (!reinstall)
            return;

        UninstallDriver();
        InstallDriver();
    }
    private void InstallDriver()
    {
        if (!File.Exists(NFDriver))
            throw new Exception("builtin driver files missing, can't install NF driver");

        try
        {
            File.Copy(NFDriver, SystemDriver);
        }
        catch (Exception e)
        {
            throw new Exception($"Copy {Name}.sys failed\n{e.Message}");
        }

        // 注册驱动文件
        if (NFAPI.nf_registerDriver(Name) == NF_STATUS.NF_STATUS_SUCCESS)
        {
            Console.WriteLine($"Install {Name} driver finished");
        }
        else
        {
            Console.WriteLine($"Register {Name} failed");
        }
    }
    private bool UninstallDriver()
    {
        if (File.Exists(SystemDriver) == false)
            return true;

        NFAPI.nf_unRegisterDriver(Name);
        File.Delete(SystemDriver);

        return true;
    }


}

