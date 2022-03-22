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

  private static void ChangeSceneTo(SceneIndex index)
  {
    SceneManager.LoadScene((int) index);
  }

  public void ChangeSceneToIntro()
  {
    ChangeSceneTo(SceneIndex.Intro);
  }

  public void ChangeSceneToHunt()
  {
    ChangeSceneTo(SceneIndex.Hunt);
  }

  public void ChangeSceneToAssembly()
  {
    FindObjectOfType<ObjectsManager>().SaveCollectedComponent();
    ChangeSceneTo(SceneIndex.Assembly);
  }

  public void ChangeSceneToFly()
  {
    ChangeSceneTo(SceneIndex.Fly);
  }
}