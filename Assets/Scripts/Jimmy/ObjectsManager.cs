using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectsManager : MonoBehaviour
{
  // public CollectedComponent localCollectedComponent;

  [SerializeField] private float mouseDragPhysicsSpeed = 10.0f;
  [SerializeField] private float mouseDragSpeed = 1.0f;
  [SerializeField] private CollectPanel collectPanel;
  [SerializeField] private DisplayPanel displayPanel;
  [SerializeField] private int collectedComponent = 0;
  [SerializeField] private int assembledPart = 0;
  [SerializeField] private int targetComponentNumber = 4;
  [SerializeField] private string componentPasscode = "";
  [SerializeField] private GameObject nextButton;
  [SerializeField] private GameObject finishAssembleButton;
  [SerializeField] private Camera _camera;
  [SerializeField] private ParticleSystem fireworkVFX;

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
  private bool hasCollectAll = false;
  private bool hasTriggerCollectAll = false;

  // private bool _canPass = false;
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
    if (displayPanel != null)
      displayPanel.gameObject.SetActive(false);
    // LoadCollectedComponent();
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
    hasCollectAll = collectedComponent == targetComponentNumber;
    if (hasCollectAll && !hasTriggerCollectAll)
    {
      FindObjectOfType<NarrationController>().RevealDialog();
      hasTriggerCollectAll = true;
    }

    // _canPass = true;
    // _canPass = localCollectedComponent.ToString() == componentPasscode;
    if (assembledPart != (targetComponentNumber - 1) || _hasOpenFinishButton) return;
    FindObjectOfType<NarrationController>().RevealDialog();
    _hasOpenFinishButton = true;
    // finishAssembleButton.SetActive(true);
    fireworkVFX.Play();
    SoundManager.Instance.PlaySFXByIndex(SFXList.Firework);
  }

  // public bool GetCanPass()
  // {
  //   return _canPass;
  // }

  public void SetCamera(Camera camera)
  {
    _camera = camera;
  }

  private void StartTouch(InputAction.CallbackContext context)
  {
    Vector2 touchPosition = _touchControls.Touch.TouchPosition.ReadValue<Vector2>();
    _posX = touchPosition.x;
    _posY = touchPosition.y;
    OnStartTouch?.Invoke(touchPosition, (float) context.startTime);

    Ray ray = _camera.ScreenPointToRay(touchPosition);
    if (Physics.Raycast(ray, out var hit))
    {
      _isTouch = true;
      if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Draggable"))
      {
        _isDrag = true;
        StartCoroutine(DragUpdate(hit.collider.gameObject));
      }
      else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Attachable") &&
               _touchControls.Touch.TouchPress.ReadValue<float>() != 0)
      {
        string hitTag = hit.transform.tag;
        bool valid = false;
        switch (hitTag)
        {
          case GameManager.Engine:
            Debug.Log(PlayerStats.Instance.selectedComponentCode);
            if (PlayerStats.Instance.selectedComponentCode == (MorseCode) CorrectMorseCode.Engine)
            {
              hit.transform.parent.gameObject.GetComponent<AttachableComponent>().ShowObj();
              AddAssembledComponent();
              valid = true;
            }

            break;
          case GameManager.Wings:
            if (PlayerStats.Instance.selectedComponentCode == (MorseCode) CorrectMorseCode.Wings)
            {
              hit.transform.parent.gameObject.GetComponent<AttachableComponent>().ShowObj();
              AddAssembledComponent();
              valid = true;
            }

            break;
          case GameManager.Propeller:
            if (PlayerStats.Instance.selectedComponentCode == (MorseCode) CorrectMorseCode.Propeller)
            {
              hit.transform.parent.gameObject.GetComponent<AttachableComponent>().ShowObj();
              AddAssembledComponent();
              valid = true;
            }

            break;
          case GameManager.Wheels:
            if (PlayerStats.Instance.selectedComponentCode == (MorseCode) CorrectMorseCode.Wheels)
            {
              hit.transform.parent.gameObject.GetComponent<AttachableComponent>().ShowObj();
              AddAssembledComponent();
              valid = true;
            }

            break;
          case GameManager.FuelTank:
            if (PlayerStats.Instance.selectedComponentCode == (MorseCode) CorrectMorseCode.FuelTank)
            {
              hit.transform.parent.gameObject.GetComponent<AttachableComponent>().ShowObj();
              AddAssembledComponent();
              valid = true;
            }

            break;
          case GameManager.Tail:
            if (PlayerStats.Instance.selectedComponentCode == (MorseCode) CorrectMorseCode.Tail)
            {
              hit.transform.parent.gameObject.GetComponent<AttachableComponent>().ShowObj();
              AddAssembledComponent();
              valid = true;
            }

            break;
        }

        if (valid)
        {
          SoundManager.Instance.PlaySuccessSFX();
        }
        else
        {
          SoundManager.Instance.PlayFailSFX();
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

  // public void SaveCollectedComponent()
  // {
  //   PlayerStats.Instance.savedCollectedComponent = localCollectedComponent;
  // }
  //
  // private void LoadCollectedComponent()
  // {
  //   localCollectedComponent = PlayerStats.Instance.savedCollectedComponent;
  //   var componentMap = localCollectedComponent.GetAllComponent();
  //   _inventoryItems = FindObjectsOfType<InventoryItem>();
  //   foreach (var inventoryItem in _inventoryItems)
  //   {
  //     var inventoryItemCategory = inventoryItem.name;
  //     int code = componentMap[inventoryItemCategory];
  //     inventoryItem.currCode = (MorseCode) code;
  //     inventoryItem.UpdateSprite(code);
  //   }
  // }

  public void AddCollectedComponent()
  {
    collectedComponent++;
  }

  private void AddAssembledComponent()
  {
    assembledPart++;
  }
}