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
  public List<GameObject> objsToPlace;

  private int _refImageCount;
  private Dictionary<string, GameObject> _arObjs;

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
    InitiateARObjs();
  }

  private void InitiateARObjs()
  {
    _arObjs = new Dictionary<string, GameObject>();
    for (int i = 0; i < _refImageCount; i++)
    {
      GameObject arObj = Instantiate(objsToPlace[i], Vector3.zero, Quaternion.identity);
      arObj.GetComponent<Collectable>().SetInventoryItem();
      _arObjs.Add(_referenceImageLibrary[i].name, arObj);
      _arObjs[_referenceImageLibrary[i].name].SetActive(false);
    }
  }

  private void ActivateTrackedObj(string _imageName)
  {
    _arObjs[_imageName].SetActive(true);
  }

  void OnImageChanged(ARTrackedImagesChangedEventArgs args)
  {
    foreach (var addedImage in args.added)
    {
      ActivateTrackedObj(addedImage.referenceImage.name);
    }

    foreach (var updated in args.updated)
    {
      _arObjs[updated.referenceImage.name].transform.position = updated.transform.position;
      _arObjs[updated.referenceImage.name].transform.rotation = updated.transform.rotation;
    }
  }
}