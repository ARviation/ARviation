using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
  [SerializeField] private GameObject enableFrame;
  [SerializeField] private GameObject inventoryIcon;
  [SerializeField] private CollectPanel collectPanel;

  private MoseCode _currentCode;
  private string _currentCategory;
  private bool isEnable = false;
  private bool isCollected = false;

  private void Start()
  {
    enableFrame.SetActive(false);
    inventoryIcon.SetActive(false);
  }

  public void OnClick()
  {
    collectPanel.OnInventoryItemClick(gameObject);
    if (isEnable)
    {
      enableFrame.SetActive(false);
      isEnable = false;
    }
    else
    {
      if (!isCollected) return;
      enableFrame.SetActive(true);
      isEnable = true;
    }
  }

  public void CloseFrame()
  {
    if (!isEnable) return;
    enableFrame.SetActive(false);
    isEnable = false;
  }

  public void OnHitComponent(MoseCode componentCode)
  {
    _currentCode = componentCode;
  }

  public void OnCollectComponent()
  {
    if (!isCollected)
    {
      isCollected = true;
      inventoryIcon.SetActive(true);
    }

    int code = (int) _currentCode;

    switch (gameObject.name)
    {
      case "Body":
        FindObjectOfType<ObjectsManager>().localCollectedComponent.body = code;
        break;
      case "Engines":
        FindObjectOfType<ObjectsManager>().localCollectedComponent.engine = code;
        break;
      case "Wings":
        FindObjectOfType<ObjectsManager>().localCollectedComponent.wings = code;
        break;
      case "Propeller":
        FindObjectOfType<ObjectsManager>().localCollectedComponent.propellers = code;
        break;
      case "Wheels":
        FindObjectOfType<ObjectsManager>().localCollectedComponent.wheels = code;
        break;
      case "OilTank":
        FindObjectOfType<ObjectsManager>().localCollectedComponent.oilTank = code;
        break;
      case "Extra":
        FindObjectOfType<ObjectsManager>().localCollectedComponent.extra = code;
        break;
    }

    inventoryIcon.GetComponent<Image>().sprite = collectPanel.GetCandidateSprite(code);
  }

  // make a method for removing current chosen component
}