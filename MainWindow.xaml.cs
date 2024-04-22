using System.Windows;
using System.Windows.Controls;

namespace wpf_application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void menu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NavButton _selected = (NavButton)menu.SelectedItem;
            navframe.Navigate(_selected.NavLink);
        }

        private void NavButton_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void navframe_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }
    }
}
