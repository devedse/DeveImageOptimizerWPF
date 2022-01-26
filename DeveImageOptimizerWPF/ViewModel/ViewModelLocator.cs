/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:DeveImageOptimizerWPF"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using Microsoft.Extensions.DependencyInjection;
using System;

namespace DeveImageOptimizerWPF.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
        }

        public MainViewModel? Main => ResolveViewModelOrThrow<MainViewModel>();
        public SettingsViewModel? Settings => ResolveViewModelOrThrow<SettingsViewModel>();
        public ConsoleViewModel? Console => ResolveViewModelOrThrow<ConsoleViewModel>();

        private T ResolveViewModelOrThrow<T>()
        {
            var viewModel = App.Current.Services.GetService<T>();
            if (viewModel == null)
            {
                throw new InvalidOperationException($"Could not resolve ViewModel {typeof(T).FullName}. Ensure it's configued in 'App.xaml.cs'");
            }
            return viewModel;
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}