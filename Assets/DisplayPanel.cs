using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayPanel : MonoBehaviour
{
  [SerializeField] private TMP_Text componentName;
  [SerializeField] private InventoryItem[] _inventoryItems;
  [SerializeField] private DisplayItem[] displayItems;

  public bool isDisplaying;

  public void OnClickCloseBtn()
  {
    ClosePanel();
  }

  public void ClosePanel()
  {
    isDisplaying = false;
    gameObject.SetActive(false);
    SoundManager.Instance.PlaySFXByIndex(SFXList.Click);
    FindObjectOfType<NarrationController>().OpenScanImage();

    foreach (InventoryItem inventoryItem in _inventoryItems)
    {
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

  public void OpenPanel(string name)
  {
    gameObject.SetActive(true);
    isDisplaying = true;
    if (componentName != null)
      componentName.text = name;
  }
}