using System.Collections;
using System.Collections.Generic;
using ANN_Kart.ANN;
using UnityEngine;

public class Layer
{
	// TODO: Layer Type will be used later while training the weights of neuron for separating the input layer.
	private LayerType layerType;
	private int numberOfNeurons;
	private List<Neuron> neurons;

	public int NumberOfNeurons => numberOfNeurons;
	public List<Neuron> Neurons => neurons;

	public Layer(LayerType layerType, int numberOfNeurons) => InitializeVariables(layerType, numberOfNeurons);

	private void InitializeVariables(LayerType layerType, int numberOfNeurons)
	{
		this.layerType = layerType;
		this.numberOfNeurons = numberOfNeurons;	
	} 

	public void InitializeNeurons(int numberOfInputsPerNeuron)
	{
		for(int i = 0; i < numberOfNeurons; i++)
			neurons.Add(new Neuron(GetNeuronType(layerType), numberOfInputsPerNeuron));
	}

	private NeuronType GetNeuronType(LayerType layerType)
	{
		switch (layerType)
		{
			case LayerType.INPUT:
				return NeuronType.INPUT;
			case LayerType.HIDDEN:
				return NeuronType.HIDDEN;
			case LayerType.OUTPUT:
				return NeuronType.OUTPUT;
			default:
				return NeuronType.INPUT;
		}
	}

	public List<float> CalculateOutput(List<float> inputValues)
	{
		if (layerType == LayerType.INPUT)
			return inputValues;

		List<float> outputValues = new List<float>();
		foreach (Neuron neuron in neurons)
			outputValues.Add(neuron.CalculateOutput(inputValues));

		return outputValues;
	}
}
