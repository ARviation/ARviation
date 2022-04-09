using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
  [SerializeField] private Image enableFrame;
  [SerializeField] private Image background;
  [SerializeField] private Image componentIcon;
  [SerializeField] private CollectPanel collectPanel;
  [SerializeField] private bool isAssembly = false;
  [SerializeField] private bool isFuselage = false;

  public MorseCode currCode;
  public MorseCode collectedCode;
  private bool _isEnable = false;
  private bool _isCollected = false;

  private void Start()
  {
    enableFrame.gameObject.SetActive(false);
    if (isAssembly && !isFuselage)
    {
      background.gameObject.SetActive(false);
      componentIcon.gameObject.SetActive(true);
      _isCollected = true;
    }
    else
    {
      background.gameObject.SetActive(true);
      componentIcon.gameObject.SetActive(false);
    }
  }

  public void OnClick()
  {
    collectPanel.OnInventoryItemClick(gameObject);
    AttachableComponent[] attachableComponents = null;
    if (isAssembly)
    {
      attachableComponents = FindObjectsOfType<AttachableComponent>();
    }

    if (_isEnable)
    {
      SoundManager.Instance.PlaySFXByIndex(SFXList.Click);
      enableFrame.gameObject.SetActive(false);
      _isEnable = false;

      if (isAssembly)
      {
        foreach (AttachableComponent attachableComponent in attachableComponents)
        {
          if (!attachableComponent.GetIsAttached())
          {
            attachableComponent.CloseOutline();
          }
        }
      }
    }
    else
    {
      if (!_isCollected) return;
      SoundManager.Instance.PlaySFXByIndex(SFXList.Click);
      enableFrame.gameObject.SetActive(true);
      _isEnable = true;
      PlayerStats.Instance.UpdateSelectedComponentCode(currCode, this);
      if (isAssembly)
      {
        foreach (AttachableComponent attachableComponent in attachableComponents)
        {
          if (!attachableComponent.GetIsAttached())
          {
            attachableComponent.ShowOutline();
          }
        }
      }
    }
  }

  public void CloseFrame()
  {
    if (!_isEnable) return;
    enableFrame.gameObject.SetActive(false);
    _isEnable = false;
  }

  public void OnHitComponent(MorseCode componentCode)
  {
    currCode = componentCode;
  }

  public void OnUseComponent()
  {
    _isCollected = false;
    _isEnable = false;
    enableFrame.gameObject.SetActive(false);
    componentIcon.gameObject.SetActive(false);
    background.gameObject.SetActive(true);
  }

  public void OnCollectComponent()
  {
    int code = (int) currCode;
    bool isRightComponent = true;
    switch (gameObject.name)
    {
      case GameManager.Fuselage:
        if (code == (int) CorrectMorseCode.Fuselage)
        {
          // FindObjectOfType<ObjectsManager>().localCollectedComponent.Fuselage = code;
          FindObjectOfType<NarrationController>().RevealDialog();
          FindObjectOfType<NarrationController>().RemoveHadHideCondition();
          SoundManager.Instance.PlaySuccessSFX();
        }
        else
        {
          SoundManager.Instance.PlayFailSFX();
          isRightComponent = false;
        }

        break;
      case GameManager.Engine:
        if (code == (int) CorrectMorseCode.Engine)
        {
          // FindObjectOfType<ObjectsManager>().localCollectedComponent.Engine = code;
          SoundManager.Instance.PlaySuccessSFX();
        }
        else
        {
          SoundManager.Instance.PlayFailSFX();
          isRightComponent = false;
        }

        break;
      case GameManager.Wings:
        if (code == (int) CorrectMorseCode.Wings)
        {
          // FindObjectOfType<ObjectsManager>().localCollectedComponent.Wings = code;
          SoundManager.Instance.PlaySuccessSFX();
        }
        else
        {
          SoundManager.Instance.PlayFailSFX();
          isRightComponent = false;
        }

        break;
      case GameManager.Propeller:
        if (code == (int) CorrectMorseCode.Propeller)
        {
          // FindObjectOfType<ObjectsManager>().localCollectedComponent.Propellers = code;
          SoundManager.Instance.PlaySuccessSFX();
        }
        else
        {
          SoundManager.Instance.PlayFailSFX();
          isRightComponent = false;
        }

        break;
      case GameManager.Wheels:
        if (code == (int) CorrectMorseCode.Wheels)
        {
          // FindObjectOfType<ObjectsManager>().localCollectedComponent.Wheels = code;
          SoundManager.Instance.PlaySuccessSFX();
        }
        else
        {
          SoundManager.Instance.PlayFailSFX();
          isRightComponent = false;
        }

        break;
      case GameManager.FuelTank:
        if (code == (int) CorrectMorseCode.FuelTank)
        {
          // FindObjectOfType<ObjectsManager>().localCollectedComponent.FuelTank = code;
          SoundManager.Instance.PlaySuccessSFX();
        }
        else
        {
          SoundManager.Instance.PlayFailSFX();
          isRightComponent = false;
        }

        break;
      case GameManager.Tail:
        if (code == (int) CorrectMorseCode.Tail)
        {
          // FindObjectOfType<ObjectsManager>().localCollectedComponent.Tail = code;
          SoundManager.Instance.PlaySuccessSFX();
        }
        else
        {
          SoundManager.Instance.PlayFailSFX();
          isRightComponent = false;
        }

        break;
    }

    if (!isRightComponent) return;

    if (!_isCollected)
    {
      _isCollected = true;
    }

    FindObjectOfType<ObjectsManager>().AddCollectedComponent();
    componentIcon.gameObject.SetActive(true);
    background.gameObject.SetActive(false);
    // for switching collected components
    ImageRecognition imageRecognition = FindObjectOfType<ImageRecognition>();
    imageRecognition.HideUsedObj(imageRecognition.GetPrefabName(currCode));
    // collectedCode = currCode;
  }

  // public void UpdateSprite(int code)
  // {
  //   if (code < 0) return;
  //   if (code >= 12) return;
  //   componentIcon.sprite = collectPanel.GetCandidateSprite(code);
  //   componentIcon.gameObject.SetActive(true);
  //   _isCollected = true;
  // }
}