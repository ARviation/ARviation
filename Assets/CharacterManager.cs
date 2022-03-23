using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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

public class CharacterManager : MonoBehaviour
{
  public static CharacterManager Instance = null;

  [SerializeField] public Sprite[] characterMoodList;
  [SerializeField] private string[] fullScriptsIntro;
  [SerializeField] private string[] fullScriptsHunt;
  [SerializeField] private string[] fullScriptsAssembly;
  [SerializeField] private string[] fulllScriptsCollectReaction;
  [SerializeField] private string[] fullScriptsFly;


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


  public string GetCharacterScript(int index)
  {
    int sceneIndex = GameManager.Instance.GetCurrentSceneIndex();
    if (sceneIndex == (int) SceneIndex.Intro)
    {
      return fullScriptsIntro[index];
    }
    else if (sceneIndex == (int) SceneIndex.Hunt)
    {
      return fullScriptsHunt[index];
    }
    else if (sceneIndex == (int) SceneIndex.Assembly)
    {
      return fullScriptsAssembly[index];
    }
    else if (sceneIndex == (int) SceneIndex.Fly)
    {
      return fullScriptsFly[index];
    }

    return "";
  }

  public string GetCollectReactionScript(int index)
  {
    return fulllScriptsCollectReaction[index];
  }

  public int GetScriptLength()
  {
    int sceneIndex = GameManager.Instance.GetCurrentSceneIndex();
    if (sceneIndex == (int) SceneIndex.Intro)
    {
      return fullScriptsIntro.Length;
    }
    else if (sceneIndex == (int) SceneIndex.Hunt)
    {
      return fullScriptsHunt.Length;
    }
    else if (sceneIndex == (int) SceneIndex.Assembly)
    {
      return fullScriptsAssembly.Length;
    }
    else if (sceneIndex == (int) SceneIndex.Fly)
    {
      return fullScriptsFly.Length;
    }

    return 0;
  }

  public Sprite GetCharacterMood(CharacterMoodIndex index)
  {
    return characterMoodList[(int) index];
  }
}