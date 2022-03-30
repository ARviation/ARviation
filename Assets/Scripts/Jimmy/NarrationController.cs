using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class NarrationController : MonoBehaviour
{
  [SerializeField] private float delay = 0.05f;
  [SerializeField] private GameObject textDisplay;
  [SerializeField] private Button btnForNextScene;
  [SerializeField] private Button btnForNextSentence;
  [SerializeField] private TMP_Text txtForNextSentence;
  [SerializeField] private Button buttonToHide;
  [SerializeField] private Image characterHolder;
  [SerializeField] private GameObject dialogObj;
  [SerializeField] private bool hasCondition = false;
  [SerializeField] private int conditionIndex;
  [SerializeField] private bool canHide = false;
  [SerializeField] private bool fuselageTutorial = false;
  [SerializeField] private bool hasImageHoder;
  [SerializeField] private Image imageHolder;

  private string _currentScript = "";
  private string _displayScript = "";
  private int _currentScriptInd = 0;
  private int _scriptLength = 0;
  private bool _isPlaying = false;
  private bool _isFinal = false;

  private void Start()
  {
    _currentScriptInd = 0;
    btnForNextScene.gameObject.SetActive(false);
    _scriptLength = CharacterManager.Instance.GetScriptLength();
    ShowNextLine();
  }

  public void OnNextClick()
  {
    if (_isPlaying || _currentScriptInd >= (_scriptLength - 1)) return;
    if (canHide)
    {
      HideDialog();
    }
    else
    {
      _currentScriptInd++;
      SoundManager.Instance.PlaySFXByIndex(SFXList.Click);
      ShowNextLine();
    }
  }

  public void OnSkipClick()
  {
    if (!_isPlaying && _currentScriptInd < (_scriptLength - 1))
    {
      _currentScriptInd = _scriptLength - 1;
      ShowNextLine();
    }
  }

  private void ShowNextLine()
  {
    ScriptElement scriptElement = CharacterManager.Instance.GetScriptElement(_currentScriptInd);
    if (hasImageHoder)
    {
      Sprite slideImage = scriptElement.slideImage;
      if (slideImage != null)
      {
        imageHolder.gameObject.SetActive(true);
        imageHolder.sprite = slideImage;
        float iWidth = slideImage.rect.width;
        float iHeight = slideImage.rect.height;
        float ratio = iWidth / iHeight;
        iHeight = Mathf.Clamp(iHeight, 320f, 800f);
        iWidth = iHeight * ratio;
        imageHolder.rectTransform.sizeDelta = new Vector2(iWidth, iHeight);
      }
      else
      {
        imageHolder.gameObject.SetActive(false);
      }
    }
    else
    {
      imageHolder.gameObject.SetActive(false);
    }

    
    btnForNextSentence.gameObject.SetActive(false);
    _isFinal = _currentScriptInd == (_scriptLength - 1);
    _currentScript = scriptElement.script;
    txtForNextSentence.text = scriptElement.nextBtnText;
    characterHolder.sprite = CharacterManager.Instance.GetCharacterMood(scriptElement.MoodIndex);
    SoundManager.Instance.PlayVoiceOver(_currentScriptInd);
    StartCoroutine(ShowText());
  }

  private IEnumerator ShowText()
  {
    _isPlaying = true;
    for (int i = 0; i <= _currentScript.Length; i++)
    {
      _displayScript = _currentScript.Substring(0, i);
      textDisplay.GetComponent<TMP_Text>().text = _displayScript;
      yield return new WaitForSeconds(delay);
    }

    _isPlaying = false;

    if (_isFinal)
    {
      if (fuselageTutorial)
      {
        Debug.Log("finish tutorial");
        FindObjectOfType<ImageRecognition>().FinishTutorial();
      }

      btnForNextSentence.gameObject.SetActive(false);
      btnForNextScene.gameObject.SetActive(true);
    }
    else
    {
      if (!hasCondition || _currentScriptInd != conditionIndex)
      {
        btnForNextSentence.gameObject.SetActive(true);
      }
      else
      {
        dialogObj.gameObject.SetActive(false);
      }
    }
  }

  public void SetHasConditionFalse()
  {
    hasCondition = false;
    btnForNextSentence.gameObject.SetActive(true);
    dialogObj.gameObject.SetActive(true);
  }

  public void OnFinishClick()
  {
    HideDialog();
  }

  public void HideDialog()
  {
    dialogObj.SetActive(false);
  }

  public void RevealDialog()
  {
    dialogObj.SetActive(true);
    canHide = false;
    _currentScriptInd++;
    ShowNextLine();
    buttonToHide.gameObject.SetActive(false);
  }
}