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
    /// Provides all functionality that allows a user to remove elements from database
    /// </summary>
    public class DeleteDB
    {

        private SystemDB systemDB;
        private User dbUser;
        public DeleteDB(User currentUser)
        {
            systemDB = SystemDB.Instance;
            dbUser = currentUser;

        }

        /// <summary>
        /// Remove account from database
        /// </summary>
        /// <param name="acc"></param>
        public void deleteAccount(Account acc)
        {
            string deleteAccountTypeSQL = "DELETE FROM AccountType WHERE AccountType.accountID = @id";
            string deleteAccountSQL = "DELETE FROM Account WHERE Account.accountID = @id";
            string deleteTransactionsSQL = "DELETE FROM TransactionDetails WHERE TransactionDetails.accountID = @id";
            string deleteAccountServiceSQL = "DELETE FROM ServiceAccount WHERE ServiceAccount.accountID = @id";
            string id = "id";          

            SQLiteCommand command;
            SQLiteConnection conn = systemDB.connect();

            try
            {
                command = conn.CreateCommand();

                performNonQuerySequence(command, deleteAccountTypeSQL, id, acc.AccID);
                performNonQuerySequence(command, deleteAccountServiceSQL, id, acc.AccID);
                performNonQuerySequence(command, deleteTransactionsSQL, id, acc.AccID);
                performNonQuerySequence(command, deleteAccountSQL, id, acc.AccID);                
            }
            catch(SQLiteException e)
            {
                Debug.WriteLine(e);
            }
            finally
            {
                systemDB.CloseConnection(conn);
            }

        }

        /// <summary>
        /// Remove transaction from database
        /// </summary>
        /// <param name="transaction"></param>
        public void deleteTransaction(Transaction transaction)
        {
            string deleteTransactionSQL = "DELETE FROM TransactionDetails WHERE TransactionDetails.transactionID = @id";            
            string id = "id";

            SQLiteCommand command;
            SQLiteConnection conn = systemDB.connect();

            try
            {
                command = conn.CreateCommand();
                performNonQuerySequence(command, deleteTransactionSQL, id, transaction.TransID);                               
            }
            catch (SQLiteException e)
            {
                Debug.WriteLine(e);
            }
            finally
            {
                systemDB.CloseConnection(conn);
            }

        }

        /// <summary>
        /// Remove Contact from database
        /// </summary>
        /// <param name="contact"></param>
        public void deleteContact(Contact contact)
        {
            string deleteUserContactSQL = "DELETE FROM UserContact WHERE UserContact.userID = @uID AND UserContact.contactID = @cID;";
            string deleteContactTransactionsSQL = "DELETE FROM TransactionDetails WHERE TransactionDetails.contactID = @cID;";
            string deleteContactSQL = "DELETE FROM Contact WHERE Contact.contactID = @id";
            string id = "id";

            SQLiteCommand command;
            SQLiteConnection conn = systemDB.connect();

            try
            {
                //REMOVES FROM USERCONTACT
                command = conn.CreateCommand();
                command.CommandText = deleteUserContactSQL;
                command.Parameters.AddWithValue("uID", dbUser.UserID);
                command.Parameters.AddWithValue("cID", contact.ContactID);
                command.ExecuteNonQuery();
                command.Parameters.Clear();

                //REMOVES FROM TRANSACTIONS
                performNonQuerySequence(command, deleteContactTransactionsSQL, "cID", contact.ContactID);
                //REMOVES FROM CONTACT
                performNonQuerySequence(command, deleteContactSQL, id, contact.ContactID);
               
            }
            catch (SQLiteException e)
            {
                Debug.WriteLine(e);
            }
            finally
            {
                systemDB.CloseConnection(conn);
            }

        }

        /// <summary>
        /// used to perform a non query with one parameter and id
        /// </summary>
        /// <param name="command"></param>
        /// <param name="statement"></param>
        /// <param name="paramID"></param>
        /// <param name="ID"></param>
        private void performNonQuerySequence(SQLiteCommand command, string statement, string paramID, int ID)
        {           
            command.CommandText = statement;
            command.Parameters.AddWithValue(paramID, ID);           
            command.ExecuteNonQuery();
            command.Parameters.Clear();
        }
    }
}
