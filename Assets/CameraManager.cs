using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public enum RotateDir : int
{
  Left = 0,
  Right = 1,
  Up = 2,
  Down = 3,
}

public class CameraManager : MonoBehaviour
{
  [SerializeField] private Camera camera_2d;
  [SerializeField] private Camera camera_3d;
  [SerializeField] private GameObject PlaneCenter;
  [SerializeField] private float smoothness2D = 1f;
  [SerializeField] private float smoothness = 0.1f;

  private float rotX = 0f;
  private float rotY = 0f;
  private float rotZ = 0f;
  private bool isButtonHeld;
  private float delay = 0.1f;
  private bool isRotateLeft = false;
  private bool isRotateRight = false;
  private bool isRotateUp = false;
  private bool isRotateDown = false;
  private Quaternion initRotation = Quaternion.identity;
  private Quaternion endRotation = Quaternion.identity;
  private float totalRotateAngle = 0f;

  private bool is2D = true;
  private bool isRotating = false;
  private RotateDir currRotateDir = RotateDir.Left;

  private void Start()
  {
    camera_2d.gameObject.SetActive(true);
    camera_3d.gameObject.SetActive(false);
    initRotation.x = rotX;
    initRotation.y = rotY;
    initRotation.z = rotZ;
    PlaneCenter.transform.rotation = initRotation;
  }

  private void Update()
  {
    if (isRotating)
    {
      switch (currRotateDir)
      {
        case RotateDir.Left:
          PlaneCenter.transform.RotateAround(PlaneCenter.transform.position, Vector3.up, smoothness2D);
          break;
        case RotateDir.Right:
          PlaneCenter.transform.RotateAround(PlaneCenter.transform.position, Vector3.down, smoothness2D);
          break;
        case RotateDir.Up:
          PlaneCenter.transform.RotateAround(PlaneCenter.transform.position, Vector3.right, smoothness2D);
          break;
        case RotateDir.Down:
          PlaneCenter.transform.RotateAround(PlaneCenter.transform.position, Vector3.left, smoothness2D);
          break;
      }

      totalRotateAngle += Mathf.Abs(smoothness2D);
      if (totalRotateAngle >= 90.0f)
      {
        totalRotateAngle = 0f;
        isRotating = false;
      }
    }
  }


  // private void Update()
  // {
  //   if (!is2D)
  //   {
  //     RotationFor3D();
  //   }
  // }
  //
  // private void RotationFor3D()
  // {
  //   if (isRotateLeft)
  //   {
  //     // StartCoroutine(RotateLeft());
  //     Debug.Log("rotate left");
  //   }
  //
  //   if (isRotateRight)
  //   {
  //     // StartCoroutine(RotateRight());
  //     Debug.Log("rotate right");
  //   }
  //
  //   if (isRotateUp)
  //   {
  //     // StartCoroutine(RotateUp());
  //     Debug.Log("rotate up");
  //   }
  //
  //   if (isRotateDown)
  //   {
  //     // StartCoroutine(RotateDown());
  //     Debug.Log("rotate down");
  //   }
  // }

  private IEnumerator StartRotate(Quaternion initRot, Quaternion endRot, float duration)
  {
    // Debug.Log(initRot);
    // Debug.Log(endRot);
    // for (float time = 0; time < duration * 2; time += Time.deltaTime)
    // {
    //   float progress = Mathf.PingPong(time, duration) / duration;
    //   PlaneCenter.transform.localRotation = Quaternion.Lerp(initRot, endRot, progress);
    //   yield return null;
    // }
    float step = smoothness / duration;
    float progress = 0.0f;
    while (progress <= 1)
    {
      PlaneCenter.transform.rotation =
        Quaternion.Lerp(initRot, endRot, progress);
      progress += step;
      yield return null;
    }

    isRotating = false;
  }

  public void OnCameraViewSwitch()
  {
    is2D = !is2D;
    if (is2D)
    {
      camera_2d.gameObject.SetActive(true);
      camera_3d.gameObject.SetActive(false);
    }
    else
    {
      camera_2d.gameObject.SetActive(false);
      camera_3d.gameObject.SetActive(true);
    }
  }

  public void OnRotateLeftDown()
  {
    if (!is2D || isRotating) return;
    currRotateDir = RotateDir.Left;
    isRotating = true;
  }

  public void OnRotateRightDown()
  {
    if (!is2D || isRotating) return;
    currRotateDir = RotateDir.Right;
    isRotating = true;
  }

  public void OnRotateUpDown()
  {
    if (!is2D || isRotating) return;
    currRotateDir = RotateDir.Up;
    isRotating = true;
  }

  public void OnRotateDownDown()
  {
    if (!is2D || isRotating) return;
    currRotateDir = RotateDir.Down;
    isRotating = true;
  }

  // public void OnRotateLeftUp()
  // {
  //   isRotateLeft = false;
  // }
  //
  // public void OnRotateRightDown()
  // {
  //   isRotateRight = true;
  //   currRotation.x -= 90.0f;
  //   if (!is2D || isRotating) return;
  //   isRotating = true;
  //   StartCoroutine(StartRotate());
  // }
  //
  // public void OnRotateRightUp()
  // {
  //   isRotateRight = false;
  // }
  //
  // public void OnRotateUpDown()
  // {
  //   isRotateUp = true;
  //   currRotation.z += 90.0f;
  //   if (!is2D || isRotating) return;
  //   isRotating = true;
  //   StartCoroutine(StartRotate());
  // }
  //
  // public void OnRotateUpUp()
  // {
  //   isRotateUp = false;
  // }
  //
  // public void OnRotateDownDown()
  // {
  //   isRotateDown = true;
  //   currRotation.z -= 90.0f;
  //   if (!is2D || isRotating) return;
  //   isRotating = true;
  //   StartCoroutine(StartRotate());
  // }
  //
  // public void OnRotateDownUp()
  // {
  //   isRotateDown = false;
  // }
}