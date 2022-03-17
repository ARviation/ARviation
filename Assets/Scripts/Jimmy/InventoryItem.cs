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

  public MoseCode currentCode;
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
    currentCode = componentCode;
  }

  public void OnCollectComponent()
  {
    if (!isCollected)
    {
      inventoryIcon.SetActive(true);
      isCollected = true;
    }

    var code = (int) currentCode;

    switch (gameObject.name)
    {
      case "Body":
        FindObjectOfType<ObjectsManager>().localCollectedComponent.Body = code;
        break;
      case "Engines":
        FindObjectOfType<ObjectsManager>().localCollectedComponent.Engine = code;
        break;
      case "Wings":
        FindObjectOfType<ObjectsManager>().localCollectedComponent.Wings = code;
        break;
      case "Propeller":
        FindObjectOfType<ObjectsManager>().localCollectedComponent.Propellers = code;
        break;
      case "Wheels":
        FindObjectOfType<ObjectsManager>().localCollectedComponent.Wheels = code;
        break;
      case "OilTank":
        FindObjectOfType<ObjectsManager>().localCollectedComponent.OilTank = code;
        break;
      case "Extra":
        FindObjectOfType<ObjectsManager>().localCollectedComponent.Extra = code;
        break;
    }

    UpdateSprite(code);
  }

  public void UpdateSprite(int code)
  {
    if (code >= 0)
    {
      inventoryIcon.GetComponent<Image>().sprite = collectPanel.GetCandidateSprite(code);
      inventoryIcon.SetActive(true);
      isCollected = true;
    }
  }

  // make a method for removing current chosen component
}