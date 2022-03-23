using UnityEngine;

public class animalStats : MonoBehaviour
{
    public enum animalState { HUNGRY, THIRSTY, HORNY, DEAD, FLEEING, DRINKING, EATING, HAPPY };
    public animalState currentState;
    public float hunger = 0;
    public float hungerRate; 
    public float thirst = 0;
    public float thirstRate; 
    public float reproductiveUrge;
    public float reproductiveUrgeRate;
    public float desireablility;
    public float visionRange;
    public float speed;
    public float drinkingSpeed;
    public float eatingSpeed;
    private float hungerThirstLimit = 85;
    public bool isMale;
    public int offspringCount;
    public string[] hunts;
    public string[] fleesFrom;
    public float pregnancyTerm;
    public float maxReproductiveUrge;
    public float happinessThreshold;
    //    public bool isPregnant = false;
    public GameObject child;



    private void updateDesireValues()        // designed to "tick" every so often to update our animals
    {
        hunger += hungerRate;
        thirst += thirstRate;
        reproductiveUrge += reproductiveUrgeRate;
        if (reproductiveUrge > maxReproductiveUrge)
        {
            reproductiveUrge = maxReproductiveUrge;
        }

    }

    public void updateState()        // designed to pick the new "driving" states
    {
        float strongestDesire = Mathf.Max(hunger, thirst, reproductiveUrge);  // should include all of the driving states
//        Debug.Log("current state is " + currentState);
        if (strongestDesire < happinessThreshold)
        {
            currentState = animalState.HAPPY;
        } else if (strongestDesire > 100)          
        {

//           Debug.Log("A " + this.name + " died. Hunger was " + hunger + "Thirst was " + thirst);
            currentState = animalState.DEAD;
        }
        else if (strongestDesire == thirst)
        {
            currentState = animalState.THIRSTY;
        }
        else if (strongestDesire == hunger)
        {
            currentState = animalState.HUNGRY;
        }
        else if (strongestDesire == reproductiveUrge)
        {
            currentState = animalState.HORNY;
        }
    }

    public bool shouldKeepDrinking()            // true if animal's state should stay as DRINKING
    {

        return (thirst > 20 && !(hunger > hungerThirstLimit));           // I think this is pretty bad logic, feel free to mess around with it
    }
    public bool shouldKeepEating()
    {
        return (hunger > 20 && !(thirst > hungerThirstLimit));
    }

    public void updateStatsAndState()           // this is called from the Deer controller to update the stats. 
    {
        updateDesireValues();
//        Debug.Log("Current state is " + currentState);
        
        switch (currentState)
        {
            case animalState.DRINKING:
                thirst -= drinkingSpeed;
                if (!shouldKeepDrinking())
                {
                    updateState();
                }
                break;
            case animalState.EATING:
                hunger -= eatingSpeed;
                
                if (!shouldKeepEating())
                {
                    updateState();
                }
                break;
            default:
                updateState();
                break;

        }
    }
}