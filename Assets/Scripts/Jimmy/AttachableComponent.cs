using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachableComponent : MonoBehaviour
{
  [SerializeField] private Material normalMat;
  [SerializeField] private Material outlineMat;
  [SerializeField] private InventoryItem item;

  private MeshRenderer _meshRenderer;
  private bool isAttached = false;

  private void Start()
  {
    _meshRenderer = GetComponent<MeshRenderer>();
    _meshRenderer.material = outlineMat;
    _meshRenderer.gameObject.SetActive(false);
  }

  public void ShowOutline()
  {
    _meshRenderer.gameObject.SetActive(true);
  }

  public void ShowObj()
  {
    _meshRenderer.material = normalMat;
    item.OnUseComponent();
  }

  public void AttachComponent()
  {
    isAttached = true;
  }

  public bool GetIsAttached()
  {
    return isAttached;
  }
}