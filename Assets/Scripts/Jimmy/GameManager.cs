using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneIndex : int
{
  Intro = 0,
  Sort = 1,
  FinishSelect = 2,
  Hunt = 3,
  Assembly = 4,
  Fly = 5,
  Wings = 6,
  FuelTank = 7,
  Propeller = 8,
  Engine = 9,
}

public class GameManager : MonoBehaviour
{
  public static GameManager Instance = null;
  public const string Fuselage = "Fuselage";
  public const string Engine = "Engine";
  public const string Wings = "Wings";
  public const string Propeller = "Propeller";
  public const string FuelTank = "FuelTank";
  public const string Wheels = "Wheels";
  public const string Tail = "Tail";

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

  private static void ChangeSceneTo(int index)
  {
    SoundManager.Instance.PlaySFXByIndex(SFXList.Click);
    SceneManager.LoadScene(index);
  }

  public void ChangeSceneTo(SceneIndex index)
  {
    ChangeSceneTo((int) index);
  }

  public void ChangeSceneToIntro()
  {
    SoundManager.Instance.PlaySFXByIndex(SFXList.Click);
    ChangeSceneTo((int) SceneIndex.Intro);
  }

  public void ChangeSceneToSort()
  {
    SoundManager.Instance.PlaySFXByIndex(SFXList.Click);
    ChangeSceneTo((int) SceneIndex.Sort);
  }

  public void ChangeSceneToFinishSelect()
  {
    SoundManager.Instance.PlaySFXByIndex(SFXList.Click);
    ChangeSceneTo((int) SceneIndex.FinishSelect);
  }

  public void ChangeSceneToHunt()
  {
    SoundManager.Instance.PlaySFXByIndex(SFXList.Click);
    ChangeSceneTo((int) SceneIndex.Hunt);
  }

  public void ChangeSceneToAssembly()
  {
    // if (FindObjectOfType<ObjectsManager>().GetCanPass())
    // {
    SoundManager.Instance.PlaySFXByIndex(SFXList.Click);
    // FindObjectOfType<ObjectsManager>().SaveCollectedComponent();
    ChangeSceneTo((int) SceneIndex.Assembly);
    // }
  }

  public void ChangeSceneToFly()
  {
    SoundManager.Instance.PlaySFXByIndex(SFXList.Click);
    ChangeSceneTo((int) SceneIndex.Fly);
  }

  public void ChangeSceneToEngine()
  {
    SoundManager.Instance.PlaySFXByIndex(SFXList.Click);
    ChangeSceneTo((int) SceneIndex.Engine);
  }

  public void ChangeSceneToWings()
  {
    SoundManager.Instance.PlaySFXByIndex(SFXList.Click);
    ChangeSceneTo((int) SceneIndex.Wings);
  }

  public void ChangeSceneToPropeller()
  {
    SoundManager.Instance.PlaySFXByIndex(SFXList.Click);
    ChangeSceneTo((int) SceneIndex.Propeller);
  }

  public void ChangeSceneToFuelTank()
  {
    SoundManager.Instance.PlaySFXByIndex(SFXList.Click);
    ChangeSceneTo((int) SceneIndex.FuelTank);
  }

  public int GetCurrentSceneIndex()
  {
    return SceneManager.GetActiveScene().buildIndex;
  }
}