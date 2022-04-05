using System;

namespace Test
{
    public class Inputs
    {
        #region Properties
        private string _typeOfInput;
        public readonly double _annualChangeInYearlyPayments;
        public int Time { get; }
        public double DiscountRate { get; }
        public double Cost { get; }
        public double Inflation { get; }
        public int YearsToRollForwardBy { get; }
        public double RollForwardInflationRate { get; }
        public bool IsContinuous { get; }
        #endregion

        #region Constructor
        public Inputs(string typeOfInput, double cost, double inflation, double annualChangeInYearlyPayments,
            int time, double discountRate, bool isContinuous, int yearsToRollForward, double rollForwardInflationRate)
        {
            _typeOfInput = typeOfInput;
            Cost = cost;
            Inflation = inflation;
            _annualChangeInYearlyPayments = annualChangeInYearlyPayments;
            Time = time;
            DiscountRate = discountRate;
            IsContinuous = isContinuous;
            YearsToRollForwardBy = yearsToRollForward;
            RollForwardInflationRate = rollForwardInflationRate;
        }
        
        public Inputs(string typeOfInput, double cost, double inflation, double annualChangeInYearlyPayments,
            int time, double discountRate, bool isContinuous)
        {
            _typeOfInput = typeOfInput;
            Cost = cost;
            Inflation = inflation;
            _annualChangeInYearlyPayments = annualChangeInYearlyPayments;
            Time = time;
            DiscountRate = discountRate;
            IsContinuous = isContinuous;
        }
        #endregion

        #region Public Methods
        public double GetAvgPercentOfAnnualChangeInPayments(int cashFlowYear)
        {
            double factor = (100 + _annualChangeInYearlyPayments)/100;
            double avgPercentOfAnnualChangeInPayments = ((Math.Pow(factor, cashFlowYear) + Math.Pow(factor, cashFlowYear + 1)) / 2);
            return avgPercentOfAnnualChangeInPayments;
        }
        #endregion
    }
}