using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CharacterMoodIndex : int
{
  Alert_1 = 0,
  Alert_2 = 1,
  Encouraging_1 = 2,
  Encouraging_2 = 3,
  Encouraging_3 = 4,
  Encouraging_4 = 5,
  Excited_1 = 6,
  Excited_2 = 7,
  Excited_3 = 8,
  Grateful_1 = 9,
  Grateful_2 = 10,
  Greeting_1 = 11,
  Greeting_2 = 12,
  Instructive_1 = 13,
  Instructive_2 = 14,
  Instructive_3 = 15,
  Instructive_4 = 16,
  Praise_1 = 17,
  Praise_2 = 18,
  Praise_3 = 19,
  Praise_4 = 20,
  Praise_5 = 21,
  Proud_1 = 22,
  Proud_2 = 23,
  SuperExcited_1 = 24,
  SuperExcited_2 = 25,
  Thinking_1 = 26,
  Thinking_2 = 27,
  Thinking_3 = 28,
  Thinking_4 = 29,
}

[Serializable]
public struct ScriptElement
{
  public string script;
  public CharacterMoodIndex MoodIndex;
  public Sprite slideImage;
  public bool showPrev;
  public bool showNext;
  public bool showNextScene;
  public bool hideAtStart;
}

public class CharacterManager : MonoBehaviour
{
  public static CharacterManager Instance = null;

  [SerializeField] public Sprite[] characterMoodList;
  [SerializeField] private ScriptElement[] scriptElementsIntro;
  [SerializeField] private ScriptElement[] scriptElementsSortOne;
  [SerializeField] private ScriptElement[] scriptElementsSortTwo;
  [SerializeField] private ScriptElement[] scriptElementsFinishSelect;
  [SerializeField] private ScriptElement[] scriptElementsHunt;
  [SerializeField] private ScriptElement[] scriptElementsAssembly;
  [SerializeField] private ScriptElement[] scriptElementsFly;
  [SerializeField] private ScriptElement[] scriptElementsReaction;
  [SerializeField] private ScriptElement[] scriptElementsWingsOne;
  [SerializeField] private ScriptElement[] scriptElementsWingsTwo;
  [SerializeField] private ScriptElement[] scriptElementsFuelTank;
  [SerializeField] private ScriptElement[] scriptElementsPropeller;
  [SerializeField] private ScriptElement[] scriptElementsEngine;

  private void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
    }
    else
    {
      Instance = this;
    }
  }

  public ScriptElement GetScriptElement(int index)
  {
    int sceneIndex = GameManager.Instance.GetCurrentSceneIndex();
    return sceneIndex switch
    {
      (int) SceneIndex.Intro => scriptElementsIntro[index],
      (int) SceneIndex.SortOne => scriptElementsSortOne[index],
      (int) SceneIndex.SortTwo => scriptElementsSortTwo[index],
      (int) SceneIndex.FinishSelect => scriptElementsFinishSelect[index],
      (int) SceneIndex.Hunt => scriptElementsHunt[index],
      (int) SceneIndex.Assembly => scriptElementsAssembly[index],
      (int) SceneIndex.Fly => scriptElementsFly[index],
      (int) SceneIndex.Engine => scriptElementsEngine[index],
      (int) SceneIndex.WingsOne => scriptElementsWingsOne[index],
      (int) SceneIndex.WingsTwo => scriptElementsWingsTwo[index],
      (int) SceneIndex.Propeller => scriptElementsPropeller[index],
      (int) SceneIndex.FuelTank => scriptElementsFuelTank[index],
      _ => throw new ArgumentOutOfRangeException()
    };
  }

  public int GetScriptLength()
  {
    int sceneIndex = GameManager.Instance.GetCurrentSceneIndex();
    return sceneIndex switch
    {
      (int) SceneIndex.Intro => scriptElementsIntro.Length,
      (int) SceneIndex.SortOne => scriptElementsSortOne.Length,
      (int) SceneIndex.SortTwo => scriptElementsSortTwo.Length,
      (int) SceneIndex.FinishSelect=> scriptElementsFinishSelect.Length,
      (int) SceneIndex.Hunt => scriptElementsHunt.Length,
      (int) SceneIndex.Assembly => scriptElementsAssembly.Length,
      (int) SceneIndex.Fly => scriptElementsFly.Length,
      (int) SceneIndex.Engine => scriptElementsEngine.Length,
      (int) SceneIndex.WingsOne => scriptElementsWingsOne.Length,
      (int) SceneIndex.WingsTwo => scriptElementsWingsTwo.Length,
      (int) SceneIndex.Propeller => scriptElementsPropeller.Length,
      (int) SceneIndex.FuelTank => scriptElementsFuelTank.Length,
      _ => 0
    };
  }

  public ScriptElement GetCollectReactionScript(int index)
  {
    return scriptElementsReaction[index];
  }

  public Sprite GetCharacterMood(CharacterMoodIndex index)
  {
    return characterMoodList[(int) index];
  }
}