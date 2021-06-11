using ENTERPRISE_CWK2.Models;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace ENTERPRISE_CWK2.ViewControllers
{
    /// <summary>
    /// Interaction logic for AddContactView.xaml
    /// </summary>
    public partial class AddContactView : UserControl
    {
        public FormTemplate ft { get; set; }
        private MainWindow mw;
        public string pageTitle { get; set; }
        public TextBox nameTB { get; set; }
        public TextBox emailTB { get; set; }
        public TextBox phoneTB { get; set; }
        public TextBox accountTB { get; set; }
        public TextBox sCodeTB { get; set; }
        private int NUM_ROWS = 5;

        public AddContactView(MainWindow mw)
        {
            InitializeComponent();
            this.mw = mw;
            initializeUIElements();

            pageTitle = "Add New Contact";

            ft = new FormTemplate(mw, NUM_ROWS);
            setPageTitle();

            ft.addNewRow("Name", nameTB);
            ft.addNewRow("Email", emailTB);
            ft.addNewRow("Phone", phoneTB);
            ft.addNewRow("Account Number", accountTB);
            ft.addNewRow("Sort Code", sCodeTB);

            ft.addBackButton();

            ft.addConfirmButton();
            ft.confirmButton.Click += ConfirmButton_Click;

            addContactControl.Content = ft;
        }

        public void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (checkContactEntriesValid())
            {
                confirmedAddContactSequence();
            }
            else
            {
                mw.createNotificationWindow(NotificationWindow.NOTIFICATION_VIEW, "You need to fill in all the boxes of this form");
                //Debug.WriteLine("You need to fill in all the boxes of the form");
            }
        }


        /// <summary>
        /// Sequence that is performed when all form entries are correctly entered
        /// </summary>
        private void confirmedAddContactSequence()
        {
            Contact newContact = new Contact();

            newContact.ContactName = nameTB.Text;
            newContact.ContactEmail = emailTB.Text;
            newContact.ContactPhone = phoneTB.Text;
            newContact.ContactAccNum = accountTB.Text;
            newContact.ContactSortCode = sCodeTB.Text;

            mw.returnToPreviousView();
            mw.dispatchDataCreateView(() => mw.mdm.dbWriter.insertContact(newContact), typeof(ContactsView));//concurrently inserts data and updates view.

        }

        public void setPageTitle()
        {
            ft.titleLabel.Content = pageTitle;
        }

        private void initializeUIElements()
        {
            nameTB = mw.uiDesigner.getNewTextBox();
            nameTB.TextChanged += NameTB_TextChanged;
            emailTB = mw.uiDesigner.getNewTextBox();
            emailTB.TextChanged += EmailTB_TextChanged;
            phoneTB = mw.uiDesigner.getNewTextBox();
            phoneTB.TextChanged += PhoneTB_TextChanged;
            accountTB = mw.uiDesigner.getNewTextBox();
            accountTB.TextChanged += AccountTB_TextChanged;
            sCodeTB = mw.uiDesigner.getNewTextBox();
            sCodeTB.TextChanged += SCodeTB_TextChanged;
        }

        private void SCodeTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            ft.verifyTextBoxState(mw.mdm.customValidator.validateSortCode(sCodeTB.Text), "Enter 6 digits in format: DD-DD-DD");
        }

        private void AccountTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            ft.verifyTextBoxState(mw.mdm.customValidator.validateNumber(accountTB.Text, FormTemplate.ACCOUNT_NUM_LENGTH), "Enter 8 digits");
        }

        private void PhoneTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            ft.verifyTextBoxState(mw.mdm.customValidator.validateNumber(phoneTB.Text, FormTemplate.PHONE_NUM_LENGTH), "Enter 11 digits");
        }

        private void EmailTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            ft.verifyTextBoxState(mw.mdm.customValidator.validateEmail(emailTB.Text), "Enter a valid email address");
        }

        private void NameTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            ft.verifyTextBoxState(mw.mdm.customValidator.validateName(nameTB.Text),
               "Enter more than 3 characters. Numbers or symbols not allowed.");
        }


        public bool checkContactEntriesValid()
        {
            return
                mw.mdm.customValidator.validateName(nameTB.Text) &&
                mw.mdm.customValidator.validateEmail(emailTB.Text) &&
                mw.mdm.customValidator.validateNumber(phoneTB.Text, FormTemplate.PHONE_NUM_LENGTH) &&
                mw.mdm.customValidator.validateNumber(accountTB.Text, FormTemplate.ACCOUNT_NUM_LENGTH) &&
                mw.mdm.customValidator.validateSortCode(sCodeTB.Text);

        }
    }
}
