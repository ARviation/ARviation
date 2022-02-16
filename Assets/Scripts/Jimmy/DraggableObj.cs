using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;

public class DraggableObj : MonoBehaviour
{
    public string draggingTag;
    public Camera cam;

    private Vector3 dis;
    private float posX;
    private float posY;
    private Vector3 position;
    private float width;
    private float height;
    
    private bool touched = false;
    private bool dragging = false;
    private bool hasGravityChg = false;

    private Transform toDrag;
    private Rigidbody toDragRigidbody;
    private Vector3 previousPosition;
    private string itemDragging = "";
    
    void Awake()
    {
        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;

        // Position used for the cube.
        // position = new Vector3(0.0f, 0.0f, 0.0f);
    }

    private void OnGUI()
    {
        // Compute a fontSize based on the size of the screen width.
        GUI.skin.label.fontSize = (int)(Screen.width / 25.0f);

        GUI.Label(new Rect(20, 20, width, height * 0.25f),
            "x = " + position.x.ToString("f2") +
            ", y = " + position.y.ToString("f2") + 
            ", item = " + itemDragging + ", hasTouched = " + touched +
            ", gravity changed = " + hasGravityChg);
    }

    private void FixedUpdate()
    {
        if (Input.touchCount != 1)
        {
            dragging = false;
            touched = false;
            if (toDragRigidbody)
            {
                SetFreeProperties(toDragRigidbody);
            }
            return;
        }

        Touch touch = Input.touches[0];
        Vector3 pos = touch.position;

        if (touch.phase == TouchPhase.Began)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(pos);

            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag(draggingTag))
            {
                itemDragging = hit.transform.name;
                toDrag = hit.transform;
                previousPosition = toDrag.position;
                toDragRigidbody = toDrag.GetComponent<Rigidbody>();

                dis = cam.WorldToScreenPoint(previousPosition);
                posX = Input.GetTouch(0).position.x - dis.x;
                posY = Input.GetTouch(0).position.y - dis.y;
                
                SetDraggingProperties(toDragRigidbody);

                touched = true;
            }
        }

        if (touched && touch.phase == TouchPhase.Moved)
        {
            // dragging = true;
            // float posXNow = Input.GetTouch(0).position.x - dis.x;
            // float posYNow = Input.GetTouch(0).position.y - dis.y;
            // Vector3 curPos = new Vector3(posXNow, posYNow, dis.z);
            //
            // Vector3 worldPos = cam.ScreenToViewportPoint(curPos) - previousPosition;
            // worldPos = new Vector3(worldPos.x, worldPos.y, 0.0f);
            //
            // toDragRigidbody.velocity = worldPos / (Time.deltaTime * 10);
            //
            // previousPosition = toDrag.position;
            
            pos = touch.position;
            pos.x = (pos.x - width) / width;
            pos.y = (pos.y - height) / height;
            position = new Vector3(-pos.x, pos.y, 0.0f);

            // Position the cube.
            transform.position = position;
        }

        if (dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
        {
            dragging = false;
            touched = false;
            previousPosition = new Vector3(0.0f, 0.0f, 0.0f);
            SetFreeProperties(toDragRigidbody);
        }
    }

    /*
     * Set the physical situation when objects were being dragged
     */
    private void SetDraggingProperties(Rigidbody rb)
    {
        rb.isKinematic = false;
        rb.useGravity = false;
        rb.drag = 20;
        hasGravityChg = true;
    }

    /*
     * Let the objects' physic goes back to normal
     */
    private void SetFreeProperties(Rigidbody rb)
    {
        rb.isKinematic = true;
        rb.useGravity = true;
        rb.drag = 5;
        hasGravityChg = false;
    }
}
