using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
  [SerializeField] private string category;
  [SerializeField] public ComponentCode componentCode;
  private InventoryItem[] inventoryItems;
  private InventoryItem targetInventoryItem;

  private void Start()
  {
    inventoryItems = FindObjectsOfType<InventoryItem>();
    LocateInventoryCategory();
    category = gameObject.tag;
  }

  public InventoryItem GetTargetInventoryItem()
  {
    return targetInventoryItem;
  }

  private void LocateInventoryCategory()
  {
    foreach (InventoryItem item in inventoryItems)
    {
      if (item.name == category)
      {
        targetInventoryItem = item;
      }
    }
  }
}
