namespace ANN_Kart.ANN
{
    public static class WeightManager
    {
        public static string CaptureWeights(ANN ann)
        {
            string recordedWeights = "";
            
            foreach (Layer layer in ann.Layers)
            {
                foreach (Neuron neuron in layer.Neurons)
                {
                    foreach (double weight in neuron.Weights)
                    {
                        recordedWeights += weight + ",";
                    }

                    recordedWeights += neuron.Bias + ",";
                }
            }
            return recordedWeights;
        }

        public static void LoadWeights(ANN ann, string weightString)
        {
            if (weightString == "") 
                return;
            
            string[] weightValues = weightString.Split(',');
            int weightStringIndex = 0;

            foreach (Layer layer in ann.Layers)
            {
                foreach (Neuron neuron in layer.Neurons)
                {
                    for (int i = 0; i < neuron.Weights.Count; i++)
                    {
                        float weightToSet = (float)System.Convert.ToDouble(weightValues[weightStringIndex]);
                        neuron.SetWeightAtIndex(i, weightToSet);
                        weightStringIndex++;
                    }
                    neuron.SetBias((float)System.Convert.ToDouble(weightValues[weightStringIndex]));
                }
            }
        }
    }

}