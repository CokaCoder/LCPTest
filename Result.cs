using System.Collections.Generic;

namespace Test
{
    public class Result
    {
        #region Properties
        public List<double[]> TotalProjections;
        public List<double[]> TotalDiscountedProjections;
        #endregion

        #region Constructor
        public Result(List<double[]> totalProjections, List<double[]> totalDiscountedProjections)
        {
            TotalDiscountedProjections = totalDiscountedProjections;
            TotalProjections = totalProjections;
        }
        #endregion
    }
}