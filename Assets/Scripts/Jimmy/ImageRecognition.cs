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
  [SerializeField] private bool isTutorial = true;

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
        // if (isTutorial)
        // {
        //   if (added.referenceImage.name != "Fuselage" ||
        //       added.referenceImage.name != prefabToInstantiate[i].name) continue;
        //   GameObject prefab = Instantiate<GameObject>(prefabToInstantiate[i].prefab, transform.parent);
        //   prefab.transform.position = added.transform.position;
        //   prefab.transform.rotation = added.transform.rotation;
        //
        //   _arObjs.Add(added.referenceImage.name, prefab);
        // }
        // else
        // {
        GameObject prefab = Instantiate<GameObject>(prefabToInstantiate[i].prefab, transform.parent);
        prefab.transform.position = added.transform.position;
        prefab.transform.rotation = added.transform.rotation;
        prefab.transform.localScale = Vector3.zero;

        _arObjs.Add(added.referenceImage.name, prefab);
        // }
      }
    }
  }

  public void FinishTutorial()
  {
    isTutorial = false;
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
    }

    foreach (var updated in args.updated)
    {
      if (isTutorial)
      {
        if (updated.referenceImage.name != "Fuselage") continue;
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
      prefab.transform.position = updated.transform.position;
      prefab.transform.rotation = updated.transform.rotation;
      prefab.transform.localScale = Vector3.one;
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
        prefab.transform.localScale = Vector3.one;
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