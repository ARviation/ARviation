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
  [SerializeField] private Sprite emptySprite;

  public MoseCode currentCode;
  private bool _isEnable = false;
  private bool _isCollected = false;

  private void Start()
  {
    enableFrame.SetActive(false);
  }

  public void OnClick()
  {
    collectPanel.OnInventoryItemClick(gameObject);
    if (_isEnable)
    {
      enableFrame.SetActive(false);
      _isEnable = false;
    }
    else
    {
      if (!_isCollected) return;
      enableFrame.SetActive(true);
      _isEnable = true;
      PlayerStats.Instance.UpdateSelectedComponentCode(currentCode, this);
    }
  }

  public void CloseFrame()
  {
    if (!_isEnable) return;
    enableFrame.SetActive(false);
    _isEnable = false;
  }

  public void OnHitComponent(MoseCode componentCode)
  {
    currentCode = componentCode;
  }

  public void OnUseComponent()
  {
    _isCollected = false;
    enableFrame.SetActive(false);
    _isEnable = false;
    inventoryIcon.GetComponent<Image>().sprite = emptySprite;
  }

  public void OnCollectComponent()
  {
    if (!_isCollected)
    {
      inventoryIcon.SetActive(true);
      _isCollected = true;
    }

    int code = (int) currentCode;
    switch (gameObject.name)
    {
      case GameManager.Fuselage:
        FindObjectOfType<ObjectsManager>().localCollectedComponent.Fuselage = code;
        break;
      case GameManager.Engine:
        FindObjectOfType<ObjectsManager>().localCollectedComponent.Engine = code;
        break;
      case GameManager.Wings:
        FindObjectOfType<ObjectsManager>().localCollectedComponent.Wings = code;
        break;
      case GameManager.Propeller:
        FindObjectOfType<ObjectsManager>().localCollectedComponent.Propellers = code;
        break;
      case GameManager.Wheels:
        FindObjectOfType<ObjectsManager>().localCollectedComponent.Wheels = code;
        break;
      case GameManager.FuelTank:
        FindObjectOfType<ObjectsManager>().localCollectedComponent.FuelTank = code;
        break;
      case GameManager.Tail:
        FindObjectOfType<ObjectsManager>().localCollectedComponent.Tail = code;
        break;
    }

    UpdateSprite(code);
  }

  public void UpdateSprite(int code)
  {
    if (code < 0) return;
    inventoryIcon.GetComponent<Image>().sprite = collectPanel.GetCandidateSprite(code);
    inventoryIcon.SetActive(true);
    _isCollected = true;
  }
}