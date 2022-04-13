using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_propeller : MonoBehaviour
{
    public GameObject propeller;
    public float propeller_rotation_speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 angles = propeller.transform.localEulerAngles;
        angles.z += propeller_rotation_speed * Time.deltaTime;
        propeller.transform.localEulerAngles = angles;

    }
}
