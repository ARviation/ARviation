using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class DisplayItem : MonoBehaviour
{
  [SerializeField] private float rotSpeed = 0.25f;
  [SerializeField] private RotateDir rotDir;

  public bool canRotate = false;

  private void Start()
  {
    gameObject.SetActive(false);
  }

  private void Update()
  {
    if (canRotate)
      switch (rotDir)
      {
        case RotateDir.Left:
          transform.RotateAround(transform.position, Vector3.left, rotSpeed);
          break;
        case RotateDir.Right:
          transform.RotateAround(transform.position, Vector3.right, rotSpeed);
          break;
        case RotateDir.Up:
          transform.RotateAround(transform.position, Vector3.up, rotSpeed);
          break;
        case RotateDir.Down:
          transform.RotateAround(transform.position, Vector3.down, rotSpeed);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
  }

  public void Display()
  {
    gameObject.SetActive(true);
  }

  public void ChangeState(bool newState)
  {
    gameObject.SetActive(newState);
    canRotate = newState;
  }
}