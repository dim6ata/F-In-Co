using ENTERPRISE_CWK2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ENTERPRISE_CWK2.Utility
{
    /// <summary>
    /// Class that is responsible for the management of models inside the system. 
    /// Acts as a master model from which the controllers have access to data by the individual model classes
    /// as well as database.
    /// </summary>
    public class ModelDataManager
    {
        //Database instances. 
        public ReadDB dbReader { get; set; }
        public WriteDB dbWriter { get; set; }
        public UpdateDB dbUpdater { get; set; }
        public DeleteDB dbDelete { get; set; }

        //Collection Instances
        public ObservableCollection<Account> accountCollection { get; set; }
        public ObservableCollection<Contact> contactCollection { get; set; }
        public ObservableCollection<Service> serviceCollection { get; set; }
        public ObservableCollection<TransactionAccount> transAccountCollection { get; set; }
        public ObservableCollection<AccountType> accTypeCollection { get; set; }

        //currently selected elements instances:
        public User currentUser { get; set; }
        public Account selectedAccount { get; set; }
        public Transaction selectedTransaction { get; set; }
        public AccountType selectedType { get; set; }
        public Contact selectedContact { get; set; }
        public CustomValidation customValidator { get; set; }

        /// <summary>
        /// Constructor that initialises the models used in the project
        /// </summary>
        public ModelDataManager()
        {
            currentUser = new User();
            dbReader = new ReadDB(currentUser);
            dbWriter = new WriteDB(currentUser);
            dbDelete = new DeleteDB(currentUser);
            dbUpdater = new UpdateDB(currentUser);

            string email = "w1696151@my.westminster.ac.uk";//this would normally be passed upon the creation.
            //string email = "test.me@meme.com";
            //string email = "w.james@fmail.com";
            dbReader.queryUserDetails(email);
            dbReader.queryUserAccounts(currentUser.UserID);
            dbReader.queryUserContacts(currentUser.UserID);            
            populateTransactionsList();           
            dbReader.queryUserServices(currentUser.UserID);
            dbReader.queryAccountTypes();

            updateCollections();

            customValidator = new CustomValidation();
        }

        /// <summary>
        /// assigns lists that are contained within models to observable collections which are used for displaying data. 
        /// It is called when data within main collections has been changed and therefore collections will need to be updated as well.
        /// </summary>
        public void updateCollections()
        {
            accountCollection = new ObservableCollection<Account>(currentUser.UserAccountsList);
            sortListsByName(currentUser.UserSortableContactList);
            contactCollection = new ObservableCollection<Contact>(currentUser.UserSortableContactList);
            serviceCollection = new ObservableCollection<Service>(currentUser.UserServicesList.Values.ToList<Service>());
            transAccountCollection = new ObservableCollection<TransactionAccount>();//populated for transactions when a specific contact is selected
            accTypeCollection = new ObservableCollection<AccountType>(currentUser.AvailableAccountTypes);

        }

        /// <summary>
        /// assigns values to transactions list for each account
        /// </summary>
        private void populateTransactionsList()
        {            
                for (int i = 0; i < currentUser.UserAccountsList.Count; i++)
                {
                    dbReader.queryUserTransactions(currentUser.UserAccountsList[i]);
                }  
        }
        /// <summary>
        /// Calculates total balance for selected user.
        /// </summary>
        /// <returns></returns>
        public string getTotalBalance()
        {
            double balance = 0;
            for (int i = 0; i < currentUser.UserAccountsList.Count; i++)
            {
                balance += currentUser.UserAccountsList[i].AccBalance;
            }
            return convertAmountDoubleToString(balance);
        }

        /// <summary>
        /// Creates a list of data that is based on selected contact's available transactions 
        /// and accounts with which the contacts have dealings with.
        /// 
        /// </summary>
        public void populateTransactionAccountList()
        {
            if (transAccountCollection.Count > 0)//enters condition only if list contains elements.
            {
                transAccountCollection.Clear();//clears the list before it fills it up again.
            }
            for (int i = 0; i < selectedContact.ContTransList.Count; i++)
            {
                TransactionAccount ta = new TransactionAccount(selectedContact.ContTransList[i],//passes the transaction
                    currentUser.UserAccountsDict[selectedContact.ContTransList[i].TransAccID], this);//passes the account
                transAccountCollection.Add(ta);
            }
        }

        /// <summary>
        /// Method that adds and amount of a transaction to Account's balance
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="acc"></param>
        public void addToBalance(Transaction trans, Account acc)
        {
            acc.AccBalance += trans.TransAmount;
        }

        /// <summary>
        /// Method that deducts an amount of a transaction from Account's balance 
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="acc"></param>
        public void RemoveFromBalance(Transaction trans, Account acc)
        {
            acc.AccBalance -= trans.TransAmount;
        }

        /// <summary>
        /// Updates transactions when a transaction has been relocated across accounts.
        /// </summary>
        /// <param name="currentTrans"></param>
        /// <param name="oldAcc"></param>
        /// <param name="newAcc"></param>
        public void updateDataUponTransactionChange(Transaction currentTrans, Account oldAcc, Account newAcc)
        {
            RemoveFromBalance(currentTrans, oldAcc);
            addToBalance(currentTrans, newAcc);

            currentTrans.TransAccID = newAcc.AccID;

            removeTransactionFromList(currentTrans, oldAcc.transactionsList);
            addTransactionToList(currentTrans, newAcc.transactionsList);

            if (currentTrans is Income)
            {
                removeTransactionFromList(currentTrans, oldAcc.incomeList);
                addTransactionToList(currentTrans, newAcc.incomeList);
            }
            else
            {
                removeTransactionFromList(currentTrans, oldAcc.expenseList);
                addTransactionToList(currentTrans, newAcc.expenseList);
            }
        }

        /// <summary>
        /// Calculates the balance of an account as it deducts the previous amount and then adds the new one.
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="acc"></param>
        /// <param name="newAmount"></param>
        public void updateBalance(Transaction trans, Account acc, double newAmount)
        {
            acc.AccBalance = (acc.AccBalance - trans.TransAmount) + newAmount;
        }

        /// <summary>
        /// Removes a transaction from a list
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="list"></param>
        public void removeTransactionFromList(Transaction trans, List<Transaction> list)
        {
            list.Remove(trans);
        }

        /// <summary>
        /// adds a transaction to list
        /// calls sort only when a new item is added. 
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="list"></param>
        public void addTransactionToList(Transaction trans, List<Transaction> list)
        {
            list.Add(trans);
            sortListsByDate(list);
        }

        /// <summary>
        /// Sorts list by date in descending order
        /// </summary>
        /// <param name="list"></param>
        public void sortListsByDate(List<Transaction> list)
        {
            list.Sort((x, y) => y.TransDate.CompareTo(x.TransDate));
        }

        /// <summary>
        /// Sorting Contacts List by name
        /// </summary>
        /// <param name="list"></param>
        public void sortListsByName(List<Contact> list)
        {
            list.Sort((x, y) => x.ContactName.CompareTo(y.ContactName));
        }

        /// <summary>
        /// Converts a £ value string to double.
        /// </summary>
        /// <param name="textValue"></param>
        /// <returns></returns>
        public double convertAmountStringToDouble(string textValue)
        {                       
                return Double.Parse(textValue, System.Globalization.NumberStyles.Currency);             
        }

        /// <summary>
        /// Converts a double amount into a displayable format including £ symbol
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string convertAmountDoubleToString(double value)
        {
            return value.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-gb"));
        }

        /// <summary>
        /// removes deleted account references from lists.
        /// </summary>
        /// <param name="account"></param>
        public void removeAccountFromLists(Account account)
        {
            removeTransactionUponAccountDelete(account.AccID);
            removeService(account.AccID);
            currentUser.UserAccountsDict.Remove(account.AccID);
            currentUser.UserAccountsList.Remove(account);
        }

        /// <summary>
        /// Removes transactions from contact list that belong to a deleted account 
        /// </summary>
        /// <param name="accID"></param>
        public void removeTransactionUponAccountDelete(int accID)
        {
            foreach (var item in currentUser.UserContactsList)//goes through Contact Dictionary of user
            {
                foreach (var trans in item.Value.ContTransList.ToList()) //goes through each transaction list of every contact 
                {
                    if (trans.TransAccID == accID)//only when the account id 
                    {
                        item.Value.ContTransList.Remove(trans);
                    }
                }
            }

        }

        /// <summary>
        /// removes a service that is related to a deleted account
        /// </summary>
        /// <param name="accID"></param>
        public void removeService(int accID)
        {
            foreach (var item in currentUser.UserServicesList)
            {
                if (item.Value.ServiceAccID == accID)
                {
                    currentUser.UserServicesList.Remove(item.Key);
                }
            }

        }
    }
}
