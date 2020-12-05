using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_App
{
    public class ContactHandler
    {
        string ConString = ConfigurationManager.ConnectionStrings["ContactConn"].ConnectionString;
        

        public List<Contact> DeleteContact(Contact contact)
        {
        }
    }
}
