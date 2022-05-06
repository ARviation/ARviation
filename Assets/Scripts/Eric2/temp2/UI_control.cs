using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_control : MonoBehaviour
{
    // Variables
    public Slider slider_m;
    float dumping_rate = 0.01f;


    // Start
    void Start()
    {
        StartCoroutine(slider_dumping());
    }


    // Update
    void Update()
    {
        
    }


    // slider dumping
    IEnumerator slider_dumping()
    {
        while (true)
        {
            if (slider_m.value > 0)
            {
                slider_m.value = Mathf.Max(0, slider_m.value - dumping_rate);
            }
            yield return null;
        }
    }
}
