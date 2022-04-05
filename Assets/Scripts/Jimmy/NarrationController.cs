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

  // [SerializeField] private ARCameraManager arCamera;
  [SerializeField] private bool hasImageHoder;
  [SerializeField] private Image imageHolder;
  [SerializeField] private bool hasSelection = false;
  [SerializeField] private bool isHunting = false;
  [SerializeField] private int huntingMorseIndex;
  [SerializeField] private int huntingHideIndex;

  private float delay;

  // private string _currentScript = "";
  private string _displayScript = "";
  public int _currentScriptInd = 0;
  private int _scriptLength = 0;
  private bool _isPlaying = false;
  private bool _isFinal = false;
  private bool isMorsePlay = false;
  private ScriptElement scriptElement;

  private void Start()
  {
    _currentScriptInd = 0;
    delay = defaultDelay;
    HideBtn();
    _scriptLength = CharacterManager.Instance.GetScriptLength();
    scriptElement = CharacterManager.Instance.GetScriptElement(_currentScriptInd);
    if (scriptElement.hideAtStart) return;
    ShowLine(scriptElement);
  }

  private void HideBtn()
  {
    btnForPrevSentence.gameObject.SetActive(false);
    btnForNextSentence.gameObject.SetActive(false);
    if (btnForNextScene != null)
      btnForNextScene.gameObject.SetActive(false);
  }

  private void Update()
  {
    if (hasHideCondition && _currentScriptInd == hideConditionIndex)
    {
      hideBackground.gameObject.SetActive(false);
    }
  }

  public void RemoveHadHideCondition()
  {
    hasHideCondition = false;
  }

  public bool GetHadHideCondition()
  {
    return hasHideCondition;
  }

  public void OnNextClick()
  {
    if (_isPlaying || _currentScriptInd >= (_scriptLength - 1)) return;
    HideBtn();
    delay = defaultDelay;
    SoundManager.Instance.PlaySFXByIndex(SFXList.Click);

    _currentScriptInd++;
    scriptElement = CharacterManager.Instance.GetScriptElement(_currentScriptInd);
    if (scriptElement.hideAtStart)
    {
      HideDialog();
      return;
    }

    ShowLine(scriptElement);
  }

  public void OnPrevClick()
  {
    if (_isPlaying || _currentScriptInd - 1 < 0) return;
    HideBtn();
    if (hasSelection)
      FindObjectOfType<MultipleSelection>().HideSelection();
    SoundManager.Instance.PlaySFXByIndex(SFXList.Click);

    _currentScriptInd--;
    scriptElement = CharacterManager.Instance.GetScriptElement(_currentScriptInd);
    ShowLine(scriptElement);
  }

  public void OnSkipClick()
  {
    delay -= 0.04f;
    if (_isPlaying || _currentScriptInd >= (_scriptLength - 1)) return;
    HideBtn();
    _currentScriptInd = _scriptLength - 1;
    scriptElement = CharacterManager.Instance.GetScriptElement(_currentScriptInd);
    ShowLine(scriptElement);
  }

  public void RevealDialog()
  {
    dialogueObj.SetActive(true);
    scriptElement = CharacterManager.Instance.GetScriptElement(_currentScriptInd);
    ShowLine(scriptElement);
  }

  private void ShowLine(ScriptElement scriptElement)
  {
    if (hasImageHoder)
    {
      ImageHolderAutoSize();
    }

    _isFinal = _currentScriptInd == (_scriptLength - 1);
    characterHolder.sprite = CharacterManager.Instance.GetCharacterMood(scriptElement.MoodIndex);
    // SoundManager.Instance.PlayVoiceOver(_currentScriptInd);
    StartCoroutine(ShowText(scriptElement.script, scriptElement.showPrev, scriptElement.showNext,
      scriptElement.showNextScene));
  }

  private void ImageHolderAutoSize()
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

  private IEnumerator ShowText(string script, bool showPrev, bool showNext, bool showNextScene)
  {
    _isPlaying = true;

    if (_isFinal)
    {
      if (hasSelection)
        FindObjectOfType<MultipleSelection>().ShowSelection();
    }

    for (int i = 0; i <= script.Length; i++)
    {
      _displayScript = script.Substring(0, i);
      textDisplay.GetComponent<TMP_Text>().text = _displayScript;
      yield return new WaitForSeconds(delay);
    }

    _isPlaying = false;

    if (isHunting && _currentScriptInd == huntingMorseIndex && !isMorsePlay)
    {
      SoundManager.Instance.PlaySFXByMorseCode(MorseCode.A);
      isMorsePlay = true;
    }

    btnForPrevSentence.gameObject.SetActive(showPrev);
    btnForNextSentence.gameObject.SetActive(showNext);
    if (btnForNextScene != null)
      btnForNextScene.gameObject.SetActive(showNextScene);
  }

  public void HideDialog()
  {
    dialogueObj.SetActive(false);
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