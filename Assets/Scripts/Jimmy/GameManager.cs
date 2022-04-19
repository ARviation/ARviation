using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneIndex : int
{
  Landing = 0,
  Intro = 1,
  SortOne = 2,
  SortTwo = 3,
  FinishSelect = 4,
  Hunt = 5,
  Assembly = 6,
  FlyIntro = 7,
  Fly = 8,
  End = 9,
  WingsOne = 10,
  WingsTwo = 11,
  FuelTank = 12,
  Engine = 13,
  Propeller = 14,
}

public class GameManager : MonoBehaviour
{
  [SerializeField] public float narratorSpeed = 0.04f;

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
    if (SoundManager.Instance != null)
      SoundManager.Instance.PlaySFXByIndex(SFXList.Click);
    SceneManager.LoadScene(index);
  }

  public void ChangeSceneTo(SceneIndex index)
  {
    ChangeSceneTo((int) index);
  }

  public void ChangeSceneToLanding()
  {
    ChangeSceneTo((int) SceneIndex.Landing);
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

  public void ChangeSceneToEnd()
  {
    ChangeSceneTo((int) SceneIndex.End);
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