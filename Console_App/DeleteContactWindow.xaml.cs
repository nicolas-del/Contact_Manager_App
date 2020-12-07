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
    /// Interaction logic for DeleteContactWindow.xaml
    /// </summary>
    public partial class DeleteContactWindow : Window
    {
        Contact contact = new Contact();

        ContactHandler contactHandler = new ContactHandler();
        public DeleteContactWindow()
        {
            InitializeComponent();
        }

        private void Delete_Contact(object sender, RoutedEventArgs e)
        {
            contact.Name = nameTextBlock.Text;
            contactHandler.DeleteContact(contact);
        }
    }
}
