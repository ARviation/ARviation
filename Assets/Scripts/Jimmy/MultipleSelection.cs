using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;

[Serializable]
public struct Selection
{
  public Sprite selectionImage;
  public bool correctness;
  public string stringtoShow;
  public CharacterMoodIndex _moodIndex;
}

public class MultipleSelection : MonoBehaviour
{
  [SerializeField] private Selection[] answer;
  [SerializeField] private Image[] selectionBox;
  [SerializeField] private SceneIndex sceneWhenSuccess;
  [SerializeField] private float feedbackDuration = 1.5f;
  [SerializeField] private bool showSelectionAtBegin = false;
  [SerializeField] private TMP_Text script;
  [SerializeField] private Image characterHolder;
  // [SerializeField] private string stringToShow;
  // [SerializeField] private CharacterMoodIndex _moodIndex;

  private NarrationController _narrationController;
  private bool _canTriggerFeedback = true;
  private int _randomSeed;

  private void Start()
  {
    _randomSeed = Random.Range(0, answer.Length);
    _narrationController = FindObjectOfType<NarrationController>();
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
      selectionBox[i].transform.parent.name =
        answer[(i + randomSeed) % length].correctness.ToString() + ((i + randomSeed) % length);
    }
  }

  public void OnClickChoice(GameObject o)
  {
    if (!_canTriggerFeedback) return;
    _narrationController.OnClickScriptBox();
    if (o.name.Substring(0, o.name.Length - 1) == "True")
    {
      OnSelectionTrue(o);
    }
    else
    {
      OnSelectionFalse(o, o.name.Substring(o.name.Length - 1, 1));
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

  private void OnSelectionFalse(GameObject o, string idxInSelect)
  {
    Selection selection = answer[int.Parse(idxInSelect)];
    SoundManager.Instance.PlaySFXByIndex(SFXList.FailTwo);
    characterHolder.sprite = CharacterManager.Instance.GetCharacterMood(selection._moodIndex);
    StartCoroutine(WrongChoice(o, selection.stringtoShow));
  }

  private IEnumerator WrongChoice(GameObject o, string stringToShow)
  {
    float timer = 0;
    float offset = 10f;
    o.GetComponent<Image>().color = new Color(255f / 255f, 165f / 255f, 165f / 255f);
    script.text = "";
    _canTriggerFeedback = false;
    while (timer <= feedbackDuration)
    {
      var position = o.transform.position;
      Vector3 newPosition = new Vector3(position.x + offset, position.y, position.z);
      o.transform.position = newPosition;
      offset *= -1;
      timer += 0.1f;

      script.text = stringToShow.Substring(0, (int) (stringToShow.Length * (timer / feedbackDuration)));

      yield return new WaitForSeconds(0.1f);
    }

    _canTriggerFeedback = true;
    o.GetComponent<Image>().color = Color.white;
  }

  // private IEnumerator ShowText(string script, bool showPrev, bool showNext, bool showNextScene)
  // {
  //   _isPlaying = true;
  //
  //   if (_isFinal)
  //   {
  //     if (hasSelection)
  //       FindObjectOfType<MultipleSelection>().ShowSelection();
  //   }
  //
  //   for (int i = 0; i <= script.Length; i++)
  //   {
  //     _displayScript = script.Substring(0, i);
  //     textDisplay.text = _displayScript;
  //     // textDisplay.GetComponent<TMP_Text>().text = _displayScript;
  //     yield return new WaitForSeconds(delay);
  //   }
  //
  //   _isPlaying = false;
  //
  //   if (isHunting && _currentScriptInd == huntingMorseIndex && !isMorsePlay)
  //   {
  //     // SoundManager.Instance.PlaySFXByMorseCode(MorseCode.A);
  //     isMorsePlay = true;
  //   }
  //
  //   btnForPrevSentence.gameObject.SetActive(showPrev);
  //   btnForNextSentence.gameObject.SetActive(showNext);
  //   if (btnForNextScene != null)
  //     btnForNextScene.gameObject.SetActive(showNextScene);
  // }

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