using System.Collections.Generic;

namespace ANN_Kart.Genetics.Crossover
{
    public interface ICrossover
    {
        public List<Chromosome> Crossover(Chromosome parent1, Chromosome parent2);
    }
}