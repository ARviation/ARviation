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
  [SerializeField] private TMP_Text textDisplay;
  [SerializeField] private Button btnForNextScene;
  [SerializeField] private Button btnForNextSentence;
  [SerializeField] private Button btnForPrevSentence;
  [SerializeField] private Button btnToReveal;
  [SerializeField] private Image characterHolder;
  [SerializeField] private GameObject dialogueObj;
  [SerializeField] private bool hasHideCondition = false;
  [SerializeField] private int hideConditionIndex;
  [SerializeField] private Image hideBackground;
  [SerializeField] private Image scanImage;

  // [SerializeField] private ARCameraManager arCamera;
  [SerializeField] private bool hasImgHolder;
  [SerializeField] private float imgHolderMinHeight = 480.0f;
  [SerializeField] private float imgHolderMaxHeight = 800.0f;
  [SerializeField] private Image imgHolder;
  [SerializeField] private bool hasSelection = false;
  [SerializeField] private bool isHunting = false;
  [SerializeField] private int huntingMorseIndex;
  [SerializeField] private int huntingHideIndex;
  [SerializeField] private bool isAssembly = false;
  [SerializeField] private GameObject inventory;
  [SerializeField] private Button[] inventoryBtn;
  [SerializeField] private GameObject[] perspectiveBtn;
  [SerializeField] private int resumeInventoryIndex;
  [SerializeField] private Color highlightColor;
  [SerializeField] private Color originalColor;
  [SerializeField] private int highlightTap;
  [SerializeField] private int highlightInventory;
  [SerializeField] private int highlightPerspective;
  [SerializeField] private Image[] hintCovers;

  private float delay;

  // private string _currentScript = "";
  private string _displayScript = "";
  public int _currentScriptInd = 0;
  private int _scriptLength = 0;
  private bool _isPlaying = false;
  private bool _isFinal = false;
  private bool isMorsePlay = false;
  private ScriptElement scriptElement;
  private bool isScanning = false;
  private bool hasOpenScanImage = false;
  private bool showAll = false;
  private ImageRecognition _imageRecognition;
  private bool hasResumeInstruction = false;

  private void Start()
  {
    _currentScriptInd = 0;
    delay = GameManager.Instance.narratorSpeed;
    if (hasHideCondition && scanImage != null)
      scanImage.gameObject.SetActive(false);
    if (isHunting)
    {
      _imageRecognition = FindObjectOfType<ImageRecognition>();
    }

    if (isAssembly)
    {
    }

    DisableBtn();
    foreach (Image hintCover in hintCovers)
    {
      hintCover.gameObject.SetActive(false);
    }

    HideBtn();
    _scriptLength = CharacterManager.Instance.GetScriptLength();
    scriptElement = CharacterManager.Instance.GetScriptElement(_currentScriptInd);
    if (scriptElement.hideAtStart) return;
    ShowLine(scriptElement);
  }

  private void DisableBtn()
  {
    foreach (Button button in inventoryBtn)
    {
      button.enabled = false;
    }

    if (!isAssembly) return;
    foreach (GameObject o in perspectiveBtn)
    {
      o.GetComponent<Button>().enabled = false;
    }
  }

  private void EnableBtn()
  {
    foreach (Button button in inventoryBtn)
    {
      button.enabled = true;
    }

    if (!isAssembly) return;
    foreach (GameObject button in perspectiveBtn)
    {
      button.GetComponent<Button>().enabled = true;
    }
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
    // if (hasHideCondition && _currentScriptInd == hideConditionIndex)
    // {
    //   
    // }

    if (isHunting)
    {
      Debug.Log("isScanning: " + isScanning);
      scanImage.gameObject.SetActive(isScanning);
      // if (isScanning)
      // {
      //   
      // }
      // else
      // {
      //   _imageRecognition.StopARDetect();
      // }

      if (_currentScriptInd == hideConditionIndex && !hasResumeInstruction)
      {
        hideBackground.gameObject.SetActive(false);
        if (!hasOpenScanImage)
        {
          OpenScanImage();
          hasOpenScanImage = true;
        }

        _imageRecognition.StartARDetect();
      }

      if (_currentScriptInd == highlightTap - 1)
      {
        isScanning = false;
      }

      if (_currentScriptInd == highlightTap)
      {
        inventory.GetComponent<Image>().color = highlightColor;
        hintCovers[0].gameObject.SetActive(true);
      }

      if (_currentScriptInd == highlightTap + 1)
      {
        inventory.GetComponent<Image>().color = originalColor;
        hideBackground.gameObject.SetActive(false);
        hintCovers[0].gameObject.SetActive(false);
        if (!hasOpenScanImage)
        {
          OpenScanImage();
          hasOpenScanImage = true;
        }

        _imageRecognition.StartARDetect();
        EnableBtn();
      }
    }

    if (isAssembly)
    {
      if (_currentScriptInd == highlightInventory)
      {
        foreach (GameObject o in perspectiveBtn)
        {
          o.gameObject.SetActive(false);
        }

        inventory.GetComponent<Image>().color = highlightColor;
        hintCovers[0].gameObject.SetActive(true);
      }

      if (_currentScriptInd == highlightPerspective)
      {
        inventory.GetComponent<Image>().color = originalColor;
        foreach (GameObject o in perspectiveBtn)
        {
          o.gameObject.SetActive(true);
          o.GetComponent<Image>().color = highlightColor;
        }

        hintCovers[0].gameObject.SetActive(false);
        hintCovers[1].gameObject.SetActive(true);
      }

      if (_currentScriptInd == highlightPerspective + 1)
      {
        foreach (GameObject o in perspectiveBtn)
        {
          o.GetComponent<Image>().color = originalColor;
        }

        hintCovers[1].gameObject.SetActive(false);
        hintCovers[2].gameObject.SetActive(true);
      }

      if (_currentScriptInd == resumeInventoryIndex)
      {
        hintCovers[2].gameObject.SetActive(false);
        EnableBtn();
      }
    }
  }

  public void ShowInstructionForHunting()
  {
    hideBackground.gameObject.SetActive(true);
    _imageRecognition.StopARDetect();
    hasResumeInstruction = true;
    Debug.Log("Show Instruction For Hunting");
  }

  public void OpenScanImage()
  {
    isScanning = true;
  }

  public void CloseScanImage()
  {
    isScanning = false;
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
    _currentScriptInd++;
    scriptElement = CharacterManager.Instance.GetScriptElement(_currentScriptInd);

    HideBtn();

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

    _currentScriptInd--;
    scriptElement = CharacterManager.Instance.GetScriptElement(_currentScriptInd);
    ShowLine(scriptElement);
  }

  public void OnClickScriptBox()
  {
    showAll = true;
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
    if (hasImgHolder)
    {
      ImageHolderAutoSize();
    }
    else
    {
      imgHolder.gameObject.SetActive(false);
    }

    showAll = false;
    _isFinal = _currentScriptInd == (_scriptLength - 1);
    characterHolder.sprite = CharacterManager.Instance.GetCharacterMood(scriptElement.MoodIndex);
    SoundManager.Instance.PlayVoiceOver(_currentScriptInd);
    if (_currentScriptInd != 0)
      SoundManager.Instance.PlaySFXByIndex(SFXList.Click);
    StartCoroutine(ShowText(scriptElement.script, scriptElement.showPrev, scriptElement.showNext,
      scriptElement.showNextScene));
  }

  private void ImageHolderAutoSize()
  {
    Sprite slideImage = scriptElement.slideImage;
    if (slideImage != null)
    {
      imgHolder.gameObject.SetActive(true);
      imgHolder.sprite = slideImage;
      float iWidth = slideImage.rect.width;
      float iHeight = slideImage.rect.height;
      float ratio = iWidth / iHeight;
      iHeight = Mathf.Clamp(iHeight, imgHolderMinHeight, imgHolderMaxHeight);
      iWidth = iHeight * ratio;
      imgHolder.rectTransform.sizeDelta = new Vector2(iWidth, iHeight);
    }
    else
    {
      imgHolder.gameObject.SetActive(false);
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
      if (showAll)
      {
        _displayScript = script;
        textDisplay.text = _displayScript;
      }
      else
      {
        _displayScript = script.Substring(0, i);
        textDisplay.text = _displayScript;
        yield return new WaitForSeconds(delay);
      }
    }

    _isPlaying = false;

    if (isHunting && _currentScriptInd == huntingMorseIndex && !isMorsePlay)
    {
      isMorsePlay = true;
    }

    btnForPrevSentence.gameObject.SetActive(showPrev);
    btnForNextSentence.gameObject.SetActive(showNext);
    if (showNext)
      btnForNextSentence.GetComponent<NextSentenceButton>().StartShakingEffect();
    if (btnForNextScene != null)
      btnForNextScene.gameObject.SetActive(showNextScene);
  }

  public bool GetIsPlaying()
  {
    return _isPlaying;
  }

  public void HideDialog()
  {
    dialogueObj.SetActive(false);
  }
}