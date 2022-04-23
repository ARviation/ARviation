using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheatButton : MonoBehaviour
{
  [SerializeField] private GameObject cheatPanel;
  [SerializeField] private string correctPassword;
  [SerializeField] private TMP_Text passwordBox;

  private void Start()
  {
    cheatPanel.SetActive(false);
  }

  public void OnClickBtn()
  {
    SoundManager.Instance.PlaySFXByIndex(SFXList.Click);
    if (cheatPanel.activeSelf)
    {
      cheatPanel.SetActive(false);
    }
    else
    {
      cheatPanel.SetActive(true);
    }
  }

  public void OnClickConfirmCheat()
  {
    if (passwordBox.text.Length < correctPassword.Length) return;
    if (string.CompareOrdinal(passwordBox.text.Substring(0, correctPassword.Length), correctPassword) != 0) return;
    SoundManager.Instance.PlaySFXByIndex(SFXList.Click);
    GameManager.Instance.ChangeSceneToFly();
  }
}