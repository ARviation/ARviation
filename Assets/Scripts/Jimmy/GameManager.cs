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
    
    private void ChangeSceneTo(SceneIndex index)
    {
        SceneManager.LoadScene((int)index);
    }

    public void ChangeSceneToIntro()
    {
        ChangeSceneTo(SceneIndex.Intro);
    }

    public void ChangeSceneToHunt()
    {
        print("here");
        ChangeSceneTo(SceneIndex.Hunt);
    }

    public void ChangeSceneToAssembly()
    {
        ChangeSceneTo(SceneIndex.Assembly);
    }

    public void ChangeSceneToEnd()
    {
        ChangeSceneTo(SceneIndex.End);
    }
}
