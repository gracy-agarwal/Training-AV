using System.Collections.Generic;

namespace ANN_Kart.Genetics.Genetics.Parent_Selection
{
    public interface ISelection
    {
        public CarPerformanceData SelectParents(List<CarPerformanceData> currentPopulationData);
    }
}