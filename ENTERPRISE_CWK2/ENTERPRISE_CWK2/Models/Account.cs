using System;
using System.Collections.Generic;

namespace ENTERPRISE_CWK2.Models
{
    public class Account
    {
        public int AccID { get; set; }
        public string AccName { get; set; }
        public string AccNum { get; set; }
        public string AccSortCode { get; set; }

        private double balance = 0;//sets initial value to zero.
        public double AccBalance { get {return balance; } set {balance = value; } }
        
        public AccountType AccType { get; set; }

        public List<Transaction> transactionsList { get; set; }
        public List<Transaction> incomeList { get; set; }
        public List<Transaction> expenseList { get; set; }

        public Account()
        {
            transactionsList = new List<Transaction>();
            incomeList = new List<Transaction>();
            expenseList = new List<Transaction>();
        }

        public void SetAccountDetails(int aID, string aName, double aBalance, string aNum, 
            string aSortCode, AccountType aType)
        {
            this.AccID = aID;
            this.AccName = aName;
            this.AccNum = aNum;
            this.AccSortCode = aSortCode;
            this.AccBalance = aBalance;
            this.AccType = aType;

        }

        public override string ToString()
        {
            //return String.Format(AccName);
            if (AccName != null)
            {
                return String.Format("{0}, Acc.:{1}", AccName, formatAccNum());
            }
            return "";
        }

        public string formatAccNum()
        {
            if (AccNum != null)
            {
                string toKeep = AccNum.Substring(5, 3);

                return String.Format("*****{0}", toKeep);
            }
            return "";
        }
    }
}
