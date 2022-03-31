using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneIndex : int
{
  Intro = 0,
  Sort = 1,
  Hunt = 2,
  Assembly = 3,
  Fly = 4,
  Engine = 5,
  Wings = 6,
  Propeller = 7,
  FuelTank = 8,
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
    SceneManager.LoadScene(index);
  }

  public void ChangeSceneToIntro()
  {
    ChangeSceneTo((int) SceneIndex.Intro);
  }

  public void ChangeSceneToSort()
  {
    ChangeSceneTo((int) SceneIndex.Sort);
  }

  public void ChangeSceneToHunt()
  {
    ChangeSceneTo((int) SceneIndex.Hunt);
  }

  public void ChangeSceneToAssembly()
  {
    if (FindObjectOfType<ObjectsManager>().GetCanPass())
    {
      FindObjectOfType<ObjectsManager>().SaveCollectedComponent();
      ChangeSceneTo((int) SceneIndex.Assembly);
    }
  }

  public void ChangeSceneToFly()
  {
    ChangeSceneTo((int) SceneIndex.Fly);
  }

  public void ChangeSceneToEngine()
  {
    ChangeSceneTo((int) SceneIndex.Engine);
  }

  public void ChangeSceneToWings()
  {
    ChangeSceneTo((int) SceneIndex.Wings);
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