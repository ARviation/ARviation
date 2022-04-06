using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{
  private MeshRenderer _meshRenderer;
  private bool isPlace = false;

  private void Start()
  {
    _meshRenderer = GetComponent<MeshRenderer>();
    _meshRenderer.gameObject.SetActive(false);
  }

  public bool GetIsPlace()
  {
    return isPlace;
  }

  public void PlaceComponent()
  {
    isPlace = true;
  }

  public void OpenHighlight()
  {
    _meshRenderer.gameObject.SetActive(true);
  }
}