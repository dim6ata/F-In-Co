using ENTERPRISE_CWK2.Models;
using ENTERPRISE_CWK2.Utility;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ENTERPRISE_CWK2.ViewControllers
{
    /// <summary>
    /// Interaction logic for ContactDetailsView.xaml
    /// </summary>
    public partial class ContactDetailsView : UserControl
    {

        private Button backButton;
        private MainWindow mw;

        public ContactDetailsView(MainWindow mw)
        {
            InitializeComponent();

            this.mw = mw;
            contactAccDetailsLabel.Content = String.Format("Acc.: {0} | Sort Code: {1}", 
                mw.mdm.selectedContact.ContactAccNum, 
                mw.mdm.selectedContact.ContactSortCode);

            contactEmailLabel.Content = String.Format("Email: {0}", mw.mdm.selectedContact.ContactEmail);
            contactPhoneLabel.Content = String.Format("Phone: {0}", mw.mdm.selectedContact.ContactPhone);
            contactTotalReceived.Content = mw.mdm.convertAmountDoubleToString(Math.Abs(mw.mdm.selectedContact.ContactReceived));
            contactTotalPaidLabel.Content = mw.mdm.convertAmountDoubleToString(Math.Abs(mw.mdm.selectedContact.ContactSpent));
            contactNameLabel.Content = mw.mdm.selectedContact.ContactName;
            contactTransactionsLabel.Content = String.Format("Transactions of {0}", mw.mdm.selectedContact.ContactName);

            backButton = mw.buttonDesigner.backButton();
            backButton.Click += BackButton_Click;
            contactButtonsStackPanel.Children.Add(backButton);
            
            contactsDataGrid.ItemsSource = mw.mdm.transAccountCollection;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            mw.returnToPreviousView();
        }
    }
}
