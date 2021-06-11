using ENTERPRISE_CWK2.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTERPRISE_CWK2.Utility
{
    /// <summary>
    /// Class that is responsible for providing all the data base updating functionality
    /// within the system.
    /// </summary>
    public class UpdateDB
    {

        private SystemDB systemDB;
        private User dbUser;

        public UpdateDB(User currentUser)
        {
            systemDB = SystemDB.Instance;
            dbUser = currentUser;

        }

        /// <summary>
        /// Updates basic account details
        /// </summary>
        /// <param name="account"></param>
        public void updateAccount(Account account)
        {
            SQLiteCommand command;
            SQLiteConnection conn = systemDB.connect();
            string updateAccountSQL = "Update Account SET accountName = @aName, accountNum = @aNum, accountSortCode = @aSCode WHERE accountID = @aID;";
            string updateAccountTypeSQL = "Update AccountType SET typeID=@tID WHERE accountID = @aID;";

            try
            {
                command = conn.CreateCommand();
                command.CommandText = updateAccountSQL;
                command.Parameters.AddWithValue("aName", account.AccName);
                command.Parameters.AddWithValue("aNum", account.AccNum);
                command.Parameters.AddWithValue("aSCode", account.AccSortCode);
                command.Parameters.AddWithValue("aID", account.AccID);
                command.ExecuteNonQuery();

                command.Parameters.Clear();
                command.CommandText = updateAccountTypeSQL;
                command.Parameters.AddWithValue("tID", account.AccType.accTypeID);
                command.Parameters.AddWithValue("aID", account.AccID);
                command.ExecuteNonQuery();
            }
            catch (SQLiteException e)
            {
                Debug.WriteLine("The SQL Error is " + e);
            }
            finally
            {
                systemDB.CloseConnection(conn);
            }
        }


        /// <summary>
        /// Updates transaction details in data base
        /// </summary>
        /// <param name="trans"></param>
        public void updateTransaction(Transaction trans)
        {
            SQLiteCommand command;
            SQLiteConnection conn = systemDB.connect();
            string updateTransactionSQL = "Update TransactionDetails SET transactionAmount = @tAmount, transactionDate = @tDate, " +
                "transactionReference = @tRef, accountID = @aID, contactID = @cID, serviceID = @sID WHERE transactionID = @tID;";
            try
            {
                command = conn.CreateCommand();
                command.CommandText = updateTransactionSQL;
                command.Parameters.AddWithValue("tAmount", trans.TransAmount);
                command.Parameters.AddWithValue("tDate", trans.TransDateString);//passing the string value as it is in converted format
                command.Parameters.AddWithValue("tRef", trans.TransReference);
                command.Parameters.AddWithValue("aID", trans.TransAccID);
                command.Parameters.AddWithValue("cID", trans.TransContact.ContactID);
                command.Parameters.AddWithValue("sID", trans.TransServiceID);
                command.Parameters.AddWithValue("tID", trans.TransID);
                command.ExecuteNonQuery();

            }
            catch (SQLiteException e)
            {
                Debug.WriteLine("The SQL Error is " + e);
            }
            finally
            {
                systemDB.CloseConnection(conn);
            }
        }

        /// <summary>
        /// Updates Balance in an account
        /// </summary>
        /// <param name="account"></param>
        internal void updateBalance(Account account)
        {
            string updateBalanceSQL = "Update Account SET accountBalance = @bAmount WHERE accountID = @id";
            SQLiteCommand command;
            SQLiteConnection conn = systemDB.connect();
            try
            {
                command = conn.CreateCommand();
                command.CommandText = updateBalanceSQL;
                command.Parameters.AddWithValue("bAmount", account.AccBalance);
                command.Parameters.AddWithValue("id", account.AccID);
                command.ExecuteNonQuery();
            }
            catch (SQLiteException e)
            {
                Debug.WriteLine("The SQL Error is " + e);
            }
            finally
            {
                systemDB.CloseConnection(conn);
            }
        }

        /// <summary>
        /// Updates Contact Details
        /// </summary>
        /// <param name="contact"></param>
        internal void updateContact(Contact contact)
        {
            string updateContactSQL = "Update Contact SET contactName = @cName, contactEmail = @cEmail, " +
                "contactPhone = @cPhone, contactAccountNum = @cAccNum, contactSortCode = @cSCode WHERE contactID = @cID";
            SQLiteCommand command;
            SQLiteConnection conn = systemDB.connect();
            try
            {
                command = conn.CreateCommand();
                command.CommandText = updateContactSQL;
                command.Parameters.AddWithValue("cName", contact.ContactName);
                command.Parameters.AddWithValue("cEmail", contact.ContactEmail);
                command.Parameters.AddWithValue("cPhone", contact.ContactPhone);
                command.Parameters.AddWithValue("cAccNum", contact.ContactAccNum);
                command.Parameters.AddWithValue("cSCode", contact.ContactSortCode);
                command.Parameters.AddWithValue("cID", contact.ContactID);
                command.ExecuteNonQuery();
            }
            catch (SQLiteException e)
            {
                Debug.WriteLine("The SQL Error is " + e);
            }
            finally
            {
                systemDB.CloseConnection(conn);
            }
        }
    }
}
