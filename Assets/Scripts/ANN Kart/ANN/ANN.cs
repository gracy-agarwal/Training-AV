using System.Collections;
using System.Collections.Generic;
using ANN_Kart.ANN;
using UnityEngine;

public class ANN
{
    private ANN_Data AnnData;
    List<Layer> layers;

    public ANN(ANN_Data AnnData)
    {
        this.AnnData = AnnData;
        InitializeLayers();
    }

    private void InitializeLayers()
    {
        layers = new List<Layer>();

        for (int i = 1; i < AnnData.NumberOfLayers; i++)
        {
            int numberOfNeuronsInLayer = AnnData.GetNumberOfNeuronAtLayer(i);
            int numberOfNeuronsInPreviousLayer = AnnData.GetNumberOfNeuronAtLayer(i - 1);
            Layer newLayer = new Layer(numberOfNeuronsInLayer, numberOfNeuronsInPreviousLayer);
            layers.Add(newLayer);
        }
    }

    
    
    
    
    // public List<double> Train(List<double> inputValues, List<double> desiredOutput)
    // {
    //     List<double> outputValues = new List<double>();
    //     outputValues = CalcOutput(inputValues);
    //     UpdateWeights(outputValues, desiredOutput);
    //     return outputValues;
    // }

    public List<double> CalcOutput(List<double> inputValues)
    {
        List<double> inputs = new List<double>();
        List<double> outputValues = new List<double>();
        int currentInput = 0;

        if (inputValues.Count != numInputs)
        {
            Debug.Log("ERROR: Number of Inputs must be " + numInputs);
            return outputValues;
        }

        inputs = new List<double>(inputValues);
        for (int i = 0; i < numHidden + 1; i++)
        {
            if (i > 0)
            {
                inputs = new List<double>(outputValues);
            }
            outputValues.Clear();

            for (int j = 0; j < layers[i].numNeurons; j++)
            {
                double N = 0;
                layers[i].neurons[j].inputs.Clear();

                for (int k = 0; k < layers[i].neurons[j].numInputs; k++)
                {
                    layers[i].neurons[j].inputs.Add(inputs[currentInput]);
                    N += layers[i].neurons[j].weights[k] * inputs[currentInput];
                    currentInput++;
                }

                N -= layers[i].neurons[j].bias;

                if (i == numHidden)
                    layers[i].neurons[j].output = ActivationFunctionO(N);
                else
                    layers[i].neurons[j].output = ActivationFunction(N);

                outputValues.Add(layers[i].neurons[j].output);
                currentInput = 0;
            }
        }
        return outputValues;
    }

    public int getLen()
    {
        int len = 0;
        foreach (Layer l in layers)
        {
            foreach (Neuron n in l.neurons)
            {
                foreach (double w in n.weights)
                {
                    len++;
                }
                len++;
            }
        }
        return len;
    }

    public List<double> getChromosome()
    {
        List<double> c = new List<double>();
        foreach (Layer l in layers)
        {
            foreach (Neuron n in l.neurons)
            {
                foreach (double w in n.weights)
                {
                    c.Add(w);
                }
                c.Add(n.bias);
            }
        }
        return c;
    }

    public void setChromosome(List<double> genes)
    {
        int x = 0;
        foreach (Layer l in layers)
        {
            foreach (Neuron n in l.neurons)
            {
                for (int i = 0; i < n.weights.Count; i++)
                {
                    n.weights[i] = genes[x];
                    x++;
                }
                n.bias = genes[x];
                x++;
            }
        }
    }

    public string PrintWeights()
    {
        string weightStr = "";
        foreach (Layer l in layers)
        {
            foreach (Neuron n in l.neurons)
            {
                foreach (double w in n.weights)
                {
                    weightStr += w + ",";
                }
                weightStr += n.bias + ",";
            }
        }
        return weightStr;
    }

    public void LoadWeights(string weightStr)
    {
        if (weightStr == "") return;
        string[] weightValues = weightStr.Split(',');
        int w = 0;
        foreach (Layer l in layers)
        {
            foreach (Neuron n in l.neurons)
            {
                for (int i = 0; i < n.weights.Count; i++)
                {
                    n.weights[i] = System.Convert.ToDouble(weightValues[w]);
                    w++;
                }
                n.bias = System.Convert.ToDouble(weightValues[w]);
                w++;
            }
        }
    }

    double ActivationFunction(double value)
    {
        return TanH(value);
    }

    double ActivationFunctionO(double value)
    {
        return TanH(value);
    }

    double TanH(double value)
    {
        double k = (double)System.Math.Exp(-2 * value);
        return 2 / (1.0f + k) - 1;
    }
}