using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;


[RequireComponent(typeof(ARRaycastManager))]
public class PlaceOnPlane : MonoBehaviour
{
    [SerializeField]
    //[Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;
    public Button button_launch;
    public Button button_return;
    public ARSessionOrigin m_ARSessionOrigin;

    //UnityEvent placementUpdate;
    //[SerializeField]
    //GameObject visualObject;

    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    public GameObject spawnedObject { get; private set; }

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();        
        //if (placementUpdate == null)
        //    placementUpdate = new UnityEvent();

        //placementUpdate.AddListener(DiableVisual);
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        if (spawnedObject != null) return;

        if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            // raycast
            var hitPose = s_Hits[0].pose;
            spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
            // button
            button_launch.GetComponent<Button>().onClick.AddListener(button_launch_task);
            button_return.GetComponent<Button>().onClick.AddListener(button_return_task);
            // remove all detected planes
            var m_ARPlaneManager = m_ARSessionOrigin.GetComponent<ARPlaneManager>();
            foreach (var plane in m_ARPlaneManager.trackables)
            {
                plane.gameObject.SetActive(false);
            }
            m_ARPlaneManager.planePrefab = null;
            m_ARPlaneManager.enabled = false;
        }
    }



    // button_launch_task
    public void button_launch_task()
    {
        Animator m_Animator = spawnedObject.transform.Find("model_offset").transform.Find("model").GetComponent<Animator>();
        m_Animator.SetBool("is_launch", true);
        m_Animator.SetBool("is_land", false);
    }


    // button_land_task
    public void button_return_task()
    {
        Animator m_Animator = spawnedObject.transform.Find("model_offset").transform.Find("model").GetComponent<Animator>();
        m_Animator.SetBool("is_launch", false);
        m_Animator.SetBool("is_land", true);
    }



    //public void DiableVisual()
    //{
    //    visualObject.SetActive(false);
    //}

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    ARRaycastManager m_RaycastManager;
}


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//Animator m_Animator = spawnedObject.transform.Find("model").GetComponent<Animator>();
/// <summary>
/// Listens for touch events and performs an AR raycast from the screen touch point.
/// AR raycasts will only hit detected trackables like feature points and planes.
///
/// If a raycast hits a trackable, the <see cref="placedPrefab"/> is instantiated
/// and moved to the hit position.
/// </summary>
//if (spawnedObject == null)
//{
//    //Debug.Log("ok000: instantiate");
//    //if (m_PlacedPrefab == null)
//    //{
//    //    Debug.Log("m_PlacedPrefab is null!!!");
//    //}
//    spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
//    button_launch.GetComponent<Button>().onClick.AddListener(button_launch_task);
//    button_return.GetComponent<Button>().onClick.AddListener(button_return_task);
//    //Debug.Log("ok000b");
//}
//else
//{
//    //repositioning of the object 
//    spawnedObject.transform.position = hitPose.position;
//}
//placementUpdate.Invoke();
