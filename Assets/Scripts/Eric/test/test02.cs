using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test02 : MonoBehaviour
{
    [SerializeField] Camera mainCamera;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            Vector3 position = raycastHit.point;
            Transform trans = raycastHit.transform;
            Collider collider = raycastHit.collider;
            //print("hit " + collider.gameObject.name);
            string name = collider.gameObject.name;
            string tag = collider.tag;
            if (tag == "AR_object" && Input.GetMouseButtonDown(0))
            {
                Color color = GetComponent<MeshRenderer>().material.color;
                if (color == Color.red)
                {
                    color = Color.blue;
                }
                else
                {
                    color = Color.red;
                }
                GetComponent<MeshRenderer>().material.color = color;
            }
        }
    }
}
