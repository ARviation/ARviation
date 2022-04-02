using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
  [SerializeField] private GameObject enableFrame;
  [SerializeField] private GameObject inventoryIcon;
  [SerializeField] private CollectPanel collectPanel;
  [SerializeField] private Sprite emptySprite;

  public MorseCode currCode;
  public MorseCode collectedCode;
  private bool _isEnable = false;
  private bool _isCollected = false;

  private void Start()
  {
    enableFrame.SetActive(false);
  }

  public void OnClick()
  {
    collectPanel.OnInventoryItemClick(gameObject);
    if (_isEnable)
    {
      enableFrame.SetActive(false);
      _isEnable = false;
    }
    else
    {
      if (!_isCollected) return;
      enableFrame.SetActive(true);
      _isEnable = true;
      PlayerStats.Instance.UpdateSelectedComponentCode(currCode, this);
    }
  }

  public void CloseFrame()
  {
    if (!_isEnable) return;
    enableFrame.SetActive(false);
    _isEnable = false;
  }

  public void OnHitComponent(MorseCode componentCode)
  {
    currCode = componentCode;
  }

  public void OnUseComponent()
  {
    _isCollected = false;
    enableFrame.SetActive(false);
    _isEnable = false;
    inventoryIcon.GetComponent<Image>().sprite = emptySprite;
  }

  public void OnCollectComponent()
  {
    if (!_isCollected)
    {
      inventoryIcon.SetActive(true);
      _isCollected = true;
      FindObjectOfType<ObjectsManager>().AddCollectedComponent();
    }

    int code = (int) currCode;
    bool valid = true;
    switch (gameObject.name)
    {
      case GameManager.Fuselage:
        if (code == (int) MorseCode.A)
        {
          FindObjectOfType<ObjectsManager>().localCollectedComponent.Fuselage = code;
          FindObjectOfType<NarrationController>().SetHasConditionFalse();
          FindObjectOfType<NarrationController>().CollectRight();
        }
        else
        {
          FindObjectOfType<NarrationController>().CollectWrong();
          valid = false;
        }

        break;
      case GameManager.Engine:
        if (code == (int) MorseCode.H)
        {
          FindObjectOfType<ObjectsManager>().localCollectedComponent.Engine = code;
          FindObjectOfType<NarrationController>().CollectRight();
        }
        else
        {
          FindObjectOfType<NarrationController>().CollectWrong();
          valid = false;
        }

        break;
      case GameManager.Wings:
        if (code == (int) MorseCode.W)
        {
          FindObjectOfType<ObjectsManager>().localCollectedComponent.Wings = code;
          FindObjectOfType<NarrationController>().CollectRight();
        }
        else
        {
          FindObjectOfType<NarrationController>().CollectWrong();
          valid = false;
        }

        break;
      case GameManager.Propeller:
        if (code == (int) MorseCode.F)
        {
          FindObjectOfType<ObjectsManager>().localCollectedComponent.Propellers = code;
          FindObjectOfType<NarrationController>().CollectRight();
        }
        else
        {
          FindObjectOfType<NarrationController>().CollectWrong();
          valid = false;
        }

        break;
      case GameManager.Wheels:
        if (code == (int) MorseCode.O)
        {
          FindObjectOfType<ObjectsManager>().localCollectedComponent.Wheels = code;
          FindObjectOfType<NarrationController>().CollectRight();
        }
        else
        {
          FindObjectOfType<NarrationController>().CollectWrong();
          valid = false;
        }

        break;
      case GameManager.FuelTank:
        if (code == (int) MorseCode.E)
        {
          FindObjectOfType<ObjectsManager>().localCollectedComponent.FuelTank = code;
          FindObjectOfType<NarrationController>().CollectRight();
        }
        else
        {
          FindObjectOfType<NarrationController>().CollectWrong();
          valid = false;
        }

        break;
      case GameManager.Tail:
        if (code == (int) MorseCode.U)
        {
          FindObjectOfType<ObjectsManager>().localCollectedComponent.Tail = code;
          FindObjectOfType<NarrationController>().CollectRight();
        }
        else
        {
          FindObjectOfType<NarrationController>().CollectWrong();
          valid = false;
        }

        break;
    }

    if (!valid) return;
    UpdateSprite(code);
    // for switching collected components
    ImageRecognition imageRecognition = FindObjectOfType<ImageRecognition>();
    imageRecognition.HideUsedObj(imageRecognition.GetPrefabName(currCode));
    // imageRecognition.UnUsedObj(imageRecognition.GetPrefabName(collectedCode));
    // collectedCode = currCode;
  }

  public void UpdateSprite(int code)
  {
    if (code < 0) return;
    if (code >= 12) return;
    inventoryIcon.GetComponent<Image>().sprite = collectPanel.GetCandidateSprite(code);
    inventoryIcon.SetActive(true);
    _isCollected = true;
  }
}