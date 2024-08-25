using UnityEngine;

namespace ANN_Kart.Genetics.Utility
{
    public static class Mutation
    {
        public static Chromosome Mutate(Chromosome chromosome)
        {
            float mutationRate = 0.01f; // Adjust this rate as needed

            for (int i = 0; i < chromosome.Genes.Count; i++)
            {
                if (UnityEngine.Random.value < mutationRate)
                {
                    chromosome.Genes[i] = UnityEngine.Random.Range(-1f, 1f); // Example mutation: randomize the gene
                }
            }

            return chromosome;
        }
    }
}