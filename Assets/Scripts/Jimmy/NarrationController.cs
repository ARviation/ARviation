using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

// TODO: clean useless code and comments after finish implementing all narration function
public class NarrationController : MonoBehaviour
{
  [SerializeField] private float defaultDelay = 0.05f;
  [SerializeField] private GameObject textDisplay;
  [SerializeField] private Button btnForNextScene;
  [SerializeField] private Button btnForNextSentence;
  [SerializeField] private Button btnForPrevSentence;
  [SerializeField] private Button btnToReveal;
  [SerializeField] private Image characterHolder;
  [SerializeField] private GameObject dialogueObj;
  [SerializeField] private bool hasHideCondition = false;
  [SerializeField] private int hideConditionIndex;
  [SerializeField] private Image hideBackground;
  [SerializeField] private ARCameraManager arCamera;
  [SerializeField] private bool hasImageHoder;
  [SerializeField] private Image imageHolder;
  [SerializeField] private bool hasSelection = false;
  [SerializeField] private bool isHunting = false;
  [SerializeField] private int huntingMorseIndex;
  [SerializeField] private int huntingHideIndex;

  private float delay;
  private string _currentScript = "";
  private string _displayScript = "";
  private int _currentScriptInd = 0;
  private int _scriptLength = 0;
  private bool _isPlaying = false;
  private bool _isFinal = false;
  private bool isMorsePlay = false;

  private void Start()
  {
    _currentScriptInd = 0;
    delay = defaultDelay;
    if (btnForNextScene != null)
      btnForNextScene.gameObject.SetActive(false);
    _scriptLength = CharacterManager.Instance.GetScriptLength();
    ShowNextLine();
  }

  private void Update()
  {
    if (hasHideCondition && _currentScriptInd == hideConditionIndex)
    {
      HideDialog();
      hideBackground.gameObject.SetActive(false);
    }

    if (isHunting && _currentScriptInd == huntingHideIndex)
    {
      HideDialog();
    }
  }

  public bool GetHadHideCondition()
  {
    return hasHideCondition;
  }

  public void OnNextClick()
  {
    delay = defaultDelay;
    if (_isPlaying || _currentScriptInd > (_scriptLength - 1)) return;
    SoundManager.Instance.PlaySFXByIndex(SFXList.Click);
    _currentScriptInd++;
    ShowNextLine();
  }

  public void OnPrevClick()
  {
    if (_currentScriptInd - 1 < 0) return;
    if (hasSelection)
      FindObjectOfType<MultipleSelection>().HideSelection();
    _currentScriptInd--;
    SoundManager.Instance.PlaySFXByIndex(SFXList.Click);
    ShowNextLine();
  }

  public void OnSkipClick()
  {
    delay -= 0.04f;
    if (_isPlaying || _currentScriptInd >= (_scriptLength - 1)) return;
    _currentScriptInd = _scriptLength - 1;
    ShowNextLine();
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

    btnForNextSentence.gameObject.SetActive(false);
    btnForPrevSentence.gameObject.SetActive(false);
    if (btnForNextScene != null)
      btnForNextScene.gameObject.SetActive(false);
    _isFinal = _currentScriptInd == (_scriptLength - 1);
    _currentScript = scriptElement.script;
    characterHolder.sprite = CharacterManager.Instance.GetCharacterMood(scriptElement.MoodIndex);
    // SoundManager.Instance.PlayVoiceOver(_currentScriptInd);
    StartCoroutine(ShowText());
  }

  private IEnumerator ShowText()
  {
    _isPlaying = true;

    if (_isFinal)
    {
      if (hasSelection)
        FindObjectOfType<MultipleSelection>().ShowSelection();
    }

    for (int i = 0; i <= _currentScript.Length; i++)
    {
      _displayScript = _currentScript.Substring(0, i);
      textDisplay.GetComponent<TMP_Text>().text = _displayScript;
      yield return new WaitForSeconds(delay);
    }

    _isPlaying = false;

    if (isHunting && _currentScriptInd == huntingMorseIndex && !isMorsePlay)
    {
      SoundManager.Instance.PlaySFXByMorseCode(MorseCode.A);
      isMorsePlay = true;
    }

    if (_isFinal)
    {
      if (_currentScriptInd != 0)
        btnForPrevSentence.gameObject.SetActive(true);
      if (btnForNextScene != null)
        btnForNextScene.gameObject.SetActive(true);
    }
    else
    {
      if (!hasHideCondition || _currentScriptInd != hideConditionIndex)
      {
        btnForNextSentence.gameObject.SetActive(true);
        if (_currentScriptInd != 0)
          btnForPrevSentence.gameObject.SetActive(true);
      }
      else
      {
        HideDialog();
      }
    }
  }

  public void SetHasConditionFalse()
  {
    hasHideCondition = false;
    btnForNextSentence.gameObject.SetActive(true);
    btnForPrevSentence.gameObject.SetActive(true);
    RevealDialog();
  }

  public void HideDialog()
  {
    dialogueObj.SetActive(false);
  }

  public void RevealDialog(bool showPrevBtn = true)
  {
    dialogueObj.SetActive(true);
    btnForPrevSentence.gameObject.SetActive(showPrevBtn);
    _currentScriptInd++;
    ShowNextLine();
    // btnToReveal.gameObject.SetActive(false);
  }

  public void CollectWrong()
  {
    SoundManager.Instance.PlaySFXByIndex(SFXList.Fail);
  }

  public void CollectRight()
  {
    SoundManager.Instance.PlaySFXByIndex(SFXList.Success);
  }
}