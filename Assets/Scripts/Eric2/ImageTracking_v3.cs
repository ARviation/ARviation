using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTracking_v3 : MonoBehaviour
{
    // Variables
    public List<string> marker_name_list;
    string marker_name = "";
    public GameObject airplane_prefab;
    public bool isMarkerDetected = false;
    public GameObject scanner;
    private ARTrackedImageManager trackedImageManager;

    GameObject airplane;
    //AudioSource source;
    Quaternion sampledRotation = new Quaternion(0, 0, 0, 0);
    Vector3 airplane_angle_init;
    int counter = 0;
    int counter_max = 100;


    // Awake
    private void Awake()
    {
        // deactivate scanner
        Debug.Log("image tracking awake starts");
        scanner.SetActive(false);

        // reset
        GameObject.Find("AR Session").GetComponent<ARSession>().Reset();

        // tracked image manager
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        // airplane
        airplane = Instantiate(airplane_prefab, Vector3.zero, Quaternion.identity);
        airplane_angle_init = airplane.transform.localEulerAngles;
        airplane.SetActive(false);

        //// sound
        //source = gameObject.AddComponent<AudioSource>();
        //AudioClip clip = Resources.Load<AudioClip>("AudioClip/sound_camera_snap");
        //source.clip = clip;        

        // activate scanner
        Debug.Log("image tracking awake ends");
        scanner.SetActive(true);
    }


    // OnEnable
    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += ImageChanged;
    }


    // OnDisable
    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= ImageChanged;
    }


    // ImageChanged
    void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        Debug.Log("flying scene: image changed");
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            string name = trackedImage.referenceImage.name;
            if (marker_name.Length == 0 & marker_name_list.IndexOf(name) >= 0)
            {
                marker_name = name;
                UpdateImage(trackedImage);
                #if UNITY_ANDROID || UNITY_IPHONE
                Handheld.Vibrate();
                #endif
                //source.Play();
                SFXmanager.playsound("scanner");
                isMarkerDetected = true;
            }                
        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            string name = trackedImage.referenceImage.name;
            if (name == marker_name)
            {
                UpdateImage(trackedImage);
            }
        }
    }


    // UpdateImage
    void UpdateImage(ARTrackedImage trackedImage)
    {
        // position
        airplane.transform.position = trackedImage.transform.position;

        // rotation
        Quaternion MarkerAngles = trackedImage.transform.rotation;
        if (counter < counter_max) counter++;
        if (counter > 1 & counter < counter_max)
        {
            Debug.Log("counter = " + counter);
            sampledRotation = Quaternion.Lerp(sampledRotation, MarkerAngles, 1.0f / (counter - 1));
        }
        airplane.transform.rotation = sampledRotation * Quaternion.Euler(airplane_angle_init);
        airplane.SetActive(true);
    }
}


////////////////////////////////////////////////////////////////////////////////////////////////////////


//string marker_L_name = "marker31_L";
//string marker_L_name = "marker40_L28";



//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.XR;
//using UnityEngine.XR.ARFoundation;
//using UnityEngine.SceneManagement;


//[RequireComponent(typeof(ARTrackedImageManager))]
//public class ImageTracking : MonoBehaviour
//{
//    // variables
//    [System.Serializable]
//    public struct AR_item
//    {
//        public string MarkerName;
//        public GameObject AugmentObject;
//    }
//    public AR_item[] AR_item_list;

//    Dictionary<string, bool> dict_detected = new Dictionary<string, bool>();
//    Dictionary<string, GameObject> dict_object = new Dictionary<string, GameObject>();

//    ARTrackedImageManager trackedImageManager;
//    AudioSource source;


//    // Awake
//    void Awake()
//    {
//        // reset
//        GameObject.Find("AR Session").GetComponent<ARSession>().Reset();

//        // initialization
//        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();

//        // set dict_object
//        for (int i = 0; i < AR_item_list.Length; i++)
//        {
//            GameObject object_AR = AR_item_list[i].AugmentObject;
//            string MarkerName = AR_item_list[i].MarkerName;
//            object_AR.name = MarkerName;
//            object_AR.SetActive(false);
//            dict_object.Add(MarkerName, object_AR);
//        }

//        // get marker list
//        var lib = GameObject.Find("AR Session Origin").GetComponent<ARTrackedImageManager>().referenceLibrary;
//        int N_marker = lib.count;
//        string[] marker_list = new string[N_marker];
//        for (int i = 0; i < N_marker; i++)
//        {
//            marker_list[i] = lib[i].name;
//            //print(marker_list[i]);
//        }

//        // set dict_detected
//        foreach (string marker in marker_list)
//        {
//            dict_detected.Add(marker, false);
//        }

//        // sound
//        source = gameObject.AddComponent<AudioSource>();
//        AudioClip clip = Resources.Load<AudioClip>("AudioClip/sound_camera_snap");
//        source.clip = clip;
//    }


//    // Start
//    void Start()
//    {
//    }


//    // OnEnable
//    private void OnEnable()
//    {
//        trackedImageManager.trackedImagesChanged += ImageChanged;
//    }


//    // OnDisable
//    private void OnDisable()
//    {
//        trackedImageManager.trackedImagesChanged -= ImageChanged;
//    }


//    // ImageChanged
//    void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
//    {
//        if (eventArgs.added.Count > 0)
//        {            
//            UpdateImage(eventArgs.added);
//            #if UNITY_ANDROID || UNITY_IPHONE
//                Handheld.Vibrate();
//            #endif
//            source.Play();

//            for (int i = 0; i < eventArgs.added.Count; i++)
//            {
//                dict_detected[eventArgs.added[i].referenceImage.name] = true;
//            }
//            return;
//        }

//        if (eventArgs.updated.Count > 0)
//        {
//            UpdateImage(eventArgs.updated);
//            return;
//        }
//    }


//    // UpdateImage
//    void UpdateImage(List<ARTrackedImage> trackedImageList)
//    {
//        foreach (ARTrackedImage trackedImage in trackedImageList)
//        {
//            string MarkerName = trackedImage.referenceImage.name;
//            Vector3 MarkerPosition = trackedImage.transform.position;
//            if (dict_object.ContainsKey(MarkerName))
//            {
//                GameObject object_AR = dict_object[MarkerName];
//                object_AR.transform.position = MarkerPosition;
//                object_AR.SetActive(true);
//            }
//        }

//    }
//}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//// read offset
//(offset, theta, phi) = csv_manager.instance.read_offset();

//// get marker list
//var lib = GameObject.Find("AR Session Origin").GetComponent<ARTrackedImageManager>().referenceLibrary;
//int N_marker = lib.count;
//string[] marker_list = new string[N_marker];
//for (int i = 0; i < N_marker; i++)
//{
//    marker_list[i] = lib[i].name;
//    //print(marker_list[i]);
//}

//// set dictionaries
//foreach (string marker in marker_list)
//{
//    dict_detected.Add(marker, false);
//}


//// sound
//source = gameObject.AddComponent<AudioSource>();
//AudioClip clip = Resources.Load<AudioClip>("AudioClip/sound_camera_snap");
//source.clip = clip;

// variables
//public GameObject AR_object;
//public GameObject AR_marker;
//public GameObject AugmentedWorld;
//public Vector3 offset = Vector3.zero;
//public float theta = 0;
//public float phi = 0;
//public bool isMarkerDetected = false;




////debug <<<
//Debug.Log("counter = " + counter);
//foreach (var image in trackedImageList)
//{
//    Debug.Log("    marker name: " + image.referenceImage.name);
//}
////debug >>>

////debug <<<
//string marker_name2 = trackedImage.referenceImage.name;
//if (marker_name2 == null)
//{
//    Debug.Log("scene_name: " + scene_name + "  maker_name: null");
//}
//else
//{
//    Debug.Log("scene_name: " + scene_name + "  maker_name: " + marker_name2);
//}
////debug >>>

//// to remove <<<
//for (int i = 0; i < eventArgs.added.Count; i++)
//{
//    dict_detected[eventArgs.added[i].referenceImage.name] = true;
//}            
//// to remove >>>

//foreach (ARTrackedImage trackedImage in trackedImageList)
//{
//    // to remove <<<
//    string MarkerName = trackedImage.referenceImage.name;
//    bool enabled = dict_enabled[MarkerName];
//    if (!enabled) continue;

//    //Vector3 offset = dict_offset[MarkerName];
//    offset = dict_offset[MarkerName];

//    float scale = dict_scale[MarkerName];
//    // to remove >>>


//    Vector3 MarkerPosition = trackedImage.transform.position;


//    //ROT <<<
//    Quaternion MarkerAngles = trackedImage.transform.rotation;
//    MarkerAngles = MarkerAngles * Quaternion.Euler(new Vector3(90, 0, 0));
//    //ROT >>>


//    // to change <<<
//    if (dict_object.ContainsKey(MarkerName))
//    {
//        GameObject object_AR = dict_object[MarkerName];

//        //POS <<<
//        object_AR.transform.position = MarkerPosition + offset - object_AR.transform.rotation * AR_marker_position;
//        //POS >>>

//        //object_AR.transform.eulerAngles = angles;
//        object_AR.transform.localScale = Vector3.one * scale;              
//        object_AR.SetActive(true);

//        Debug.Log("object_AR name " + object_AR.name);
//        Debug.Log("object_AR position " + object_AR.transform.position);

//        //ROT <<<
//        if (counter > 1 & counter < 200)
//        {                    
//            sampledRotation = Quaternion.Lerp(sampledRotation, MarkerAngles, 1.0f / (counter - 1));  //use marker rotation at the start and neglect marker rotation gradually (lerp)        
//            object_AR.transform.rotation = sampledRotation;
//        }
//        //ROT >>>
//    }
// }
// to change >>>
//// to remove <<<
////Debug.Log("ok0002");
//// get marker list
//var lib = GameObject.Find("AR Session Origin").GetComponent<ARTrackedImageManager>().referenceLibrary;
//int N_marker = lib.count;
//string[] marker_list = new string[N_marker];
//for (int i = 0; i < N_marker; i++)
//{
//    marker_list[i] = lib[i].name;
//    //print(marker_list[i]);
//}

//// set dictionaries
//foreach (string marker in marker_list)
//{
//    dict_offset.Add(marker, new Vector3(0, 0, 0));
//    dict_scale.Add(marker, 1f);
//    dict_enabled.Add(marker, true);
//    dict_detected.Add(marker, false);
//}
//// to remove >>>

//// to change <<<
//csv_manager.instance.read_offset(dict_offset, dict_scale);
//// to change >>>


//// to remove <<<
////Debug.Log("ok0001");
//for (int i = 0; i < AR_item_list.Length; i++)
//{
//    //GameObject prefab_AR = AR_item_list[i].AugmentObject;
//    GameObject object_AR = AR_item_list[i].AugmentObject;
//    string MarkerName = AR_item_list[i].MarkerName;
//    //GameObject object_AR = Instantiate(prefab_AR, Vector3.zero, Quaternion.identity);
//    //object_AR.name = MarkerName;
//    //POS <<<
//    //print("ok0001");
//    //object_AR.transform.position = -AR_marker.transform.position;
//    //POS >>>
//    object_AR.SetActive(false);
//    dict_object.Add(MarkerName, object_AR);
//    //Debug.Log("aug pos " + object_AR.name + "  " + object_AR.transform.position);
//}
//// to remove >>>



//// fields (old)
//[System.Serializable]
//public struct AR_item
//{
//    public string MarkerName;
//    public GameObject AugmentObject;
//}
//public AR_item[] AR_item_list;

////public GameObject prefab_AR = null;

//public Dictionary<string, Vector3> dict_offset = new Dictionary<string, Vector3>();
//public Dictionary<string, float> dict_scale = new Dictionary<string, float>();
//public Dictionary<string, bool> dict_enabled = new Dictionary<string, bool>();
//public Dictionary<string, bool> dict_detected = new Dictionary<string, bool>();

////GameObject object_AR = null;
//Dictionary<string, GameObject> dict_object = new Dictionary<string, GameObject>();




//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.XR;
//using UnityEngine.XR.ARFoundation;


//[RequireComponent(typeof(ARTrackedImageManager))]
//public class ImageTracking : MonoBehaviour
//{
//    // fields
//    [System.Serializable]
//    public struct AR_item
//    {
//        public string MarkerName;
//        public GameObject AugmentObject;
//    }
//    public AR_item[] AR_item_list;
//    public GameObject AR_marker;
//    Vector3 AR_marker_position;

//    //ROT <<<
//    Quaternion sampledRotation = new Quaternion(0, 0, 0, 0);
//    int counter = 0;
//    //ROT >>>

//    //public GameObject prefab_AR = null;

//    public Dictionary<string, Vector3> dict_offset = new Dictionary<string, Vector3>();
//    public Dictionary<string, float> dict_scale = new Dictionary<string, float>();
//    public Dictionary<string, bool> dict_enabled = new Dictionary<string, bool>();
//    public Dictionary<string, bool> dict_detected = new Dictionary<string, bool>();

//    //GameObject object_AR = null;
//    Dictionary<string, GameObject> dict_object = new Dictionary<string, GameObject>();

//    ARTrackedImageManager trackedImageManager;

//    AudioSource source;


//    // Awake
//    void Awake()
//    {
//        //Debug.Log("ok0001");
//        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
//        for (int i = 0; i < AR_item_list.Length; i++)
//        {
//            //GameObject prefab_AR = AR_item_list[i].AugmentObject;
//            GameObject object_AR = AR_item_list[i].AugmentObject;
//            string MarkerName = AR_item_list[i].MarkerName;
//            //GameObject object_AR = Instantiate(prefab_AR, Vector3.zero, Quaternion.identity);
//            //object_AR.name = MarkerName;
//            //POS <<<
//            //print("ok0001");
//            //object_AR.transform.position = -AR_marker.transform.position;
//            //POS >>>
//            object_AR.SetActive(false);
//            dict_object.Add(MarkerName, object_AR);
//            //Debug.Log("aug pos " + object_AR.name + "  " + object_AR.transform.position);
//        }

//        AR_marker_position = AR_marker.transform.position;
//    }


//    // Start
//    void Start()
//    {
//        //Debug.Log("ok0002");
//        // get marker list
//        var lib = GameObject.Find("AR Session Origin").GetComponent<ARTrackedImageManager>().referenceLibrary;
//        int N_marker = lib.count;
//        string[] marker_list = new string[N_marker];
//        for (int i = 0; i < N_marker; i++)
//        {
//            marker_list[i] = lib[i].name;
//            //print(marker_list[i]);
//        }

//        // set dictionaries
//        foreach (string marker in marker_list)
//        {
//            dict_offset.Add(marker, new Vector3(0, 0, 0));
//            dict_scale.Add(marker, 1f);
//            dict_enabled.Add(marker, true);
//            dict_detected.Add(marker, false);
//        }
//        csv_manager.instance.read_offset(dict_offset, dict_scale);

//        // sound
//        source = gameObject.AddComponent<AudioSource>();
//        //AudioClip clip = Resources.Load<AudioClip>("AudioClip/sound_ui_hover");
//        AudioClip clip = Resources.Load<AudioClip>("AudioClip/sound_camera_snap");
//        source.clip = clip;
//        //source.Play();       
//    }


//    ////debug <<<
//    //private void Update()
//    //{
//    //    AR_marker_position = AR_marker.transform.position;
//    //    Vector3 AR_marker_localPosition = AR_marker.transform.localPosition;
//    //    Debug.Log("AR marker position: " + AR_marker_position.x + " " + AR_marker_position.y + " " + AR_marker_position.z + " ");
//    //    Debug.Log("AR marker local position: " + AR_marker_localPosition.x + " " + AR_marker_localPosition.y + " " + AR_marker_localPosition.z + " ");
//    //}
//    ////debug >>>


//    // OnEnable
//    private void OnEnable()
//    {
//        trackedImageManager.trackedImagesChanged += ImageChanged;
//    }


//    // OnDisable
//    private void OnDisable()
//    {
//        trackedImageManager.trackedImagesChanged -= ImageChanged;
//    }


//    // ImageChanged
//    void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
//    {
//        //Debug.Log("ok0003");
//        if (eventArgs.added.Count > 0)
//        {
//            UpdateImage(eventArgs.added);
//            Handheld.Vibrate();
//            source.Play();
//            for (int i = 0; i < eventArgs.added.Count; i++)
//            {
//                dict_detected[eventArgs.added[i].referenceImage.name] = true;
//            }
//            return;
//        }

//        if (eventArgs.updated.Count > 0)
//        {
//            UpdateImage(eventArgs.updated);
//            return;
//        }
//    }


//    // UpdateImage
//    void UpdateImage(List<ARTrackedImage> trackedImageList)
//    {
//        //ROT <<<
//        counter++;
//        //ROT >>>
//        foreach (ARTrackedImage trackedImage in trackedImageList)
//        {
//            string MarkerName = trackedImage.referenceImage.name;
//            bool enabled = dict_enabled[MarkerName];
//            if (!enabled) continue;
//            Vector3 offset = dict_offset[MarkerName];
//            float scale = dict_scale[MarkerName];
//            Vector3 MarkerPosition = trackedImage.transform.position;

//            ////debug <<<
//            //Debug.Log("MarkerPosition: " + MarkerPosition.x + " " + MarkerPosition.y + " " + MarkerPosition.z + " ");
//            //Debug.Log("offset: " + offset.x + " " + offset.y + " " + offset.z + " ");
//            ////debug >>>

//            //ROT <<<
//            Quaternion MarkerAngles = trackedImage.transform.rotation;
//            MarkerAngles = MarkerAngles * Quaternion.Euler(new Vector3(90, 0, 0));
//            //ROT >>>
//            //Vector3 MarkerAngles = trackedImage.transform.eulerAngles;
//            //Vector3 angles = MarkerAngles + new Vector3(90, 0, 0);

//            //POS <<<
//            //print("AR_marker_position " + AR_marker_position);
//            //Vector3 position = MarkerPosition + offset - AR_marker_position;            
//            //Vector3 position = MarkerPosition + offset;
//            //Debug.Log("position " + position);
//            //Debug.Log("MarkerPosition " + MarkerPosition);
//            //Debug.Log("AR_marker_position " + AR_marker_position);      
//            //POS >>>

//            if (dict_object.ContainsKey(MarkerName))
//            {
//                GameObject object_AR = dict_object[MarkerName];

//                //POS <<<
//                object_AR.transform.position = MarkerPosition + offset - object_AR.transform.rotation * AR_marker_position;
//                //POS >>>

//                //object_AR.transform.eulerAngles = angles;
//                object_AR.transform.localScale = Vector3.one * scale;
//                object_AR.SetActive(true);

//                Debug.Log("object_AR name " + object_AR.name);
//                Debug.Log("object_AR position " + object_AR.transform.position);

//                //ROT <<<
//                if (counter > 1 & counter < 200)
//                {
//                    sampledRotation = Quaternion.Lerp(sampledRotation, MarkerAngles, 1.0f / (counter - 1));  //use marker rotation at the start and neglect marker rotation gradually (lerp)        
//                    object_AR.transform.rotation = sampledRotation;
//                }
//                //ROT >>>

//                ////POS <<<
//                //Vector3 AR_marker_position_new = AR_marker.transform.position;
//                //Vector3 AR_marker_localPosition_new = AR_marker.transform.localPosition;
//                //Vector3 pos1 = AR_marker.transform.parent.transform.position;
//                //Vector3 pos2 = AR_marker.transform.position;
//                //Vector3 pos3 = AR_marker.transform.localPosition;
//                //Debug.Log("AR_marker_position_new " + AR_marker_position_new);
//                //Debug.Log("AR_marker_localPosition_new " + AR_marker_localPosition_new);
//                //Debug.Log("pos1 = " + pos1);
//                //Debug.Log("pos2 = " + pos2);
//                //Debug.Log("pos3 = " + pos3);
//                ////POS >>>
//            }

//        }
//    }

//}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//if (isFirstCall)
//{
//    object_AR.transform.Rotate(0, theta, 0);
//    object_AR.transform.localScale *= scale;
//}  

//Vector3 angles = trackedImage.transform.eulerAngles;
//Vector3 angles_new = angles;
//object_AR.transform.eulerAngles = angles_new;

//object_AR.transform.eulerAngles = trackedImageList[0].transform.eulerAngles;

//int n = trackedImageList.Count;
//Vector3 position = Vector3.zero;
// calculate AR object position by averaging anchors of tracked images 
//Debug.Log("m = " + trackedImageList.Count);

//Vector3 position_this = trackedImage.transform.position + offset;
//position += position_this / n;



//// Update
//void Update()
//{
//    //if (Input.GetKeyDown(KeyCode.Space))
//    //{
//    //    for (int i = 0; i < 1; i++)
//    //    {
//    //        print("i = " + i);
//    //        source.Play();
//    //    }
//    //}
//    //foreach (string key in dict_enabled.Keys)
//    //{
//    //    print(key + " " + dict_enabled[key]);
//    //}
//    //Debug.Log("ok0012");
//}

//Debug.Log("tracked image name: " + name + "  position = [" + position.x + ", " + position.y + ", " + position.z + "]");
//Debug.Log("   prefab name: " + prefab.name + " position =  [" + pos0.x + ", " + pos0.y + ", " + pos0.z + "]");

//// UpdateImage
//void UpdateImage(ARTrackedImage trackedImage)
//{
//    string name = trackedImage.referenceImage.name;
//    object_AR.transform.position = trackedImage.transform.position;
//    object_AR.SetActive(true);
//    //GameObject prefab = spawnedPrefabs[name];        
//    //prefab.transform.position = trackedImage.transform.position;
//    //prefab.SetActive(true);
//    //foreach (GameObject go in spawnedPrefabs.Values)
//    //{
//    //    if(go.name != name)
//    //    {
//    //        go.SetActive(false);
//    //    }
//    //}
//}



//foreach(ARTrackedImage trackedImage in eventArgs.added)
//{
//    //Debug.Log("image added...");
//    UpdateImage(trackedImage);
//}
//foreach (ARTrackedImage trackedImage in eventArgs.updated)
//{
//    //Debug.Log("image updated...");
//    UpdateImage(trackedImage);
//}
//foreach (ARTrackedImage trackedImage in eventArgs.removed)
//{
//    //Debug.Log("image removed...");
//    spawnedPrefabs[trackedImage.name].SetActive(false);
//}



//[System.Serializable]
//public struct TagOffset
//{
//    public string TagName;
//    public Vector3 offset;
//}
//public TagOffset[] TagOffset_list;




//foreach (GameObject prefab in placeablePrefabs)
//{
//    GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
//    newPrefab.name = prefab.name;
//    spawnedPrefabs.Add(prefab.name, newPrefab);
//    newPrefab.SetActive(false);
//}





//// set offset dictionary (to change)
//for (int i = 0; i < TagOffset_list.Length; i++)
//{
//    dict_offset.Add(TagOffset_list[i].TagName, TagOffset_list[i].offset);
//}

//// set enabled dictionary (to change)
//for (int i = 0; i < TagOffset_list.Length; i++)
//{
//    dict_enabled.Add(TagOffset_list[i].TagName, true);
//}

//// set detected dictionary (to change)
//for (int i = 0; i < TagOffset_list.Length; i++)
//{
//    dict_detected.Add(TagOffset_list[i].TagName, false);
//}

//Debug.Log("ok0010");

//// set dict_offset, dict_detected, dict_enabled
//dict_offset = csv_manager.instance.read_offset();
//foreach (string key in dict_offset.Keys)
//{
//    dict_detected.Add(key, false);
//    dict_enabled.Add(key, true);
//}



//Debug.Log("ok0011");

////debug <<<
//void Update()
//{
//    List<string> key_list = new List<string>(dict_detected.Keys);
//    int n = 0;
//    foreach (string key in dict_detected.Keys)
//    {
//        if (dict_detected[key]) n = n + 1;
//    }
//    Debug.Log("# detected = " + n);
//}
////debug >>>
///







//if (isFirstCall)
//{
//    object_AR.transform.Rotate(0, theta, 0);
//    object_AR.transform.localScale *= scale;
//}  

//Vector3 angles = trackedImage.transform.eulerAngles;
//Vector3 angles_new = angles;
//object_AR.transform.eulerAngles = angles_new;

//object_AR.transform.eulerAngles = trackedImageList[0].transform.eulerAngles;

//int n = trackedImageList.Count;
//Vector3 position = Vector3.zero;
// calculate AR object position by averaging anchors of tracked images 
//Debug.Log("m = " + trackedImageList.Count);

//Vector3 position_this = trackedImage.transform.position + offset;
//position += position_this / n;



//// Update
//void Update()
//{
//    //if (Input.GetKeyDown(KeyCode.Space))
//    //{
//    //    for (int i = 0; i < 1; i++)
//    //    {
//    //        print("i = " + i);
//    //        source.Play();
//    //    }
//    //}
//    //foreach (string key in dict_enabled.Keys)
//    //{
//    //    print(key + " " + dict_enabled[key]);
//    //}
//    //Debug.Log("ok0012");
//}

//Debug.Log("tracked image name: " + name + "  position = [" + position.x + ", " + position.y + ", " + position.z + "]");
//Debug.Log("   prefab name: " + prefab.name + " position =  [" + pos0.x + ", " + pos0.y + ", " + pos0.z + "]");

//// UpdateImage
//void UpdateImage(ARTrackedImage trackedImage)
//{
//    string name = trackedImage.referenceImage.name;
//    object_AR.transform.position = trackedImage.transform.position;
//    object_AR.SetActive(true);
//    //GameObject prefab = spawnedPrefabs[name];        
//    //prefab.transform.position = trackedImage.transform.position;
//    //prefab.SetActive(true);
//    //foreach (GameObject go in spawnedPrefabs.Values)
//    //{
//    //    if(go.name != name)
//    //    {
//    //        go.SetActive(false);
//    //    }
//    //}
//}



//foreach(ARTrackedImage trackedImage in eventArgs.added)
//{
//    //Debug.Log("image added...");
//    UpdateImage(trackedImage);
//}
//foreach (ARTrackedImage trackedImage in eventArgs.updated)
//{
//    //Debug.Log("image updated...");
//    UpdateImage(trackedImage);
//}
//foreach (ARTrackedImage trackedImage in eventArgs.removed)
//{
//    //Debug.Log("image removed...");
//    spawnedPrefabs[trackedImage.name].SetActive(false);
//}



//[System.Serializable]
//public struct TagOffset
//{
//    public string TagName;
//    public Vector3 offset;
//}
//public TagOffset[] TagOffset_list;




//foreach (GameObject prefab in placeablePrefabs)
//{
//    GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
//    newPrefab.name = prefab.name;
//    spawnedPrefabs.Add(prefab.name, newPrefab);
//    newPrefab.SetActive(false);
//}





//// set offset dictionary (to change)
//for (int i = 0; i < TagOffset_list.Length; i++)
//{
//    dict_offset.Add(TagOffset_list[i].TagName, TagOffset_list[i].offset);
//}

//// set enabled dictionary (to change)
//for (int i = 0; i < TagOffset_list.Length; i++)
//{
//    dict_enabled.Add(TagOffset_list[i].TagName, true);
//}

//// set detected dictionary (to change)
//for (int i = 0; i < TagOffset_list.Length; i++)
//{
//    dict_detected.Add(TagOffset_list[i].TagName, false);
//}

//Debug.Log("ok0010");

//// set dict_offset, dict_detected, dict_enabled
//dict_offset = csv_manager.instance.read_offset();
//foreach (string key in dict_offset.Keys)
//{
//    dict_detected.Add(key, false);
//    dict_enabled.Add(key, true);
//}



//Debug.Log("ok0011");

////debug <<<
//void Update()
//{
//    List<string> key_list = new List<string>(dict_detected.Keys);
//    int n = 0;
//    foreach (string key in dict_detected.Keys)
//    {
//        if (dict_detected[key]) n = n + 1;
//    }
//    Debug.Log("# detected = " + n);
//}
////debug >>>