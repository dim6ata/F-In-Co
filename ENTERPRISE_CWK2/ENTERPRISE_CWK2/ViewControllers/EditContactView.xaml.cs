using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace ENTERPRISE_CWK2.ViewControllers
{

    /// <summary>
    /// Interaction logic for EditContactView.xaml
    /// </summary>
    public partial class EditContactView : UserControl
    {
        private AddContactView aCV;
        private MainWindow mw;
        public EditContactView(MainWindow mw)
        {
            InitializeComponent();
            this.mw = mw;
            aCV = new AddContactView(mw);
            aCV.pageTitle = "Edit Contact";
            aCV.setPageTitle();

            //Code to retrieve data from current user and populate text boxes:
            aCV.nameTB.Text = mw.mdm.selectedContact.ContactName;
            aCV.emailTB.Text = mw.mdm.selectedContact.ContactEmail;
            aCV.phoneTB.Text = mw.mdm.selectedContact.ContactPhone;
            aCV.accountTB.Text = mw.mdm.selectedContact.ContactAccNum;
            aCV.sCodeTB.Text = mw.mdm.selectedContact.ContactSortCode;

            aCV.ft.cancelButtonHandler(aCV.ConfirmButton_Click);//remove button handler of add contact.

            aCV.ft.confirmButton.Click += ConfirmButton_Click;

            editContactControl.Content = aCV;

        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (aCV.checkContactEntriesValid())
            {
                confirmedEditContactSequence();
            }
            else
            {
                mw.createNotificationWindow(NotificationWindow.NOTIFICATION_VIEW, "You need to fill in all the boxes of this form");
            }
        }

        private void confirmedEditContactSequence()
        {
            mw.mdm.selectedContact.ContactName = aCV.nameTB.Text;
            mw.mdm.selectedContact.ContactEmail = aCV.emailTB.Text;
            mw.mdm.selectedContact.ContactPhone = aCV.phoneTB.Text;
            mw.mdm.selectedContact.ContactAccNum = aCV.accountTB.Text;
            mw.mdm.selectedContact.ContactSortCode = aCV.sCodeTB.Text;

            mw.returnToPreviousView();
            mw.dispatchDataCreateView(() => mw.mdm.dbUpdater.updateContact(mw.mdm.selectedContact), typeof(ContactsView));//concurrently inserts data and updates view.

        }
    }
}
