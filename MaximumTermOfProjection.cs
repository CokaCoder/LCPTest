using System.Collections.Generic;
using System.Linq;

namespace Test
{
    public class MaximumTermOfProjection
    {
        public static int SetTermOfProjection(List<Inputs> listOfInputs)
        {
            List<int> termOfProjections = new List<int>(4);
            foreach (Inputs input in listOfInputs)
            {
                int termOfProjection = input.Time;
                termOfProjections.Add(termOfProjection);
            }
            
            int[] arrayOfTerms = termOfProjections.ToArray();

            int maximumTerm = arrayOfTerms.Max();
            return maximumTerm;
        }
    }
}