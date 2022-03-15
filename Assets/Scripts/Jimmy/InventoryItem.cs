using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ARviation
{
  public class InventoryItem : MonoBehaviour
  {
    [SerializeField] private GameObject enableFrame;
    [SerializeField] private GameObject inventoryIcon;
    [SerializeField] private CollectPanel collectPanel;

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

      inventoryIcon.GetComponent<Image>().sprite = collectPanel.GetCandidateSprite((int) _currentCode);
    }

    // make a method for removing current chosen component
  }
}