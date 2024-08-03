using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Previous_Chromosome {

    public List<double> genes = new List<double>();
    int chromosomeLength = 0;

    public Previous_Chromosome(int l)
    {
        chromosomeLength = l;
       // SetRandom();
    }

   /* private void SetRandom()
    {
        genes.Clear();
        for(int i=0; i<chromosomeLength; i++)
        {
            genes.Add(Random.Range(-1, 1));
        }
    }
   */

    public void SetGene(int pos, double value)
    {
        genes[pos] = value;
    }

    public double GetGene(int pos)
    {
        return genes[pos];
    }

    public void Combine(Chromosome c1, Chromosome c2)
    {
        for(int i=0; i<chromosomeLength; i++)
        {
            int r = Random.Range(0, 10);
            if(r<5)
            {
                double g = c1.genes[i];
                genes[i] = g;
            } else
            {
                double g = c2.genes[i];
                genes[i] = g;
            }
        }
    }

    public void UniformCrossover(Previous_Chromosome c1, Previous_Chromosome c2, Previous_Brain b1, Previous_Brain b2)
    {
        for (int i = 0; i < chromosomeLength; i++)
        {
            int r = Random.Range(0, 10);
            if (r < 5)
            {
                b1.chromosome.genes[i] = c1.genes[i];
                b2.chromosome.genes[i] = c2.genes[i];
            }
            else
            {
                b1.chromosome.genes[i] = c2.genes[i];
                b2.chromosome.genes[i] = c1.genes[i];
            }
        }
    }

    public void SinglePointCrossover(Previous_Chromosome c1, Previous_Chromosome c2, Previous_Brain b1, Previous_Brain b2)
    {
        int r = Random.Range(0,chromosomeLength);

        for (int i = 0; i < chromosomeLength; i++)
        {
            if (i < r)
            {
                b1.chromosome.genes[i] = c1.genes[i];
                b2.chromosome.genes[i] = c2.genes[i];
            }
            else
            {
                b1.chromosome.genes[i] = c2.genes[i];
                b2.chromosome.genes[i] = c1.genes[i];
            }
        }
    }

    public void DoublePointCrossover(Previous_Chromosome c1, Previous_Chromosome c2, Previous_Brain b1, Previous_Brain b2)
    {
        int r1 = Random.Range(0, chromosomeLength-1);
        int r2 = Random.Range(r1, chromosomeLength);

        for (int i = 0; i < chromosomeLength; i++)
        {
            if (i < r1)
            {
                b1.chromosome.genes[i] = c1.genes[i];
                b2.chromosome.genes[i] = c2.genes[i];
            }
            else if(i<r2)
            {
                b1.chromosome.genes[i] = c2.genes[i];
                b2.chromosome.genes[i] = c1.genes[i];
            } else
            {
                b1.chromosome.genes[i] = c1.genes[i];
                b2.chromosome.genes[i] = c2.genes[i];
            }
        }
    }

    public void SimpleArithmeticCrossover(Previous_Chromosome c1, Previous_Chromosome c2, Previous_Brain b1, Previous_Brain b2)
    {
        int r = Random.Range(0, chromosomeLength);
        float alpha = 0.5f;

        for (int i = 0; i < chromosomeLength; i++)
        {
            if (i < r)
            {
                b1.chromosome.genes[i] = c1.genes[i];
                b2.chromosome.genes[i] = c2.genes[i];
            }
            else
            {
                b1.chromosome.genes[i] = (alpha * c1.genes[i]) + ((1 - alpha) * c2.genes[i]);
                b2.chromosome.genes[i] = (alpha * c2.genes[i]) + ((1 - alpha) * c1.genes[i]);
            }
        }
    }

    public void WholeArithmeticCrossover(Previous_Chromosome c1, Previous_Chromosome c2, Previous_Brain b1, Previous_Brain b2)
    {
        float alpha = 0.5f;

        for (int i = 0; i < chromosomeLength; i++)
        {
            b1.chromosome.genes[i] = (alpha * c1.genes[i]) + ((1 - alpha) * c2.genes[i]);
            b2.chromosome.genes[i] = (alpha * c2.genes[i]) + ((1 - alpha) * c1.genes[i]);
         
        }
    }


    public void Mutate()
    {
        for(int i=0; i<chromosomeLength; i++)
        {
            
            genes[i] = Random.Range(-1, 1);

        }
    }

    public void CopyGenes(GameObject parent)
    {
        for(int i=0; i<chromosomeLength; i++)
        {
            genes[i] = parent.GetComponent<Previous_Brain>().chromosome.genes[i];
        }
    }

}
