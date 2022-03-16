using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneIndex : int
{
  Intro = 0,
  Hunt = 1,
  Assembly = 2,
  End = 3,
}

public class GameManager : MonoBehaviour
{
  public static GameManager Instance = null;

  public CollectedComponent savedCollectedComponent;

  private void Awake()
  {
    if (Instance == null)
    {
      savedCollectedComponent = gameObject.AddComponent<CollectedComponent>();
      DontDestroyOnLoad(gameObject);
      Instance = this;
    }
    else if (Instance != this)
    {
      Destroy(gameObject);
    }
  }

  private void ChangeSceneTo(SceneIndex index)
  {
    SceneManager.LoadScene((int) index);
  }

  public void ChangeSceneToIntro()
  {
    ChangeSceneTo(SceneIndex.Intro);
  }

  public void ChangeSceneToHunt()
  {
    Debug.Log("123");
    ChangeSceneTo(SceneIndex.Hunt);
  }

  public void ChangeSceneToAssembly()
  {
    FindObjectOfType<ObjectsManager>().SaveCollectedComponent();
    ChangeSceneTo(SceneIndex.Assembly);
  }

  public void ChangeSceneToEnd()
  {
    ChangeSceneTo(SceneIndex.End);
  }
}