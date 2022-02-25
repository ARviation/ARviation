using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageRecognition : MonoBehaviour
{
  [System.Serializable]
  public struct ARItem
  {
    public string MarkerName;
    public GameObject AugmentObj;
  }

  [SerializeField] private ARItem[] ARItemList;

  private Dictionary<string, bool> activeObjs = new Dictionary<string, bool>();
  private Dictionary<string, bool> detectedObjs = new Dictionary<string, bool>();
  private Dictionary<string, GameObject> arObjs = new Dictionary<string, GameObject>();

  private ARTrackedImageManager _arTrackedImageManager;

  void Awake()
  {
    _arTrackedImageManager = FindObjectOfType<ARTrackedImageManager>();

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

    foreach (ARItem arItem in ARItemList)
    {
      GameObject arObj = Instantiate(arItem.AugmentObj, Vector3.zero, Quaternion.identity);
      string markerName = arItem.MarkerName;
      arObj.name = arItem.MarkerName;
      arObjs.Add(arItem.MarkerName, arObj);
      arObj.SetActive(false);
      activeObjs.Add(markerName, true);
      detectedObjs.Add(markerName, false);
    }
  }

  void OnEnable()
  {
    _arTrackedImageManager.trackedImagesChanged += OnImageChanged;
  }

  void OnDisable()
  {
    _arTrackedImageManager.trackedImagesChanged -= OnImageChanged;
  }

  void OnImageChanged(ARTrackedImagesChangedEventArgs args)
  {
    Debug.Log("OnImageChanged");
    if (args.added.Count > 0)
    {
      Debug.Log("added");
      UpdateImage(args.added);
    }

    if (args.updated.Count > 0)
    {
      Debug.Log("updated");
      UpdateImage(args.updated);
    }
    // foreach (ARTrackedImage trackedImage in args.added)
    // {
    //   UpdateImage(trackedImage);
    // }
    //
    // foreach (ARTrackedImage trackedImage in args.updated)
    // {
    //   UpdateImage(trackedImage);
    // }
    
    foreach (ARTrackedImage trackedImage in args.removed)
    {
      arObjs[trackedImage.name].SetActive(false);
    }
  }

  void UpdateImage(List<ARTrackedImage> trackedImageList)
  {
    Debug.Log("update image");
    foreach (ARTrackedImage trackedImage in trackedImageList)
    {
      string name = trackedImage.referenceImage.name;
      detectedObjs[name] = true;
      if (!arObjs.ContainsKey(name)) continue;

      GameObject arObj = arObjs[name];
      Vector3 position = trackedImage.transform.position;
      if (activeObjs[name])
      {
        arObj.transform.position = position;
      }

      GameObject prefab = arObjs[name];
      prefab.transform.position = position;
      prefab.SetActive(true);
    }
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
  }
}