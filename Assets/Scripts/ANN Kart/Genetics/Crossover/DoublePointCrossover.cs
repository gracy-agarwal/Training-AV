using System.Collections.Generic;

namespace ANN_Kart.Genetics.Crossover
{
    public class DoublePointCrossover : ICrossover
    {
        public List<Chromosome> Crossover(Chromosome parent1, Chromosome parent2)
        {
            Chromosome offspring1 = new Chromosome();
            Chromosome offspring2 = new Chromosome();

            int point1 = UnityEngine.Random.Range(0, parent1.Genes.Count);
            int point2 = UnityEngine.Random.Range(point1, parent1.Genes.Count);

            for (int i = 0; i < parent1.Genes.Count; i++)
            {
                if (i < point1 || i > point2)
                {
                    offspring1.Genes[i] = parent1.Genes[i];
                    offspring2.Genes[i] = parent2.Genes[i];
                }
                else
                {
                    offspring1.Genes[i] = parent2.Genes[i];
                    offspring2.Genes[i] = parent1.Genes[i];
                }
            }

            return new List<Chromosome> { offspring1, offspring2 };
        }
    }
}