using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroNarration : MonoBehaviour
{ 
  [SerializeField] private float delay = 0.1f;
  [SerializeField] private string[] fullScripts;
  [SerializeField] private GameObject textDisplay;
  [SerializeField] private float destroyAfterSeconds = 2.0f;

  private string currentText = "";
  private string tmpCurrentText = "";
  private int currentScriptInd = 0;
  private bool isPlaying = false;

  private void Start()
  {
    tmpCurrentText = fullScripts[currentScriptInd];
    StartCoroutine(ShowText());
  }

  public void OnNextClick()
  {
    if (!isPlaying && currentScriptInd < fullScripts.Length)
    {
      tmpCurrentText = fullScripts[++currentScriptInd];
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
  }
}
