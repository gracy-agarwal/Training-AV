using System.Collections.Generic;
using ANN_Kart.ANN;
using UnityEngine;

public class Neuron
{
	private NeuronType neuronType;
	private int numberOfInputs;
	private float bias;
	private float output;
	private List<float> weights;
	private List<float> inputs;

	public Neuron(NeuronType neuronType, int numberOfInputs)
	{
		InitializeVariables(neuronType, numberOfInputs);
		InitializeWeights();
		InitializeBias();
	}

	private void InitializeVariables(NeuronType neuronType, int numberOfInputs)
	{
		this.neuronType = neuronType;
		this.numberOfInputs = numberOfInputs;
		weights = new List<float>();
		inputs = new List<float>();
	}
	
	private void InitializeBias()
	{
		if (neuronType == NeuronType.INPUT)
			return;
		
		bias = Random.Range(-1f,1f);
	} 
	private void InitializeWeights()
	{
		if (neuronType == NeuronType.INPUT)
			return;
		
		float weightRange = CalculateWeightRange();
		for(int i = 0; i < numberOfInputs; i++)
			weights.Add(Random.Range(-weightRange,weightRange));
	}

	/// <summary>
	/// This function calculates a weight range based on the number of inputs to the neuron.
	/// This calculation is intended for weight initialization using a heuristic similar to Xavier initialization, 
	/// </summary>
	/// <returns></returns>
	private float CalculateWeightRange() => (1.0f / Mathf.Sqrt(numberOfInputs));
}
