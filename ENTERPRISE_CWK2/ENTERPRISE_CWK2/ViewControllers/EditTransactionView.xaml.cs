using ENTERPRISE_CWK2.Models;
using System.Windows.Controls;

namespace ENTERPRISE_CWK2.ViewControllers
{
    /// <summary>
    /// Interaction logic for EditTransactionView.xaml
    /// Allows to present either Edit(Income or Expenses) view in the same user control page.
    /// </summary>
    public partial class EditTransactionView : UserControl
    {
        public EditTransactionView(MainWindow mw)
        {
            InitializeComponent();

            if(mw.mdm.selectedTransaction is Income)
            {
                editTransactionControl.Content = new EditIncomeView(mw);
            }
            else
            {
                editTransactionControl.Content = new EditExpenseView(mw);
            }

        }
    }
}
