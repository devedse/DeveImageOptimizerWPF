using DeveCoolLib.ConsoleOut;
using DeveCoolLib.Streams;
using DeveImageOptimizerWPF.ViewModel;
using DeveImageOptimizerWPF.ViewModel.ObservableData;
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
            Services = ConfigureServices();

            this.InitializeComponent();

            Console.WriteLine("DeveImageOptimizerWPF started");
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
            var loggerExtractinator = LoggerExtractinator.CreateLoggerExtractinatorAndSetupConsoleRedirection();
            services.AddSingleton(loggerExtractinator);

            // Viewmodels
            services.AddTransient<MainViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<ConsoleViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
