using Microsoft.Toolkit.Mvvm.ComponentModel;
using PropertyChanged;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace DeveImageOptimizerWPF.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class ConsoleViewModel : ObservableRecipient
    {
        public ObservableCollection<string> ConsoleOutput { get; set; } = new ObservableCollection<string>(new List<string>() { "Hoi", "Doei" });
        public string ConsoleInput { get; set; } = "HALLO";
        public string Testje => ConsoleOutput.Count().ToString();

        public ConsoleViewModel()
        {
            //Console.OpenStandardOutput();

        }

        public async Task Runner()
        {

        }
    }
}
