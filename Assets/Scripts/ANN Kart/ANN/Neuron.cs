using System.Collections.Generic;
using ANN_Kart.ANN;
using ANN_Kart.ANN.Activation_Functions;
using UnityEngine;

public class Neuron
{
	private NeuronType neuronType;
	private int numberOfInputs;
	private float bias;
	private float output;
	private List<float> weights;
	private List<float> inputs;

	public List<float> Weights => weights;
	public float Bias => bias;
	public int NumberOfInputs => numberOfInputs;

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
			bias = 0;
		else 
			bias = Random.Range(-1f,1f);
	} 
	
	private void InitializeWeights()
	{
		// Initialize weights even for the input layer, setting them to 0.
		float weightRange = CalculateWeightRange();
		for(int i = 0; i < numberOfInputs; i++)
		{
			if (neuronType == NeuronType.INPUT)
				weights.Add(0f);  // Set input neuron weights to 0.
			else
				weights.Add(Random.Range(-weightRange, weightRange));
		}
	}


	/// <summary>
	/// This function calculates a weight range based on the number of inputs to the neuron.
	/// This calculation is intended for weight initialization using a heuristic similar to Xavier initialization, 
	/// </summary>
	/// <returns></returns>
	private float CalculateWeightRange() => (1.0f / Mathf.Sqrt(numberOfInputs));

	public void SetWeightAtIndex(int weightIndex, float weightToSet) => weights[weightIndex] = weightToSet;

	public void SetBias(float biasToSet) => bias = biasToSet;

	public float CalculateOutput(List<float> inputValues)
	{
		float weightedSum = 0;

		for (int i=0; i<weights.Count; i++)
			weightedSum += inputValues[i] * weights[i];

		weightedSum += bias;
		IActivationFunction activationFunction = new TangentFunction();
		weightedSum = activationFunction.CalculateOutput(weightedSum);
		return weightedSum;
	}
}
