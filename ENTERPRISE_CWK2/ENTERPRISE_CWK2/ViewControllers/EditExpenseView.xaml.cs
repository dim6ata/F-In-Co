using ENTERPRISE_CWK2.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ENTERPRISE_CWK2.ViewControllers
{
    /// <summary>
    /// Interaction logic for EditExpenseView.xaml
    /// </summary>
    public partial class EditExpenseView : UserControl
    {
        private MainWindow mw;
        private AddExpenseView aEV;
        public EditExpenseView(MainWindow mw)
        {

            InitializeComponent();
            this.mw = mw;
            aEV = new AddExpenseView(mw);
            aEV.pageTitle = "Edit Expense";
            aEV.setPageTitle();
            
            aEV.ft.addBackButton();

            aEV.ft.cancelButtonHandler(aEV.ConfirmButton_Click);
            aEV.ft.confirmButton.Click += ConfirmButton_Click;

            populateVisualElements();

            aEV.referenceTB.Text = mw.mdm.selectedTransaction.TransReference;

            editExpenseControl.Content = aEV;
        }

        /// <summary>
        /// 
        /// </summary>
        private void populateVisualElements()
        {
            //choose selected contact:
            foreach (Contact contact in aEV.paidToCB.Items)
            {
                if (contact.ContactName == mw.mdm.selectedTransaction.TransContact.ContactName)
                {
                    aEV.paidToCB.SelectedIndex = aEV.paidToCB.Items.IndexOf(contact);
                }
            }

            aEV.dPicker.SelectedDate = mw.mdm.selectedTransaction.TransDate;

            aEV.amountTB.Text = mw.mdm.selectedTransaction.TransAmount.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-gb"));

            //choose selected account
            foreach (Account account in aEV.paidByCB.Items)
            {
                if (account.AccID == mw.mdm.selectedTransaction.TransAccID)
                {
                    aEV.paidByCB.SelectedIndex = aEV.paidByCB.Items.IndexOf(account);
                }
            }

            //choose type of service:
            foreach (Service service in aEV.serviceTypeCB.Items)
            {
                if (service.ServiceID == mw.mdm.selectedTransaction.TransServiceID)
                {
                    aEV.serviceTypeCB.SelectedIndex = aEV.serviceTypeCB.Items.IndexOf(service);
                }
            }
        }

        /// <summary>
        /// Handler for confirm button click in Edit Expense View
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (aEV.checkExpenseEntryValid())
            {
                confirmedEditExpenseSequence();
            }
            else
            {
                mw.createNotificationWindow(NotificationWindow.NOTIFICATION_VIEW, "You need to fill in all the boxes of this form");
            }
        }

        private void confirmedEditExpenseSequence()
        {
            //paidToCB is for contact
            //paidByCB is for account

            //CHANGES OF
            //CONTACT:
            Contact newContact = (Contact)aEV.paidToCB.SelectedItem;
            if (mw.mdm.selectedTransaction.TransContact.ContactID != newContact.ContactID)
            {
                mw.mdm.removeTransactionFromList(mw.mdm.selectedTransaction,//passes the transaction
                    mw.mdm.selectedTransaction.TransContact.ContTransList);//passes the list of transactions
                mw.mdm.addTransactionToList(mw.mdm.selectedTransaction,//passes the transaction
                    newContact.ContTransList);//passes the list of transactions for the newly selecte contact
                mw.mdm.selectedTransaction.TransContact = newContact;
            }

            //DATE:

            mw.mdm.selectedTransaction.TransDate = (DateTime)aEV.dPicker.SelectedDate;
            mw.mdm.selectedTransaction.setTransactionString();


            bool hasBalanceChanged = false;//used as a flag to change to true only if the following conditions are met:

            //AMOUNT:
            if (mw.mdm.selectedTransaction.TransAmountString != aEV.amountTB.Text)//enters condition if the amount has been changed
            {
                double newAmount = mw.mdm.convertAmountStringToDouble(aEV.amountTB.Text);
                mw.mdm.selectedTransaction.TransAmountString = aEV.amountTB.Text;//used for updating visual elements in lists
                mw.mdm.updateBalance(mw.mdm.selectedTransaction, mw.mdm.selectedAccount, newAmount);//calculates the new balance
                mw.mdm.selectedTransaction.TransAmount = newAmount;
                hasBalanceChanged = true;
            }

            //ACCOUNT:
            Account newAcc = (Account)aEV.paidByCB.SelectedItem;

            if (mw.mdm.selectedTransaction.TransAccID != newAcc.AccID)//only does the change if there has been migration of transactions across accounts.
            {
                mw.mdm.updateDataUponTransactionChange(mw.mdm.selectedTransaction, mw.mdm.selectedAccount, newAcc);
                mw.dispatchData(() => mw.mdm.dbUpdater.updateBalance(newAcc));
                hasBalanceChanged = true;
            }

            //updates balance of account only if the transaction amount is changed or transaction is moved to another account
            if (hasBalanceChanged)
            {
                mw.dispatchData(() => mw.mdm.dbUpdater.updateBalance(mw.mdm.selectedAccount));
            }

            //SERVICE:
            Service serv = (Service)aEV.serviceTypeCB.SelectedItem;
            if (serv.ServiceID != mw.mdm.selectedTransaction.TransServiceID)
            {
                mw.mdm.selectedTransaction.TransServiceID = serv.ServiceID;
            }

            //REFERENCE
            if (mw.mdm.selectedTransaction.TransReference != aEV.referenceTB.Text)
            {
                mw.mdm.selectedTransaction.TransReference = aEV.referenceTB.Text;
            }

            mw.updateBalanceLabel();
            mw.returnToPreviousView();//returns to previous view in order to avoid multiple attempts at editing the same transaction


            mw.dispatchDataButtonsUpdate(() => mw.mdm.dbUpdater.updateTransaction(mw.mdm.selectedTransaction));//sends transaction data to database concurrently

        }
    }
}
