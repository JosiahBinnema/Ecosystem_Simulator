using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomPlants : MonoBehaviour
{
    public GameObject plant;
    private Collider2D col;
    

    private float minX = 0;
    private float maxX = 0;
    private float minZ = 0;
    private float maxZ = 0;
    private float randZ = 0;
    private float randX = 0;
    public float rate = 5;
    private BoxCollider box;
    public Terrain terrain;

    void Start()
    {
        box = GetComponent<BoxCollider>();
        
    
        //should start generating after 1st num and start again every 2nd num
        InvokeRepeating("genPlant", 0f, rate);
        
        
    }

    void genPlant()
    {
        
        maxX = box.bounds.max.x;
        minX = box.bounds.min.x;
        maxZ = box.bounds.max.z;
        minZ = box.bounds.min.z;
        randX = Random.Range(minX, maxX);
        randZ = Random.Range(minZ, maxZ);
        Vector3 plantPosition = new Vector3(randX, 0, randZ);
        plantPosition.y = terrain.SampleHeight(plantPosition);


        
        
        



        GameObject newPlant = Instantiate(plant, plantPosition, Quaternion.identity);
        newPlant.name = newPlant.name.Replace("(Clone)", "");       //added code to remove the (clone) from the name

    }
}

