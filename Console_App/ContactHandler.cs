﻿using Microsoft.Win32;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Windows;

namespace Console_App {
    public class ContactHandler 
    {
        

        string ConString = ConfigurationManager.ConnectionStrings["ContactConn"].ConnectionString;


        public void AddContact(Contact contact) {
            using (SqlConnection con = new SqlConnection(ConString)) {
                con.Open();

                string query = "INSERT INTO ContactList(Name, Phone_Number, Address, Birthday) VALUES (@Name, @Phone_Number, @Address, @Birthday)";

                SqlCommand command = new SqlCommand(query, con);

                command.Parameters.AddWithValue("@Name", contact.Name);
                command.Parameters.AddWithValue("@Phone_Number", contact.PhoneNumber);
                command.Parameters.AddWithValue("@Address", contact.Address);
                command.Parameters.AddWithValue("@Birthday", contact.Birthday);

                command.ExecuteNonQuery();

                if (command.ExecuteNonQuery() >= 1) {
                    MessageBox.Show("Successfully added new contact!", "Confirmation", MessageBoxButton.OK);
                }
                else
                    MessageBox.Show("ERROR: Couldn't add new contact!", "Confirmation", MessageBoxButton.OK);
                con.Close();
            }
        }

        public void ViewContact(Contact contact) {
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

                command.ExecuteNonQuery();

                if (command.ExecuteNonQuery() >= 1)
                    MessageBox.Show("Successfully edited contact!", "Confirmation", MessageBoxButton.OK);
                else
                    MessageBox.Show("ERROR: Couldn't edit contact!", "Confirmation", MessageBoxButton.OK);

                con.Close();
            }
        }

        public void DeleteContact(Contact contact) {
            using (SqlConnection con = new SqlConnection(ConString)) {
                con.Open();

                string query = "DELETE FROM ContactList WHERE Name = @Name";

                SqlCommand command = new SqlCommand(query, con);

                command.Parameters.AddWithValue("@Name", contact.Name);

                command.ExecuteNonQuery();

                if (command.ExecuteNonQuery() >= 1)
                    MessageBox.Show("Successfully deleted contact!", "Confirmation", MessageBoxButton.OK);
                else
                    MessageBox.Show("ERROR: Couldn't delete contact!", "Confirmation", MessageBoxButton.OK);
                con.Close();
            }
        }

        public void ImportCSV(Contact contact) {
            using (SqlConnection con = new SqlConnection(ConString)) {
                con.Open();

                string query = "INSERT INTO ContactList(Name, Phone_Number, Address, Birthday) VALUES (@Name, @Phone_Number, @Address, @Birthday)";

                SqlCommand command = new SqlCommand(query, con);

                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";

                if (openFileDialog1.ShowDialog() == true) {
                    StreamReader reader = new StreamReader(File.OpenRead(openFileDialog1.FileName));

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
                command.ExecuteNonQuery();

                if (command.ExecuteNonQuery() >= 1)
                    MessageBox.Show("Successfully added new contact!", "Confirmation", MessageBoxButton.OK);
                else
                    MessageBox.Show("ERROR: Couldn't add new contact!", "Confirmation", MessageBoxButton.OK);

                con.Close();
            }
        }

    }
}
