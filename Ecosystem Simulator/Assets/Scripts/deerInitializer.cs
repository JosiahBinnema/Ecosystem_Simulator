using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class deerInitializer : MonoBehaviour
{
    animalStats statsScript;
    birthing birthingScript;
    health lifeScript;

    void Start()
    {
        this.name = "Deer";
        statsScript = GetComponent<animalStats>();
        lifeScript = GetComponent<health>();
        StartCoroutine(spawning());

    }

    IEnumerator spawning()
    {
        birthingScript = GetComponent<birthing>();

        this.name = "Deer";

        lifeScript.life = 1;
        statsScript.hunts = new string[] { "Plant" };
        statsScript.fleesFrom = new string[] { "Cougar" };
        statsScript.offspringCount = 1;
        statsScript.pregnancyTerm = 1f;
        statsScript.maxReproductiveUrge = 80f;
        statsScript.happinessThreshold = 20;

        startingValues();                      //disable miracleBirth and isMale if you're testing and annoyed
        statsScript.isMale = birthingScript.pickSex();
        GetComponent<NavMeshAgent>().speed = statsScript.speed;
        yield return new WaitForSeconds(birthingScript.nextFloat(0f, 0.9f));
    }

    public void startingValues()          //birth without parents, just randomly making some stats
    {

        lifeScript.life = 1;
        statsScript.hunger = nextFloat(0, 40);
        statsScript.thirst = nextFloat(0, 40);
        statsScript.reproductiveUrge = nextFloat(0, 40);
        statsScript.reproductiveUrgeRate = birthingScript.pickReproductiveUrgeRate();
        statsScript.visionRange = nextFloat(90,100);
        statsScript.speed = Random.Range(10,20);
        statsScript.drinkingSpeed = Random.Range(7, 14);
        statsScript.eatingSpeed = Random.Range(7, 14);
        statsScript.desireablility = Random.Range(0, 100);
        birthingScript.deerRates(statsScript);
    }

    public float nextFloat(float min, float max)
    {
        return Random.Range(min, max); 
    }
}
