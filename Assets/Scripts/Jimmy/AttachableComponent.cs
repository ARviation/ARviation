using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachableComponent : MonoBehaviour
{
  [SerializeField] private GameObject[] outlineObjs;
  [SerializeField] private InventoryItem item;
  [SerializeField] private GameObject fuelTank;

  private MeshRenderer _meshRenderer;
  private bool isAttached = false;

  private void Start()
  {
    _meshRenderer = GetComponent<MeshRenderer>();
    AdjustOutline(false);
    _meshRenderer.enabled = false;
    if (fuelTank != null)
    {
      fuelTank.SetActive(false);
    }
  }

  private void AdjustOutline(bool value)
  {
    foreach (var outlineObj in outlineObjs)
    {
      outlineObj.gameObject.SetActive(value);
    }
  }

  public void ShowOutline()
  {
    AdjustOutline(true);
  }

  public void CloseOutline()
  {
    AdjustOutline(false);
  }

  public void ShowObj()
  {
    AttachableComponent[] attachableComponents = FindObjectsOfType<AttachableComponent>();
    foreach (var attachableComponent in attachableComponents)
    {
      attachableComponent.CloseOutline();
    }
    
    AttachComponent();
    AdjustOutline(false);
    _meshRenderer.enabled = true;
    item.OnUseComponent();
    if (fuelTank != null)
    {
      fuelTank.SetActive(true);
    }
  }

  private void AttachComponent()
  {
    isAttached = true;
  }

  public bool GetIsAttached()
  {
    return isAttached;
  }
}