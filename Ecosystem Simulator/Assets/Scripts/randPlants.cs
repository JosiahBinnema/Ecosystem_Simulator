using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randPlants : MonoBehaviour
{

    public GameObject plant;

    public float minX = 0;
    public float maxX = 0;
    public float minZ = 0;
    public float maxZ = 0;
    public float rate = 0;

    void Start()
    {

        //should start generating after 1st num and start again every 2nd num
        InvokeRepeating("genPlant", 0f, rate);
    }

    void genPlant()
    {

   //     Debug.Log("spawning");
        Vector3 plantPosition = new Vector3(Random.Range(minX, maxX), -1f, Random.Range(minZ, maxZ));
        GameObject newPlant = Instantiate(plant, plantPosition, Quaternion.identity);
        newPlant.name = newPlant.name.Replace("(Clone)", "");       //added code to remove the (clone) from the name

    }

}
