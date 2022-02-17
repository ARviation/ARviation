using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;

public class DraggableObjAdv : MonoBehaviour
{
    private Vector3 position;
    private float width;
    private float height;

    private bool touched = false;
    private bool dragging = false;
    private bool hasGravityChg = false;
    private Rigidbody _rigidbody;

    void Awake()
    {
        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;

        // Position used for the cube.
        position = new Vector3(0.0f, 0.0f, 0.0f);
    }

    void OnGUI()
    {
        // Compute a fontSize based on the size of the screen width.
        GUI.skin.label.fontSize = (int)(Screen.width / 25.0f);

        GUI.Label(new Rect(20, 20, width, height * 0.25f),
            "x = " + position.x.ToString("f2") +
            ", y = " + position.y.ToString("f2") + 
            ", hasTouched = " + touched +
            ", hasDragged = " + dragging +
            ", gravity changed = " + hasGravityChg);
    }

    private void Start()
    {
        _rigidbody = transform.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touched = true;

            // Move the cube if the screen has the finger moving.
            if (touch.phase == TouchPhase.Moved)
            {
                dragging = true;

                Vector2 pos = touch.position;
                pos.x = (pos.x - width) / width;
                pos.y = (pos.y - height) / height;
                position = new Vector3(-pos.x, pos.y, 0.0f);
                SetDraggingProperties(_rigidbody);
                
                // Position the cube.
                transform.position = position;
            }

            if (Input.touchCount == 2)
            {
                touch = Input.GetTouch(1);

                if (touch.phase == TouchPhase.Began)
                {
                    // Halve the size of the cube.
                    transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    // Restore the regular size of the cube.
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
            }
        }
        else
        {
            dragging = false;
            touched = false;
            SetFreeProperties(_rigidbody);
            return;
        }
        
        // if (Input.touchCount != 1)
        // {
        //     dragging = false;
        //     touched = false;
        //     SetFreeProperties(_rigidbody);
        //     return;
        // }
        //
        // Touch touch = Input.touches[0];
        // touched = true;
        // // Vector3 pos = touch.position;
        //
        // // Handle screen touches
        //
        // // Move the cube if the screen has the finger moving.
        // if (touch.phase == TouchPhase.Moved)
        // {
        //     dragging = true;
        //     Vector2 pos = touch.position;
        //     pos.x = (pos.x - width) / width;
        //     pos.y = (pos.y - height) / height;
        //     position = new Vector3(-pos.x, pos.y, 0.0f);
        //     SetDraggingProperties(_rigidbody);
        //
        //     // Position the cube.
        //     transform.position = position;
        // }
        //
        // if (Input.touchCount == 2)
        // {
        //     touch = Input.GetTouch(1);
        //
        //     if (touch.phase == TouchPhase.Began)
        //     {
        //         // Halve the size of the cube.
        //         transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        //     }
        //
        //     if (touch.phase == TouchPhase.Ended)
        //     {
        //         // Restore the regular size of the cube.
        //         transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        //     }
        // }
    }
    
    /*
     * Set the physical situation when objects were being dragged
     */
    private void SetDraggingProperties(Rigidbody rb)
    {
        // rb.isKinematic = false;
        // rb.useGravity = false;
        // rb.drag = 2;
        hasGravityChg = true;
    }

    /*
     * Let the objects' physic goes back to normal
     */
    private void SetFreeProperties(Rigidbody rb)
    {
        // rb.isKinematic = true;
        // rb.useGravity = true;
        // rb.drag = 1;
        hasGravityChg = false;
    }
}
