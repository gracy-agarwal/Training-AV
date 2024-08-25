using System.Collections.Generic;

namespace ANN_Kart.Genetics.Crossover
{
    public class UniformCrossover : ICrossover
    {
        public List<Chromosome> Crossover(Chromosome parent1, Chromosome parent2)
        {
            Chromosome offspring1 = new Chromosome(parent1.Length);
            Chromosome offspring2 = new Chromosome(parent1.Length);

            for (int i = 0; i < parent1.Genes.Count; i++)
            {
                if (UnityEngine.Random.value < 0.5f)
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