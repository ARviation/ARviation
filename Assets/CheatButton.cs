using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CheatButton : MonoBehaviour
{
  [SerializeField] private GameObject cheatPanel;
  [SerializeField] private Button cheatPanelBtn;
  [SerializeField] private string correctPassword;
  [SerializeField] private TMP_Text passwordBox;
  [SerializeField] private Sprite disableImg;
  [SerializeField] private Sprite enableImg;

  [SerializeField] private float feedbackDuration = 1.5f;

  private void Start()
  {
    cheatPanel.SetActive(false);
  }

  private void Update()
  {
    if (passwordBox.text.Length >= 2)
    {
      cheatPanelBtn.GetComponent<Image>().sprite = enableImg;
    }
    else
    {
      cheatPanelBtn.GetComponent<Image>().sprite = disableImg;
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

  public void OnClickConfirmCheat()
  {
    bool isValid = (passwordBox.text.Length - 1 == correctPassword.Length) &&
                   (string.CompareOrdinal(passwordBox.text.Substring(0, correctPassword.Length), correctPassword) == 0);
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