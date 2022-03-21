using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachableComponent : MonoBehaviour
{
  [SerializeField] private GameObject objToHide;
  [SerializeField] private InventoryItem item;

  private void Start()
  {
    objToHide.gameObject.SetActive(false);
  }

  public void ShowObj()
  {
    objToHide.gameObject.SetActive(true);
    item.OnUseComponent();
  }
}