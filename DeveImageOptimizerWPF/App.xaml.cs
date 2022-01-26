using DeveCoolLib.ConsoleOut;
using DeveCoolLib.Streams;
using DeveImageOptimizerWPF.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows;

namespace DeveImageOptimizerWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ConfigureConsoleLogger();

            Services = ConfigureServices();

            this.InitializeComponent();
        }

        private void ConfigureConsoleLogger()
        {
            var originalOut = Console.OpenStandardOutput();
            var movingMemoryStream = new MovingMemoryStream();
            
            var multiOut = new MultiStream(originalOut, movingMemoryStream);
            var writer = new StreamWriter(multiOut);

            Console.SetOut(writer);
        }

        /// <summary>
        /// Gets the current <see cref="App"/> instance in use
        /// </summary>
        public new static App Current => (App)Application.Current;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public IServiceProvider Services { get; }

        /// <summary>
        /// Configures the services for the application.
        /// </summary>
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            //services.AddSingleton<IFilesService, FilesService>();
            //services.AddSingleton<ISettingsService, SettingsService>();
            //services.AddSingleton<IClipboardService, ClipboardService>();
            //services.AddSingleton<IShareService, ShareService>();
            //services.AddSingleton<IEmailService, EmailService>();

            // Viewmodels
            services.AddTransient<MainViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<ConsoleViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
