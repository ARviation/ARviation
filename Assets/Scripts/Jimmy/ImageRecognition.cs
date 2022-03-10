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
  // [System.Serializable]
  // public struct ARItem
  // {
  //   public string MarkerName;
  //   public GameObject AugmentObj;
  // }

  // [SerializeField] private ARItem[] ARItemList;
  // [SerializeField] private GameObject ARObjsParent;
  public List<GameObject> ObjsToPlace;
  
  private int refImageCount;
  // private Dictionary<string, bool> activeObjs = new Dictionary<string, bool>();
  // private Dictionary<string, bool> detectedObjs = new Dictionary<string, bool>();
  private Dictionary<string, GameObject> arObjs;

  private ARTrackedImageManager _arTrackedImageManager;
  private IReferenceImageLibrary _referenceImageLibrary;

  void Awake()
  {
    _arTrackedImageManager = GetComponent<ARTrackedImageManager>();

    // int numberOfARItem = ARItemList.Length;
    // for (int i = 0; i < numberOfARItem; i++)
    // {
    //   GameObject arObj = ARItemList[i].AugmentObj;
    //   string markerName = ARItemList[i].MarkerName;
    //   arObj.name = markerName;
    //   arObj.SetActive(false);
    //   arObjs.Add(markerName, arObj);
    //   activeObjs.Add(markerName, true);
    //   detectedObjs.Add(markerName, false);
    // }

    Debug.Log("Awake");
    // foreach (ARItem arItem in ARItemList)
    // {
    //   string markerName = arItem.MarkerName;
    //   GameObject arObj = Instantiate(arItem.AugmentObj, Vector3.zero, Quaternion.identity);
    //   arObj.transform.parent = ARObjsParent.transform;
    //   arObj.name = markerName;
    //   arObjs.Add(markerName, arObj);
    //   // activeObjs.Add(markerName, true);
    //   // detectedObjs.Add(markerName, false);
    //   arObj.SetActive(false);
    // }
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
    refImageCount = _referenceImageLibrary.count;
    InitiateARObjs();
  }

  private void InitiateARObjs()
  {
    Debug.Log("Initiate AR objs");
    arObjs = new Dictionary<string, GameObject>();
    for (int i = 0; i < refImageCount; i++)
    {
      GameObject arObj = Instantiate(ObjsToPlace[i], Vector3.zero, Quaternion.identity);
      arObjs.Add(_referenceImageLibrary[i].name, arObj);
      arObjs[_referenceImageLibrary[i].name].SetActive(false);
      // ARItemList[i].MarkerName = _referenceImageLibrary[i].name;
      // ARItemList[i].AugmentObj.SetActive(false);
    }
  }

  private void ActivateTrackedObj(string _imageName)
  {
    Debug.Log("Activate tracking image");
    arObjs[_imageName].SetActive(true);
  }

  void OnImageChanged(ARTrackedImagesChangedEventArgs args)
  {
    foreach (var addedImage in args.added)
    {
      ActivateTrackedObj(addedImage.referenceImage.name);
    }

    foreach (var updated in args.updated)
    {
      Debug.Log("Update existing image: " + updated.name);
      arObjs[updated.referenceImage.name].transform.position = updated.transform.position;
      arObjs[updated.referenceImage.name].transform.rotation = updated.transform.rotation;
    }
    // Debug.Log("OnImageChanged");
    // if (args.added.Count > 0)
    // {
    //   Debug.Log("added");
    //   UpdateImage(args.added);
    // }
    //
    // if (args.updated.Count > 0)
    // {
    //   Debug.Log("updated");
    //   UpdateImage(args.updated);
    // }
    //
    // foreach (ARTrackedImage trackedImage in args.removed)
    // {
    //   arObjs[trackedImage.name].SetActive(false);
    //   // detectedObjs[trackedImage.name] = false;
    // }
  }
  
  // public 
  //
  // void UpdateImage(List<ARTrackedImage> trackedImageList)
  // {
  //   foreach (ARTrackedImage trackedImage in trackedImageList)
  //   {
  //     string name = trackedImage.referenceImage.name;
  //     Debug.Log("Update image: " + name);
  //     // detectedObjs[name] = true;
  //     if (!arObjs.ContainsKey(name)) continue;
  //
  //     GameObject arObj = arObjs[name];
  //     Vector3 position = trackedImage.transform.position;
  //     // if (activeObjs[name])
  //     // if (arObjs[name])
  //     // {
  //     arObj.transform.position = position;
  //     // }
  //
  //     // arObj.transform.position = position;
  //     arObj.SetActive(true);
  //     // detectedObjs[name] = true;
  //   }
    // string name = trackedImage.referenceImage.name;
    // Vector3 position = trackedImage.transform.position;
    //
    // GameObject prefab = spawnedPrefabs[name];
    // prefab.transform.position = position;
    // prefab.SetActive(true);

    // foreach (GameObject obj in spawnedPrefabs.Values)
    // {
    //   if (obj.name != name)
    //   {
    //     // obj.SetActive(false);
    //   }
    // }
  // }
}