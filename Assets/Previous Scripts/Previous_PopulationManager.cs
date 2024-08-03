using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class Previous_PopulationManager : MonoBehaviour {

	public GameObject kartPrefab;
    public GameObject startingPos;
	public int populationSize = 50;
	List<GameObject> population = new List<GameObject>();
    public float elapsed = 0;
    public float trialTime = 10;
	public static int generation = 1;
    float CurrentBestFitnessValue = 0f;
    float PreviousBestFitnessValue = 0f;
    int z = 0;
    public int currentUnitNumber;
    public List<float> Fitness = new List<float>();
    public List<float> BestFitnessList = new List<float>();
    public List<float> MeanFitnessList = new List<float>();

    GUIStyle guiStyle = new GUIStyle();
	void OnGUI()
	{
		guiStyle.fontSize = 25;
		guiStyle.normal.textColor = Color.white;
		GUI.BeginGroup (new Rect (10, 10, 250, 150));
		GUI.Box (new Rect (0,0,140,140), "Stats", guiStyle);
		GUI.Label(new Rect (10,25,200,30), "Gen: " + generation, guiStyle);
		GUI.Label(new Rect (10,50,200,30), string.Format("Time: {0:0.00}",elapsed), guiStyle);
		GUI.Label(new Rect (10,75,200,30), "Population Size: " + population.Count, guiStyle);
        GUI.Label(new Rect(10, 100, 200, 30), "CBM Value: " + CurrentBestFitnessValue, guiStyle);
        GUI.EndGroup ();
	}
    

	// Use this for initialization
	void Start () {
        /*	for(int i = 0; i < populationSize; i++)
            {
                GameObject k = Instantiate(kartPrefab, startingPos.transform.position, this.transform.rotation);
                k.GetComponent<Brain>().Init();
                population.Add(k);
            }

            sortedPopulation = population;
          */

        currentUnitNumber = 0;

        for(int i=0; i<5; i++)
        {
            GameObject k = Instantiate(kartPrefab, startingPos.transform.position, this.transform.rotation);
            k.GetComponent<Previous_Brain>().Init();
            k.GetComponent<Previous_Brain>().alive = true;
            population.Add(k);
        }


    }

	List<GameObject> Breed(GameObject parent1, GameObject parent2)
	{
		GameObject offspring1 = Instantiate(kartPrefab, startingPos.transform.position, this.transform.rotation);
        GameObject offspring2 = Instantiate(kartPrefab, startingPos.transform.position, this.transform.rotation);
        Previous_Brain b1 = offspring1.GetComponent<Previous_Brain>();
        Previous_Brain b2 = offspring2.GetComponent<Previous_Brain>();
        if (Random.Range(0,100) < 10) //mutate 10 in 100
		{
			b1.Init();
			b1.chromosome.Mutate();
            b1.ann.setChromosome(b1.chromosome.genes);
            b2.Init();
            b2.chromosome.Mutate();
            b2.ann.setChromosome(b2.chromosome.genes);
        }
		else
		{ 
			b1.Init();
            b2.Init();
			b1.chromosome.UniformCrossover(parent1.GetComponent<Previous_Brain>().chromosome,parent2.GetComponent<Previous_Brain>().chromosome, b1, b2);
            b1.ann.setChromosome(b1.chromosome.genes);
            b2.ann.setChromosome(b2.chromosome.genes);
        }

        List<GameObject> offsprings = new List<GameObject>();
        offsprings.Add(offspring1);
        offsprings.Add(offspring2);

        return offsprings;
	}

	void BreedNewPopulation()
	{
		List<GameObject> sortedList = population.OrderBy(o => (o.GetComponent<Previous_Brain>().overallFitness)).ToList();
        List<GameObject> parentpool = new List<GameObject>();

        population.Clear();

        // select the top 40% of the population in parent pool for breeding
        if(generation < 6)
        {
            for (int i = 30; i < 50; i++)
            {
                parentpool.Add(sortedList[i]);
            }
        } else if(generation < 11)
        {
            for (int i = 25; i < 45; i++)
            {
                parentpool.Add(sortedList[i]);
            }
        } else
        {
            for (int i = 20; i < 40; i++)
            {
                parentpool.Add(sortedList[i]);
            }
        }
		

        // select the best 20%
        for(int i = 40; i < sortedList.Count; i++)
        {
            GameObject selectedOffspring = Selection(sortedList[i]); 
            population.Add(selectedOffspring);
        }
        
        // add new offsprings
        for(int i=0; i<parentpool.Count; i++)
        {

            GameObject parent1 = TournamentSelection(parentpool);
            GameObject parent2 = TournamentSelection(parentpool);
/*
            while(parent1.GetComponent<Brain>().overallFitness == parent2.GetComponent<Brain>().overallFitness)
            {
                parent2 = TournamentSelection(parentpool);
            }
*/
            List<GameObject> offsprings = new List<GameObject>();
            offsprings = Breed(parent1, parent2);
            population.Add(offsprings[0]);
            population.Add(offsprings[1]);

        }
      
        //destroy all parents and previous population
        for (int i = 0; i < sortedList.Count; i++)
		{
			Destroy(sortedList[i]);
		}

        for (int i = 0; i < parentpool.Count; i++)
        {
            Destroy(parentpool[i]);
        }

        generation++;
	}
	
	// Update is called once per frame
	void Update () {

        if(generation == 6)
        {
            trialTime = 30;
        }

        if(generation == 16)
        {
            trialTime = 60;
        }

		elapsed += Time.deltaTime;

        /* z++;

        if(z % 20 == 0)
        {
            
            z = 0;
        } 
        */

        // Debug.Log(population.Count);

    //    sortedPopulation = population.OrderBy(o => (o.GetComponent<Brain>().overallFitness)).ToList();
    //    CurrentBestFitnessValue = sortedPopulation[sortedPopulation.Count - 1].GetComponent<Brain>().overallFitness;


        if(generation == 26)
        {
            SaveFitnessToFile();
            Application.Quit();
        }


        if(generation == 1)
        {

            if (currentUnitNumber < populationSize - 5) 
            {

                if (elapsed > trialTime)
                {
                    for(int i=currentUnitNumber; i<currentUnitNumber+5; i++)
                    {
                        population[i].GetComponent<Previous_Brain>().alive = false;
                    }
                    
                    currentUnitNumber = currentUnitNumber + 5;

                    for (int i = 0; i < 5; i++)
                    {
                        GameObject k = Instantiate(kartPrefab, startingPos.transform.position, this.transform.rotation);
                        k.GetComponent<Previous_Brain>().Init();
                        k.GetComponent<Previous_Brain>().alive = true;
                        population.Add(k);
                    }

                    elapsed = 0;
                }
                else if (ifDead())
                {
                    currentUnitNumber = currentUnitNumber + 5;

                    for (int i = 0; i < 5; i++)
                    {
                        GameObject k = Instantiate(kartPrefab, startingPos.transform.position, this.transform.rotation);
                        k.GetComponent<Previous_Brain>().Init();
                        k.GetComponent<Previous_Brain>().alive = true;
                        population.Add(k);
                    }

                    elapsed = 0;
                }

            } else
            {

                if (elapsed > trialTime)
                {
                    StoreFitness();
                    BreedNewPopulation();
                    currentUnitNumber = 0;
                    elapsed = 0;
                    for(int i=0; i<5; i++)
                    {
                        population[i].GetComponent<Previous_Brain>().alive = true;
                    }
                }
                else if (ifDead())
                {
                    StoreFitness();
                    BreedNewPopulation();
                    currentUnitNumber = 0;
                    elapsed = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        population[i].GetComponent<Previous_Brain>().alive = true;
                    }
                }

            }

        } else
        {
            if(currentUnitNumber < population.Count - 5)
            {

                if (elapsed > trialTime)
                {
                    for (int i = currentUnitNumber; i < currentUnitNumber + 5; i++)
                    {
                        population[i].GetComponent<Previous_Brain>().alive = false;
                    }

                    currentUnitNumber = currentUnitNumber + 5;

                    for (int i = currentUnitNumber; i < currentUnitNumber + 5; i++)
                    {
                        population[i].GetComponent<Previous_Brain>().alive = true;
                    }

                    elapsed = 0;
                }
                else if (ifDead())
                {
                    currentUnitNumber = currentUnitNumber + 5;

                    for (int i = currentUnitNumber; i < currentUnitNumber + 5; i++)
                    {
                        population[i].GetComponent<Previous_Brain>().alive = true;
                    }

                    elapsed = 0;
                }

            } else
            {

                if (elapsed > trialTime)
                {
                    StoreFitness();
                    BreedNewPopulation();
                    currentUnitNumber = 0;
                    elapsed = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        population[i].GetComponent<Previous_Brain>().alive = true;
                    }
                }
                else if (ifDead())
                {
                    StoreFitness();
                    BreedNewPopulation();
                    currentUnitNumber = 0;
                    elapsed = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        population[i].GetComponent<Previous_Brain>().alive = true;
                    }
                }

            }

        }



/*
        if (elapsed > trialTime)
        {
            BreedNewPopulation();
            elapsed = 0;
        } else

		if(!ifAlive())
		{
			BreedNewPopulation();
            elapsed = 0;
		}
*/

	}


    void SaveFitnessToFile()
    {
        string path = Application.dataPath + "/WAC.txt";
        StreamWriter wf = File.CreateText(path);
        for(int i=0; i<generation - 1; i++)
        {
            wf.WriteLine(i + 1 + "," + BestFitnessList[i] + "," + MeanFitnessList[i]);
        }
        wf.Close();
    }


    public void StoreFitness()
    {
        Fitness.Clear();

        float sum = 0;

        for(int i=0; i<populationSize; i++)
        {
            Fitness.Add(population[i].GetComponent<Previous_Brain>().overallFitness);
            sum = sum + population[i].GetComponent<Previous_Brain>().overallFitness;
        }

        Fitness.Sort();

        CurrentBestFitnessValue = Fitness[49];

        BestFitnessList.Add(CurrentBestFitnessValue);

        MeanFitnessList.Add((sum) / (float)populationSize);

    }


    public bool ifDead()
    {
        for(int i = currentUnitNumber; i < currentUnitNumber + 5; i++)
        {
            if(population[i].GetComponent<Previous_Brain>().alive == true)
            {
                return false;
            }
        }

        return true;

    }

    public bool ifAlive()
    {
        for(int i=0; i<populationSize; i++)
        {
            GameObject p = population[i];
            bool a = p.GetComponent<Previous_Brain>().alive;
            if(a)
            {
                return true;
            }
        }
        return false;
    }

    public GameObject FitnessProportionateSelection(List<GameObject> parentpool)
    {

        float sumOfFitness = 0;
        for(int i=0; i<parentpool.Count; i++)
        {
            sumOfFitness = sumOfFitness + parentpool[i].GetComponent<Previous_Brain>().overallFitness;
        }

        List<float> cdf = new List<float>();
        float previousProbability = 0;

        for(int i=0; i<parentpool.Count; i++)
        {
            cdf.Add(previousProbability + (parentpool[i].GetComponent<Previous_Brain>().overallFitness/sumOfFitness));
            previousProbability = cdf[i];
        }

        float r = Random.Range(0f, 0.99999f);
        int pos = parentpool.Count - 1;

        for(int i=0; i<cdf.Count; i++)
        {
            if(r <cdf[i])
            {
                pos = i;
                break;
            }
        }

        return parentpool[pos];

    }

    public GameObject TournamentSelection(List<GameObject> parentpool)
    {
        int r1 = Random.Range(0, parentpool.Count);
        int r2 = Random.Range(0, parentpool.Count);
        int r3 = Random.Range(0, parentpool.Count);
        int r4 = Random.Range(0, parentpool.Count);

        List<GameObject> selected = new List<GameObject>();

        selected.Add(parentpool[r1]);
        selected.Add(parentpool[r2]);
        selected.Add(parentpool[r3]);
        selected.Add(parentpool[r4]);

        selected = selected.OrderBy(o => o.GetComponent<Previous_Brain>().overallFitness).ToList();

        return selected[selected.Count - 1];

        /*
                if(parentpool[r1].GetComponent<Brain>().overallFitness > parentpool[r2].GetComponent<Brain>().overallFitness)
                {
                    if(parentpool[r1].GetComponent<Brain>().overallFitness > parentpool[r3].GetComponent<Brain>().overallFitness)
                    {
                        if(parentpool[r1].GetComponent<Brain>().overallFitness > parentpool[r4].GetComponent<Brain>().overallFitness)
                        {
                            return parentpool[r1];
                        } else
                        {
                            return parentpool[r4];
                        }
                    } else
                    {
                        if (parentpool[r3].GetComponent<Brain>().overallFitness > parentpool[r4].GetComponent<Brain>().overallFitness)
                        {
                            return parentpool[r3];
                        }
                        else
                        {
                            return parentpool[r4];
                        }
                    }
                } else
                {
                    if (parentpool[r2].GetComponent<Brain>().overallFitness > parentpool[r3].GetComponent<Brain>().overallFitness)
                    {
                        if (parentpool[r2].GetComponent<Brain>().overallFitness > parentpool[r4].GetComponent<Brain>().overallFitness)
                        {
                            return parentpool[r2];
                        }
                        else
                        {
                            return parentpool[r4];
                        }
                    }
                    else
                    {
                        if (parentpool[r3].GetComponent<Brain>().overallFitness > parentpool[r4].GetComponent<Brain>().overallFitness)
                        {
                            return parentpool[r3];
                        }
                        else
                        {
                            return parentpool[r4];
                        }
                    }
                }
                */
    }
    
    public GameObject Selection(GameObject parent)
    {
        GameObject offspring = Instantiate(kartPrefab, startingPos.transform.position, this.transform.rotation);
        Previous_Brain b = offspring.GetComponent<Previous_Brain>();
        b.Init();
        b.chromosome.CopyGenes(parent);

        if(b.chromosomeLength != b.chromosome.genes.Count)
        {
            Debug.Log("Oh Shit");
        }

        b.ann.setChromosome(b.chromosome.genes);

        return offspring;

    }

}
