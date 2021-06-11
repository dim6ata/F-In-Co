using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTERPRISE_CWK2.Models
{
    public class Prediction
    {          
        /*TIME RELATED FIELDS*/
        public DateTime pastDate { get; set; }
        public DateTime presentDate { get; set; }        
        public DateTime chosenFutureDate { get; set; }       
        public int InitialDaysCount { get; set; }
        public int FinalDaysCount { get; set; }        
        /*AMOUNT RELATED FIELDS*/
        public double pastAmount { get; set; }
        public double presentAmount { get; set; }
        private double growthRate;        
        public double futureAmount { get; private set; }
        
        /// <summary>
        /// returns growth rate based on present amount, past amount and the count of days between them
        /// </summary>
        /// <returns></returns>
        public double getGrowthRate()
        {
            growthRate = getGrowthRate(presentAmount, pastAmount, InitialDaysCount);
            return growthRate;
        }

        /// <summary>
        /// Calculates future amount for prediction
        /// </summary>
        public void calculateFutureAmount()
        {
            futureAmount = getRateInTime(pastAmount, getGrowthRate(), FinalDaysCount);
            //return futureAmount;
        }

        /// <summary>
        ///    getting the growth rate value for a specific range of dates. 
        /// </summary>
        /// <param name="present"> holds the current value</param>
        /// <param name="past"> holds the value from the beginning of calculation</param>
        /// <param name="count">number of divisions be it days/months/years </param>
        /// <returns> the value of growth rate in double</returns>
        public double getGrowthRate(double presentAmount, double pastAmount, int countDays)
        {
            return Math.Pow((presentAmount / pastAmount), (1.0 / countDays)) - 1; //1.0 to return value in double
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="past"></param>
        /// <param name="growthRate"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public double getRateInTime(double pastAmount, double growthRate, int countDays)
        {

            return pastAmount * (Math.Pow(growthRate + 1, countDays));
        }


    }
}
