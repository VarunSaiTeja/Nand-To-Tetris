using Microsoft.Win32;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Code_Converter
{
    /// <summary>
    /// Interaction logic for VM_Translator.xaml
    /// </summary>
    public partial class VM_Translator : Page
    {
        static bool file_selected = false;
        static FolderBrowserDialog folderDialog = new FolderBrowserDialog();
        public VM_Translator()
        {
            InitializeComponent();
        }

        private void Load_File(object sender, RoutedEventArgs e)
        {
            DialogResult folderSelected = folderDialog.ShowDialog();
            if (folderSelected==DialogResult.OK)
            {
                VMPath.Text = folderDialog.SelectedPath;
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
                if (MySnackbar.IsActive)
                {
                    MySnackbar.IsActive = false;
                }
                Translator.VM_Translator.ConvertFile(folderDialog.SelectedPath);
                DialogHost.IsOpen = true;
            }
        }

        private void HideDialog(object sender, RoutedEventArgs e)
        {
            DialogHost.IsOpen = false;
        }

        private void UndoFile(object sender, RoutedEventArgs e)
        {
            VMPath.Clear();
            file_selected = false;
            MySnackbar.IsActive = false;
        }

        private void Go_Back(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Main());
        }
    }
}