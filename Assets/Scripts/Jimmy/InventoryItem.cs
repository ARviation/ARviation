using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
  [SerializeField] private GameObject enableFrame;
  [SerializeField] private GameObject inventoryIcon;

  private CollectPanel _collectPanel;
  private MoseCode _currentCode;
  private bool isEnable = false;
  private bool isCollected = false;

  private void Awake()
  {
    _collectPanel = FindObjectOfType<CollectPanel>();
  }

  private void Start()
  {
    enableFrame.SetActive(false);
    inventoryIcon.SetActive(false);
  }

  public void OnClick()
  {
    _collectPanel.OnInventoryItemClick(gameObject);
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
    Debug.Log("OnCollectComponent");
    if (!isCollected)
    {
      isCollected = true;
      inventoryIcon.SetActive(true);
    }
    Debug.Log(_collectPanel.name);
    inventoryIcon.GetComponent<Image>().sprite = _collectPanel.GetCandidateSprite((int) _currentCode);
  }

  // make a method for removing current chosen component
}