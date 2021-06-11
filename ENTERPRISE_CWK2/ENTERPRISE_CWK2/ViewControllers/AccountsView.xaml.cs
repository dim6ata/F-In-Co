using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ENTERPRISE_CWK2.Models;
using ENTERPRISE_CWK2.Utility;

namespace ENTERPRISE_CWK2.ViewControllers
{
    /// <summary>
    /// Interaction logic for AccountsView.xaml
    /// </summary>
    public partial class AccountsCtrl : UserControl
    {

        private List<AccountCustomCtrl> acc;//list of custom account buttons
        private ObservableCollection<Transaction> transactionObsColList;
        private int SELECTOR_ID;
        private ComboBoxItem selectedItem;
        private Button addAccountButton;
        private Button editAccountButton;
        private MainWindow mw;
        private Button editTransactionButton;
        private Button deleteAccountButton;
        private Button deleteTransactionButton;
        private const int NO_SELECTION = -1;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="mw"></param>
        public AccountsCtrl(MainWindow mw)
        {
            InitializeComponent();

            this.mw = mw;

            if (editAccountButton == null)
            {
                editAccountButton = mw.buttonDesigner.editButton();
                editAccountButton.Click += EditAccountButton_Click;
                accButtonsStackPanel.Children.Insert(0, editAccountButton);
                hideButtons(editAccountButton);

                deleteAccountButton = mw.buttonDesigner.deleteButton();
                deleteAccountButton.Click += DeleteAccountButton_Click;
                accButtonsStackPanel.Children.Insert(0, deleteAccountButton);
                hideButtons(deleteAccountButton);
            }

            addAccountButton = mw.buttonDesigner.addButton();
            addAccountButton.Click += AddButton_Click;
            accButtonsStackPanel.Children.Add(addAccountButton);

            
            //setting up button for editing transactions. 
            editTransactionButton = mw.buttonDesigner.editButton();
            editTransactionButton.Click += EditTransactionButton_Click;
            createTransactionButtons(editTransactionButton);

            deleteTransactionButton = mw.buttonDesigner.deleteButton();
            deleteTransactionButton.Click += DeleteTransactionButton_Click;
            createTransactionButtons(deleteTransactionButton);

            fillAccountButtonDetails();

        }

        private void createTransactionButtons(Button button)
        {
            transactionButtonStackPanel.Children.Insert(0, button);
            hideButtons(button);
        }

        private void showButtons(Button button)
        {
            button.Visibility = Visibility.Visible;
        }

        private void hideButtons(Button button)
        {
            button.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Handler for add button clicks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            mw.CreateAddAccount();
        }

        /// <summary>
        /// Fills custom account buttons with relevant information based on specific user accounts.
        /// </summary>
        public void fillAccountButtonDetails()
        {

            if (accountsHolderStackPanel.Children.Count > 0)//if there are any buttons before this is called they are removed. 
            {
                Debug.WriteLine("DEBUG - the element should be cleared");
                accountsHolderStackPanel.Children.Clear();//allows for new buttons to be generated.
                disselectAccountButtons(NO_SELECTION);//clears all previous selections 
            }

            if (mw.mdm.currentUser.UserAccountsList != null)
            {
                acc = new List<AccountCustomCtrl>();
                int numButtons = mw.mdm.currentUser.UserAccountsList.Count();

                for (int i = 0; i < numButtons; i++)
                {
                    acc.Add(new AccountCustomCtrl(this, i));
                    acc[i].LabelText = mw.mdm.currentUser.UserAccountsList[i].AccType.accTypeName;
                    acc[i].NameText = mw.mdm.currentUser.UserAccountsList[i].AccName;
                    acc[i].DetailsText = SetupAccountDetails(mw.mdm.currentUser.UserAccountsList[i]);
                    acc[i].BalanceText = mw.mdm.currentUser.UserAccountsList[i].AccBalance
                        .ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-gb"));

                    accountsHolderStackPanel.Children.Add(acc[i]);
                }
            }
        }

        /// <summary>
        /// provides formatting for account details in custom button
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        private string SetupAccountDetails(Account acc)
        {
            return String.Format("Acc.:{0} | Sort Code:{1}", acc.AccNum, acc.AccSortCode);
        }


        /// <summary>
        /// manages selected AccountCustomCtrl buttons by disabling the selected and 
        /// enabling all the rest.
        /// </summary>
        /// <param name="id"></param>
        private void ManageAccButtonSelection(int id)
        {

            for (int i = 0; i < acc.Count; i++)
            {
                if (acc[i].buttonID == id)
                {
                    acc[id].customButton.IsEnabled = false;

                    accountDataGrid.Visibility = Visibility.Visible;
                    transactionLabel.Visibility = Visibility.Visible;
                    transactionsComboBox.Visibility = Visibility.Visible;
                    transactionsComboBox.SelectedIndex = 0; //resets the combo box to view all when a new account button is selected. 

                    //makes edit account buttons visible
                    showButtons(editAccountButton);
                    showButtons(deleteAccountButton);

                    //once the elements are showing also the small buttons 
                    //will need to be displayed edit, and delete for account. 
                    mw.mdm.selectedAccount = mw.mdm.currentUser.UserAccountsList[id];

                    refineTransactionsListView(mw.mdm.selectedAccount.transactionsList);

                    displayTransactionButtons();

                }
                else
                {
                    acc[i].customButton.IsEnabled = true;
                }
            }

        }
        /// <summary>
        /// Method that hides all visible elements, 
        /// disselects accounts control buttons
        /// removes data from selected account
        /// </summary>
        /// <param name="id"></param>
        public void disselectAccountButtons(int id)
        {
            if (editAccountButton != null)
            {
                hideButtons(this.editAccountButton);
                hideButtons(this.deleteAccountButton);
                hideButtons(this.editTransactionButton);
                hideButtons(this.deleteTransactionButton);
                accountDataGrid.Visibility = Visibility.Hidden;
                transactionLabel.Visibility = Visibility.Hidden;
                transactionsComboBox.Visibility = Visibility.Hidden;
                ManageAccButtonSelection(id);
                mw.mdm.selectedAccount = null;
            }
        }

        private void displayTransactionButtons()
        {
            if (accountDataGrid != null)
            {
                if (accountDataGrid.SelectedIndex >= 0)//if the data grid is selected
                {
                    if (!editTransactionButton.IsVisible)//when the transaction buttons are not visible = display them
                    {
                        showButtons(editTransactionButton);
                        showButtons(deleteTransactionButton);
                    }
                }

                else//when the transaction buttons are visible - hide them
                {
                    hideButtons(editTransactionButton);
                    hideButtons(deleteTransactionButton);
                }
            }
        }

        /// <summary>
        /// Assigns different transaction lists and displays them in data grid. 
        /// Used when combo box values are changed.
        /// </summary>
        /// <param name="tList"></param>
        private void refineTransactionsListView(List<Transaction> tList)
        {
            transactionObsColList =
                        new ObservableCollection<Transaction>(tList);

            accountDataGrid.ItemsSource = transactionObsColList;
        }

        
        /**************************************************************************************
                                                HANDLERS
         **************************************************************************************/

        /// <summary>
        /// method that is called each time a click event is handled 
        /// by each custom button from AccountCustomCtrl
        /// </summary>
        /// <param name="id">receives the id associated to the specific button clicked</param>
        public void CustomButtonLocalHandler(int id)
        {
            SELECTOR_ID = id;
            ManageAccButtonSelection(id);
        }


        private void EditAccountButton_Click(object sender, RoutedEventArgs e)
        {
            mw.createEditAccount();
        }

        /// <summary>
        /// Deletes accounts button upon click on delete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteAccountButton_Click(object sender, RoutedEventArgs e)
        {
           
            mw.createNotificationWindow(NotificationWindow.CONFIRMATION_VIEW, 
                $"Are you certain you would like to delete \n{mw.mdm.selectedAccount.AccName}'s Account?");
            if (mw.nw.isConfirmed)
            {
                mw.mdm.removeAccountFromLists(mw.mdm.selectedAccount);
                mw.mdm.updateCollections();
                mw.dispatchData(() => mw.mdm.dbDelete.deleteAccount(mw.mdm.selectedAccount));
                fillAccountButtonDetails();
            }
            //mw.dispatchData(() => mw.mdm.updateCollections());
        }


        /// <summary>
        /// method that handles the click of edit transaction button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditTransactionButton_Click(object sender, RoutedEventArgs e)
        {
            mw.CreateEditTransaction();
        }


        /// <summary>
        /// Deletes a transaction 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteTransactionButton_Click(object sender, RoutedEventArgs e)
        {
            mw.createNotificationWindow(NotificationWindow.CONFIRMATION_VIEW,
                $"Are you certain you would like to delete \n{mw.mdm.selectedAccount.AccName}'s Transaction?");

            if (mw.nw.isConfirmed)
            {
                mw.mdm.RemoveFromBalance(mw.mdm.selectedTransaction, mw.mdm.selectedAccount);

                mw.mdm.removeTransactionFromList(mw.mdm.selectedTransaction, mw.mdm.selectedAccount.transactionsList);
                mw.mdm.removeTransactionFromList(mw.mdm.selectedTransaction, mw.mdm.selectedTransaction.TransContact.ContTransList);

                if (mw.mdm.selectedTransaction is Income)
                {
                    mw.mdm.removeTransactionFromList(mw.mdm.selectedTransaction, mw.mdm.selectedAccount.incomeList);
                }
                else
                {
                    mw.mdm.removeTransactionFromList(mw.mdm.selectedTransaction, mw.mdm.selectedAccount.expenseList);
                }

                mw.dispatchData(() => mw.mdm.dbDelete.deleteTransaction(mw.mdm.selectedTransaction));
                fillAccountButtonDetails();
                mw.dispatchData(() => mw.mdm.updateCollections());
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AccountDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mw.mdm.selectedTransaction = (Transaction)accountDataGrid.SelectedItem;
            displayTransactionButtons();
        }


        /// <summary>
        /// Refines the list displayed in data grid depending on which element is selected in combo box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            selectedItem = ((sender as ComboBox).SelectedItem) as ComboBoxItem;

            switch (selectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last())
            {
                case "View All":
                    refineTransactionsListView(mw.mdm.currentUser.UserAccountsList[SELECTOR_ID].transactionsList);
                    break;

                case "View Income":
                    refineTransactionsListView(mw.mdm.currentUser.UserAccountsList[SELECTOR_ID].incomeList);
                    break;
                case "View Expenses":
                    refineTransactionsListView(mw.mdm.currentUser.UserAccountsList[SELECTOR_ID].expenseList);
                    break;

            }

            displayTransactionButtons();
        }
    }
}