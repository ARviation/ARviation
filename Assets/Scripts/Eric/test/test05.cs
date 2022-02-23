using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test05 : MonoBehaviour
{
    // Variables
    public Camera mainCamera;
    public GameObject go;
    //public bool is_follow_camera = true;
    public Vector3 offset;


    //// Start
    //void Start()
    //{
    //    go.SetActive(true);
    //    go.transform.position = mainCamera.transform.position + mainCamera.transform.rotation * offset;
    //    go.transform.rotation = mainCamera.transform.rotation;
    //}


    //// Update
    //void Update()
    //{
    //    if (is_follow_camera)
    //    {
    //        go.SetActive(true);
    //        go.transform.position = mainCamera.transform.position + mainCamera.transform.rotation * offset;
    //        go.transform.rotation = mainCamera.transform.rotation;
    //    }
    //    else
    //    {
    //        go.SetActive(false);
    //    }
    //}

    // spawn
    public void spawn()
    {
        print("cube!!!");
        go.transform.position = mainCamera.transform.position + mainCamera.transform.rotation * offset;
        go.transform.rotation = mainCamera.transform.rotation;
    }

}
