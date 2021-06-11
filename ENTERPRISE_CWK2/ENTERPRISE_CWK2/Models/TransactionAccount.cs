using ENTERPRISE_CWK2.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTERPRISE_CWK2.Models
{
    /// <summary>
    /// This class combines fields from Transaction and Account in order to display custom data that is related to both. 
    /// Currently used for displaying data in ContactDetailsView
    /// </summary>
    public class TransactionAccount
    {

        public string TransactionType { get; set; }
        public int TransactionID { get; set; }
        public double TransactionAmount { get; set; }
        public string TransactionAmountString { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionReference { get; set; }
        public string TransactionName { get; set; }
        public string TransactionAccNum { get; set; }
        public string TransactionSortCode { get; set; }

        public TransactionAccount(Transaction t, Account a, ModelDataManager mdm)
        {
            TransactionID = t.TransID;
            TransactionAmount = t.TransAmount;
            TransactionAmountString = mdm.convertAmountDoubleToString(Math.Abs(t.TransAmount));
            TransactionDate = t.TransDate;
            TransactionReference = t.TransReference;
            TransactionName = a.AccName;
            TransactionAccNum = a.AccNum;
            TransactionSortCode = a.AccSortCode;

            //reverses IN and OUT as the income for an account would be an expense of a contact
            if (t.TransType == "IN")
            {
                TransactionType = "OUT";
            }
            else
            {
                TransactionType = "IN";
            }
        }
    }
}
