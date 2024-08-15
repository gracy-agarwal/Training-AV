using System;
using System.Collections.Generic;
using ANN_Kart.ANN;
using ANN_Kart.ANN.Activation_Functions;
using ANN_Kart.Genetics.Genetics;
using DefaultNamespace;
using UnityEngine;

namespace ANN_Kart.Genetics
{
    public class CarController : MonoBehaviour
    {
        // References:
        [SerializeField] private CarData carData;
        [SerializeField] private ANN_Data annData;
        private Chromosome chromosome;
        private ANN.ANN ann;
        
        // Variables:
        private CarState currentState;
        private float distanceTravelled;
        private float lifetime;
        private Vector3 previousPosition;
        private float overallFitness;

        public CarState CurrentState => currentState;
        public float OverallFitness => overallFitness;

        private void Awake()
        {
            ann = new ANN.ANN(annData);
            previousPosition = transform.position;
            currentState = CarState.ALIVE;
            // Weights are being randomized until now.
            // For the first population it is fine.
            // For the next Population the chromosome will need to be injected while initializing the CarController.
        }

        public void UpdateCar()
        {
            List<float> inputs = GetSensorInputs();
            List<float> outputs = ann.CalculateOutput(inputs);

            ApplyMovement(outputs);
        }

        private List<float> GetSensorInputs()
        {
            List<float> inputs = new List<float>();
            inputs.Add(RaycastInDirection(transform.forward));
            inputs.Add(RaycastInDirection(transform.right));
            inputs.Add(RaycastInDirection(-transform.right));
            inputs.Add(RaycastInDirection(Quaternion.AngleAxis(-45, Vector3.up) * transform.right));
            inputs.Add(RaycastInDirection(Quaternion.AngleAxis(45, Vector3.up) * -transform.right));
            return inputs;
        }

        private float RaycastInDirection(Vector3 directionToRaycastIn)
        {
            RaycastHit hit;
            float hitProximity = 0;

            if (Physics.Raycast(transform.position, directionToRaycastIn, out hit, carData.visibleDistance))
            {
                hitProximity = 1 - Round(hit.distance / carData.visibleDistance);
                Debug.DrawRay(transform.position, directionToRaycastIn * carData.visibleDistance, Color.red);
            }
            else
                Debug.DrawRay(transform.position, directionToRaycastIn * carData.visibleDistance, Color.green);

            return hitProximity;
        }
        
        /// <summary>
        /// This function rounds a floating-point number x to the nearest 0.5 increment.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        float Round(float x) => (float)System.Math.Round(x * 2, MidpointRounding.AwayFromZero) / 2.0f;

        private void ApplyMovement(List<float> outputs)
        {
            IActivationFunction activationFunction = new SigmoidFunction();
            float translationInput = activationFunction.CalculateOutput(outputs[0]);
            activationFunction = new TangentFunction();
            float rotationInput = activationFunction.CalculateOutput(outputs[1]);

            float translation = translationInput * carData.TranslationSpeed * Time.deltaTime;
            float rotation = rotationInput * carData.RotationSpeed * Time.deltaTime;

            transform.Translate(0, 0, translation);
            transform.Rotate(0, rotation, 0);

            UpdateFitnessFactors();
        }

        private void UpdateFitnessFactors()
        {
            lifetime += Time.deltaTime;
            distanceTravelled += (transform.position - previousPosition).magnitude;
            overallFitness = FitnessCalculator.CalculateFitness(lifetime, distanceTravelled);
        }

        public Chromosome GetChromosome()
        {
            List<float> genes = ann.GetWeightsAnsBiases();
            Chromosome chromosome = new Chromosome(genes.Count);
            for (int i = 0; i < genes.Count; i++)
                chromosome.SetGeneAtPosition(i, genes[i]);
            return chromosome;
        }

        public void SetChromosome(Chromosome chromosome) => ann.SetWeightsAndBiases(chromosome.Genes);
        
        void OnCollisionEnter(Collision collidedObject)
        {
            Debug.Log("Collided with some object", collidedObject.gameObject);
            if (collidedObject.transform.GetComponent<Wall>())
            {
                Debug.Log("Collided With Wall");
                currentState = CarState.DEAD;
            }
        }
    }
}