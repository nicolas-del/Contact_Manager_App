using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
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
        private List<Contact> listContacts = new List<Contact>();

        ContactHandler contactHandler = ContactHandler.Instance;

        public MainWindow()
        {
            InitializeComponent();
            listContacts = contactHandler.ViewAllContact();
            lvDataBinding.ItemsSource = listContacts;
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

        private void ImportCSV_Button(object sender, RoutedEventArgs e) {
            contactHandler.ImportCSV();
        }

        private void ExportCSV_Button(object sender, RoutedEventArgs e) {
            contactHandler.ExportCSV();
        }

        public void ReloadWindow_Button(object sender, RoutedEventArgs e) {
            MainWindow mw = new MainWindow();
            mw.Show();
        }
    }
}
