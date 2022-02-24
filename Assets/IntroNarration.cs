using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IntroNarration : MonoBehaviour
{
  [SerializeField] private float delay = 0.1f;
  [SerializeField] private string[] fullScripts;
  [SerializeField] private GameObject textDisplay;
  [SerializeField] private float destroyAfterSeconds = 2.0f;
  [SerializeField] private Button _buttonForNextScene;
  [SerializeField] private Button _buttonForNextSent;

  private string currentText = "";
  private string tmpCurrentText = "";
  private int currentScriptInd = 0;
  private int scriptLength = 0;
  private bool isPlaying = false;
  private bool isFinal = false;

  private void Start()
  {
    _buttonForNextScene.gameObject.SetActive(false);
    scriptLength = fullScripts.Length;
    tmpCurrentText = fullScripts[currentScriptInd];
    StartCoroutine(ShowText());
  }

  private void Update()
  {
    isFinal = currentScriptInd == (scriptLength - 1);
  }

  public void OnNextClick()
  {
    if (!isPlaying && currentScriptInd < (fullScripts.Length - 1))
    {
      tmpCurrentText = fullScripts[++currentScriptInd];
      StartCoroutine(ShowText());
    }
  }

  public void OnSkipClip()
  {
    if (!isPlaying && currentScriptInd < (fullScripts.Length - 1))
    {
      currentScriptInd = fullScripts.Length - 1;
      tmpCurrentText = fullScripts[currentScriptInd];
      StartCoroutine(ShowText());
    }
  }

  IEnumerator ShowText()
  {
    isPlaying = true;
    for (int i = 0; i < tmpCurrentText.Length; i++)
    {
      currentText = tmpCurrentText.Substring(0, i);
      textDisplay.GetComponent<TMP_Text>().text = currentText;
      yield return new WaitForSeconds(delay);
    }

    isPlaying = false;

    if (isFinal)
    {
      _buttonForNextSent.gameObject.SetActive(false);
      _buttonForNextScene.gameObject.SetActive(true);
    }
  }
}