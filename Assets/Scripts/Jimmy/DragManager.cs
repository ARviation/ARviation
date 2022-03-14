using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public enum PlaneComponent : int
{
  Engine = 1,
  Wings = 2,
  Propeller = 3,
  Wheels = 4,
  OilTank = 5,
}

public class DragManager : MonoBehaviour
{
  [SerializeField] private float mouseDragPhysicsSpeed = 10.0f;
  [SerializeField] private float mouseDragSpeed = 1.0f;
  [SerializeField] private CollectPanel _collectPanel;

  public delegate void StartTouchEvent(Vector2 position, float time);

  public event StartTouchEvent OnStartTouch;

  public delegate void EndTouchEvent(Vector2 position, float time);

  public event EndTouchEvent OnEndTouch;
  private TouchControls _touchControls;
  private Camera mainCamera;
  private Vector3 velocity = Vector3.zero;
  private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();

  private float width;
  private float height;
  private bool isEnable = false;
  private bool isPress = false;
  private bool isTouch = false;
  private bool isDrag = false;
  private float posX = .0f;
  private float posY = .0f;
  [SerializeField] private bool canDrag = true;

  private void Awake()
  {
    mainCamera = Camera.main;
    _touchControls = new TouchControls();
    width = Screen.width / 2.0f;
    height = Screen.height / 2.0f;
    if (_collectPanel != null)
    {
      _collectPanel = FindObjectOfType<CollectPanel>();
      _collectPanel.gameObject.SetActive(false);
    }
  }

  private void OnGUI()
  {
    // Compute a fontSize based on the size of the screen width.
    GUI.skin.label.fontSize = (int) (Screen.width / 50.0f);

    GUI.Label(new Rect(20, 20, width, height * 0.25f),
      "pos(X) = " + posX + " pos(Y) = " + posY +
      "enabled = " + isEnable + " pressed = " + isPress + " touched = " + isTouch + " drag = " + isDrag);
  }

  private void Start()
  {
    _touchControls.Touch.TouchPress.started += ctx => StartTouch(ctx);
    _touchControls.Touch.TouchPress.canceled += ctx => EndTouch(ctx);
  }

  private void OnEnable()
  {
    _touchControls.Enable();
    isEnable = true;
  }

  private void OnDisable()
  {
    _touchControls.Disable();
    isEnable = false;
  }

  void StartTouch(InputAction.CallbackContext context)
  {
    Vector2 touchPosition = _touchControls.Touch.TouchPosition.ReadValue<Vector2>();
    posX = touchPosition.x;
    posY = touchPosition.y;
    if (OnStartTouch != null)
      OnStartTouch(touchPosition, (float) context.startTime);

    Ray ray = mainCamera.ScreenPointToRay(touchPosition);
    RaycastHit hit;
    if (Physics.Raycast(ray, out hit))
    {
      isTouch = true;
      if (canDrag)
      {
        if (hit.collider != null && (hit.collider.gameObject.layer == LayerMask.NameToLayer("Draggable")))
        {
          isDrag = true;
          StartCoroutine(DragUpdate(hit.collider.gameObject));
        }
        else
        {
          isDrag = false;
        }
      }
      else
      {
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Collectable") &&
            _touchControls.Touch.TouchPress.ReadValue<float>() != 0)
        {
          InventoryItem inventoryItem = hit.transform.GetComponent<Collectable>().GetInventoryItem();
          MoseCode code = hit.transform.GetComponent<Collectable>().componentCode;
          inventoryItem.OnHitComponent(code);
          _collectPanel.gameObject.SetActive(true);
          _collectPanel.SetInventoryItem(inventoryItem);
        }
      }
    }

    isTouch = false;
    isDrag = false;
  }

  void EndTouch(InputAction.CallbackContext context)
  {
    if (OnEndTouch != null)
      OnEndTouch(_touchControls.Touch.TouchPosition.ReadValue<Vector2>(), (float) context.time);
  }

  private IEnumerator DragUpdate(GameObject clickObj)
  {
    float initialDistance = Vector3.Distance(clickObj.transform.position, mainCamera.transform.position);
    clickObj.TryGetComponent<Rigidbody>(out var rb);
    while (_touchControls.Touch.TouchPress.ReadValue<float>() != 0)
    {
      Ray ray = mainCamera.ScreenPointToRay(_touchControls.Touch.TouchPosition.ReadValue<Vector2>());
      if (rb != null)
      {
        Vector3 direction = ray.GetPoint(initialDistance) - clickObj.transform.position;
        rb.velocity = direction * mouseDragPhysicsSpeed;
        yield return _waitForFixedUpdate;
      }
      else
      {
        clickObj.transform.position = Vector3.SmoothDamp(clickObj.transform.position,
          ray.GetPoint(initialDistance), ref velocity, mouseDragSpeed);
        yield return null;
      }
    }
  }
}