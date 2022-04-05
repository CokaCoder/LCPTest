using System;

namespace Test
{
    public class Projector
    {
        #region Properties
        public Inputs Inputs;
        #endregion

        #region Constructor
        public Projector(Inputs inputs)
        {
            Inputs = inputs;
        }
        #endregion

        #region Public Methods
        public double[] GetInflatedCost()
        {
            double[] projections = new double[Inputs.Time];
            double factor = (Inputs.Inflation + 100) / 100;

            for (int i = 0; i < Inputs.Time; i++)
            {
                projections[i] = Inputs.Cost * GetProjectionFactor(factor, i);
            }

            return projections;
        }
        
        public double[] GetInflatedCostWuthDecrement()
        {
            double[] projections = GetInflatedCost();

            double[] projectionsWithDecrement = new double[Inputs.Time];
            double factor = (100 + Inputs._annualChangeInYearlyPayments)/100;

            for (int i = 0; i < projections.Length; i++)
            {
                projectionsWithDecrement[i] = projections[i] * Math.Pow(factor, i + 1);
            }

            return projectionsWithDecrement;
        }

        public void UpdateInflatedCostWithDecrement(double[] salaryProjectionsWithoutDecrement)
        {
            for (int i = 0; i < Inputs.Time; i++)
            {
                salaryProjectionsWithoutDecrement[i] *= Inputs.GetAvgPercentOfAnnualChangeInPayments(i);
            }
        }
        #endregion

        #region Private Methods
        private static double GetProjectionFactor(double factor, int cashFlowYear)
        {
            return (Math.Pow(factor, cashFlowYear));
        }
        #endregion
    }
}