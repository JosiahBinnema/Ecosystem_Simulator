using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class dataCollection : MonoBehaviour
{
    float nextAction = 1.0f;
    float nextTime = 60.0f;

    List<stats> deerStats = new List<stats>();              //Holds all the deer stats 
    List<stats> cougarStats = new List<stats>();            //Holds all the cougar stats
    List<int> deerPopulation = new List<int>();
    List<int> cougarPopulation = new List<int>();

    bool end = false;
    bool neverRunAgain = false; 

    IEnumerator Data(float time)
    {

        List<float> deerDesirability = new List<float>();            //Holds all of a type of a deer trait to calculate its average
        List<float> deerVision = new List<float>();
        List<float> deerSpeed = new List<float>();

        List<float> cougarDesirability = new List<float>();            //Holds all of a type of a cougar trait to calculate its avergae
        List<float> cougarVision = new List<float>();
        List<float> cougarSpeed = new List<float>();

        //This geos through all of the objects and if they are of the right type, then they should get added appropriately
        Object[] allObjects = FindObjectsOfType(typeof(GameObject));

        foreach (object o in allObjects)
        {
            GameObject g = (GameObject)o;
            if (g.name == "Deer")
            {
                deerDesirability.Add(g.GetComponent<animalStats>().desireablility);
                deerVision.Add(g.GetComponent<animalStats>().visionRange);
                deerSpeed.Add(g.GetComponent<animalStats>().speed);
            }
            else if (g.name == "Cougar")
            {
                cougarDesirability.Add(g.GetComponent<animalStats>().desireablility);
                cougarVision.Add(g.GetComponent<animalStats>().visionRange);
                cougarSpeed.Add(g.GetComponent<animalStats>().speed);
            }

            
        }

        int deerPop = deerDesirability.Count;                    //Is the population of the deer
        int cougarPop = cougarDesirability.Count;                //Is the population of the cougar 


        if (deerPop == 0 || cougarPop == 0)
        {
            end = true;
            
        }


        if (!end)
        {
            stats newDeerInput = new stats();                   //Creates a new variable that holds all of the averages of the deer for this time
            stats newCougarInput = new stats();                 //Creates a new variable that holds all of the averages of the cougar for this time

            newDeerInput.desireablility = getAverage(deerDesirability);      //Finds the averages for the traits of the deer
            newDeerInput.visionRange = getAverage(deerVision);
            newDeerInput.speed = getAverage(deerSpeed);
            deerPopulation.Add(deerPop);
            deerStats.Add(newDeerInput);                    //Saves the values that were found
            
            newCougarInput.desireablility = getAverage(cougarDesirability);  //Finds the averages for the traits of the cougar
            newCougarInput.visionRange = getAverage(cougarVision);
            newCougarInput.speed = getAverage(cougarSpeed);
            cougarPopulation.Add(cougarPop);
            cougarStats.Add(newCougarInput);            //Saves the values that were found
        
        }

        yield return new WaitForSeconds(time);
    }

    //Calculates an individual average for a trait 
    float getAverage(List<float> stats)
    {
        int i = 0;
        float total = 0;
        while (i < stats.Count-1)
        {
            total = total + stats[i];
            i++;
        }

        return (total / stats.Count);
    }

    void printResults()
    {
        //Here is where the output thing would be called for now just do a for loop of the list and variables
        for (int i = 0; i < deerStats.Count; i++)
        {
            Debug.Log("Deer Desirability: " + deerStats[i].desireablility + " Deer Vision: " + deerStats[i].visionRange + " Deer Speed:" + deerStats[i].speed + " Deer Population: " + deerPopulation[i]);
        }
        
        for (int i = 0; i < cougarStats.Count; i++)
        {
           Debug.Log("Cougar Desirability: " + cougarStats[i].desireablility + " Cougar Vision: " + cougarStats[i].visionRange + " Cougar Speed:" + cougarStats[i].speed + " Cougar Population: " + cougarPopulation[i]);
        }
    }

    //https://support.unity.com/hc/en-us/articles/115000341143-How-do-I-read-and-write-data-from-a-text-file-
    //The interesting code below, came from above 

    public void WriteToFile()
    {
        string path = Application.persistentDataPath + "/data.txt"; //edit this path to redirect the simulation output.

        //IF YOU CANNOT FIND THE FILE: UNCOMMENT THE LINE BELOW 
        Debug.Log(Application.persistentDataPath);

        StreamWriter writer = new StreamWriter(path, true);

        writer.WriteLine("Deer Desirability");

        for (int i = 0; i < deerStats.Count; i++)
        {
            writer.WriteLine(deerStats[i].desireablility);
        }

        writer.WriteLine("Deer Vision");

        for (int i = 0; i < deerStats.Count; i++)
        {
            writer.WriteLine(deerStats[i].visionRange);
        }

        writer.WriteLine("Deer Speed");

        for (int i = 0; i < deerStats.Count; i++)
        {
            writer.WriteLine(deerStats[i].speed);
        }

        writer.WriteLine("Deer Population");
        for(int i  = 0; i < deerStats.Count; i++)
        {
            writer.WriteLine(deerPopulation[i]);
        }

        writer.WriteLine("Cougar Desireability");
        for (int i = 0; i < cougarStats.Count; i++)
        {
            writer.WriteLine(cougarStats[i].desireablility);
        }

        writer.WriteLine("Cougar Vision");
        for (int i = 0; i < cougarStats.Count; i++)
        {
            writer.WriteLine(cougarStats[i].visionRange);
        }

        writer.WriteLine("Cougar Speed");
        for (int i = 0; i < cougarStats.Count; i++)
        {
            writer.WriteLine(cougarStats[i].speed);
        }

        writer.WriteLine("Cougar Population");
        for (int i = 0; i < cougarStats.Count; i++)
        {
            writer.WriteLine(cougarPopulation[i]);
        }

        writer.Close();
    }

    //If button is clicked end = true
    public void OnButtonPress()
    {
        printResults();
        WriteToFile();
        killEverything();
    }

    //Destroys all the animals
    public void killEverything()
    {
        Object[] allObjects = FindObjectsOfType(typeof(GameObject));

        foreach (object o in allObjects)
        {
            GameObject g = (GameObject)o;
            if (g.name == "Deer" || g.name == "Cougar" || g.name == "DeerMeat" || g.name == "CougarMeat")
            {
                Destroy(g);
            }
        }
    }

    //Here is where the code goes to create the graph, now how to create a graph 
    public void graph()
    { 
      //Figure Out how to call another scene from here and how to do the graphing thing
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Escape))
        {
            end = true;
        }

        if (Time.time > nextAction && end == false && neverRunAgain == false)          //To get the information ever nextAction number of seconds and to ensure that the information is only gathered until the end of the simulation 
        {
            nextAction = nextAction + nextTime;
            StartCoroutine(Data(nextTime));

            //Printing frequently to debug 
           // printResults();
        }
        else if(end == true && neverRunAgain == false)
        {
            printResults();
            WriteToFile();
            neverRunAgain = true;
            killEverything();
        }

        
    }


}
