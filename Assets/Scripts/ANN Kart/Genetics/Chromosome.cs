using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chromosome
{
    private List<float> genes;

    public int Length => genes.Count;

    public Chromosome(int length) => genes = new List<float>(length);

    public void SetGeneAtPosition(int position, float value) => genes[position] = value;

    public float GetGeneAtPosition(int position) => genes[position];
}