using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private GameObject enableFrame;

    private bool isEnable = false;

    private void Start()
    {
       enableFrame.SetActive(false); 
    }

    public void OnClick()
    {
        if (isEnable)
        {
            enableFrame.SetActive(false);
            isEnable = false;
        }
        else
        {
            enableFrame.SetActive(true);
            isEnable = true;
        }
    }
}
