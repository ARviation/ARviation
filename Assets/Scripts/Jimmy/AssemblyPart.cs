using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemblyPart : MonoBehaviour
{
  [SerializeField] private GameObject hiddenObj;

  private void Start()
  {
    hiddenObj.SetActive(false);
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.transform.CompareTag("Wings"))
    {
      hiddenObj.SetActive(true);
      other.gameObject.SetActive(false);
    }
  }
}