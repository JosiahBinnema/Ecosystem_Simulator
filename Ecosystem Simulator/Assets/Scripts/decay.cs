using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class decay : MonoBehaviour
{
    health life;
    public float decayRate = 5;
    public float startDecay = 0;

    void Start()
    {
        life = GetComponent<health>();
        InvokeRepeating("Decay", startDecay, 1);
    }

    void Decay()
    {
        
        
        life.takeDamage(decayRate);
        
        
        
    }
}
