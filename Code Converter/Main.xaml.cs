using System.Windows;
using System.Windows.Controls;

namespace Code_Converter
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        public Main()
        {
            InitializeComponent();
        }

        private void AssemblyConverter(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Assembler());
        }

        private void VMConverter(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new VM_Translator());
        }
    }
}
