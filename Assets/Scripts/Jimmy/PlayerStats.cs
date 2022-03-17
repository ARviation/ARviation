using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
   public static PlayerStats Instance = null;
   
  public CollectedComponent savedCollectedComponent;
   private void Awake()
     {
       if (Instance == null)
       {
         GameObject o;
         savedCollectedComponent = (o = gameObject).AddComponent<CollectedComponent>();
         DontDestroyOnLoad(o);
         Instance = this;
       }
       else if (Instance != this)
       {
         Destroy(gameObject);
       }
     }
}
