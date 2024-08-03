using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Previous_Layer {

	public int numNeurons;
	public List<Previous_Neuron> neurons = new List<Previous_Neuron>();

	public Previous_Layer(int nNeurons, int numNeuronInputs)
	{
		numNeurons = nNeurons;
		for(int i = 0; i < nNeurons; i++)
		{
			neurons.Add(new Previous_Neuron(numNeuronInputs));
		}
	}
}
