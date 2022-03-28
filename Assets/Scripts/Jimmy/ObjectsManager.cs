using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectsManager : MonoBehaviour
{
  public CollectedComponent localCollectedComponent;

  [SerializeField] private float mouseDragPhysicsSpeed = 10.0f;
  [SerializeField] private float mouseDragSpeed = 1.0f;
  [SerializeField] private CollectPanel collectPanel;
  [SerializeField] private int collectedComponent = 0;
  [SerializeField] private int assembledPart = 0;
  [SerializeField] private int targetComponentNumber = 4;
  [SerializeField] private string componentPasscode = "";
  [SerializeField] private GameObject nextButton;
  [SerializeField] private GameObject finishAssembleButton;

  public delegate void StartTouchEvent(Vector2 position, float time);

  public event StartTouchEvent OnStartTouch;

  public delegate void EndTouchEvent(Vector2 position, float time);

  public event EndTouchEvent OnEndTouch;
  private TouchControls _touchControls;
  private Camera _mainCamera;
  private Vector3 _velocity = Vector3.zero;
  private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();
  private InventoryItem[] _inventoryItems;

  private readonly float _width = Screen.width / 2.0f;
  private readonly float _height = Screen.height / 2.0f;
  private bool _isEnable = false;
  private bool _isPress = false;
  private bool _isTouch = false;
  private bool _isDrag = false;
  private float _posX = .0f;
  private float _posY = .0f;
  private bool _showPass = false;
  private bool _canPass = false;
  private bool _finishAssemble = false;
  private bool _hasOpenFinishButton = false;

  private void Awake()
  {
    _mainCamera = Camera.main;
    _touchControls = new TouchControls();
  }

  private void OnGUI()
  {
    // // Compute a fontSize based on the size of the screen width.
    // GUI.skin.label.fontSize = (int) (Screen.width / 50.0f);
    //
    // GUI.Label(new Rect(20, 20, _width, _height * 0.25f),
    //   "pos(X) = " + _posX + " pos(Y) = " + _posY +
    //   "enabled = " + _isEnable + " pressed = " + _isPress + " touched = " + _isTouch + " drag = " + _isDrag);
  }

  private void Start()
  {
    if (nextButton != null)
    {
      nextButton.SetActive(false);
    }

    if (finishAssembleButton != null)
    {
      finishAssembleButton.SetActive(false);
    }

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

  private void Update()
  {
    _showPass = collectedComponent == targetComponentNumber;
    if (_showPass)
    {
      nextButton.SetActive(true);
    }


    Debug.Log(localCollectedComponent.ToString());
    Debug.Log(componentPasscode);
    Debug.Log(localCollectedComponent.ToString() == componentPasscode);
    _canPass = localCollectedComponent.ToString() == componentPasscode;
    if (assembledPart != (targetComponentNumber - 1) || _hasOpenFinishButton) return;
    _hasOpenFinishButton = true;
    finishAssembleButton.SetActive(true);
  }

  public bool GetCanPass()
  {
    return _canPass;
  }

  private void StartTouch(InputAction.CallbackContext context)
  {
    Vector2 touchPosition = _touchControls.Touch.TouchPosition.ReadValue<Vector2>();
    _posX = touchPosition.x;
    _posY = touchPosition.y;
    OnStartTouch?.Invoke(touchPosition, (float) context.startTime);

    Ray ray = _mainCamera.ScreenPointToRay(touchPosition);
    if (Physics.Raycast(ray, out var hit))
    {
      _isTouch = true;
      if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Draggable"))
      {
        _isDrag = true;
        StartCoroutine(DragUpdate(hit.collider.gameObject));
      }
      else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Collectable") &&
               _touchControls.Touch.TouchPress.ReadValue<float>() != 0)
      {
        InventoryItem inventoryItem = hit.transform.GetComponent<Collectable>().GetInventoryItem();
        Collectable collectable = hit.transform.GetComponent<Collectable>();
        MoseCode code = collectable.componentCode;
        if (code == MoseCode.A)
        {
          FindObjectOfType<NarrationController>().SetHasConditionFalse();
        }

        inventoryItem.OnHitComponent(code);
        string componentName = hit.transform.name;
        componentName = componentName.Replace("(Clone)", "").Trim();
        collectPanel.OpenPanel(componentName);
        collectPanel.SetInventoryItem(inventoryItem);
      }
      else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Attachable") &&
               _touchControls.Touch.TouchPress.ReadValue<float>() != 0)

      {
        string hitTag = hit.transform.tag;
        switch (hitTag)
        {
          case GameManager.Engine:
            if (PlayerStats.Instance.selectedComponentCode == MoseCode.H)
            {
              hit.transform.gameObject.GetComponent<AttachableComponent>().ShowObj();
              AddAssembledComponent();
            }

            break;
          case GameManager.Wings:
            if (PlayerStats.Instance.selectedComponentCode == MoseCode.W)
            {
              hit.transform.gameObject.GetComponent<AttachableComponent>().ShowObj();
              AddAssembledComponent();
            }

            break;
          case GameManager.Propeller:
            if (PlayerStats.Instance.selectedComponentCode == MoseCode.F)
            {
              hit.transform.gameObject.GetComponent<AttachableComponent>().ShowObj();
              AddAssembledComponent();
            }

            break;
          case GameManager.Wheels:
            if (PlayerStats.Instance.selectedComponentCode == MoseCode.O)
            {
              hit.transform.gameObject.GetComponent<AttachableComponent>().ShowObj();
              AddAssembledComponent();
            }

            break;
          case GameManager.FuelTank:
            if (PlayerStats.Instance.selectedComponentCode == MoseCode.E)
            {
              hit.transform.gameObject.GetComponent<AttachableComponent>().ShowObj();
              AddAssembledComponent();
            }

            break;
          case GameManager.Tail:
            if (PlayerStats.Instance.selectedComponentCode == MoseCode.U)
            {
              hit.transform.gameObject.GetComponent<AttachableComponent>().ShowObj();
              AddAssembledComponent();
            }

            break;
        }
      }
    }

    _isTouch = false;
    _isDrag = false;
  }

  private void EndTouch(InputAction.CallbackContext context)
  {
    OnEndTouch?.Invoke(_touchControls.Touch.TouchPosition.ReadValue<Vector2>(), (float) context.time);
  }

  private IEnumerator DragUpdate(GameObject clickObj)
  {
    float initialDistance = Vector3.Distance(clickObj.transform.position, _mainCamera.transform.position);
    clickObj.TryGetComponent<Rigidbody>(out var rb);
    while (_touchControls.Touch.TouchPress.ReadValue<float>() != 0)
    {
      Ray ray = _mainCamera.ScreenPointToRay(_touchControls.Touch.TouchPosition.ReadValue<Vector2>());
      if (rb != null)
      {
        Vector3 direction = ray.GetPoint(initialDistance) - clickObj.transform.position;
        rb.velocity = direction * mouseDragPhysicsSpeed;
        yield return _waitForFixedUpdate;
      }
      else
      {
        clickObj.transform.position = Vector3.SmoothDamp(clickObj.transform.position,
          ray.GetPoint(initialDistance), ref _velocity, mouseDragSpeed);
        yield return null;
      }
    }
  }

  public void SaveCollectedComponent()
  {
    PlayerStats.Instance.savedCollectedComponent = localCollectedComponent;
  }

  private void LoadCollectedComponent()
  {
    localCollectedComponent = PlayerStats.Instance.savedCollectedComponent;
    var componentMap = localCollectedComponent.GetAllComponent();
    _inventoryItems = FindObjectsOfType<InventoryItem>();
    foreach (var inventoryItem in _inventoryItems)
    {
      var inventoryItemCategory = inventoryItem.name;
      int code = componentMap[inventoryItemCategory];
      inventoryItem.currentCode = (MoseCode) code;
      inventoryItem.UpdateSprite(code);
    }
  }

  public void AddCollectedComponent()
  {
    collectedComponent++;
  }

  private void AddAssembledComponent()
  {
    assembledPart++;
  }
}