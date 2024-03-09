using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using HarfBuzzSharp;
using Survey_converter.Services;
using Survey_converter.ViewModels;
using Survey_converter.Views;
using System;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;

namespace Survey_converter
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (CultureInfo.CurrentCulture.Name == "ru-Ru")
                Languages.Resources.Culture = new CultureInfo("ru-RU");
            else
                Languages.Resources.Culture = CultureInfo.CurrentCulture;

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Line below is needed to remove Avalonia data validation.
                // Without this line you will get duplicate validations from both Avalonia and CT
                BindingPlugins.DataValidators.RemoveAt(0);
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };

                var services = new ServiceCollection();

                services.AddSingleton<IFilesService>(x => new FilesService(desktop.MainWindow));

                Services = services.BuildServiceProvider();
            }

            base.OnFrameworkInitializationCompleted();
        }

        public new static App? Current => Application.Current as App;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public IServiceProvider? Services { get; private set; }
    }
}