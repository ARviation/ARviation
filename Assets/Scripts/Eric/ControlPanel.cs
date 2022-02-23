using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    // variables
    public bool is_AR_Camera = true;


    // Start
    void Start()
    {
        // set camera
        if (is_AR_Camera)
        {
            GameObject.Find("Main Camera").gameObject.SetActive(false);
        }
        else
        {
            GameObject.Find("AR Session").gameObject.SetActive(false);
            GameObject.Find("AR Session Origin").gameObject.SetActive(false);
        }
    }
}
