using System.Collections.Generic;
using UnityEngine;

public class Neuron {

	private int numberOfInputs;
	private float bias;
	private float output;
	private List<float> weights;
	private List<float> inputs;

	public Neuron(int numberOfInputs)
	{
		InitializeVariables(numberOfInputs);
		InitializeWeights();
		InitializeBias();
	}

	private void InitializeVariables(int numberOfInputs)
	{
		this.numberOfInputs = numberOfInputs;
		weights = new List<float>();
		inputs = new List<float>();
	}
	
	private void InitializeBias() => bias = Random.Range(-1f,1f);
	
	private void InitializeWeights()
	{
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
