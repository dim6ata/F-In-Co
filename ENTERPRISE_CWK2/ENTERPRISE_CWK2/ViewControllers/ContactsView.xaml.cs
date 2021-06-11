using ENTERPRISE_CWK2.Models;
using System.Windows;
using System.Windows.Controls;

namespace ENTERPRISE_CWK2.ViewControllers
{
    /// <summary>
    /// Controller for ContactsView.xaml
    /// </summary>
    public partial class ContactsView : UserControl
    {
        private MainWindow mw;
        public Contact selectedContact { get; set; }
        public Button addContactButton { get; set; }
        public Button editContactButton { get; set; }
        public Button viewContactButton { get; set; }
        public Button deleteContactButton { get; set; }
        public ContactsView(MainWindow mw)
        {
            InitializeComponent();
            this.mw = mw;

            //intitialise small buttons
            addContactButton = mw.buttonDesigner.addButton();
            addContactButton.Click += AddContactButton_Click;
            contactButtonsStackPanel.Children.Add(addContactButton);

            editContactButton = mw.buttonDesigner.editButton();
            editContactButton.Click += EditContactButton_Click;
            createInnerButtons(editContactButton);

            deleteContactButton = mw.buttonDesigner.deleteButton();
            deleteContactButton.Click += DeleteContactButton_Click;
            createInnerButtons(deleteContactButton);

            viewContactButton = mw.buttonDesigner.viewButton();
            viewContactButton.Click += ViewContactButton_Click;
            createInnerButtons(viewContactButton);

            contactsDataGrid.ItemsSource = mw.mdm.contactCollection;
        }


        /*********************************************************************************************
         *                                   UI ELEMENTS HANDLERS
         **********************************************************************************************/

        /// <summary>
        /// Handler for deleting a transaction. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteContactButton_Click(object sender, RoutedEventArgs e)
        {
            mw.createNotificationWindow(NotificationWindow.CONFIRMATION_VIEW,
                $"Are you certain you would like to remove \n{mw.mdm.selectedContact.ContactName} from your contacts?");

            if (mw.nw.isConfirmed)
            {
                confirmedContactDeletion();
            }
        }

        /// <summary>
        /// Deletes a contact
        /// </summary>
        private void confirmedContactDeletion()
        {
            //Before deleting contact all associated transactions need to be removed from Account Transactions Lists
            //The balances of all accounts need to be settled             
            foreach (Transaction transaction in mw.mdm.selectedContact.ContTransList)
            {
                mw.mdm.RemoveFromBalance(transaction, mw.mdm.currentUser.UserAccountsDict[transaction.TransAccID]);

                //updates balance of account that is associated with current transaction in database concurrently:
                mw.dispatchData(() => mw.mdm.dbUpdater.updateBalance(mw.mdm.currentUser.UserAccountsDict[transaction.TransAccID]));

                //removes transaction from account lists:
                mw.mdm.removeTransactionFromList(transaction,
                    mw.mdm.currentUser.UserAccountsDict[transaction.TransAccID].transactionsList);//list argument to be removed
                if (transaction is Income)
                {
                    mw.mdm.removeTransactionFromList(transaction,
                        mw.mdm.currentUser.UserAccountsDict[transaction.TransAccID].incomeList);//list argument that is to be removed                        
                }
                else
                {
                    mw.mdm.removeTransactionFromList(transaction,
                         mw.mdm.currentUser.UserAccountsDict[transaction.TransAccID].expenseList);
                }
            }
            //remove contact from lists
            mw.mdm.currentUser.UserContactsList.Remove(mw.mdm.selectedContact.ContactID);
            mw.mdm.currentUser.UserSortableContactList.Remove(mw.mdm.selectedContact);

            //remove contact from database.
            mw.dispatchDataCreateView(() => mw.mdm.dbDelete.deleteContact(mw.mdm.selectedContact), typeof(ContactsView));

            mw.updateBalanceLabel();
        }

        private void ViewContactButton_Click(object sender, RoutedEventArgs e)
        {
            mw.createViewContact();
        }

        private void EditContactButton_Click(object sender, RoutedEventArgs e)
        {
            mw.createEditContact();
        }

        private void AddContactButton_Click(object sender, RoutedEventArgs e)
        {
            mw.createAddContact();
        }

        private void ContactsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mw.mdm.selectedContact = (Contact)contactsDataGrid.SelectedItem;
            displaySelectedButtons();
            mw.mdm.selectedContact.calculateTransactions();
            mw.mdm.populateTransactionAccountList();
        }

        /*********************************************************************************************
         *                                   UTILITY METHODS
         **********************************************************************************************/

        private void displaySelectedButtons()
        {
            if (contactsDataGrid != null)
            {
                if (contactsDataGrid.SelectedIndex >= 0)//ensures that datagrid is selected before making buttons visible.
                {
                    if (!editContactButton.IsVisible)
                    {
                        showInnerButtons(editContactButton);
                        showInnerButtons(deleteContactButton);
                        showInnerButtons(viewContactButton);
                    }
                }
                else
                {
                    hideInnerButtons(editContactButton);
                    hideInnerButtons(deleteContactButton);
                    hideInnerButtons(viewContactButton);
                }
            }
        }

        private void createInnerButtons(Button button)
        {
            contactButtonsStackPanel.Children.Insert(0, button);
            hideInnerButtons(button);
        }

        private void showInnerButtons(Button button)
        {
            button.Visibility = Visibility.Visible;
        }

        private void hideInnerButtons(Button button)
        {
            button.Visibility = Visibility.Hidden;
        }
    }
}
