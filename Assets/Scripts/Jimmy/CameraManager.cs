using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public enum RotateDir : int
{
  Left = 0,
  Right = 1,
  Up = 2,
  Down = 3,
}

public class CameraManager : MonoBehaviour
{
  [SerializeField] private Camera camera2D;
  [SerializeField] private GameObject cameraAR;
  [SerializeField] private GameObject planeCenter;
  [SerializeField] private float smoothness2D = 1f;
  [SerializeField] private float smoothness = 0.1f;
  [SerializeField] private TMP_Text controllerText;
  [SerializeField] private float rotateDeg = 45.0f;

  private const float RotX = 0f;
  private const float RotY = 0f;
  private const float RotZ = 0f;
  private bool isButtonHeld;
  private float delay = 0.1f;
  private bool _isRotateLeft = false;
  private bool _isRotateRight = false;
  private bool _isRotateUp = false;
  private bool _isRotateDown = false;
  private Quaternion _initRotation = Quaternion.identity;
  private Quaternion _endRotation = Quaternion.identity;
  private float _totalRotateAngle = 0f;

  private bool _is2D = true;
  private bool _isRotating = false;
  private RotateDir _currRotateDir = RotateDir.Left;

  private void Start()
  {
    camera2D.gameObject.SetActive(true);
    cameraAR.gameObject.SetActive(false);
    _initRotation.x = RotX;
    _initRotation.y = RotY;
    _initRotation.z = RotZ;
    planeCenter.transform.rotation = _initRotation;
  }

  private void Update()
  {
    if (!_isRotating) return;
    switch (_currRotateDir)
    {
      case RotateDir.Left:
        planeCenter.transform.RotateAround(planeCenter.transform.position, Vector3.up, smoothness2D);
        break;
      case RotateDir.Right:
        planeCenter.transform.RotateAround(planeCenter.transform.position, Vector3.down, smoothness2D);
        break;
      case RotateDir.Up:
        planeCenter.transform.RotateAround(planeCenter.transform.position, Vector3.right, smoothness2D);
        break;
      case RotateDir.Down:
        planeCenter.transform.RotateAround(planeCenter.transform.position, Vector3.left, smoothness2D);
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }

    _totalRotateAngle += Mathf.Abs(smoothness2D);
    if (!(_totalRotateAngle >= rotateDeg)) return;
    _totalRotateAngle = 0f;
    _isRotating = false;
  }

  private IEnumerator StartRotate(Quaternion initRot, Quaternion endRot, float duration)
  {
    float step = smoothness / duration;
    float progress = 0.0f;
    while (progress <= 1)
    {
      planeCenter.transform.rotation =
        Quaternion.Lerp(initRot, endRot, progress);
      progress += step;
      yield return null;
    }

    _isRotating = false;
  }

  public void OnCameraViewSwitch()
  {
    _is2D = !_is2D;
    if (_is2D)
    {
      camera2D.gameObject.SetActive(true);
      cameraAR.gameObject.SetActive(false);
      controllerText.text = "View";
    }
    else
    {
      camera2D.gameObject.SetActive(false);
      cameraAR.gameObject.SetActive(true);
      controllerText.text = "Assemble";
    }
  }

  public void OnRotateLeftDown()
  {
    if (!_is2D || _isRotating) return;
    _currRotateDir = RotateDir.Left;
    _isRotating = true;
  }

  public void OnRotateRightDown()
  {
    if (!_is2D || _isRotating) return;
    _currRotateDir = RotateDir.Right;
    _isRotating = true;
  }

  public void OnRotateUpDown()
  {
    if (!_is2D || _isRotating) return;
    _currRotateDir = RotateDir.Up;
    _isRotating = true;
  }

  public void OnRotateDownDown()
  {
    if (!_is2D || _isRotating) return;
    _currRotateDir = RotateDir.Down;
    _isRotating = true;
  }
}