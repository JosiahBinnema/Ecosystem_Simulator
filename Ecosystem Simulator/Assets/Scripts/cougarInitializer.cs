using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class cougarInitializer : MonoBehaviour
{
    animalStats statsScript;
    birthing birthingScript;
    health lifeScript;

    void Start()
    {
        statsScript = GetComponent<animalStats>();
        birthingScript = GetComponent<birthing>();
        lifeScript = GetComponent<health>();
        StartCoroutine(spawning());
    }

    IEnumerator spawning()
    {

        this.name = "Cougar";
        lifeScript.life = 10;
        statsScript.hunts = new string[] { "Deer","DeerMeat", "Meat"};
        statsScript.fleesFrom = new string[] { "" };
        statsScript.offspringCount = 1;
        statsScript.pregnancyTerm = 20f;
        statsScript.maxReproductiveUrge = 60;
        statsScript.happinessThreshold = 0;
        miracleBirth();                                   //disable miracleBirth and isMale if you're testing and annoyed
        statsScript.isMale = birthingScript.pickSex();
        GetComponent<NavMeshAgent>().speed = statsScript.speed;
        yield return new WaitForSeconds(birthingScript.nextFloat(0f, 0.9f));
    }

    public void miracleBirth()          //birth without parents, just randomly making some stats
    {


        statsScript.hunger = birthingScript.nextFloat(0, 40);
        statsScript.thirst = birthingScript.nextFloat(0, 40);
        statsScript.reproductiveUrge = birthingScript.nextFloat(0, 40);
        statsScript.reproductiveUrgeRate = birthingScript.pickReproductiveUrgeRate();
        statsScript.visionRange = birthingScript.nextFloat(110,120);
        statsScript.speed = birthingScript.nextFloat(15,25);
        statsScript.drinkingSpeed = birthingScript.nextFloat(5, 15);
        statsScript.eatingSpeed = birthingScript.nextFloat(5, 15);
        statsScript.desireablility = Random.Range(0, 100);

        birthingScript.cougarRates(statsScript);
    }

}

 