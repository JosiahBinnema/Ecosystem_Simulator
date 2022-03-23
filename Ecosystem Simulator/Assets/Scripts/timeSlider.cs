using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class timeSlider : MonoBehaviour
{
    [SerializeField] Slider timeController;
    private float difference = 0.2f;
    Color red = new Color(1, 0, 0, 1);
    Color white = new Color(1,1,1,1);

    // Start is called before the first frame update
    void Start()                
    {
         
        timeController.onValueChanged.AddListener((v) =>        //When the slider value is adjusted, the timescale is adjusted to that value
        {
            Time.timeScale = v;

        }   
        );
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            timeController.value = timeController.value - difference;
        }

        if (Input.GetKey(KeyCode.E))
        {
            timeController.value = timeController.value + difference;
        }

        
    }
}
