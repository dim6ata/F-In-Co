using ENTERPRISE_CWK2.Models;
using System;
using System.Data.SQLite;
using System.Diagnostics;

namespace ENTERPRISE_CWK2.Utility
{
    /// <summary>
    /// Class that is responsible for all reading database queries 
    /// </summary>
    public class ReadDB
    {
        private string paramID = "@id";
        private string paramEmail = "@uEmail";
        private User user;
        private SystemDB systemDB;
        public ReadDB(User user)
        {
            this.user = user;
            systemDB = SystemDB.Instance;
        }

        /// <summary>
        /// Retrieves user details from database
        /// </summary>
        /// <param name="email"></param>
        public void queryUserDetails(string email)
        {
            string selectUser = "SELECT User.userID, User.userName, User.userEmail, " +
                "User.userAddress, User.userPostCode " +
                "FROM User WHERE User.userEmail = @uEmail";
            SQLiteDataReader reader = null;
            SQLiteCommand command;
            SQLiteConnection conn = systemDB.connect();

            try
            {
                command = conn.CreateCommand();
                command.CommandText = selectUser;
                command.Parameters.AddWithValue(paramEmail, email);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    user.UserID = reader.GetInt32(0);
                    user.UserName = reader.GetString(1);
                    user.UserEmail = reader.GetString(2);
                    user.UserAddress = reader.GetString(3);
                    user.UserPostCode = reader.GetString(4);
                }

            }
            catch (SQLiteException e)
            {
                Debug.WriteLine("There was an SQL reading issue - " + e);
            }
            finally
            {
                systemDB.CloseConnections(reader, conn);
            }
        }

        /// <summary>
        /// Retrieves accounts that are associated to current user from database
        /// </summary>
        /// <param name="userID"></param>
        public void queryUserAccounts(int userID)
        {
            string selectAccount = "SELECT Account.accountID, Account.accountName, " +
                    "IFNULL(Account.accountBalance, 0), Account.accountNum, " +
                    "Account.accountSortCode, TypeAC.typeName, TypeAC.typeID " +
                    "FROM Account " +
                    "JOIN AccountType " +
                    "ON Account.accountID = AccountType.accountID " +
                    "JOIN TypeAC ON AccountType.typeID = TypeAC.typeID " +
                    "WHERE Account.accountUserID = @id " +
                    "ORDER BY Account.accountID";
            SQLiteDataReader reader = null;
            SQLiteCommand command;
            SQLiteConnection conn = systemDB.connect();

            try
            {
                command = conn.CreateCommand();
                command.CommandText = selectAccount;
                command.Parameters.AddWithValue(paramID, userID);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Account acc = new Account();
                    AccountType accType = new AccountType();

                    int accID = reader.GetInt32(0);
                    accType.accTypeID = reader.GetInt32(6);
                    accType.accTypeName = reader.GetString(5);

                    acc.SetAccountDetails(accID, reader.GetString(1), reader.GetDouble(2),
                        reader.GetString(3), reader.GetString(4), accType);

                    user.UserAccountsList.Add(acc);
                    user.UserAccountsDict.Add(accID, acc);//populate dictionary                    
                }
            }
            catch (SQLiteException e)
            {
                Debug.WriteLine("There was an SQL reading issue - " + e);
            }
            finally
            {
                systemDB.CloseConnections(reader, conn);
            }
        }

        /// <summary>
        /// Retrieves Contact information from database.
        /// </summary>
        /// <param name="userID"></param>
        public void queryUserContacts(int userID)
        {
            string selectContact = "SELECT Contact.contactID, Contact.contactName," +
                    " Contact.contactPhone, Contact.contactAccountNum, " +
                    "Contact.contactSortCode, UserContact.userID, " +
                    "Contact.contactEmail " +
                    "FROM Contact " +
                    "JOIN UserContact " +
                    "ON Contact.contactID = UserContact.contactID " +
                    "AND UserContact.userID = @id " +
                    "ORDER BY Contact.contactName";
            SQLiteDataReader reader = null;
            SQLiteCommand command;
            SQLiteConnection conn = systemDB.connect();
            try
            {
                command = conn.CreateCommand();
                command.CommandText = selectContact;
                command.Parameters.AddWithValue(paramID, userID);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Contact contact = new Contact();
                    int cID = reader.GetInt32(0);
                    contact.SetContactDetails(cID, reader.GetString(1),
                        reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetInt32(5), reader.GetString(6));

                    user.UserContactsList.Add(cID, contact);
                    user.UserSortableContactList.Add(contact);
                }
            }
            catch (SQLiteException e)
            {
                Debug.WriteLine("There was an SQL reading issue - " + e);
            }
            finally
            {
                systemDB.CloseConnections(reader, conn);
            }
        }

        /// <summary>
        /// Retrieves Transactions for each account from database.
        /// </summary>
        /// <param name="acc"></param>
        public void queryUserTransactions(Account acc)
        {
            string selectTransaction = "SELECT TransactionDetails.transactionID, TransactionDetails.transactionAmount," +
            " TransactionDetails.transactionDate, TransactionDetails.transactionReference, " +
            "TransactionDetails.contactID, TransactionDetails.accountID, TransactionDetails.serviceID, " +
            "TransactionDetails.isIncome " +
            "FROM TransactionDetails " +
            "JOIN Account" +
            " ON TransactionDetails.accountID = Account.accountID " +
            "AND TransactionDetails.accountID = @id " +
            "ORDER BY TransactionDetails.transactionDate DESC;";
            SQLiteDataReader reader = null;
            SQLiteCommand command;
            SQLiteConnection conn = systemDB.connect();

            try
            {
                command = conn.CreateCommand();
                command.CommandText = selectTransaction;
                command.Parameters.AddWithValue(paramID, acc.AccID);
                reader = command.ExecuteReader();
                Transaction transaction;

                while (reader.Read())
                {
                    int tID = reader.GetInt32(0);
                    double tAmount = Convert.ToDouble(reader.GetString(1));
                    DateTime tDateTime = reader.GetDateTime(2);
                    string tRef = reader.GetString(3);
                    int contactID = reader.GetInt32(4);
                    int accID = reader.GetInt32(5);
                    int serviceID = reader.GetInt32(6);

                    int isIncome = Convert.ToInt32(reader.GetBoolean(7));

                    if (isIncome == 1)
                    {
                        transaction = new Income();
                        transaction.TransType = "IN";
                        acc.incomeList.Add(transaction);
                    }
                    else
                    {
                        transaction = new Expense();
                        transaction.TransType = "OUT";
                        acc.expenseList.Add(transaction);
                    }

                    transaction.setTransactionDetails(tID, tAmount, tDateTime, tRef,
                        user.UserContactsList[contactID], accID, serviceID);

                    acc.transactionsList.Add(transaction);

                    user.UserContactsList[contactID].ContTransList.Add(transaction);//finds an element in contact dictionary.
                }
            }
            catch (SQLiteException e)
            {
                Debug.WriteLine("There was an SQL reading issue - " + e);
            }
            finally
            {
                systemDB.CloseConnections(reader, conn);
            }

        }

        /// <summary>
        /// Retrieves Services from database
        /// </summary>
        /// <param name="userID"></param>
        public void queryUserServices(int userID)
        {
            string selectService = "SELECT Service.serviceID, Service.serviceName, Account.accountID " +
            "FROM Service JOIN ServiceAccount ON Service.serviceID = ServiceAccount.serviceID " +
            "JOIN Account ON ServiceAccount.accountID = Account.accountID " +
            "AND Account.accountUserID = @id " +
            "GROUP BY Service.serviceID;";
        SQLiteDataReader reader = null;
            SQLiteCommand command;
            SQLiteConnection conn = systemDB.connect();
            try
            {
                command = conn.CreateCommand();
                command.CommandText = selectService;
                command.Parameters.AddWithValue(paramID, userID);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Service service = new Service();
                    int sID = reader.GetInt32(0);
                    service.ServiceID = sID;
                    service.ServiceName = reader.GetString(1);
                    service.ServiceAccID = reader.GetInt32(2);

                    user.UserServicesList.Add(sID, service);
                }
            }
            catch (SQLiteException e)
            {
                Debug.WriteLine("There was an SQL reading issue - " + e);
            }
            finally
            {
                systemDB.CloseConnections(reader, conn);
            }
        }

        /// <summary>
        /// Retrieves account types that are stored on database.
        /// </summary>
        public void queryAccountTypes()
        {
            string selectTypes = "SELECT TypeAC.typeID, TypeAC.typeName FROM TypeAC ORDER BY TypeAC.typeID;";
            SQLiteDataReader reader = null;
            SQLiteCommand command;
            SQLiteConnection conn = systemDB.connect();
            try
            {

                command = conn.CreateCommand();
                command.CommandText = selectTypes;
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    AccountType aType = new AccountType();
                    int aID = reader.GetInt32(0);
                    string aName = reader.GetString(1);
                    aType.accTypeID = aID;
                    aType.accTypeName = aName;
                    user.AvailableAccountTypes.Add(aType);
                }
            }
            catch (SQLiteException e)
            {
                Debug.WriteLine("There was an SQL reading issue - " + e);
            }
            finally
            {
                systemDB.CloseConnections(reader, conn);
            }
        }
    }
}
