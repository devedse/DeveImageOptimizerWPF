using System.Reflection;
using System.Windows;

namespace DeveImageOptimizerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Title = $"DeveImageOptimizer {Assembly.GetEntryAssembly().GetName().Version}";
        }
    }
}
