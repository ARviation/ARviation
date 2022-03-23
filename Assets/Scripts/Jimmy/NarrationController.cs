using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NarrationController : MonoBehaviour
{
  [SerializeField] private float delay = 0.05f;
  [SerializeField] private GameObject textDisplay;
  [SerializeField] private Button _buttonForNextScene;
  [SerializeField] private Button _buttonForNextSent;
  [SerializeField] private CharacterMoodIndex[] _moodIndices;
  [SerializeField] private Image characterHolder;
  [SerializeField] private GameObject dialogObj;
  [SerializeField] private bool hasCondition = false;
  [SerializeField] private int conditionIndex;
  [SerializeField] private bool canHide = false;

  private string currentScript = "";
  private string displayScript = "";
  private int currentScriptInd = 0;
  private int scriptLength = 0;
  private bool isPlaying = false;
  private bool isFinal = false;

  private void Start()
  {
    currentScriptInd = 0;
    _buttonForNextScene.gameObject.SetActive(false);
    scriptLength = CharacterManager.Instance.GetScriptLength();
    ShowNextLine();
  }

  public void OnNextClick()
  {
    if (isPlaying || currentScriptInd >= (scriptLength - 1)) return;
    if (canHide)
    {
      HideDialog();
    }
    else
    {
      currentScriptInd++;
      ShowNextLine();
    }
  }

  public void OnSkipClick()
  {
    if (!isPlaying && currentScriptInd < (scriptLength - 1))
    {
      currentScriptInd = scriptLength - 1;
      ShowNextLine();
    }
  }

  private void ShowNextLine()
  {
    _buttonForNextSent.gameObject.SetActive(false);
    isFinal = currentScriptInd == (scriptLength - 1);
    currentScript = CharacterManager.Instance.GetCharacterScript(currentScriptInd);
    SoundManager.Instance.PlayVoiceOver(currentScriptInd);
    characterHolder.sprite = CharacterManager.Instance.GetCharacterMood(_moodIndices[currentScriptInd]);
    StartCoroutine(ShowText());
  }

  private IEnumerator ShowText()
  {
    isPlaying = true;
    for (int i = 0; i < currentScript.Length; i++)
    {
      displayScript = currentScript.Substring(0, i);
      textDisplay.GetComponent<TMP_Text>().text = displayScript;
      yield return new WaitForSeconds(delay);
    }

    isPlaying = false;

    if (isFinal)
    {
      _buttonForNextSent.gameObject.SetActive(false);
      _buttonForNextScene.gameObject.SetActive(true);
    }
    else
    {
      if (!hasCondition || currentScriptInd != conditionIndex)
      {
        _buttonForNextSent.gameObject.SetActive(true);
      }
    }
  }

  public void SetHasConditionFalse()
  {
    hasCondition = false;
    _buttonForNextSent.gameObject.SetActive(true);
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
    currentScriptInd++;
    ShowNextLine();
  }
}