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
    /// Interaction logic for ViewContactWindow.xaml
    /// </summary>
    public partial class ViewContactWindow : Window
    {
        Contact contact;
        public ViewContactWindow(Contact contact)
        {
            InitializeComponent();

            this.contact = contact;

            nameTextBox.Text = contact.Name;
            phoneNumberTextBox.Text = contact.PhoneNumber;
            addressTextBox.Text = contact.Address;
            birthdayTextBox.Text = contact.Birthday;
        }
    }
}
