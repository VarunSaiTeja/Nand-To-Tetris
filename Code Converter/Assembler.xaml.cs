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
        static bool file_selected = false;
        static OpenFileDialog fileDialog = new OpenFileDialog
        {
            Filter = "Assembly|*.asm"
        };
        public Assembler()
        {
            InitializeComponent();
        }

        private void Load_File(object sender, RoutedEventArgs e)
        {
            if ((bool)fileDialog.ShowDialog())
            {
                AsmPath.Text = fileDialog.FileName;
                file_selected = true;
                MySnackbar.IsActive = true;
                Thread thread = new Thread(HideSnackBar);
                thread.Start();
            }
        }

        private void HideSnackBar()
        {
            try
            {
                Thread.Sleep(3000);
                this.Dispatcher.Invoke(() =>
                {
                    MySnackbar.IsActive = false;
                });
            }
            catch (System.Exception)
            { }
        }

        private void ConvertNow(object sender, RoutedEventArgs e)
        {
            if (file_selected)
            {
                if(MySnackbar.IsActive)
                {
                    MySnackbar.IsActive = false;
                }
                Translator.Assembler.ConvertFile(fileDialog.FileName);
                DialogHost.IsOpen = true;
            }
        }

        private void HideDialog(object sender, RoutedEventArgs e)
        {
            DialogHost.IsOpen = false;
        }

        private void UndoFile(object sender, RoutedEventArgs e)
        {
            AsmPath.Clear();
            file_selected = false;
            MySnackbar.IsActive = false;
        }

        private void Go_Back(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Main());
        }
    }
}
