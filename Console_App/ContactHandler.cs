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

                int count = command.ExecuteNonQuery();
                if (count == 1)
                    Console.WriteLine("Successfully added new contact!");
                else
                    Console.WriteLine("ERROR: Couldn't add new contact!");

                con.Close();
            }
        }

        public void ViewContact(Contact contact) {

        }

        public void EditContact(Contact contact) {

        }

        public void DeleteContact(Contact contact) {
            using (SqlConnection con = new SqlConnection(ConString)) {
                con.Open();

                string query = "DELETE FROM ContactList WHERE Id = @Id";

                SqlCommand command = new SqlCommand(query, con);

                command.Parameters.AddWithValue("@Id", contact.Id);

                if (command.ExecuteNonQuery() == 1)
                    Console.WriteLine("Successfully deleted contact!");
                else
                    Console.WriteLine("ERROR: Couldn't delete contact!");

                con.Close();
            }
        }
    }
}