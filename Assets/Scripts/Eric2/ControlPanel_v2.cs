using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel_v2 : MonoBehaviour
{
    // variables
    public bool is_AR_Camera = true;
    public GameObject airplane_prefab;
    public UI_control_v3 m_UI_control;

    // Awake
    void Awake()
    {
        // set camera
        if (is_AR_Camera)
        {
            GameObject.Find("Main Camera").gameObject.SetActive(false);
            GameObject.Find("ground").gameObject.SetActive(false);
            m_UI_control.is_quit = false;
        }
        else
        {
            GameObject.Find("AR Session").gameObject.SetActive(false);
            GameObject.Find("AR Session Origin").gameObject.SetActive(false);
            GameObject.Find("Main Camera").gameObject.SetActive(true);
            GameObject.Find("ground").gameObject.SetActive(true);
            m_UI_control.scan_prompt_screen.SetActive(false);
            m_UI_control.is_quit = true;
            Vector3 hit_position = new Vector3(1, 0, 1);
            Quaternion hit_rotation = Quaternion.Euler(0, 30, 0);
            GameObject spawnedObject = Instantiate(airplane_prefab, hit_position, hit_rotation);
            spawnedObject.name = "airplane_prefab";
        }
    }


    //// Update
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        SFXmanager.playsound("scanner");
    //    }

    //    if (Input.GetKeyDown(KeyCode.Return))
    //    {
    //        SFXmanager.stopsound("scanner");
    //    }
        
    //}
}
