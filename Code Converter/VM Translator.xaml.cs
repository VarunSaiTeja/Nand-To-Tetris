using Microsoft.Win32;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Code_Converter
{
    /// <summary>
    /// Interaction logic for VM_Translator.xaml
    /// </summary>
    public partial class VM_Translator : Page
    {
        public VM_Translator()
        {
            InitializeComponent();
        }

        private void Load_File(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Filter = "Virtual Machine|*.vm"
            };

            if ((bool)fileDialog.ShowDialog())
            {
                AsmPath.Text = fileDialog.FileName;
                Translator.VM_Translator.ConvertFile(fileDialog.FileName);
                MySnackbar.IsActive = true;
                Thread thread = new Thread(HideSnackBar);
                thread.Start();
            }
        }

        private void HideSnackBar()
        {
            Thread.Sleep(3000);
            this.Dispatcher.Invoke(() =>
            {
                MySnackbar.IsActive = false;
            });
        }

        private void ConvertNow(object sender, RoutedEventArgs e)
        {
            DialogHost.IsOpen = true;
        }

        private void HideDialog(object sender, RoutedEventArgs e)
        {
            DialogHost.IsOpen = false;
        }
    }
}
