using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test03 : MonoBehaviour
{
    public Camera mainCamera;

    public float x = 0;
    public float y = 0;
    public float z = 0;
    
    // Start
    void Start()
    {
        
    }

    // Update
    void Update()
    {
        Vector3 position = new Vector3(x, y, z);
        transform.position = mainCamera.ScreenToWorldPoint(position);
        transform.rotation = mainCamera.transform.rotation;
    }
}
