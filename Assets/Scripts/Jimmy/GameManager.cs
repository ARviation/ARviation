using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneIndex : int
{
  Intro = 0,
  SortOne = 1,
  SortTwo = 2,
  FinishSelect = 3,
  Hunt = 4,
  Assembly = 5,
  FlyIntro = 6,
  Fly = 7,
  End = 8,
  WingsOne = 9,
  WingsTwo = 10,
  FuelTank = 11,
  Engine = 12,
  Propeller = 13,
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
    ChangeSceneTo((int) SceneIndex.Intro);
  }

  public void ChangeSceneToSortOne()
  {
    ChangeSceneTo((int) SceneIndex.SortOne);
  }

  public void ChangeSceneToSortTwo()
  {
    ChangeSceneTo((int) SceneIndex.SortTwo);
  }

  public void ChangeSceneToFinishSelect()
  {
    ChangeSceneTo((int) SceneIndex.FinishSelect);
  }

  public void ChangeSceneToHunt()
  {
    ChangeSceneTo((int) SceneIndex.Hunt);
  }

  public void ChangeSceneToAssembly()
  {
    // if (FindObjectOfType<ObjectsManager>().GetCanPass())
    // {
    // FindObjectOfType<ObjectsManager>().SaveCollectedComponent();
    ChangeSceneTo((int) SceneIndex.Assembly);
    // }
  }

  public void ChangeSceneToFly()
  {
    ChangeSceneTo((int) SceneIndex.Fly);
  }

  public void ChangeSceneToEngine()
  {
    ChangeSceneTo((int) SceneIndex.Engine);
  }

  public void ChangeSceneToWingsOne()
  {
    ChangeSceneTo((int) SceneIndex.WingsOne);
  }

  public void ChangeSceneToWingsTwo()
  {
    ChangeSceneTo((int) SceneIndex.WingsTwo);
  }

  public void ChangeSceneToPropeller()
  {
    ChangeSceneTo((int) SceneIndex.Propeller);
  }

  public void ChangeSceneToFuelTank()
  {
    ChangeSceneTo((int) SceneIndex.FuelTank);
  }

  public int GetCurrentSceneIndex()
  {
    return SceneManager.GetActiveScene().buildIndex;
  }
}