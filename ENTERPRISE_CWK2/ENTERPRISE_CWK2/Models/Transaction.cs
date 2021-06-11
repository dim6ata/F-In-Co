using System;

namespace ENTERPRISE_CWK2.Models
{
    /// <summary>
    /// Abstract class transaction that acts as a model that keeps all transaction data elements in working memory
    /// </summary>
    public abstract class Transaction
    {

        public int TransID { get; set; }
        public abstract double TransAmount { get; set; }//abstract property that is utilised in transaction subclasses - ensuring correct amount entry.
        public string TransAmountString { get; set; }
        public DateTime TransDate { get; set; }
        
        public string TransDateString{ get; private set; }
        public string TransReference { get; set; }       
        public int TransAccID { get; set; }
        public int TransServiceID { get; set; }
        public Contact TransContact { get; set; }
        public string TransType { get; set; }

        public void setTransactionDetails(int tID, double tAmount, DateTime tDate,
            string tRef, Contact tCon, int tAID, int sID)
        {
            TransID = tID;
            TransAmount = tAmount;
            TransDate = tDate;
            TransReference = tRef;
            TransContact = tCon;
            TransAccID = tAID;
            TransServiceID = sID;
            TransAmountString = TransAmount.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-gb"));
        }

        /// <summary>
        ///To string method overrid - used for displaying transaction
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0} {1} {2} {3} {4} {5} {6}", TransID, TransContact.ContactName,
                TransContact.ContactAccNum, TransContact.ContactSortCode, TransReference, TransDate, TransAmount);
        }
        
        /// <summary>
        /// converts date picker DateTime format to a data base DateTime format YYYY-MM-DD 
        /// </summary>
        public void setTransactionString()
        {
            TransDateString = TransDate.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// abstract method that allows writing of type to database.
        /// </summary>
        /// <returns></returns>
        public abstract bool getType();
       
    }
}
