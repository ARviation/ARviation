using System;
using System.Collections;
using UnityEngine;

public enum RotateDir : int
{
  Left = 0,
  Right = 1,
  Up = 2,
  Down = 3,
}

public class CameraManager : MonoBehaviour
{
  [SerializeField] private Camera cameraFront;
  [SerializeField] private Camera cameraTop;
  [SerializeField] private GameObject planeCenter;
  [SerializeField] private float smoothness2D = 1f;
  [SerializeField] private float smoothness = 0.1f;
  [SerializeField] private float rotateDeg = 45.0f;

  private const float RotX = 0f;
  private const float RotY = 0f;
  private const float RotZ = 0f;
  private Quaternion _initRotation = Quaternion.identity;
  private Quaternion _endRotation = Quaternion.identity;
  private float _totalRotateAngle = 0f;

  private bool _isRotating = false;
  private RotateDir _currRotateDir = RotateDir.Left;

  private void Start()
  {
    cameraFront.enabled = true;
    cameraTop.enabled = false;
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

  public void OnRotateLeftDown()
  {
    if (_isRotating) return;
    _currRotateDir = RotateDir.Left;
    _isRotating = true;
    SoundManager.Instance.PlaySFXByIndex(SFXList.Click);
  }

  public void OnRotateRightDown()
  {
    if (_isRotating) return;
    _currRotateDir = RotateDir.Right;
    _isRotating = true;
    SoundManager.Instance.PlaySFXByIndex(SFXList.Click);
  }

  public void OnTopViewDown()
  {
    if (_isRotating) return;
    cameraTop.enabled = true;
    cameraFront.enabled = false;
    FindObjectOfType<ObjectsManager>().SetCamera(cameraTop);
    SoundManager.Instance.PlaySFXByIndex(SFXList.Click);
  }

  public void On3DViewDown()
  {
    if (_isRotating) return;
    cameraTop.enabled = false;
    cameraFront.enabled = true;
    FindObjectOfType<ObjectsManager>().SetCamera(cameraFront);
    SoundManager.Instance.PlaySFXByIndex(SFXList.Click);
  }
}