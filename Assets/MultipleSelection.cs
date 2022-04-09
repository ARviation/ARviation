using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Serializable]
public struct Selection
{
  public Sprite selectionImage;
  public bool correctness;
}

public class MultipleSelection : MonoBehaviour
{
  [SerializeField] private Selection[] answer;
  [SerializeField] private Image[] selectionBox;
  [SerializeField] private SceneIndex sceneWhenSuccess;
  [SerializeField] private float feedbackDuration = 1.5f;
  [SerializeField] private bool showSelectionAtBegin = false;

  private bool _canTriggerFeedback = true;
  private int _randomSeed;

  private void Start()
  {
    _randomSeed = Random.Range(0, answer.Length);
    InitializeSelectionBox(_randomSeed);
    if (!showSelectionAtBegin)
      HideSelection();
  }

  private void InitializeSelectionBox(int randomSeed)
  {
    int length = answer.Length;
    for (int i = 0; i < length; i++)
    {
      selectionBox[i].sprite = answer[(i + randomSeed) % length].selectionImage;
      selectionBox[i].transform.parent.name = answer[(i + randomSeed) % length].correctness.ToString();
    }
  }

  public void OnClickChoice(GameObject o)
  {
    if (!_canTriggerFeedback) return;
    if (o.name == "True")
    {
      OnSelectionTrue(o);
    }
    else
    {
      OnSelectionFalse(o);
    }
  }

  private void OnSelectionTrue(GameObject o)
  {
    SoundManager.Instance.PlaySFXByIndex(SFXList.Success);
    StartCoroutine(CorrectChoice(o));
  }

  private IEnumerator CorrectChoice(GameObject o)
  {
    float timer = 0;
    float offset = 20f;
    o.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 213f / 255f);
    _canTriggerFeedback = false;
    while (timer <= feedbackDuration)
    {
      var position = o.transform.position;
      Vector3 newPosition = new Vector3(position.x, position.y + offset, position.z);
      o.transform.position = newPosition;
      offset *= -1;
      timer += 0.3f;
      yield return new WaitForSeconds(0.3f);
    }

    _canTriggerFeedback = true;
    o.GetComponent<Image>().color = Color.white;
    GameManager.Instance.ChangeSceneTo(sceneWhenSuccess);
  }

  private void OnSelectionFalse(GameObject o)
  {
    SoundManager.Instance.PlaySFXByIndex(SFXList.FailTwo);
    StartCoroutine(WrongChoice(o));
  }

  private IEnumerator WrongChoice(GameObject o)
  {
    float timer = 0;
    float offset = 10f;
    o.GetComponent<Image>().color = new Color(255f / 255f, 165f / 255f, 165f / 255f);
    _canTriggerFeedback = false;
    while (timer <= feedbackDuration)
    {
      var position = o.transform.position;
      Vector3 newPosition = new Vector3(position.x + offset, position.y, position.z);
      o.transform.position = newPosition;
      offset *= -1;
      timer += 0.1f;
      yield return new WaitForSeconds(0.1f);
    }

    _canTriggerFeedback = true;
    o.GetComponent<Image>().color = Color.white;
  }

  public void ShowSelection()
  {
    foreach (Image image in selectionBox)
    {
      image.transform.parent.gameObject.SetActive(true);
    }
  }

  public void HideSelection()
  {
    foreach (Image image in selectionBox)
    {
      image.transform.parent.gameObject.SetActive(false);
    }
  }
}