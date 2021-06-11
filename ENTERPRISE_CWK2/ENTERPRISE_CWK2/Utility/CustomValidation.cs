using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ENTERPRISE_CWK2.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomValidation
    {
        /// <summary>
        /// Validates name and reference input
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool validateName(string name)
        {
            if (name.Length == 0)
            { return false; }
           
            if (name.Any(Char.IsDigit) || name.Any(Char.IsSymbol) || name.Any(Char.IsPunctuation))
            { return false; }
            if (name.Length < 3)
            { return false; }

            return true;            
        }

        /// <summary>
        /// Validates account number entries 
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool validateNumber(string number, int numChar)
        {
            if(number.Length != numChar) { return false; }
            if (!number.All(Char.IsDigit)) { return false; }
            return true;
        }

        /// <summary>
        /// Matches a regular expression to ensure sort code in correct format.
        /// Regular expression referenced from:
        /// PhonicUK (2013) validate Bank Sort Code. StackOverflow. Source: https://stackoverflow.com/questions/16716315/validate-bank-sort-code/16716460
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool validateSortCode(string code)
        {
            return Regex.IsMatch(code, @"\b([0-9]{2})-([0-9]{2})-([0-9]{2})\b");

        }
           
        /// <summary>
        /// Amount validation
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool validateAmount(string amount)
        {           
            try
            {
                Double.Parse(amount, System.Globalization.NumberStyles.Currency);
                return true;
            }
            catch (Exception)
            {
                return false;
            }           
        }

        /// <summary>
        /// 
        /// Email Validation Referenced from:
        /// (GeeksForGeeks, No Date). Source:  https://www.geeksforgeeks.org/check-email-address-valid-not-java/ 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool validateEmail(string email)
        {
            return Regex.IsMatch(email, @"^[a-zA-Z0-9_+&*-]+(?:\.[a-zA-Z0-9_+&*-]+)*@(?:[a-zA-Z0-9-]+\.)+[a-zA-Z]{2,7}$");                      
        }

        /// <summary>
        /// Date validation to prevent user from selecting past dates
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool validateDate(DateTime date)
        {
            try
            {
                return date > DateTime.Now;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
