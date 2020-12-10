using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
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

        Contact contactInstance = new Contact();

        public Contact Contact { get; set; }

        public List<Contact> generalList = new List<Contact>();

        public void AddContact(Contact contact)
        {
            contactInstance = contact;
            Contact = contactInstance;
            using (SqlConnection con = new SqlConnection(ConString)) {
                con.Open();

                string query = "INSERT INTO ContactList(Name, Phone_Number, Address, Birthday) VALUES (@Name, @Phone_Number, @Address, @Birthday)";

                SqlCommand command = new SqlCommand(query, con);

                command.Parameters.AddWithValue("@Name", contact.Name);
                command.Parameters.AddWithValue("@Phone_Number", contact.PhoneNumber);
                command.Parameters.AddWithValue("@Address", contact.Address);
                command.Parameters.AddWithValue("@Birthday", contact.Birthday);

                if (command.ExecuteNonQuery() >= 1) {
                    MessageBox.Show("Successfully added new contact!", "Confirmation", MessageBoxButton.OK);
                }
                else
                    MessageBox.Show("ERROR: Couldn't add new contact!", "Confirmation", MessageBoxButton.OK);
            }
        }

        public void EditContact(Contact contact)
        {
            contactInstance = contact;
            Contact = contactInstance;
            using (SqlConnection con = new SqlConnection(ConString))
            {
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


        public void DeleteContact(Contact contact)
        {
            contactInstance = contact;
            Contact = contactInstance;
            using (SqlConnection con = new SqlConnection(ConString))
            {
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

        public List<Contact> UpdateListContact() 
        {
            using (SqlConnection con = new SqlConnection(ConString)) 
            {
                con.Open();

                string query = "SELECT * FROM ContactList";
                SqlCommand command = new SqlCommand(query, con);

                using (SqlDataReader reader = command.ExecuteReader()) 
                {
                    while (reader.Read()) 
                    {
                        Contact.Name = reader["Name"].ToString();
                        Contact.PhoneNumber = reader["Phone_Number"].ToString();
                        Contact.Address = reader["Address"].ToString();
                        Contact.Birthday= reader["Birthday"].ToString();
                        generalList.Add(Contact);
                    }
                }
            }
            return generalList;
        }


        public List<Contact> ViewAllContact()
        {

            using (SqlConnection con = new SqlConnection(ConString))
            {
                con.Open();

                string query = "SELECT * FROM ContactList";

                SqlCommand command = new SqlCommand(query, con);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Contact contact = new Contact();
                        if (Int32.TryParse(reader["Id"].ToString(), out int id))
                        {
                            contact.Id = id;
                        }
                        contact.Name = reader["Name"].ToString();
                        contact.PhoneNumber = reader["Phone_Number"].ToString();
                        contact.Address = reader["Address"].ToString();
                        contact.Birthday = reader["Birthday"].ToString();
                        generalList.Add(contact);
                    }
                }
            }
            return generalList;
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

        public void ImportCSV() {

            using (SqlConnection con = new SqlConnection(ConString)) {
                con.Open();

                Contact contact = new Contact();

                string query = "INSERT INTO ContactList(Name, Phone_Number, Address, Birthday) VALUES (@Name, @Phone_Number, @Address, @Birthday)";

                SqlCommand command = new SqlCommand(query, con);

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "CSV files (*.csv)|*.csv";

                if (openFileDialog.ShowDialog() == true) {
                    StreamReader reader = new StreamReader(File.OpenRead(openFileDialog.FileName));

                    while (!reader.EndOfStream) {
                        string line = reader.ReadLine();

                        if (!String.IsNullOrWhiteSpace(line)) {
                            string[] values = line.Split(',');

                            contact.Name = values[0];
                            contact.PhoneNumber = values[1];
                            contact.Address = values[2];
                            contact.Birthday = values[3];

                            command.Parameters.AddWithValue("@Name", contact.Name);
                            command.Parameters.AddWithValue("@Phone_Number", contact.PhoneNumber);
                            command.Parameters.AddWithValue("@Address", contact.Address);
                            command.Parameters.AddWithValue("@Birthday", contact.Birthday);
                        }
                    }
                }

                if (command.ExecuteNonQuery() >= 1)
                    MessageBox.Show("Successfully added new contact!", "Confirmation", MessageBoxButton.OK);
                else
                    MessageBox.Show("ERROR: Couldn't add new contact!", "Confirmation", MessageBoxButton.OK);
            }
        }

        public void ExportCSV() {
            using (SqlConnection con = new SqlConnection(ConString)) {
                con.Open();

                Contact contact = new Contact();

                string query = "SELECT Name, Phone_Number, Address, Birthday FROM ContactList";

                SqlCommand command = new SqlCommand(query, con);

                SqlDataReader reader = command.ExecuteReader();

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";

                if (saveFileDialog.ShowDialog() == true) {
                    List<string> lines = new List<string>();
                    string headerLine = "";

                    if (reader.Read()) {
                        string[] columns = new string[reader.FieldCount];
                        for (int i = 0; i < reader.FieldCount; i++)
                            columns[i] = reader.GetName(i);
                        headerLine = string.Join(",", columns);
                        lines.Add(headerLine);
                    }

                    while (reader.Read()) {
                        object[] values = new object[reader.FieldCount];
                        reader.GetValues(values);
                        lines.Add(string.Join(",", values));
                    }

                    File.WriteAllLines(saveFileDialog.FileName, lines);

                    reader.Close();

                    if (command.ExecuteNonQuery() >= 1)
                        MessageBox.Show("Successfully exported contact list!", "Confirmation", MessageBoxButton.OK);
                    else
                        MessageBox.Show("ERROR: Couldn't export contact list!", "Confirmation", MessageBoxButton.OK);
                }
            }
        }

    }
}
