using System;
using System.Collections.Generic;
using ANN_Kart.Genetics;
using ANN_Kart.Genetics.Genetics;
using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    // References:
    [SerializeField] private PopulationData populationData;
    
    // Variables:
    private int currentGeneration;
    private int carsSpawnedInCurrentPopulation;
    private IterationState currentStateOfIteration;
    private List<CarPerformanceData> currentPopulation;
    private List<CarController> currentIteration;
    private List<List<CarPerformanceData>> recordedPopulationData;

    void Start()
    {
        InitializeVariables();
        SpawnNextIteration();
    }

    private void InitializeVariables()
    {
        currentGeneration = 1;
        carsSpawnedInCurrentPopulation = 0;
        currentPopulation = new List<CarPerformanceData>();
        recordedPopulationData = new List<List<CarPerformanceData>>();
        currentIteration = new List<CarController>();
        currentStateOfIteration = IterationState.IN_PROGRESS;
    }

    private void Update()
    {
        if(CurrentPopulationIsActive())
            ProcessCurrentPopulation();
        else
        {
            ResetCurrentPopulation();
            CheckForEndOfSimulation();
        }
    }
    
    private bool CurrentPopulationIsActive()
    {
        if (carsSpawnedInCurrentPopulation == populationData.PopulationSize)
            return CurrentIterationIsActive();
        return true;
    } 

    private void ProcessCurrentPopulation()
    {
        if(CurrentIterationIsActive())
            UpdateAllActiveCars();
        else
            SpawnNextIteration();
    }

    private bool CurrentIterationIsActive()
    {
        if (currentIteration.Count == 0)
            return false;
        return !currentIteration.TrueForAll(car => car.CurrentState == CarState.DEAD);  
    } 
    
    private void UpdateAllActiveCars()
    {
        foreach (CarController car in currentIteration)
        {
            if(car.CurrentState == CarState.ALIVE)
                car.UpdateCar();
        }
    }
    private void SpawnNextIteration()
    {
        ResetPreviousIteration();
        for (int i = 0; i < populationData.IterationSize; i++)
        {
            CarController newCar = Instantiate(populationData.carPrefab, populationData.startingPosition, transform.rotation);
            currentIteration.Add(newCar);
            carsSpawnedInCurrentPopulation++;

            // If it's not the first generation then the weights and biases (chromosome) must not be random but derived from previous generation.
            if (currentGeneration != 0)
            {
                // TODO: Recorded Chromosome need to be used here.
                // TODO: The Recorded data will be received through crossover and mutation algorithms somehow.
                // newCar.SetChromosome();
            }
        }
    }

    private void ResetPreviousIteration()
    {
        if (currentIteration.Count == 0)
            return;
        
        foreach (CarController car in currentIteration)
        {
            currentPopulation.Add(GetPerformanceDataForCar(car));
            Destroy(car.gameObject);
        }
        currentIteration.Clear();
    }

    private CarPerformanceData GetPerformanceDataForCar(CarController car)
    {
        CarPerformanceData newPerformanceData;
        newPerformanceData.FitnessValue = car.OverallFitness;
        newPerformanceData.Chromosome = car.GetChromosomeFromANN();
        return newPerformanceData;
    }

    private void ResetCurrentPopulation()
    {
        ResetPreviousIteration();
        currentGeneration++;
        carsSpawnedInCurrentPopulation = 0;
        recordedPopulationData.Add(new List<CarPerformanceData>(currentPopulation));
        currentPopulation.Clear();
    }

    private void CheckForEndOfSimulation()
    {
        if (currentGeneration <= 25)
            return;
        // Save Data on disk from recordedPopulationData.
        // Application.Quit();
    }
    
}
