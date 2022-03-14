using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoseCode : int
{
  A = 0,
  R = 1,
  Y = 2,
  J = 3,
  P = 4,
  N = 5,
}

public class CollectPanel : MonoBehaviour
{
  [SerializeField] private Sprite[] candidates;

  private InventoryItem[] _inventoryItems;
  private InventoryItem _inventoryItem;

  private void Awake()
  {
    _inventoryItems = FindObjectsOfType<InventoryItem>();
  }

  public void SetInventoryItem(InventoryItem inventoryItem)
  {
    _inventoryItem = inventoryItem;
  }

  public void OnCollectBtnClick()
  {
    Debug.Log("OnCollectBtnClick");
    Debug.Log(_inventoryItem.name);
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
  }

  public void ClosePanel()
  {
    gameObject.SetActive(false);
  }

  public void OpenPanel()
  {
    gameObject.SetActive(true);
  }

  public Sprite GetCandidateSprite(int candidateCode)
  {
    return candidates[candidateCode];
  }
}