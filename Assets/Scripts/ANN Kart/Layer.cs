using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer {

	public int numberOfNeurons;
	public List<Neuron> neurons;

	public Layer(int numberOfNeurons, int numberOfInputsPerNeuron)
	{
		InitializeVariables(numberOfNeurons);
		InitializeNeurons(numberOfInputsPerNeuron);
	}

	private void InitializeVariables(int numberOfNeurons) => this.numberOfNeurons = numberOfNeurons;

	private void InitializeNeurons(int numberOfInputsPerNeuron)
	{
		for(int i = 0; i < numberOfNeurons; i++)
			neurons.Add(new Neuron(numberOfInputsPerNeuron));
	}
	
}
