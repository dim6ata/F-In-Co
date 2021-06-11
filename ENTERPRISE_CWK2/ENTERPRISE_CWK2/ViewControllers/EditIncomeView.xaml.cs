using ENTERPRISE_CWK2.Models;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace ENTERPRISE_CWK2.ViewControllers
{
    /// <summary>
    /// Interaction logic for EditIncomeView.xaml
    /// </summary>
    public partial class EditIncomeView : UserControl
    {
        private MainWindow mw;
        public AddIncomeView aIV { get; set; }
        public EditIncomeView(MainWindow mw)
        {

            InitializeComponent();
            this.mw = mw;
            aIV = new AddIncomeView(mw);
            aIV.pageTitle = "Edit Income";
            aIV.setPageTitle();
           
            aIV.ft.addBackButton();

            aIV.ft.cancelButtonHandler(aIV.ConfirmButton_Click);
            aIV.ft.confirmButton.Click += ConfirmButton_Click;

            populateVisualElements();

            aIV.referenceTB.Text = mw.mdm.selectedTransaction.TransReference;
            editIncomeControl.Content = aIV;
        }

        /// <summary>
        /// 
        /// </summary>
        private void populateVisualElements()
        {

            /*
             POPULATE VISUAL ELEMENTS WITH DATA THAT ALREADY EXISTS FOR SELECTED TRANSACTION
             */

            //choose selected contact:
            foreach (Contact contact in aIV.paidByCB.Items)
            {
                if (contact.ContactName == mw.mdm.selectedTransaction.TransContact.ContactName)
                {
                    aIV.paidByCB.SelectedIndex = aIV.paidByCB.Items.IndexOf(contact);
                }
            }

            aIV.dPicker.SelectedDate = mw.mdm.selectedTransaction.TransDate;


            aIV.amountTB.Text = mw.mdm.selectedTransaction.TransAmount.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-gb"));

            //choose selected account
            foreach (Account account in aIV.paidToCB.Items)
            {
                if (account.AccID == mw.mdm.selectedTransaction.TransAccID)
                {
                    aIV.paidToCB.SelectedIndex = aIV.paidToCB.Items.IndexOf(account);
                }
            }

            //choose type of service:
            foreach (Service service in aIV.serviceTypeCB.Items)
            {
                if (service.ServiceID == mw.mdm.selectedTransaction.TransServiceID)
                {
                    aIV.serviceTypeCB.SelectedIndex = aIV.serviceTypeCB.Items.IndexOf(service);
                }
            }
        }

        /// <summary>
        /// Handler for confirm button click in Edit Income View
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (aIV.checkIncomeEntriesValid())
            {
                confirmedEditIncomeSequence();
            }
            else
            {
                mw.createNotificationWindow(NotificationWindow.NOTIFICATION_VIEW, "You need to fill in all the boxes of this form");
            }
        }

        

        private void confirmedEditIncomeSequence()
        {
            //paidByCB is for contact
            //paidToCB is for account

            //CHANGES OF
            //CONTACT:
            Contact newContact = (Contact)aIV.paidByCB.SelectedItem;
            if (mw.mdm.selectedTransaction.TransContact.ContactID != newContact.ContactID)
            {
                mw.mdm.removeTransactionFromList(mw.mdm.selectedTransaction,//passes the transaction
                    mw.mdm.selectedTransaction.TransContact.ContTransList);//passes the list of transactions
                mw.mdm.addTransactionToList(mw.mdm.selectedTransaction,//passes the transaction
                    newContact.ContTransList);//passes the list of transactions for the newly selecte contact
                mw.mdm.selectedTransaction.TransContact = newContact;
            }

            //DATE:

            mw.mdm.selectedTransaction.TransDate = (DateTime)aIV.dPicker.SelectedDate;
            mw.mdm.selectedTransaction.setTransactionString();


            bool hasBalanceChanged = false;//used as a flag to change to true only if the following conditions are met:

            //AMOUNT:
            if (mw.mdm.selectedTransaction.TransAmountString != aIV.amountTB.Text)//enters condition if the amount has been changed
            {
                double newAmount = mw.mdm.convertAmountStringToDouble(aIV.amountTB.Text);
                mw.mdm.selectedTransaction.TransAmountString = aIV.amountTB.Text;//used for updating visual elements in lists
                mw.mdm.updateBalance(mw.mdm.selectedTransaction, mw.mdm.selectedAccount, newAmount);//calculates the new balance
                mw.mdm.selectedTransaction.TransAmount = newAmount;
                hasBalanceChanged = true;
            }

            //ACCOUNT:
            Account newAcc = (Account)aIV.paidToCB.SelectedItem;

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
            Service serv = (Service)aIV.serviceTypeCB.SelectedItem;
            if (serv.ServiceID != mw.mdm.selectedTransaction.TransServiceID)
            {
                mw.mdm.selectedTransaction.TransServiceID = serv.ServiceID;
            }

            //REFERENCE
            if (mw.mdm.selectedTransaction.TransReference != aIV.referenceTB.Text)
            {
                mw.mdm.selectedTransaction.TransReference = aIV.referenceTB.Text;
            }

            mw.updateBalanceLabel();
            mw.returnToPreviousView();//returns to previous view in order to avoid multiple attempts at editing the same transaction
            mw.dispatchDataButtonsUpdate(() => mw.mdm.dbUpdater.updateTransaction(mw.mdm.selectedTransaction));//sends transaction data to database concurrently

        }
    }
}
