using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectPanel : MonoBehaviour
{
  [SerializeField] private Sprite[] candidates;
  [SerializeField] private TMP_Text componentName;
  [SerializeField] private DisplayItem[] displayItems;

  private InventoryItem[] _inventoryItems;
  private InventoryItem _inventoryItem;

  private void Awake()
  {
    _inventoryItems = FindObjectsOfType<InventoryItem>();
  }

  public void SetInventoryItem(InventoryItem inventoryItem, string name)
  {
    _inventoryItem = inventoryItem;
    _inventoryItem.SetComponentName(name);
  }

  public void OnCollectBtnClick()
  {
    _inventoryItem.OnCollectComponent();
    ClosePanel();
  }

  public void OnCancelBtnClick()
  {
    ClosePanel();
  }

  public void OnInventoryItemClick(GameObject obj)
  {
    foreach (InventoryItem inventoryItem in _inventoryItems)
    {
      if (inventoryItem.name != obj.name)
        inventoryItem.CloseFrame();
    }

    if (displayItems != null)
    {
      foreach (DisplayItem displayItem in displayItems)
      {
        displayItem.ChangeState(false);
      }
    }
  }

  public void ClosePanel()
  {
    gameObject.SetActive(false);
  }

  public void OpenPanel(string name)
  {
    gameObject.SetActive(true);
    if (componentName != null)
      componentName.text = name;
  }

  public Sprite GetCandidateSprite(int candidateCode)
  {
    return candidates[candidateCode];
  }
}