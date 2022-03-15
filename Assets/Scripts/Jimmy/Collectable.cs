using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Collectable : MonoBehaviour
{
  [SerializeField] public MoseCode componentCode;
  private InventoryItem _matchedInventoryItem;
  private InventoryItem[] _inventoryItems;

  private void Awake()
  {
    _inventoryItems = FindObjectsOfType<InventoryItem>();
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
    return _matchedInventoryItem;
  }

  private void LocateInventoryCategory()
  {
    foreach (InventoryItem item in _inventoryItems)
    {
      if (!item.CompareTag("Untagged"))
      {
        if (item.CompareTag(gameObject.tag))
        {
          _matchedInventoryItem = item;
        }
      }
    }
  }
}