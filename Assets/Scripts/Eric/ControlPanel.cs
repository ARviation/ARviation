using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    // variables
    public bool is_AR_Camera = true;
    public GameObject airplane_prefab;

    // Awake
    void Awake()
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
            GameObject.Find("Main Camera").gameObject.SetActive(true);

            Vector3 hit_position = new Vector3(1, 0, 1);
            Quaternion hit_rotation = Quaternion.Euler(0, 30, 0);
            GameObject spawnedObject = Instantiate(airplane_prefab, hit_position, hit_rotation);
            spawnedObject.name = "airplane_prefab";
        }
    }
}
