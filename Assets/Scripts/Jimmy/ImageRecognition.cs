using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageRecognition : MonoBehaviour
{
  [SerializeField] TrackedPrefab[] prefabToInstantiate;

  private int _refImageCount;
  private Dictionary<string, GameObject> _arObjs = new Dictionary<string, GameObject>();

  private ARTrackedImageManager _arTrackedImageManager;
  private IReferenceImageLibrary _referenceImageLibrary;

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
    _referenceImageLibrary = _arTrackedImageManager.referenceLibrary;
    _refImageCount = _referenceImageLibrary.count;
    // InstantiateObj();
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

        _arObjs.Add(added.referenceImage.name, prefab);
      }
    }
    // _arObjs = new Dictionary<string, GameObject>();
    // for (int i = 0; i < _refImageCount; i++)
    // {
    //   Debug.Log("Prefab name: " + prefabToInstantiate[i].name);
    //   string prefabName = prefabToInstantiate[i].name;
    //   GameObject prefab = prefabToInstantiate[i].prefab;
    //   GameObject arObj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
    //   arObj.GetComponent<Collectable>().SetInventoryItem();
    //   for (int j = 0; j < _refImageCount; j++)
    //   {
    //     if (_referenceImageLibrary[j].name == prefabName)
    //     {
    //       Debug.Log("Image library name: " + _referenceImageLibrary[i].name);
    //       _arObjs.Add(_referenceImageLibrary[j].name, arObj);
    //       _arObjs[_referenceImageLibrary[j].name].SetActive(false);
    //     }
    //   }
    // }
  }

  private void ActivateTrackedObj(string _imageName)
  {
    Debug.Log("Activate: name " + _imageName);
    _arObjs[_imageName].SetActive(true);
  }

  private void DeactivateTrackedObj(string _imageName)
  {
    Debug.Log("Deactivate: name" + _imageName);
    _arObjs[_imageName].SetActive(false);
  }

  void OnImageChanged(ARTrackedImagesChangedEventArgs args)
  {
    foreach (var added in args.added)
    {
      InstantiateObj(added);
      // ActivateTrackedObj(added.referenceImage.name);
    }

    foreach (var updated in args.updated)
    {
      if (updated.trackingState == TrackingState.Tracking)
      {
        UpdateTrackingObj(updated);
      }
      else if (updated.trackingState == TrackingState.Limited)
      {
        UpdateLimitedObj(updated);
      }
      else
      {
        UpdateNoneObj(updated);
      }
      // Debug.Log("Update: name: ");
      // Debug.Log(updated.referenceImage.name);
      // for (int i = 0; i < _arObjs.Count; i++)
      // {
      //   Debug.Log("found it");
      //   if (!_arObjs.TryGetValue(updated.referenceImage.name, out GameObject prefab)) continue;
      //   prefab.transform.position = updated.transform.position;
      //   prefab.transform.rotation = updated.transform.rotation;
      //   prefab.SetActive(true);
      // }

      // _arObjs[updated.referenceImage.name].transform.position = updated.transform.position;
      // _arObjs[updated.referenceImage.name].transform.rotation = updated.transform.rotation;
    }

    foreach (var remove in args.removed)
    {
      // DeactivateTrackedObj(remove.referenceImage.name);
      DestroyObj(remove);
    }
  }

  private void UpdateTrackingObj(ARTrackedImage updated)
  {
    for (int i = 0; i < _arObjs.Count; i++)
    {
      if (!_arObjs.TryGetValue(updated.referenceImage.name, out GameObject prefab)) continue;
      prefab.transform.position = updated.transform.position;
      prefab.transform.rotation = updated.transform.rotation;
      prefab.SetActive(true);
    }
  }

  private void UpdateLimitedObj(ARTrackedImage updated)
  {
    for (int i = 0; i < _arObjs.Count; i++)
    {
      if (!_arObjs.TryGetValue(updated.referenceImage.name, out GameObject prefab)) continue;
      if (!prefab.GetComponent<ARTrackedImage>().destroyOnRemoval)
      {
        prefab.transform.position = updated.transform.position;
        prefab.transform.rotation = updated.transform.rotation;
        prefab.SetActive(true);
      }
      else
      {
        prefab.SetActive(false);
      }
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
}

[System.Serializable]
public struct TrackedPrefab
{
  public string name;
  public GameObject prefab;
}