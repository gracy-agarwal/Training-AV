using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chromosome
{
    public List<double> genes;

    public Chromosome(int length)
    {
        genes = new List<double>(length);
    }

}