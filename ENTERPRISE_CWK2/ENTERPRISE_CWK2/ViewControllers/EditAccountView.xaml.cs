using ENTERPRISE_CWK2.Models;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace ENTERPRISE_CWK2.ViewControllers
{
    /// <summary>
    /// Interaction logic for EditAccountView.xaml
    /// </summary>
    public partial class EditAccountView : UserControl
    {
        private MainWindow mw;
        private AddAccountView aAV;
        /// <summary>
        /// A controller for editing an existing account.
        /// </summary>
        /// <param name="mw"></param>
        public EditAccountView(MainWindow mw)
        {
            InitializeComponent();
            this.mw = mw;

            aAV = new AddAccountView(mw);
            aAV.pageTitle = "Edit Account";
            aAV.setPageTitle();

            aAV.accNameTB.Text = mw.mdm.selectedAccount.AccName;
            aAV.accNumTB.Text = mw.mdm.selectedAccount.AccNum;
            aAV.sCodeTB.Text = mw.mdm.selectedAccount.AccSortCode;

            foreach (AccountType accountType in aAV.accTypeCB.Items)
            {
                if (accountType.accTypeID == mw.mdm.selectedAccount.AccType.accTypeID)
                {
                    aAV.accTypeCB.SelectedIndex = aAV.accTypeCB.Items.IndexOf(accountType);
                }
            }

            aAV.ft.cancelButtonHandler(aAV.ConfirmButton_Click);
            aAV.ft.confirmButton.Click += ConfirmButton_Click;

            editAccountControl.Content = aAV;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {            
            if (aAV.checkAccountEntriesValid()) {
                confirmedEditAccountSequence();  
            }
            else
            {
                mw.createNotificationWindow(NotificationWindow.NOTIFICATION_VIEW, "You need to fill in all the boxes of this form");
            }
        }

        private void confirmedEditAccountSequence()
        {
            mw.mdm.selectedAccount.AccName = aAV.accNameTB.Text;
            mw.mdm.selectedAccount.AccNum = aAV.accNumTB.Text;
            mw.mdm.selectedAccount.AccSortCode = aAV.sCodeTB.Text;
            mw.mdm.selectedAccount.AccType = (AccountType)aAV.accTypeCB.SelectedItem;

            mw.returnToPreviousView();
            mw.dispatchDataButtonsUpdate(() => mw.mdm.dbUpdater.updateAccount(mw.mdm.selectedAccount));
        }
    }
}
