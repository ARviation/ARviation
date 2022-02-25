using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectPanel : MonoBehaviour
{
  private InventoryItem _inventoryItem;

  public void SetInventoryItem(InventoryItem inventoryItem)
  {
    _inventoryItem = inventoryItem;
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
  
  public void ClosePanel()
  {
    gameObject.SetActive(false);
  }

  public void OpenPanel()
  {
    gameObject.SetActive(true);
  }
}