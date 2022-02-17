using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DragManager : MonoBehaviour
{
    public string draggingTag;
    public Camera cam;

    private float width;
    private float height;
    private bool _isDragActive = false;
    private Vector2 _screenPosition;
    private Vector3 _worldPosition;
    private Draggable _lastDragged;

    private void Awake()
    {
        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;
        
        DragManager[] managers = FindObjectsOfType<DragManager>();
        if (managers.Length > 1)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (_isDragActive)
        {
            if (Input.GetMouseButtonUp(0) ||
                 (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended))
            {
                Drop();
                return;    
            }
        }
        
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            _screenPosition = new Vector2(mousePos.x, mousePos.y);
            
        } 
        else if (Input.touchCount > 0)
        {
            _screenPosition = Input.GetTouch(0).position;
        }
        else
        {
            return;
        }

        Debug.Log(_screenPosition);
        _worldPosition = Camera.main.ScreenToViewportPoint(_screenPosition);
        Debug.Log(_worldPosition);
        if (_isDragActive)
        {
            Drag(_worldPosition.x, _worldPosition.y);
        }
        else
        {
            RaycastHit hit;
            Vector3 pos = Input.mousePosition;
            // Debug.Log(pos);
            // pos.x = (pos.x - width) / width;
            // pos.y = (pos.y - height) / height;
            Ray ray = cam.ScreenPointToRay(pos);

            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag(draggingTag))
            {
                Debug.Log("here");
                Draggable draggable = hit.transform.gameObject.GetComponent<Draggable>();
                if (draggable != null)
                {
                    _lastDragged = draggable;
                    InitDrag();
                }
            }
        }
    }

    void InitDrag()
    {
        _isDragActive = true;
    }

    void Drag(float x, float y)
    {
        _lastDragged.transform.position = new Vector3(x, y, 0.0f);
    }

    void Drop()
    {
        _isDragActive = false;
    }
}
