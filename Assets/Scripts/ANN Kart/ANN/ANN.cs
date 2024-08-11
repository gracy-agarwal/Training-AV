// TODO: Recreate ANN

using System;
using System.Collections;
using System.Collections.Generic;
using ANN_Kart.ANN;
using UnityEngine;

namespace ANN_Kart.ANN
{
    public class ANN
    {
        private ANN_Data annData;
        private List<Layer> layers;
        public List<Layer> Layers => layers;
        
        public ANN(ANN_Data annData)
        {
            this.annData = annData;
            CreateLayers();
            InitializeNeuronsInLayers();
        }

        private void CreateLayers()
        {
            layers = new List<Layer>();

            for (int i = 0; i < annData.NumberOfLayers; i++)
            {
                Layer newLayer = new Layer(annData.GetLayerType(i), annData.GetNumberOfNeuronAtLayer(i));
                layers.Add(newLayer);
            }
        }

        private void InitializeNeuronsInLayers()
        {
            for (int i = 0; i < layers.Count; i++)
                layers[i].InitializeNeurons(annData.GetNumberOfInputsPerNeuronAtLayer(i));
        }

        public List<float> CalculateOutput(List<float> inputValues)
        {
            if (!IsInputCountValid(inputValues))
            {
                // Debug.LogError("Input Count Mismatch");
                return null;
            }

            List<float> outputValues = new List<float>();

            foreach (Layer layer in layers)
            {
                outputValues = layer.CalculateOutput(inputValues);
                inputValues = outputValues;
            }

            return outputValues;
        }

        private bool IsInputCountValid(List<float> inputValues) => inputValues.Count == annData.GetNumberOfNeuronAtLayer(0);

        public List<float> GetWeightsAnsBiases()
        {
            List<float> weightsAndBiases = new List<float>();
            
            foreach (Layer layer in layers)
            {
                foreach (Neuron neuron in layer.Neurons)
                {
                    foreach (float weight in neuron.Weights)
                        weightsAndBiases.Add(weight);
                    weightsAndBiases.Add(neuron.Bias);
                }
            }
            return weightsAndBiases;
        }

        public void SetWeightsAndBiases(List<float> weightsAndBiasesToSet)
        {
            int currentIndex = 0;
            
            foreach (Layer layer in layers)
            {
                foreach (Neuron neuron in layer.Neurons)
                {
                    neuron.Weights.Clear();
                    for (int i = 0; i < neuron.NumberOfInputs; i++)
                    {
                        neuron.Weights.Add(weightsAndBiasesToSet[currentIndex]);
                        currentIndex++;
                    }
                    neuron.SetBias(weightsAndBiasesToSet[currentIndex]);
                    currentIndex++;
                }
            }
            
        }
    }
}