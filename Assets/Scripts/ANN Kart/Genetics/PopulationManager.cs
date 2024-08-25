using System;
using System.Collections.Generic;
using System.Linq;
using ANN_Kart.Genetics;
using ANN_Kart.Genetics.Crossover;
using ANN_Kart.Genetics.Genetics;
using ANN_Kart.Genetics.Genetics.Parent_Selection;
using ANN_Kart.Genetics.Utility;
using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    // References:
    [SerializeField] private PopulationData populationData;
    
    // Variables:
    private int currentGeneration;
    private int carsSpawnedInCurrentPopulation;
    private int derivedCarDataIndex;
    private IterationState currentStateOfIteration;
    private List<CarPerformanceData> currentPopulation;
    private List<CarController> currentIteration;
    private List<List<CarPerformanceData>> recordedPopulationData;
    private List<Chromosome> newGenerationData;

    void Start()
    {
        InitializeVariables();
        SpawnNextIteration();
    }

    private void InitializeVariables()
    {
        currentGeneration = 1;
        carsSpawnedInCurrentPopulation = 0;
        derivedCarDataIndex = 0;
        currentPopulation = new List<CarPerformanceData>();
        recordedPopulationData = new List<List<CarPerformanceData>>();
        currentIteration = new List<CarController>();
        currentStateOfIteration = IterationState.IN_PROGRESS;
        newGenerationData = new List<Chromosome>();
    }

    private void Update()
    {
        if(CurrentPopulationIsActive())
            ProcessCurrentPopulation();
        else
        {
            ResetPreviousIteration();
            BreedNewPopulation();
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
            if (currentGeneration != 1)
            {
                newCar.SetChromosome(newGenerationData[derivedCarDataIndex].Genes);
                derivedCarDataIndex++;
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
        currentGeneration++;
        carsSpawnedInCurrentPopulation = 0;
        recordedPopulationData.Add(new List<CarPerformanceData>(currentPopulation));
        currentPopulation.Clear();
        derivedCarDataIndex = 0;
    }

    private void CheckForEndOfSimulation()
    {
        if (currentGeneration <= 25)
            return;
        // Save Data on disk from recordedPopulationData.
        // Application.Quit();
    }
    
    // TODO: should create a list of chromosomes from the current population and store them in a list to be used for initializing next generation.
    private void BreedNewPopulation()
    {
        newGenerationData.Clear();
        List<CarPerformanceData> parentPool = new List<CarPerformanceData>();
        List<CarPerformanceData> sortedPopulationData = currentPopulation.OrderByDescending(data => data.FitnessValue).ToList();
        
        // Select top 10 cars for the next population (without any changes) and add them to the new generation data.
        for (int i = 0; i < 10; i++)
            newGenerationData.Add(sortedPopulationData[i].Chromosome);
        
        // Use Tournament Selection algorithm to select 20 parents from the current population.
        for (int i = 0; i < 20; i++)
        {
            ISelection tournamentSelection = new TournamentSelection();
            CarPerformanceData selectedParent = tournamentSelection.SelectParents(sortedPopulationData);
            parentPool.Add(selectedParent);
        }
        
        // Use crossover algorithm to generate 40 unique chromosomes and add them to the newGenerationData.
        for (int i = 0; i < 40; i += 2)
        {
            CarPerformanceData parent1 = parentPool[i % parentPool.Count];
            CarPerformanceData parent2 = parentPool[(i + 1) % parentPool.Count];

            // Perform crossover
            ICrossover uniformCorssover = new UniformCrossover();
            List<Chromosome> offspring = uniformCorssover.Crossover(parent1.Chromosome, parent2.Chromosome);

            // Apply mutation to the offspring
            offspring[0] = Mutation.Mutate(offspring[0]);
            offspring[1] = Mutation.Mutate(offspring[1]);

            // Add the offspring to the new generation
            newGenerationData.Add(offspring[0]);
            newGenerationData.Add(offspring[1]);
        }
    }
}
