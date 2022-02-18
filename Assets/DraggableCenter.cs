using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class DraggableCenter : MonoBehaviour
{
    public GameObject targetObj;
    public Transform camPivot, camTarget, camRoot;

    private float distance = 10.0f;
    private float rot = 0f;
    
    private void Start()
    {
        this.transform.position = targetObj.transform.position;
        this.transform.rotation = targetObj.transform.rotation;
    }

    private void Update()
    {
        throw new NotImplementedException();
    }
}
