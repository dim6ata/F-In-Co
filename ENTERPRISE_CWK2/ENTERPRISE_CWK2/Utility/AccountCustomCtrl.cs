using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ENTERPRISE_CWK2.ViewControllers;

namespace ENTERPRISE_CWK2.Utility
{

    public class AccountCustomCtrl : Control
    {
        private Label accountTypeLabel;
        public String LabelText { get; set; }
        private TextBlock tbName;
        public String NameText { get; set; }
        private TextBlock tbDetails;
        public String DetailsText { get; set; }
        private TextBlock tbBalance;
        public String BalanceText { get; set; }
        public Button customButton { get; set; }
        public int buttonID { get; private set; }
        private AccountsCtrl controller;
        public delegate void CallAccountsList(int id);
        CallAccountsList callAccList;

        public AccountCustomCtrl(AccountsCtrl ctrlCopy, int id)
        {
            this.buttonID = id;
            this.controller = ctrlCopy;
            this.callAccList = controller.CustomButtonLocalHandler;//initialise delegate

            //FOR DEBUGGING:
            //this.Style = (Style)Application.Current.Resources["customButtonStyle"]; //for calling a static method. 
        }

        /// <summary>
        /// retrieves xaml defined elements prior to creating the Account Custom Button object
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            try
            {
                
                accountTypeLabel = Template.FindName("accTypeLabel", this) as Label;
                tbName = Template.FindName("accNameTB", this) as TextBlock;
                tbDetails = Template.FindName("accDetailsTB", this) as TextBlock;
                tbBalance = Template.FindName("accBalanceTB", this) as TextBlock;
                
                customButton = Template.FindName("customAccountButton", this) as Button;
                customButton.Click += CustomButton_Click;
                
                UpdateElements();

            }
            catch (Exception e)
            {

                Console.WriteLine("There has been an error creating this button " + e);
            }

            base.OnApplyTemplate();
        }

        /// <summary>
        /// on click handler defined for each AccountCustomCtrl button that is created. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomButton_Click(object sender, RoutedEventArgs e)
        {     
            //Debug.WriteLine("Button clicked - " + buttonID);            
            callAccList.Invoke(buttonID);    //dispatches each click to CustomButtonLocal handler in AccountsCtrl(AccountsView.cs)                   
        }


        /// <summary>
        /// Updates visual elements of compound button
        /// </summary>
        public void UpdateElements()
        {
            accountTypeLabel.Content = LabelText;
            tbName.Text = NameText;
            tbDetails.Text = DetailsText;
            tbBalance.Text = BalanceText;

        }
    }
}