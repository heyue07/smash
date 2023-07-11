using common.libs.database;
using Microsoft.Extensions.DependencyInjection;
using smash.plugins;
using smash.plugin;
using System.Reflection;
using common.libs;

namespace smash
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Mutex mutex = new Mutex(true, System.Diagnostics.Process.GetCurrentProcess().ProcessName, out bool isAppRunning);
            if (isAppRunning == false)
            {
                Environment.Exit(1);
            }

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //����UI�߳��쳣
            Application.ThreadException += Application_ThreadException;
            //�����UI�߳��쳣
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();


            //ע�����
            ServiceProvider serviceProvider = null;
            ServiceCollection serviceCollection = new ServiceCollection();
            //ע�� ����ע�����Ӧ ʹ�ÿ����ڱ�ĵط�ͨ��ע��ķ�ʽ��� ServiceProvider ��������ȡ��������
            serviceCollection.AddSingleton((e) => serviceProvider);

            Assembly[] assemblys = AppDomain.CurrentDomain.GetAssemblies();
            //����ע��
            serviceCollection.AddSingleton<MainForm>();
            serviceCollection.AddSingleton<StartUpArgInfo>();
            serviceCollection.AddSingleton<Config>();
            serviceCollection.AddTransient(typeof(IConfigDataProvider<>), typeof(ConfigDataFileProvider<>));

            IPlugin[] plugins = PluginLoader.LoadBefore(serviceCollection, assemblys);
            serviceProvider = serviceCollection.BuildServiceProvider();

            //��ȡ���壬�Ϳ�����
            PluginLoader.TabForms = ReflectionHelper.GetInterfaceSchieves(assemblys, typeof(ITabForm)).Distinct().Select(c => (ITabForm)serviceProvider.GetService(c)).ToArray();
            PluginLoader.Controllers = ReflectionHelper.GetInterfaceSchieves(assemblys, typeof(IController)).Distinct().Select(c => (IController)serviceProvider.GetService(c)).ToArray();

            PluginLoader.LoadAfter(plugins, serviceProvider, assemblys);

            //����������
            StartUpArgInfo startUpArgInfo = serviceProvider.GetService<StartUpArgInfo>();
            startUpArgInfo.Args = args;
            Application.Run(serviceProvider.GetService<MainForm>());
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"ϵͳ�쳣:{(e.ExceptionObject as Exception).Message}");
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show($"ϵͳ�쳣:{(e.Exception).Message}");
        }

    }

    public sealed class StartUpArgInfo
    {
        public string[] Args { get; set; }
    }
}