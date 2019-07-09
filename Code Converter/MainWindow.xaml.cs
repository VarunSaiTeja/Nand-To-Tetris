using System.Diagnostics;
using System.Windows;

namespace Code_Converter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MyFrame.Navigate(new Main());
        }

        private void Donate(object sender, RoutedEventArgs e)
        {
            Process.Start("https://paypal.me/VarunSaiTeja");
        }

        private void GitHub(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/VarunSaiTeja");
        }

        private void YouTube(object sender, RoutedEventArgs e)
        {
            Process.Start("https://youtube.com/VarunTeja");
        }
    }
}
