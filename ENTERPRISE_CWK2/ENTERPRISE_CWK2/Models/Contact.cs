using System;
using System.Collections.Generic;

namespace ENTERPRISE_CWK2.Models
{
    /// <summary>
    /// Model class that contains all data about Contacts
    /// </summary>
    public class Contact
    {

        public int ContactID { get; set; }
        public int ContactUserID { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string ContactAccNum { get; set; }
        public string ContactSortCode { get; set; }
        public List<Transaction> ContTransList { get; set; }
        public double ContactReceived { get; set; }
        public double ContactSpent { get; set; }


        public Contact()
        {
            ContTransList = new List<Transaction>();
        }

        public void SetContactDetails(int cID, string cName, string cPhoneNum,
             string cAccountNum, string cSortCode, int uID, string email)
        {
            this.ContactID = cID;
            this.ContactUserID = uID;
            this.ContactName = cName;
            this.ContactPhone = cPhoneNum;
            this.ContactAccNum = cAccountNum;
            this.ContactSortCode = cSortCode;
            this.ContactEmail = email;
        }

        public override string ToString()
        {
            return String.Format("{0}", ContactName);
        }

        /// <summary>
        /// Calculates transactions for each contact
        /// </summary>
        public void calculateTransactions()
        {
            //resets values before adding again
            ContactSpent = 0;
            ContactReceived = 0;
            //note income and expenses are reversed here since an income for a user is an expense for a contact and vice versa.
            for (int i = 0; i < ContTransList.Count; i++)
            {
                if (ContTransList[i] is Income)
                {                    
                    ContactSpent -= ContTransList[i].TransAmount;
                }
                else
                {
                    if(ContTransList[i] is Expense)
                    {                                               
                        ContactReceived += ContTransList[i].TransAmount;
                    }
                }

            }

        }
    }

}
