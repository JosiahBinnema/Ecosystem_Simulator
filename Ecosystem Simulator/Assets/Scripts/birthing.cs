using System.Collections;
using UnityEngine;

public class birthing : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(spawning());
    }

    IEnumerator spawning()
    {
        yield return new WaitForSeconds(nextFloat(0f, 0.9f));
    }

    animalStats statsScript;
    float mutationAmount = 0.2f;  // should be a %change in trait (0.2 for 20% change) //make larger for more variance. 

    public float pickThirstRate() // these scripts need to be smart, and take in things like movespeed and vision range to help balance that stuff
    {
        return nextFloat(0.1f, 0.5f);
    }
    public float pickHungerRate() // these scripts need to be smart, and take in things like movespeed and vision range to help balance that stuff
    {
        return nextFloat(0.1f, 0.5f);
    }
    public float pickReproductiveUrgeRate() // these scripts need to be smart, and take in things like movespeed and vision range to help balance that stuff
    {
        return nextFloat(0.1f, 0.5f);
    }
    public float nextFloat(float min, float max)
    {
        return Random.Range(min, max);
    }

    public void birth(animalStats father, animalStats mother)       //initialize deer with parents. HERE IS WHERE YOU MAKE EYESIGHT ETC. HAVE DRAWBACKS
    {
        
        statsScript = GetComponent<animalStats>();

        
        this.statsScript.currentState = animalStats.animalState.HAPPY;
        this.statsScript.hunger = mother.hunger;
        this.statsScript.thirst = mother.thirst;
        this.statsScript.reproductiveUrge = 0;
        this.statsScript.reproductiveUrgeRate = averageMutate(father.reproductiveUrgeRate, mother.reproductiveUrgeRate, getMutation());
        this.statsScript.visionRange = averageMutate(father.visionRange, mother.visionRange, getMutation());
        this.statsScript.speed = averageMutate(father.speed, mother.speed, getMutation());
        this.statsScript.drinkingSpeed = averageMutate(father.eatingSpeed, mother.eatingSpeed, getMutation());
        this.statsScript.eatingSpeed = averageMutate(father.eatingSpeed, mother.eatingSpeed, getMutation());
        this.statsScript.desireablility = averageMutate(father.desireablility, mother.desireablility, getMutation());

        if (this.name == "Deer")
        {
            deerRates(statsScript);
        } else if (this.name == "Cougar")
        {
            cougarRates(statsScript);
        } else
        {
            otherRates(statsScript);
        }

    }
    public void deerRates(animalStats statsScript)
    {
        float deerBaseRate = (0.25f + ((statsScript.speed * 0.01f) + (statsScript.visionRange * 0.001f)));
        statsScript.hungerRate = deerBaseRate * 1; 
        statsScript.thirstRate = deerBaseRate * 1;
    }
    public void cougarRates(animalStats statsScript)
    {
        float cougarBaseRate = (0.1f + ((statsScript.speed * 0.005f) + (statsScript.visionRange * 0.0005f)));
        statsScript.hungerRate = cougarBaseRate;
        statsScript.thirstRate = cougarBaseRate;
    }
    public void otherRates(animalStats statsScript)
    {
        float otherBaseRate = (0.3f + ((statsScript.speed * 0.05f) + (statsScript.visionRange * 0.0005f)));
        statsScript.hungerRate = otherBaseRate;
        statsScript.thirstRate = otherBaseRate;
    }

        public float getMutation()
    {
        return nextFloat((1f - mutationAmount), (1f + mutationAmount));
    }
    public float averageMutate(float fatherStat, float motherStat, float mutationAmount)           // good candidate for unit testing
    {
        return (((fatherStat + motherStat) / 2) * mutationAmount);
    }
    public bool pickSex()
    {
        return (Random.Range(0, 2) == 0);
    }
}
