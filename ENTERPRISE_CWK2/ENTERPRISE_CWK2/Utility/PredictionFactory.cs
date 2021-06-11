using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTERPRISE_CWK2.Models;
using ENTERPRISE_CWK2.ViewControllers;

namespace ENTERPRISE_CWK2.Utility
{
    /// <summary>
    /// Class retrieves all data that is used by Prediction model to calculate prediction
    /// amount for a future date.
    /// </summary>
    public class PredictionFactory
    {
        /// <summary>
        /// Struct that keeps location data of a PredictionFactory
        /// Used to populate factory at a specific grid location
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public struct Location
        {
            public int row { get; private set; }
            public int column { get; private set; }
            
            public Location(int row, int column)
            {
                this.row = row;
                this.column = column;
            }
        }

        public Location location { get; set; }
        public Prediction prediction { get; set; }
        private double tempBalanceAmount;


        /// <summary>
        /// Custom Constructor
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="colNum"></param>
        /// <param name="selectedRowItem"></param>
        /// <param name="selectedAccount"></param>
        /// <param name="selectedDate"></param>
        public PredictionFactory(int rowNum, int colNum, string selectedRowItem, Account selectedAccount, DateTime selectedDate)
        {
            tempBalanceAmount = 0; 
            prediction = new Prediction();            
            location = new Location(rowNum, colNum);
            prediction.chosenFutureDate = selectedDate;
            prediction.presentDate = DateTime.Now;

            switch (selectedRowItem)
            {
                case ReportTemplateView.INCOME:
                    retrievePredictionDetails(selectedAccount.incomeList);//calculates only for income entries
                    break;
                case ReportTemplateView.EXPENSE:
                    retrievePredictionDetails(selectedAccount.expenseList);//calculates only for expenses
                    break;

                case ReportTemplateView.NET:
                    retrievePredictionDetails(selectedAccount.transactionsList);//calculates both income and expense entries
                    break;
            }

            prediction.calculateFutureAmount();//calculates the expected future amount.
        }

        /// <summary>
        /// Retrieves prediction details based on a transaction list.
        /// </summary>
        /// <param name="list"></param>
        private void retrievePredictionDetails(List<Transaction> list)
        {
            bool dateFound = false;
            for (int i = (list.Count - 1); i >= 0; i--)//reversed traversion as elements are ordered in reverse by date - oldest ones last.
            {
                tempBalanceAmount += list[i].TransAmount;//calculates temporary balance by adding values of all transactions up until a certain date is reached

                if (!dateFound)//this will run until a past date is established
                {
                    if (isPastDateInRange(list[i].TransDate, prediction.presentDate))//checks if the date is within the 3 months period up to now.
                    {
                        dateFound = true;
                        prediction.pastDate = list[i].TransDate;
                        prediction.pastAmount = Math.Abs(tempBalanceAmount);//set to math.abs if it doesn't calculate properly.
                        prediction.InitialDaysCount = getDaysCount(prediction.pastDate, prediction.presentDate);
                        prediction.FinalDaysCount = getDaysCount(prediction.pastDate, prediction.chosenFutureDate);
                    }
                }
            }
            //absolute amount is taken to properly calculate the positive and negative amounts.
            prediction.presentAmount = Math.Abs(tempBalanceAmount);//assigns the calculation of each element 
        }

        /// <summary>
        /// Retrieves the count of days between two dates
        /// </summary>
        /// <param name="olderDate"></param>
        /// <param name="newerDate"></param>
        /// <returns></returns>
        private int getDaysCount(DateTime olderDate, DateTime newerDate)
        {
            return (newerDate - olderDate).Days;
        }

        /// <summary>
        /// Determines if a date is within a 3 months range
        /// </summary>
        /// <param name="pastDate"></param>
        /// <param name="currentDate"></param>
        /// <returns>Returns true when the date is close to 3 months before current</returns>
        private bool isPastDateInRange(DateTime pastDate, DateTime currentDate)
        {
            DateTime suggestedDate = currentDate.AddMonths(-3);
            return (pastDate >= suggestedDate);
        }
    }
}
