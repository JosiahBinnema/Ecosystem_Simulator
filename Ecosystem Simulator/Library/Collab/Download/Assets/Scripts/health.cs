using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health : MonoBehaviour
{
    public GameObject Meat;
    public float life;
    
    public void takeDamage(float damage)
    {
        life -= damage;
        if(life < 1)
        {
            //if (this.tag != "vegetation")
            if(this.name == "Deer" || this.name == "Cougar")   
            {
                Vector3 meatPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                GameObject newMeat = Instantiate(Meat, meatPosition, Quaternion.identity);
                newMeat.name = newMeat.name.Replace("(Clone)", "");       //added code to remove the (clone) from the name
            }
            Destroy(gameObject);
        }
    }
    

    
}
