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

  private int _randomSeed;

  private void Start()
  {
    _randomSeed = Random.Range(0, answer.Length);
    InitializeSelectionBox(_randomSeed);
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
    StartCoroutine(ToNextComponent(o));
  }

  private IEnumerator ToNextComponent(GameObject o)
  {
    float timer = 0;
    float offset = 10f;
    o.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 213f / 255f);
    while (timer <= 2)
    {
      var position = o.transform.position;
      Vector3 oldPosition = position;
      Vector3 newPosition = new Vector3(position.x, position.y + offset, position.z);
      o.transform.position = newPosition;
      offset *= -1;
      timer += 0.3f;
      yield return new WaitForSeconds(0.3f);
    }

    o.GetComponent<Image>().color = Color.white;
    GameManager.Instance.ChangeSceneTo(sceneWhenSuccess);
  }

  private void OnSelectionFalse(GameObject o)
  {
    SoundManager.Instance.PlaySFXByIndex(SFXList.Fail);
    StartCoroutine(StartShimmer(o));
  }

  private IEnumerator StartShimmer(GameObject o)
  {
    float timer = 0;
    float offset = 10f;
    // o.GetComponent<Image>().color = Color.red;
    o.GetComponent<Image>().color = new Color(255f / 255f, 165f / 255f, 165f / 255f);
    while (timer <= 2)
    {
      var position = o.transform.position;
      Vector3 oldPosition = position;
      Vector3 newPosition = new Vector3(position.x + offset, position.y, position.z);
      o.transform.position = newPosition;
      offset *= -1;
      timer += 0.1f;
      yield return new WaitForSeconds(0.1f);
    }

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