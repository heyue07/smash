using System.Diagnostics;
using System.Reflection;
using smash.forms;
using smash.libs;
using smash.libs.hijack;

namespace smash.forms
{
    public partial class MainForm : Form
    {
        private NotifyIcon notifyIcon = null;
        Image unright = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream(@"smash.public.right-gray.png"));
        Image right = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream(@"smash.public.right.png"));
        Icon icon = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream(@"smash.public.icon.ico"));
        Icon iconGray = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream(@"smash.public.icon-gray.ico"));
        string name = "netch���̽ٳִ���";

        private readonly Config config = new Config();
        private readonly HijackController hijackController;
        private readonly SysProxyController SysProxyController;

        bool hideForm;

        public MainForm(string[] args)
        {
            HideForm(args);
            config = Config.Load();
            hijackController = new HijackController(config);
            SysProxyController = new SysProxyController(config);

            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            InitializeComponent();
            InitialTray();
            BindInfo();
        }

        #region ����
        private void InitialTray()
        {
            notifyIcon = new NotifyIcon();
            notifyIcon.BalloonTipTitle = name;
            notifyIcon.BalloonTipText = name + "������";
            notifyIcon.Text = name;

            notifyIcon.Icon = iconGray;
            notifyIcon.Visible = true;

            notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            notifyIcon.ContextMenuStrip.Items.Add("����������", unright, StartUp);
            notifyIcon.ContextMenuStrip.Items.Add("������������", unright, Switch);
            notifyIcon.ContextMenuStrip.Items.Add("�˳�", null, Close);
            notifyIcon.DoubleClick += ContextMenuStrip_MouseDoubleClick;

            StartUp();
        }
        private void ContextMenuStrip_MouseDoubleClick(object sender, EventArgs e)
        {
            ShowInTaskbar = true;
            Show();
        }
        bool menuClose = false;
        private void Close(object sender, EventArgs e)
        {
            menuClose = true;
            this.Close();
        }
        private new void Closing(object sender, FormClosingEventArgs e)
        {
            HideForm();
            if (menuClose == false)
            {
                e.Cancel = true;
            }
            menuClose = false;
        }
        private void OnMainFormClosed(object sender, FormClosedEventArgs e)
        {
            Stop();
        }
        private void HideForm(string[] args)
        {
            hideForm = args.Length > 0 && args[0] == "/service";
        }
        private void HideForm()
        {
            ShowInTaskbar = false;
            Hide();
        }
        #endregion

        #region ������
        class Model
        {
            public string Key { get; set; }
            public string Path { get; set; }
        }
        private Model GetInfo()
        {
            string currentPath = Application.StartupPath;
            string exeName = AppDomain.CurrentDomain.FriendlyName;
            string keyName = exeName.Replace(".exe", "");
            return new Model
            {
                Key = keyName,
                Path = System.IO.Path.Combine(currentPath, exeName)
            };
        }
        bool isStartUp = false;
        private void StartUp(object sender, EventArgs e)
        {
            Model model = GetInfo();
            try
            {
                if (isStartUp == false)
                {
                    CommandHelper.Windows("schtasks.exe", new string[] {
                        "schtasks.exe /create /tn \""+model.Key+"\" /rl highest /sc ONSTART /delay 0000:30 /tr \""+model.Path+" /service\" /f"
                    });
                    notifyIcon.BalloonTipText = "������������";
                    notifyIcon.ShowBalloonTip(1000);
                }
                else
                {
                    CommandHelper.Windows("schtasks.exe", new string[] {
                        "schtasks /delete  /TN "+model.Key+" /f"
                    });
                    notifyIcon.BalloonTipText = "��ȡ��������";
                    notifyIcon.ShowBalloonTip(1000);
                }
            }
            catch (Exception ex)
            {
                notifyIcon.BalloonTipText = ex.Message;
                notifyIcon.ShowBalloonTip(1000);
            }
            StartUp();
        }
        private void StartUp()
        {
            Model model = GetInfo();
            string res = CommandHelper.Windows("", new string[] {
                "schtasks.exe /query /fo TABLE|findstr \"" + model.Key + "\""
            });
            bool has = false;
            foreach (string item in res.Split('\n'))
            {
                if (item.StartsWith(model.Key))
                {
                    has = true;
                    break;
                }
            }

            if (has == false)
            {
                isStartUp = false;
                notifyIcon.ContextMenuStrip.Items[0].Image = unright;
            }
            else
            {
                notifyIcon.ContextMenuStrip.Items[0].Image = right;
                isStartUp = true;
            }
        }
        #endregion

        #region �Ӵ���

        ProxySettingForm proxyWin = null;
        private void OnMainMenuProxyClick(object sender, EventArgs e)
        {
            proxyWin = new ProxySettingForm(config);
            proxyWin.FormClosed += (object sender, FormClosedEventArgs e) =>
            {
                proxyWin = null;
                BindInfo();
            };
            proxyWin.ShowDialog();
        }

        HijackOptionsForm optionsWin = null;
        private void OnMainMenuHijackOptionsClick(object sender, EventArgs e)
        {
            optionsWin = new HijackOptionsForm(config);
            optionsWin.FormClosed += (object sender, FormClosedEventArgs e) =>
            {
                optionsWin = null;
                BindInfo();
            };
            optionsWin.ShowDialog();
        }

        AboutForm aboutWin = null;
        private void OnMainMenuAboutClick(object sender, EventArgs e)
        {
            aboutWin = new AboutForm();
            aboutWin.FormClosed += (object sender, FormClosedEventArgs e) =>
            {
                aboutWin = null;
                BindInfo();
            };
            aboutWin.ShowDialog();
        }

        HijackProcessForm processWin = null;
        private void OnMainMenuHijackProcessClick(object sender, EventArgs e)
        {
            processWin = new HijackProcessForm(config);
            processWin.FormClosed += (object sender, FormClosedEventArgs e) =>
            {
                processWin = null;
                BindInfo();
            };
            processWin.ShowDialog();
        }

        SysProxySettingForm sysProxySettingForm = null;
        private void OnMainMenuSysProxyClick(object sender, EventArgs e)
        {
            sysProxySettingForm = new SysProxySettingForm(config);
            sysProxySettingForm.FormClosed += (object sender, FormClosedEventArgs e) =>
            {
                sysProxySettingForm = null;
                BindInfo();
            };
            sysProxySettingForm.ShowDialog();
        }

        #endregion


        private void BindInfo()
        {
            BindGroup();
            BindProxy();
            BindSysProxy();
            config.Save();
        }

        #region ���̽ٳ�
        private void cmbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGroup.SelectedItem != null)
            {
                config.Process = config.Processs.FirstOrDefault(c => c.Name == cmbGroup.SelectedItem.ToString());
            }
            BindFileNames();
        }
        private void BindGroup()
        {
            int index = cmbGroup.SelectedIndex;
            cmbGroup.DataSource = null;
            cmbGroup.DataSource = config.Processs.Select(c => c.Name).ToList();
            cbUseHijack.Checked = config.UseHijack;
            if (index < config.Processs.Count && config.Processs.Count > 0)
            {
                cmbGroup.SelectedIndex = index < 0 ? 0 : index;
            }
        }
        private void BindFileNames()
        {
            listProcess.DataSource = null;
            if (config.Process == null) return;
            listProcess.DataSource = config.Process.FileNames;
            listProcess.ClearSelected();
        }
        private void cbUseHijack_CheckedChanged(object sender, EventArgs e)
        {
            config.UseHijack = cbUseHijack.Checked;
            BindInfo();
        }
        #endregion

        #region ����
        private void BindProxy()
        {
            int index = cmbProxy.SelectedIndex;
            cmbProxy.DataSource = null;
            if (config.Proxy != null)
            {
                cmbProxy.DataSource = config.Proxys.Select(c => c.Name).ToList();
            }
            if (index < config.Proxys.Count && config.Proxys.Count > 0)
            {
                cmbProxy.SelectedIndex = index < 0 ? 0 : index;
            }
        }
        private void cmbProxy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProxy.SelectedItem != null)
            {
                config.Proxy = config.Proxys.FirstOrDefault(c => c.Name == cmbProxy.SelectedItem.ToString());
                labelProxy.Text = $"{config.Proxy.Host}:{config.Proxy.Port}";
            }
        }
        #endregion

        #region ϵͳ����
        private void BindSysProxy()
        {
            int index = cmbSysProxy.SelectedIndex;
            cmbSysProxy.DataSource = null;
            if (config.SysProxys != null)
            {
                cmbSysProxy.DataSource = config.SysProxys.Select(c => c.Name).ToList();
            }
            cbUseSysProxy.Checked = config.UseSysProxy;
            if (index < config.SysProxys.Count && config.SysProxys.Count > 0)
            {
                cmbSysProxy.SelectedIndex = index < 0 ? 0 : index;
            }
        }
        private void cmbSysProxy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSysProxy.SelectedItem != null)
            {
                config.SysProxy = config.SysProxys.FirstOrDefault(c => c.Name == cmbSysProxy.SelectedItem.ToString());
                lbPac.Text = $"{config.SysProxy.Pac}";
                cbIsPac.Checked = config.SysProxy.IsPac;
                cbIsEnv.Checked = config.SysProxy.IsEnv;
            }
        }
        private void cbUseSysProxy_CheckedChanged(object sender, EventArgs e)
        {
            config.UseSysProxy = cbUseSysProxy.Checked;
            BindInfo();
        }
        private void cbIsEnv_CheckedChanged(object sender, EventArgs e)
        {
            if (config.SysProxy != null)
            {
                config.SysProxy.IsEnv = cbIsEnv.Checked;
                BindInfo();
            }
        }
        private void cbIsPac_CheckedChanged(object sender, EventArgs e)
        {
            if (config.SysProxy != null)
            {
                config.SysProxy.IsPac = cbIsPac.Checked;
                BindInfo();
            }
        }
        #endregion


        bool starting = false;
        bool running = false;
        private void SetButtonTest()
        {
            this.Invoke(() =>
            {
                if (starting)
                {
                    startBtn.Text = "���ڲ���";
                }
                else if (running)
                {
                    startBtn.Text = "ֹͣ����";
                    notifyIcon.Icon = icon;
                    notifyIcon.ContextMenuStrip.Items[1].Image = right;
                }
                else
                {
                    startBtn.Text = "��������";
                    notifyIcon.Icon = iconGray;
                    notifyIcon.ContextMenuStrip.Items[1].Image = unright;
                }
            });
        }

        private void Switch(object sender, EventArgs e)
        {
            if (running)
            {
                Stop();
            }
            else
            {
                Start();
            }
        }
        private void Start()
        {
            if (config.UseHijack == false && config.UseSysProxy == false)
            {
                MessageBox.Show("����Ҫʹ��һ�ַ�ʽ����Ȼû����");
                return;
            }

            if (config.UseHijack && config.Process == null)
            {
                MessageBox.Show("δѡ���̽ٳֹ���");
                return;
            }
            if (config.UseSysProxy && config.SysProxy == null)
            {
                MessageBox.Show("δѡϵͳ�������");
                return;
            }


            if (starting) return;
            starting = true;
            SetButtonTest();

            Task.Run(() =>
            {
                try
                {
                    hijackController.Start();
                    SysProxyController.Start();

                    starting = false;
                    running = true;
                    config.Running = true;
                    config.Save();
                }
                catch (Exception ex)
                {
                    Stop();
                    starting = false;
                    Debug.WriteLine(ex + "");
                    MessageBox.Show(ex.Message);
                }
                SetButtonTest();
                CommandHelper.Windows(string.Empty, new string[] { "ipconfig/flushdns" });
            });
        }
        private void Stop()
        {
            if (starting) return;
            starting = true;
            SetButtonTest();
            Task.Run(() =>
            {
                try
                {
                    hijackController.Stop();
                    SysProxyController.Stop();
                    starting = false;
                    running = false;
                    config.Running = false;
                    config.Save();
                    Helper.FlushMemory();
                }
                catch (Exception ex)
                {
                    starting = false;
                    MessageBox.Show(ex.Message);
                }
                SetButtonTest();
                CommandHelper.Windows(string.Empty, new string[] { "ipconfig/flushdns" });
            });
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            Switch(null, null);
        }
        private void OnWinLoad(object sender, EventArgs e)
        {
            Helper.FlushMemory();
            if (config.Running)
            {
                Start();
            }
            if (hideForm)
            {
                HideForm();
            }
        }


    }
}