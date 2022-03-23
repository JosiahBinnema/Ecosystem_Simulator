using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Collections;

public class AnimalController : MonoBehaviour
{
    public NavMeshAgent agent;
    animalStats statsScript;
    health life;



    public float wanderRadius = 1000;
    public float fleeingRange = 50;

    private Vector3 dummyVector = new Vector3(-20.0f, -20.0f, -20.0f);
    public float refreshRate = 1f;    // how often do we update our urges (in seconds)
    private float timeCounter = 0.3f;  // should make this a global feature to work with "simulation mode"

    private void carryPregnancy(animalStats father)
    {
        StartCoroutine(childBirth(statsScript.pregnancyTerm, father));
    }
    IEnumerator childBirth(float waitTime, animalStats father)
    {
        float tempRate = statsScript.reproductiveUrgeRate;

        agent.speed = (agent.speed / 2);
        statsScript.reproductiveUrgeRate = 0;
        
        yield return new WaitForSeconds(waitTime);
        
        for (int i = 0; i < statsScript.offspringCount; i++) {
            agent.speed = 0;
            GameObject baby = Instantiate(gameObject);
            birthing babyMaker = baby.GetComponent<birthing>();
            babyMaker.birth(statsScript, father);
            Debug.Log( this.name + " carried to term");
            yield return new WaitForSeconds(1.5f);
        }
        agent.speed = statsScript.speed;
        statsScript.reproductiveUrge = 0;
        statsScript.reproductiveUrgeRate = tempRate;
//        yield return new WaitForSeconds(15f);
 //       statsScript.isPregnant = false;

    }
    private Vector3 chooseClosest(List<Collider> sources)        //"efficient" code for finding the closest object in the collider list
    {
        Collider closestSource = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Collider potentialTarget in sources)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistance)
            {
                closestDistance = dSqrToTarget;
                closestSource = potentialTarget;
            }
        }
        if (closestSource != null )
        {
            if (closestSource.tag == "drinkable")       // makes them less buggy around water.
            {
                return closestSource.ClosestPoint(this.transform.position);
            } else
            {
                return closestSource.transform.position;
            }
            
        } else 
        {
            return this.transform.position;
        }
    }
    private Vector3 wanderLocation(float mapRadius)
    {
        if (agent.remainingDistance < 5)
        {
            Vector3 randomDirection = Random.insideUnitSphere * mapRadius;
            randomDirection += transform.position;
            NavMeshHit hit;
            Vector3 finalPosition = Vector3.zero;
            if (NavMesh.SamplePosition(randomDirection, out hit, mapRadius, 1))
            {
                finalPosition = hit.position;
            }
//            Debug.Log("wandering to location " + finalPosition);
            return finalPosition;
        } else
        {
//            Debug.Log("still going to location " + agent.remainingDistance + "to go to" + agent.pathEndPosition);
            return agent.pathEndPosition;

        }
    }
    private Vector3 predatorSearch(Collider[] col)            // returns dummy vector if no predators, otherwise returns opposite direction of predator
    {                                              
        List<Collider> predators = new List<Collider>();
        foreach (Collider potentialPredator in col)
        {
            if (statsScript.fleesFrom.Contains(potentialPredator.name))
            {
                predators.Add(potentialPredator);
            }
        }
        if (predators.Count == 0)
        {
            return dummyVector;
        }
        else
        {
            Vector3 direction = Vector3.MoveTowards(this.transform.position, chooseClosest(predators), -fleeingRange);
            return direction;
        }
    }
    private Vector3 findFood(Collider[] visibleObjects)
    {
        List<Collider> foodList = new List<Collider>();

        foreach (Collider potentialSource in visibleObjects)
        {
            if (statsScript.hunts.Contains(potentialSource.name))
            {
                foodList.Add(potentialSource);
            }
        }

        if (foodList.Count == 0)
        {
//            Debug.Log("no food in vision range. Wandering!");
            return wanderLocation(wanderRadius);
        }
        else
        {
            return chooseClosest(foodList);
        }
    }
    private Vector3 findWater(Collider[] visibleObjects)
    {

        List<Collider> waterSources = new List<Collider>();
        foreach (Collider potentialSource in visibleObjects)
        {
            if (potentialSource.tag == "drinkable")
            {
                waterSources.Add(potentialSource);
            }
        }
        if (waterSources.Count == 0)
        {
//            Debug.Log("no water in vision range. Wandering!");
            return wanderLocation(wanderRadius);
        }
        else
        {
            return chooseClosest(waterSources);
        }

    }
    private Vector3 findMate(Collider[] visibleObjects)     // TODO
    {
        float maxSexiness = -1f;
        Collider hottestMate = null;
        foreach (Collider potentialMate in visibleObjects)
        {
            if (potentialMate.name == this.name)
            {
                animalStats otherAnimalScript = potentialMate.GetComponent<animalStats>();
                if (statsScript.isMale != otherAnimalScript.isMale)
                {
                    if (otherAnimalScript.desireablility > maxSexiness)
                    {
                        maxSexiness = otherAnimalScript.desireablility;
                        hottestMate = potentialMate;
                    }
                }
            }
        }
        if (hottestMate == null)
        {
//            Debug.Log("no mate in vision range. Wandering!");
            return wanderLocation(wanderRadius);
        }
        else
        {
            return hottestMate.transform.position;
        }
    }
    private Vector3 nextLocation(animalStats.animalState currentState)    // finds locations for all states where navigation is required
    {
        Collider[] visibleObjects = (Physics.OverlapSphere(this.transform.position, statsScript.visionRange));
        Vector3 temp = predatorSearch(visibleObjects);
        if (temp != dummyVector) // we found a predator
        {
            statsScript.currentState = animalStats.animalState.FLEEING;
            return temp;
        }
        else
        {
            switch (currentState)
            {
                case animalStats.animalState.HUNGRY:
                    return findFood(visibleObjects);
                case animalStats.animalState.THIRSTY:
                    return findWater(visibleObjects);
                case animalStats.animalState.HORNY:
                    return findMate(visibleObjects);               //TODO
                default:
                    return wanderLocation(wanderRadius);
            }
        }
    }



    private void OnTriggerStay(Collider other)             
    {
        if (Time.time >= timeCounter - 0.1)
        {
            switch (statsScript.currentState)
            {
                case animalStats.animalState.DRINKING:
                case animalStats.animalState.THIRSTY:
                    if (other.gameObject.tag == "drinkable")
                    {
                        agent.velocity = Vector3.zero;
                        statsScript.currentState = animalStats.animalState.DRINKING;
                    }
                    break;
                case animalStats.animalState.EATING:
                case animalStats.animalState.HUNGRY:
                    if (statsScript.hunts.Contains(other.name))         // subtract health from thing in this case
                    {
                        agent.velocity = Vector3.zero;
                        statsScript.currentState = animalStats.animalState.EATING;
                        life = other.GetComponent<health>();
                        
                        
                        if((life.life - statsScript.eatingSpeed) < 1)
                        {
                            life.takeDamage(statsScript.eatingSpeed);
                            statsScript.updateState();
                        }
                        else
                        {
                            life.takeDamage(statsScript.eatingSpeed);
                        }


                    }
                    break;
                case animalStats.animalState.FRICKING:
                case animalStats.animalState.HORNY:
                    animalStats otherStatsScript = other.GetComponent<animalStats>();
                    if (this.name == other.name && statsScript.isMale != otherStatsScript.isMale && statsScript.reproductiveUrge > 0 && otherStatsScript.reproductiveUrge > 0)  //&& !otherStatsScript.isPregnant
                    {
                        Debug.Log("sex happens now ");
                        agent.velocity = Vector3.zero;
                        statsScript.reproductiveUrge = 0;
                        otherStatsScript.reproductiveUrge = 0;
                        if (!statsScript.isMale)
                        {
 //                           statsScript.isPregnant = true;
                            carryPregnancy(otherStatsScript);
                        } else
                        {
                            AnimalController otherAnimal = other.GetComponent<AnimalController>();
//                            otherStatsScript.isPregnant = true;
                            otherAnimal.carryPregnancy(statsScript);
                            otherStatsScript.reproductiveUrge = 0;
                        }
                    }

                    break;
            }
        }
    }
    private void updateNaviation(animalStats.animalState currentState)
    {
        switch (currentState)
        {
            case animalStats.animalState.DEAD:
                life = this.GetComponent<health>();
                life.takeDamage(Mathf.Infinity);
                break;
            case animalStats.animalState.DRINKING:
                break;
            case animalStats.animalState.EATING:
                break;
            case animalStats.animalState.FRICKING:
                break;
            default:
                agent.SetDestination(nextLocation(currentState));        // should only be called on states that need a new location (FLEEING,HUNGRY,THIRSTY,HORNY)
                break;
        }
    }
    void Start()
    {
        statsScript = GetComponent<animalStats>();
    }
    void Update()
    {
        if (Time.time > timeCounter)        // making the update run less often
        {
            timeCounter += refreshRate;

            animalStats.animalState currentState = statsScript.currentState;  //

            statsScript.updateStatsAndState();
            updateNaviation(currentState);

        }

    }

}
