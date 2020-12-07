using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Console_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        ObservableCollection<Contact> ContactList = new ObservableCollection<Contact>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void lvDataBinding_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Contact selectContact = (Contact)lvDataBinding.SelectedItem;

            if (selectContact != null)
            {
                ViewContactWindow viewWindow = new ViewContactWindow(selectContact);
                viewWindow.ShowDialog();
            }
        }

        private void AddContact_Button(object sender, RoutedEventArgs e)
        {
            AddContactWindow acw = new AddContactWindow();
            acw.Show();
        }

        private void EditContact_Button(object sender, RoutedEventArgs e)
        {
            EditContactWindow ecw = new EditContactWindow();
            ecw.Show();
        }

        private void DeleteContact_Button(object sender, RoutedEventArgs e)
        {
            DeleteContactWindow dcw = new DeleteContactWindow();
            dcw.Show();
        }
    }
}
