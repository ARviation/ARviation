using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneIndex : int
{
  Intro = 0,
  Hunt = 1,
  Assembly = 2,
  Fly = 3,
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

  private SceneIndex currentSceneIndex = SceneIndex.Intro;

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
    currentSceneIndex = SceneIndex.Intro;
    ChangeSceneTo((int) currentSceneIndex);
  }

  public void ChangeSceneToHunt()
  {
    currentSceneIndex = SceneIndex.Hunt;
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

  public int GetCurrentSceneIndex()
  {
    return SceneManager.GetActiveScene().buildIndex;
  }
}