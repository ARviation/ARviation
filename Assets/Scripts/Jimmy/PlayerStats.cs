using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
  public static PlayerStats Instance = null;
  public CollectedComponent savedCollectedComponent;
  public MorseCode selectedComponentCode;
  public InventoryItem selectedComponent;

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

  public void UpdateSelectedComponentCode(MorseCode code, InventoryItem item)
  {
    selectedComponentCode = code;
    selectedComponent = item;
  }

  public void AttachComponent()
  {
    selectedComponent.OnUseComponent();
  }
}