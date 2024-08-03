using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class Previous_Brain : MonoBehaviour {

	public int chromosomeLength = 0;
    public int numberOfInputsOfNN = 5;
    public int numberOfOutputsOfNN = 2;
    public int numberOfHiddenLayers = 2;
    public int numberOfNeuronPerHiddenLayer = 5;
    public double learningRate = 0.5;
    public Previous_ANN ann;
    public Previous_Chromosome chromosome;
    public bool alive;
    public float visibleDistance = 50;
    public float speed = 50.0F;
    public float rotationSpeed = 100.0F;
    public float translation;
    public float rotation;
    public float distanceTravelled = 0;
    public float timeAlive = 0;
    int z = 0;
    Vector3 startPosition;
    public GameObject endPosition;
    public float roadDistance = 0;
    public float averageSpeed = 0;
    public float distanceTravelledMultiplier = 0.8f;
    public float averageSpeedMultiplier = 0.4f;
    public float timeAliveMultiplier = 0.8f;
    public float overallFitness = 0;
    float sumofTranslation = 0;
    public float averageTranslationSpeed = 0;
    float sumofRotation = 0;
    public float averageRotationalSpeed = 0;
    // public int epochs = 1000;
    // bool trainingDone = false;
    // float trainingProgress = 0;
    // double sse = 0;
    // double lastSSE = 1;
    // public bool loadFromFile = true;
    public Previous_PopulationManager pm;

    public void CalChromosomeLength()
    {
        chromosomeLength = ann.getLen();
    }

    public void InitChromosome()
    {
        CalChromosomeLength();
        chromosome = new Previous_Chromosome(chromosomeLength);
        List<double> g = new List<double>();
        g = ann.getChromosome();
        for (int i = 0; i < chromosomeLength; i++)
        {
            chromosome.genes.Add(g[i]);
        }
    }

    public void Init()
	{
        alive = false;
        //initialise ANN
        ann = new Previous_ANN(numberOfInputsOfNN, numberOfOutputsOfNN, numberOfHiddenLayers, numberOfNeuronPerHiddenLayer, learningRate);
        //initialise Chromosome
        InitChromosome();
        startPosition = this.transform.position;
        roadDistance = (endPosition.transform.position - startPosition).magnitude;
	}

    public void InitOffspring()
    {
        alive = true;
        //initialise ANN
        ann = new Previous_ANN(numberOfInputsOfNN, numberOfOutputsOfNN, numberOfHiddenLayers, numberOfNeuronPerHiddenLayer, learningRate);
        CalChromosomeLength();
        chromosome = new Previous_Chromosome(chromosomeLength);
        startPosition = this.transform.position;
        roadDistance = (endPosition.transform.position - startPosition).magnitude;
    }
   

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "wall")
        {
            // Debug.Log("hit");
            // distanceTravelled = 0;
            alive = false;
            this.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    void SaveWeightsToFile()
    {
        string path = Application.dataPath + "/weights.txt";
        StreamWriter wf = File.CreateText(path);
        wf.WriteLine(ann.PrintWeights());
        wf.Close();
    }

    void LoadWeightsFromFile()
    {
        string path = Application.dataPath + "/weights.txt";
        StreamReader wf = File.OpenText(path);

        if (File.Exists(path))
        {
            string line = wf.ReadLine();
            ann.LoadWeights(line);
        }
    }


    void Update()
    {
        if(!alive) return;

        if(Previous_PopulationManager.generation >= 6)
        {
            if(11 < timeAlive && timeAlive < 13)
            {
                if( overallFitness == 0 )
                {
                    alive = false;
                }
            }
        }

        timeAlive = timeAlive + Time.deltaTime;

        
        // distanceTravelled = roadDistance - (endPosition.transform.position - this.transform.position).magnitude;

        distanceTravelled = (this.transform.position - startPosition).magnitude + distanceTravelled;

        startPosition = this.transform.position;

        averageSpeed = distanceTravelled / timeAlive;

        overallFitness = (distanceTravelled * distanceTravelledMultiplier) + (averageSpeed * averageSpeedMultiplier) + (timeAlive * timeAliveMultiplier);
        


        List<double> calcOutputs = new List<double>();
        List<double> inputs = new List<double>();

        //raycasts
        RaycastHit hit;
        float fDist = 0, rDist = 0, lDist = 0, r45Dist = 0, l45Dist = 0;

        //forward
        if (Physics.Raycast(transform.position, this.transform.forward, out hit, visibleDistance))
        {
            fDist = 1 - Round(hit.distance / visibleDistance);
            Debug.DrawLine(transform.position, (this.transform.forward * visibleDistance) + transform.position, color: Color.red);
        } else
        {
            Debug.DrawLine(transform.position, (this.transform.forward * visibleDistance) + transform.position, color: Color.green);
        }

        

        //right
        if (Physics.Raycast(transform.position, this.transform.right, out hit, visibleDistance))
        {
            rDist = 1 - Round(hit.distance / visibleDistance);
            Debug.DrawLine(transform.position, (this.transform.right * visibleDistance) + transform.position, color: Color.red);
        } else
        {
            Debug.DrawRay(transform.position, (this.transform.right * visibleDistance) + transform.position, color: Color.green);
        }

        //left
        if (Physics.Raycast(transform.position, -this.transform.right, out hit, visibleDistance))
        {
            lDist = 1 - Round(hit.distance / visibleDistance);
            Debug.DrawRay(transform.position, (-this.transform.right * visibleDistance) + transform.position, color: Color.red);
        } else
        {
            Debug.DrawRay(transform.position, (-this.transform.right * visibleDistance) + transform.position, color: Color.green);
        }


        //right 45
        if (Physics.Raycast(transform.position,
                            Quaternion.AngleAxis(-45, Vector3.up) * this.transform.right, out hit, visibleDistance))
        {
            r45Dist = 1 - Round(hit.distance / visibleDistance);
            Debug.DrawLine(transform.position, ((this.transform.forward + this.transform.right) * visibleDistance) + transform.position, color: Color.red);
        } else
        {
            Debug.DrawRay(transform.position, ((this.transform.forward + this.transform.right) * visibleDistance) + transform.position, color: Color.green);
        }


        //left 45
        if (Physics.Raycast(transform.position,
                            Quaternion.AngleAxis(45, Vector3.up) * -this.transform.right, out hit, visibleDistance))
        {
            l45Dist = 1 - Round(hit.distance / visibleDistance);
            Debug.DrawRay(transform.position, ((this.transform.forward - this.transform.right) * visibleDistance) + transform.position, color: Color.red);
        } else
        {
            Debug.DrawRay(transform.position, ((this.transform.forward - this.transform.right) * visibleDistance) + transform.position, color: Color.green);

        }

        inputs.Add(fDist);
        inputs.Add(rDist);
        inputs.Add(lDist);
        inputs.Add(r45Dist);
        inputs.Add(l45Dist);

        calcOutputs = ann.CalcOutput(inputs);

        float translationInput = Map(-1, 1, 0, 1, (float)calcOutputs[0]);
        float rotationInput = Map(-1, 1, 0, 1, (float)calcOutputs[1]);

        translation = (translationInput * speed * Time.deltaTime);
        rotation = rotationInput * rotationSpeed * Time.deltaTime;

        this.transform.Translate(0, 0, translation);
        this.transform.Rotate(0, rotation, 0);

        sumofTranslation = sumofTranslation + translation;
        sumofRotation = sumofRotation + rotation;

        z++;

        averageTranslationSpeed = sumofTranslation / z;
        averageRotationalSpeed = sumofRotation / z;

        if(averageTranslationSpeed < 0.2 || Mathf.Abs(averageRotationalSpeed) > 1.4)
        {
            overallFitness = 0;
        }


    }


    float Round(float x)
    {
        return (float)System.Math.Round(x, 2, System.MidpointRounding.AwayFromZero);
    }


    float Map(float newfrom, float newto, float origfrom, float origto, float value)
    {
        if (value <= origfrom)
            return newfrom;
        else if (value >= origto)
            return newto;
        return (newto - newfrom) * ((value - origfrom) / (origto - origfrom)) + newfrom;
    }


    /* void FixedUpdate()
     {
         if(!alive) return;

         // read DNA
         float h = 0;
         float v = dna.GetGene(0);

         if(seeWall)
         { 
             h = dna.GetGene(1);
         }

         this.transform.Translate(0,0,v*0.0001f);
         this.transform.Rotate(0,h,0);
         distanceTravelled = Vector3.Distance(startPosition,this.transform.position);
     }
     */

}
