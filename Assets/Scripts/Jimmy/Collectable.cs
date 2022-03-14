using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Collectable : MonoBehaviour
{
  [SerializeField] public MoseCode componentCode;
  public InventoryItem matchedInventoryItem;
  private InventoryItem[] inventoryItems;

  private void Awake()
  {
    inventoryItems = FindObjectsOfType<InventoryItem>();
  }


  private void Start()
  {
    SetInventoryItem();
  }

  public void SetInventoryItem()
  {
    LocateInventoryCategory();
  }

  public InventoryItem GetInventoryItem()
  {
    return matchedInventoryItem;
  }

  private void LocateInventoryCategory()
  {
    foreach (InventoryItem item in inventoryItems)
    {
      if (!item.CompareTag("Untagged"))
      {
        if (item.CompareTag(gameObject.tag))
        {
          matchedInventoryItem = item;
        }
      }
    }
  }
}