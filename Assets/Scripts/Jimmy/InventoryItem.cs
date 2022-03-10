using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MoseCode : int
{
  A = 0,
  R = 1,
  Y = 2,
  J = 3,
  P = 4,
  N = 5,
}

public class InventoryItem : MonoBehaviour
{
  [SerializeField] private GameObject enableFrame;
  [SerializeField] private GameObject inventoryIcon;
  [SerializeField] private Sprite[] candidates;

  private MoseCode _currentCode;
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
      inventoryIcon.GetComponent<Image>().sprite = candidates[(int) _currentCode];
    }
  }

  // make a method for removing current chosen component
}