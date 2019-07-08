using Microsoft.Win32;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Code_Converter
{
    /// <summary>
    /// Interaction logic for Assembler.xaml
    /// </summary>
    public partial class Assembler : Page
    {
        public Assembler()
        {
            InitializeComponent();
        }

        private void Load_File(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Filter = "Assembly|*.asm"
            };

            if ((bool)fileDialog.ShowDialog())
            {
                AsmPath.Text = fileDialog.FileName;
                Translator.Assembler.ConvertFile(fileDialog.FileName);
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
