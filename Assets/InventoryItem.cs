using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public enum ComponentCode : int
{
  A = 0,
  B = 1,
  C = 2,
}

public class InventoryItem : MonoBehaviour
{
  [SerializeField] private GameObject enableFrame;
  [SerializeField] private GameObject inventoryIcon;
  [SerializeField] private Sprite[] candidates;

  private ComponentCode _currentCode;
  private bool isEnable = false;
  private bool isCollected = false;

  private void Start()
  {
    enableFrame.SetActive(false);
    inventoryIcon.SetActive(false);
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

  public void OnHitComponent(ComponentCode componentCode)
  {
    _currentCode = componentCode;
  }

  public void OnCollectComponent()
  {
    if (!isCollected)
    {
      isCollected = true;
      inventoryIcon.SetActive(true);
      inventoryIcon.GetComponent<Image>().sprite = candidates[(int) _currentCode];
    }
  }
}