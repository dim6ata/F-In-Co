using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTERPRISE_CWK2.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserAddress { get; set; }
        public string UserPostCode { get; set; }       
        public Dictionary<int, Service> UserServicesList{ get; set; }
        public List<Account> UserAccountsList { get; set; }
        public Dictionary<int,Account> UserAccountsDict { get; set; }
        public Dictionary<int,Contact> UserContactsList { get; set; }
        public List<Contact> UserSortableContactList { get; set; }
        public List<AccountType> AvailableAccountTypes;
              
        public User()
        {
            UserAccountsList = new List<Account>();
            UserSortableContactList = new List<Contact>();
            UserContactsList = new Dictionary<int,Contact>();
            UserServicesList = new Dictionary<int, Service>();
            UserAccountsDict = new Dictionary<int, Account>();
            AvailableAccountTypes = new List<AccountType>();            
        }


        public override string ToString()
        {
            return String.Format("Current User is: [ID: {0}], [Name: {1}], [Email:{2}], " +
                "[Address:{3}], [Post Code: {4}]", 
                UserID, UserName, UserEmail,
                UserAddress, UserPostCode);
        }


    }
}
