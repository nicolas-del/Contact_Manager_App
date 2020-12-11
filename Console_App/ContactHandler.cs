using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Windows;

namespace Console_App {

    public sealed class ContactHandler {

        string ConString = ConfigurationManager.ConnectionStrings["ContactConn"].ConnectionString;

        ContactHandler() { }

        static readonly ContactHandler instance = new ContactHandler();

        public static ContactHandler Instance {
            get { return instance; }
        }

        public ObservableCollection<Contact> ContactList { get; set; }

        public void AddContact(Contact contact) {

            using (SqlConnection con = new SqlConnection(ConString)) {
                con.Open();

                string query = "INSERT INTO ContactList(Name, Phone_Number, Address, Birthday) VALUES (@Name, @Phone_Number, @Address, @Birthday)";

                SqlCommand command = new SqlCommand(query, con);

                command.Parameters.AddWithValue("@Name", contact.Name);
                command.Parameters.AddWithValue("@Phone_Number", contact.PhoneNumber);
                command.Parameters.AddWithValue("@Address", contact.Address);
                command.Parameters.AddWithValue("@Birthday", contact.Birthday);

                if (command.ExecuteNonQuery() >= 1)
                    MessageBox.Show("Successfully added new contact!", "Confirmation", MessageBoxButton.OK);
                else
                    MessageBox.Show("ERROR: Couldn't add new contact!", "Confirmation", MessageBoxButton.OK);

                ViewAllContact();
            }
        }

        public void ViewSpecificContact(Contact contact) {
            using (SqlConnection con = new SqlConnection(ConString)) {
                con.Open();

                string query = "SELECT * FROM ContactList";

                SqlCommand command = new SqlCommand(query, con);

                using (SqlDataReader reader = command.ExecuteReader()) {

                    while (reader.Read()) {
                        if (Int32.TryParse(reader["Id"].ToString(), out int id)) {
                            contact.Id = id;
                        }
                        contact.Name = reader["Name"].ToString();
                        contact.PhoneNumber = reader["Phone_Number"].ToString();
                        contact.Address = reader["Address"].ToString();
                        contact.Birthday = reader["Birthday"].ToString();
                    }
                }
            }
        }

        public List<Contact> ViewAllContact() {
            List<Contact> list = new List<Contact>();

            using (SqlConnection con = new SqlConnection(ConString)) {
                con.Open();

                string query = "SELECT * FROM ContactList";

                SqlCommand command = new SqlCommand(query, con);

                using (SqlDataReader reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        Contact contact = new Contact();
                        if (Int32.TryParse(reader["Id"].ToString(), out int id)) {
                            contact.Id = id;
                        }
                        contact.Name = reader["Name"].ToString();
                        contact.PhoneNumber = reader["Phone_Number"].ToString();
                        contact.Address = reader["Address"].ToString();
                        contact.Birthday = reader["Birthday"].ToString();
                        list.Add(contact);
                    }
                }
            }
            return list;
        }

        public void EditContact(Contact contact) {
            using (SqlConnection con = new SqlConnection(ConString)) {
                con.Open();

                string query = "UPDATE ContactList SET Name = @Name, Phone_Number = @Phone_Number, Address = @Address, Birthday = @Birthday WHERE Name = @Name";

                SqlCommand command = new SqlCommand(query, con);

                command.Parameters.AddWithValue("@Id", contact.Id);
                command.Parameters.AddWithValue("@Name", contact.Name);
                command.Parameters.AddWithValue("@Phone_Number", contact.PhoneNumber);
                command.Parameters.AddWithValue("@Address", contact.Address);
                command.Parameters.AddWithValue("@Birthday", contact.Birthday);

                if (command.ExecuteNonQuery() >= 1)
                    MessageBox.Show("Successfully edited contact!", "Confirmation", MessageBoxButton.OK);
                else
                    MessageBox.Show("ERROR: Couldn't edit contact!", "Confirmation", MessageBoxButton.OK);
            }
        }

        public void DeleteContact(Contact contact) {
            using (SqlConnection con = new SqlConnection(ConString)) {
                con.Open();

                string query = "DELETE FROM ContactList WHERE Name = @Name";

                SqlCommand command = new SqlCommand(query, con);

                command.Parameters.AddWithValue("@Name", contact.Name);

                if (command.ExecuteNonQuery() >= 1)
                    MessageBox.Show("Successfully deleted contact!", "Confirmation", MessageBoxButton.OK);
                else
                    MessageBox.Show("ERROR: Couldn't delete contact!", "Confirmation", MessageBoxButton.OK);
            }
        }

        public void ImportCSV() {
            using (SqlConnection con = new SqlConnection(ConString)) {
                con.Open();

                int confirmation = 0;

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "CSV files (*.csv)|*.csv";

                if (openFileDialog.ShowDialog() == true) {
                    StreamReader reader = new StreamReader(File.OpenRead(openFileDialog.FileName));

                    for (int i = 0; i < openFileDialog.FileName.Length; i++) {
                        while (!reader.EndOfStream) {
                            string line = reader.ReadLine();

                            Contact contact = new Contact();

                            string query = "INSERT INTO ContactList(Name, Phone_Number, Address, Birthday) VALUES (@Name, @Phone_Number, @Address, @Birthday)";

                            SqlCommand command = new SqlCommand(query, con);

                            if (!string.IsNullOrWhiteSpace(line)) {
                                string[] values = line.Split(',');

                                contact.Name = values[0];
                                contact.PhoneNumber = values[1];
                                contact.Address = values[2];
                                contact.Birthday = values[3];

                                command.Parameters.AddWithValue("@Name", contact.Name);
                                command.Parameters.AddWithValue("@Phone_Number", contact.PhoneNumber);
                                command.Parameters.AddWithValue("@Address", contact.Address);
                                command.Parameters.AddWithValue("@Birthday", contact.Birthday);

                                if (command.ExecuteNonQuery() >= 1)
                                    confirmation++;
                            }
                        }
                    }
                }

                if (confirmation >= 1)
                    MessageBox.Show("Successfully added new contact!", "Confirmation", MessageBoxButton.OK);
                else
                    MessageBox.Show("ERROR: Couldn't add new contact!", "Confirmation", MessageBoxButton.OK);
            }
        }

        private List<Contact> listContacts = new List<Contact>();
        public void ExportCSV() {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv";

            listContacts = ViewAllContact();
            List<string> list = new List<string>();

            if (saveFileDialog.ShowDialog() == true) {
                try {
                    foreach (var c in listContacts)
                        list.Add(c.Name + "," + c.PhoneNumber + "," + c.Address + "," + c.Birthday);

                    File.WriteAllLines(saveFileDialog.FileName, list);

                    MessageBox.Show("Successfully exported contact list!", "Confirmation", MessageBoxButton.OK);
                }
                catch {
                    MessageBox.Show("ERROR: Couldn't export contact list!", "Confirmation", MessageBoxButton.OK);
                }
            }
        }
        public List<Contact> SearchBar(string name) {
            listContacts = ViewAllContact();
            List<Contact> list = new List<Contact>();
            Contact contact = new Contact();

            try {
                foreach (var c in listContacts) {
                    if (c.Name.ToLower().Equals(name.ToLower())) {
                        contact.Id = c.Id;
                        contact.Name = c.Name;
                        contact.PhoneNumber = c.PhoneNumber;
                        contact.Address = c.Address;
                        contact.Birthday = c.Birthday;
                        list.Add(contact);
                    }
                }
                if (list.Count == 0)
                    list = listContacts;
            }
            catch {
                MessageBox.Show("ERROR!");
            }
            return list;
        }
    }
}
