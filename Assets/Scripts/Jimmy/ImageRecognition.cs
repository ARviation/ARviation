using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageRecognition : MonoBehaviour
{
  [SerializeField] TrackedPrefab[] prefabToInstantiate;
  [SerializeField] private CollectPanel collectPanel;
  [SerializeField] private Image alreadyUsedImageHolder;

  private int _refImageCount;
  private Dictionary<string, GameObject> _arObjs = new Dictionary<string, GameObject>();
  private Dictionary<string, bool> _arObjsUsed = new Dictionary<string, bool>();
  private Dictionary<MorseCode, string> _arObjsMarker = new Dictionary<MorseCode, string>();

  private ARTrackedImageManager _arTrackedImageManager;
  private IReferenceImageLibrary _referenceImageLibrary;
  private NarrationController _narrationController;
  private bool hasMorseCodePlayed = false;

  void Awake()
  {
    _arTrackedImageManager = GetComponent<ARTrackedImageManager>();
  }

  void OnEnable()
  {
    _arTrackedImageManager.trackedImagesChanged += OnImageChanged;
  }

  void OnDisable()
  {
    _arTrackedImageManager.trackedImagesChanged -= OnImageChanged;
  }

  private void Start()
  {
    _narrationController = FindObjectOfType<NarrationController>();
    _referenceImageLibrary = _arTrackedImageManager.referenceLibrary;
    _refImageCount = _referenceImageLibrary.count;
    alreadyUsedImageHolder.gameObject.SetActive(false);
  }

  private void InstantiateObj(ARTrackedImage added)
  {
    for (int i = 0; i < _refImageCount; i++)
    {
      if (added.referenceImage.name == prefabToInstantiate[i].name)
      {
        GameObject prefab = Instantiate<GameObject>(prefabToInstantiate[i].prefab, transform.parent);
        prefab.transform.position = added.transform.position;
        prefab.transform.rotation = added.transform.rotation;
        prefab.transform.localScale = Vector3.zero;

        _arObjs.Add(added.referenceImage.name, prefab);
        _arObjsUsed.Add(added.referenceImage.name, false);
        _arObjsMarker.Add(prefabToInstantiate[i].code, added.referenceImage.name);
      }
    }
  }

  void OnImageChanged(ARTrackedImagesChangedEventArgs args)
  {
    foreach (var added in args.added)
    {
      InstantiateObj(added);
    }

    foreach (var updated in args.updated)
    {
      if (_narrationController.GetHadHideCondition())
      {
        if (updated.referenceImage.name == GameManager.Fuselage)
          UpdateImage(updated);
      }
      else
      {
        UpdateImage(updated);
      }
    }

    foreach (var remove in args.removed)
    {
      DestroyObj(remove);
    }
  }

  private void UpdateImage(ARTrackedImage image)
  {
    if (image.trackingState == TrackingState.Tracking)
    {
      UpdateTrackingObj(image);
    }
    else if (image.trackingState == TrackingState.Limited)
    {
      UpdateLimitedObj(image);
    }
    else
    {
      UpdateNoneObj(image);
    }
  }

  private void UpdateTrackingObj(ARTrackedImage updated)
  {
    for (int i = 0; i < _arObjs.Count; i++)
    {
      if (!_arObjs.TryGetValue(updated.referenceImage.name, out GameObject prefab)) continue;
      if (!_arObjsUsed.TryGetValue(updated.referenceImage.name, out bool used)) continue;
      if (used)
      {
        alreadyUsedImageHolder.gameObject.SetActive(true);
      }
      else
      {
        alreadyUsedImageHolder.gameObject.SetActive(false);
        prefab.transform.position = updated.transform.position;
        prefab.transform.rotation = updated.transform.rotation;
        prefab.transform.localScale = Vector3.one;
        prefab.SetActive(true);

        InventoryItem inventoryItem = prefab.GetComponent<Collectable>().GetInventoryItem();
        Collectable collectable = prefab.GetComponent<Collectable>();
        MorseCode code = collectable.componentCode;
        if (!hasMorseCodePlayed)
        {
          SoundManager.Instance.PlaySFXByMorseCode(code);
          hasMorseCodePlayed = true;
        }

        inventoryItem.OnHitComponent(code);
        string componentName = prefab.name;
        componentName = componentName.Replace("(Clone)", "").Trim();
        collectPanel.OpenPanel(componentName);
        collectPanel.SetInventoryItem(inventoryItem);
      }
    }
  }

  private void UpdateLimitedObj(ARTrackedImage updated)
  {
    for (int i = 0; i < _arObjs.Count; i++)
    {
      if (!_arObjs.TryGetValue(updated.referenceImage.name, out GameObject prefab)) continue;
      if (!_arObjsUsed.TryGetValue(updated.referenceImage.name, out bool used)) continue;

      hasMorseCodePlayed = false;
      alreadyUsedImageHolder.gameObject.SetActive(false);
      prefab.SetActive(false);
      collectPanel.ClosePanel();
    }
  }

  private void UpdateNoneObj(ARTrackedImage updated)
  {
    for (int i = 0; i < _arObjs.Count; i++)
    {
      if (_arObjs.TryGetValue(updated.referenceImage.name, out GameObject prefab))
      {
        prefab.SetActive(false);
      }

      if (!_arObjsUsed.TryGetValue(updated.referenceImage.name, out bool used)) continue;
      if (used)
      {
        prefab.SetActive(false);
      }
    }
  }

  private void DestroyObj(ARTrackedImage removed)
  {
    for (int i = 0; i < _arObjs.Count; i++)
    {
      if (_arObjs.TryGetValue(removed.referenceImage.name, out GameObject prefab))
      {
        _arObjs.Remove(removed.referenceImage.name);
        Destroy(prefab);
      }
    }
  }

  public void HideUsedObj(string name)
  {
    if (!_arObjs.TryGetValue(name, out GameObject prefab))
      return;
    prefab.SetActive(false);
    _arObjsUsed[name] = true;
  }

  public void UnUsedObj(string name)
  {
    if (!_arObjs.TryGetValue(name, out GameObject prefab))
      return;
    prefab.SetActive(true);
    _arObjsUsed[name] = false;
  }

  public string GetPrefabName(MorseCode code)
  {
    return !_arObjsMarker.TryGetValue(code, out string name) ? "" : name;
  }
}

[System.Serializable]
public struct TrackedPrefab
{
  public string name;
  public GameObject prefab;
  public MorseCode code;
}