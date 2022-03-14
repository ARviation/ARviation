using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
  [SerializeField] public MoseCode componentCode;
  private string category;
  private InventoryItem[] inventoryItems;
  private InventoryItem matchedInventoryItem;

  private void Start()
  {
    inventoryItems = FindObjectsOfType<InventoryItem>();
    LocateInventoryCategory();
    category = gameObject.tag;
  }

  public InventoryItem GetInventoryItem()
  {
    category = gameObject.tag;
    inventoryItems = FindObjectsOfType<InventoryItem>();
    LocateInventoryCategory();
    return matchedInventoryItem;
  }

  private void LocateInventoryCategory()
  {
    foreach (InventoryItem item in inventoryItems)
    {
      if (item.name == category)
      {
        matchedInventoryItem = item;
      }
    }
  }
}