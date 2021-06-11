using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTERPRISE_CWK2.Models
{
    /// <summary>
    /// Class that extends transactions
    /// </summary>
    public class Expense : Transaction
    {
        //use this class to set the value of a transaction to negative
        private bool isIncome = false;
        private double amount;
        public override double TransAmount { get { return amount; } set { amount = - (Math.Abs(value)); } }//sets transaction amount to negative.
        public override bool getType()
        {
            return isIncome;
        }

    }
}
