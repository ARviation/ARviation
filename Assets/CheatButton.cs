using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CheatButton : MonoBehaviour
{
  [SerializeField] private GameObject cheatPanel;
  [SerializeField] private Button cheatPanelBtnHunt;
  [SerializeField] private Button cheatPanelBtnFly;
  [SerializeField] private string correctPassword;
  [SerializeField] private TMP_Text passwordBoxHunt;
  [SerializeField] private TMP_Text passwordBoxFly;
  [SerializeField] private Sprite disableImg;
  [SerializeField] private Sprite enableImg;

  [SerializeField] private float feedbackDuration = 1.5f;

  private void Start()
  {
    cheatPanel.SetActive(false);
  }

  private void Update()
  {
    if (passwordBoxHunt.text.Length >= 2)
    {
      cheatPanelBtnHunt.GetComponent<Image>().sprite = enableImg;
    }
    else
    {
      cheatPanelBtnHunt.GetComponent<Image>().sprite = disableImg;
    }

    if (passwordBoxFly.text.Length >= 2)
    {
      cheatPanelBtnFly.GetComponent<Image>().sprite = enableImg;
    }
    else
    {
      cheatPanelBtnFly.GetComponent<Image>().sprite = disableImg;
    }
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

  public void OnClickConfirmCheatHunt()
  {
    bool isValid = (passwordBoxHunt.text.Length - 1 == correctPassword.Length) &&
                   (string.CompareOrdinal(passwordBoxHunt.text.Substring(0, correctPassword.Length), correctPassword) == 0);
    if (!isValid)
    {
      SoundManager.Instance.PlaySFXByIndex(SFXList.Click);
      StartCoroutine(WrongChoice(cheatPanel));
      return;
    }

    SoundManager.Instance.PlaySFXByIndex(SFXList.Click);
    GameManager.Instance.ChangeSceneToHunt();
  }

  public void OnClickConfirmCheatFly()
  {
    bool isValid = (passwordBoxFly.text.Length - 1 == correctPassword.Length) &&
                   (string.CompareOrdinal(passwordBoxFly.text.Substring(0, correctPassword.Length), correctPassword) == 0);
    if (!isValid)
    {
      SoundManager.Instance.PlaySFXByIndex(SFXList.Click);
      StartCoroutine(WrongChoice(cheatPanel));
      return;
    }

    SoundManager.Instance.PlaySFXByIndex(SFXList.Click);
    GameManager.Instance.ChangeSceneToFly();
  }

  private IEnumerator WrongChoice(GameObject o)
  {
    float timer = 0;
    float offset = 10f;
    while (timer <= feedbackDuration)
    {
      var position = o.transform.position;
      Vector3 newPosition = new Vector3(position.x + offset, position.y, position.z);
      o.transform.position = newPosition;
      offset *= -1;
      timer += 0.1f;

      yield return new WaitForSeconds(0.1f);
    }
  }
}