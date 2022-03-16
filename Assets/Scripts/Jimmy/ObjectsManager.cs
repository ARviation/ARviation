using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlaneComponent : int
{
  Engine = 1,
  Wings = 2,
  Propeller = 3,
  Wheels = 4,
  OilTank = 5,
}

public class ObjectsManager : MonoBehaviour
{
  public static ObjectsManager Instance = null;

  public CollectedComponent localCollectedComponent;

  [SerializeField] private float mouseDragPhysicsSpeed = 10.0f;
  [SerializeField] private float mouseDragSpeed = 1.0f;
  [SerializeField] private CollectPanel collectPanel;

  public delegate void StartTouchEvent(Vector2 position, float time);

  public event StartTouchEvent OnStartTouch;

  public delegate void EndTouchEvent(Vector2 position, float time);

  public event EndTouchEvent OnEndTouch;
  private TouchControls _touchControls;
  private Camera mainCamera;
  private Vector3 velocity = Vector3.zero;
  private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();

  private float _width = Screen.width / 2.0f;
  private float _height = Screen.height / 2.0f;
  private bool _isEnable = false;
  private bool _isPress = false;
  private bool _isTouch = false;
  private bool _isDrag = false;
  private float _posX = .0f;
  private float _posY = .0f;
  // [SerializeField] private bool canDrag = true;

  private void Awake()
  {
    mainCamera = Camera.main;
    _touchControls = new TouchControls();
  }

  private void OnGUI()
  {
    // Compute a fontSize based on the size of the screen width.
    GUI.skin.label.fontSize = (int) (Screen.width / 50.0f);

    GUI.Label(new Rect(20, 20, _width, _height * 0.25f),
      "pos(X) = " + _posX + " pos(Y) = " + _posY +
      "enabled = " + _isEnable + " pressed = " + _isPress + " touched = " + _isTouch + " drag = " + _isDrag);
  }

  private void Start()
  {
    if (collectPanel != null)
      collectPanel.gameObject.SetActive(false);
    LoadCollectedComponent();
    _touchControls.Touch.TouchPress.started += ctx => StartTouch(ctx);
    _touchControls.Touch.TouchPress.canceled += ctx => EndTouch(ctx);
  }


  private void OnEnable()
  {
    _touchControls.Enable();
    _isEnable = true;
  }

  private void OnDisable()
  {
    _touchControls.Disable();
    _isEnable = false;
  }

  void StartTouch(InputAction.CallbackContext context)
  {
    Vector2 touchPosition = _touchControls.Touch.TouchPosition.ReadValue<Vector2>();
    _posX = touchPosition.x;
    _posY = touchPosition.y;
    if (OnStartTouch != null)
      OnStartTouch(touchPosition, (float) context.startTime);

    Ray ray = mainCamera.ScreenPointToRay(touchPosition);
    RaycastHit hit;
    if (Physics.Raycast(ray, out hit))
    {
      _isTouch = true;
      // if (canDrag)
      // {
      if (hit.collider != null && (hit.collider.gameObject.layer == LayerMask.NameToLayer("Draggable")))
      {
        _isDrag = true;
        StartCoroutine(DragUpdate(hit.collider.gameObject));
      }

      // else
      // {
      // _isDrag = false;
      // }
      // }
      // else
      // {
      else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Collectable") &&
               _touchControls.Touch.TouchPress.ReadValue<float>() != 0)
      {
        InventoryItem inventoryItem = hit.transform.GetComponent<Collectable>().GetInventoryItem();
        Collectable collectable = hit.transform.GetComponent<Collectable>();
        MoseCode code = collectable.componentCode;
        inventoryItem.OnHitComponent(code);
        collectPanel.OpenPanel();
        collectPanel.SetInventoryItem(inventoryItem);
        // }
      }
    }

    _isTouch = false;
    _isDrag = false;
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

  public void SaveCollectedComponent()
  {
    GameManager.Instance.savedCollectedComponent = localCollectedComponent;
  }

  private void LoadCollectedComponent()
  {
    localCollectedComponent = GameManager.Instance.savedCollectedComponent;
    Debug.Log(localCollectedComponent.ToString());
  }
}