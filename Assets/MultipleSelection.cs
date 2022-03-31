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

  private int _randomSeed;

  private void Start()
  {
    _randomSeed = Random.Range(0, answer.Length);
    InitializeSelectionBox(_randomSeed);
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
      Debug.Log("right choice");
    }
    else
    {
      Debug.Log("wrong choice");
    }
  }
}