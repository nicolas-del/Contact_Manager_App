using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Console_App {
    public class ContactHandler {
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

                if (command.ExecuteNonQuery() >= 1)
                    Console.WriteLine("Successfully added new contact!");
                else
                    Console.WriteLine("ERROR: Couldn't add new contact!");

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

                string query = "UPDATE ContactList SET Name = @Name, Phone_Number = @Phone_Number, Address = @Address, Birthday = @Birthday WHERE Id = @Id";

                SqlCommand command = new SqlCommand(query, con);

                command.Parameters.AddWithValue("@Id", contact.Id);
                command.Parameters.AddWithValue("@Name", contact.Name);
                command.Parameters.AddWithValue("@Phone_Number", contact.PhoneNumber);
                command.Parameters.AddWithValue("@Address", contact.Address);
                command.Parameters.AddWithValue("@Birthday", contact.Birthday);

                if (command.ExecuteNonQuery() >= 1)
                    Console.WriteLine("Successfully edited contact!");
                else
                    Console.WriteLine("ERROR: Couldn't edit contact!");

                con.Close();
            }
        }

        public void DeleteContact(Contact contact) {
            using (SqlConnection con = new SqlConnection(ConString)) {
                con.Open();

                string query = "DELETE FROM ContactList WHERE Id = @Id";

                SqlCommand command = new SqlCommand(query, con);

                command.Parameters.AddWithValue("@Id", contact.Id);

                if (command.ExecuteNonQuery() >= 1)
                    Console.WriteLine("Successfully deleted contact!");
                else
                    Console.WriteLine("ERROR: Couldn't delete contact!");

                con.Close();
            }
        }
    }
}