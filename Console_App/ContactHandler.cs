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
        

        public void AddContact(Contact contact) 
        {
            
        }


        public void ViewContact(Contact contact) 
        {
        
        }

        public void EditContact(Contact contact) 
        {
            
        }

        public void DeleteContact(Contact contact)
        {

        }
    }
}
