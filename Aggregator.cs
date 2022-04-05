using System.Collections.Generic;

namespace Test
{
    public class Aggregator
    {
        #region Properties

        #endregion

        #region Constructor

        #endregion

        #region Public Methods
        public static double[] AggregateYearlyProjections(List<double[]> listOfProjections, List<Inputs> listOfInputs)
        {
            double[] totalArray = new double[MaximumTermOfProjection.SetTermOfProjection(listOfInputs)];

            foreach (var projection in listOfProjections)
            {
                for (int i = 0; i < projection.Length; i++)
                {
                    totalArray[i] += projection[i];
                }
            }

            return totalArray;
        }
        public static double GetTotalSum(double[] array)
        {
            double sum = 0;

            for (int i = 0; i < array.Length; i++)
            {
                sum += array[i];
            }
            return sum;
        }
        #endregion
    }
}