using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Console_App
{
    /// <summary>
    /// Interaction logic for AddContactWindow.xaml
    /// </summary>
    public partial class AddContactWindow : Window
    {
        Contact contact = new Contact();

        ContactHandler contactHandler = new ContactHandler();

        public AddContactWindow()
        {
            InitializeComponent();
        }

        private void Create_Contact(object sender, RoutedEventArgs e)
        {
            contact.Name = nameTextBlock.Text;
            contact.PhoneNumber = phoneNumberTextBlock.Text;
            contact.Address = addressTextBlock.Text;
            contact.Birthday = birthdayTextBlock.Text;

            contactHandler.AddContact(contact);
        }
    }
}
