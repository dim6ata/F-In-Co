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
    public class Income : Transaction
    {
        //use this class to set the value of a transaction to positive
        private bool isIncome = true;

        private double amount;
        public override double TransAmount { get {return amount;} set { amount = Math.Abs(value); } }

        public override bool getType()
        {
            return isIncome;
        }

    }
}
