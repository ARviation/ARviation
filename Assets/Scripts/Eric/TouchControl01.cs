using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl01 : MonoBehaviour
{
    // variables
    [SerializeField] Camera mainCamera;
    AudioSource source;
    public GameObject UI_object;
    public ImageTracking imageTracking;
    

    // Start
    void Start()
    {
        // UI_object
        UI_object.SetActive(false);

        // sound
        source = gameObject.AddComponent<AudioSource>();
        AudioClip clip = Resources.Load<AudioClip>("AudioClip/sound_ui_hover");
        source.clip = clip;
    }


    // Update
    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            Vector3 position = raycastHit.point;
            Transform trans = raycastHit.transform;
            Collider collider = raycastHit.collider;
            string name_ = collider.gameObject.name;
            string tag_ = collider.tag;
            if (tag_ == "AR_object" && Input.GetMouseButtonDown(0) && extract_label(name_) == extract_label(name))
            {
                source.Play();
                UI_object.SetActive(true);
            }
        }
    }


    // button_select
    public void button_select()
    {
        //print("select");
        source.Play();
        UI_object.SetActive(false);
        string MarkerName = transform.gameObject.name.Replace("object", "marker");
        imageTracking.dict_active[MarkerName] = false;
        GameObject object_part = Instantiate(gameObject);
        Inventory inventory = GameObject.Find("Inventory").GetComponent<Inventory>();  
        inventory.addNewPart(MarkerName, object_part);
    }


    // button_cancel
    public void button_cancel()
    {
        //print("cancel");
        source.Play();
        UI_object.SetActive(false);
    }


    // extract label
    string extract_label(string x)
    {
        int i = x.IndexOf("_");
        string x_;
        if (i > 0)
        {
            x_ = x.Substring(i + 1);
        }
        else
        {
            x_ = x;
        }
        return x_;
    }
}


/////////////////////////////////////////////////////////////////////////////////////////////


//#if UNITY_ANDROID || UNITY_IPHONE
//Handheld.Vibrate();
//#endif

//#if UNITY_ANDROID || UNITY_IPHONE
//Handheld.Vibrate();
//#endif



////debug <<<
//int i1 = name_.IndexOf("_");
//int i2 = name.IndexOf("_");                
//print("name_ = " + name_.Substring(i1) + "  name = " + name.Substring(i2));
////debug >>>
///


//print("hit " + collider.gameObject.name);


//#if UNITY_ANDROID || UNITY_IPHONE
//Handheld.Vibrate();
//#endif
//Color color = GetComponent<MeshRenderer>().material.color;
//if (color == Color.red)
//{
//    color = Color.blue;
//}
//else
//{
//    color = Color.red;
//}
//GetComponent<MeshRenderer>().material.color = color;


//transform.Find(name.Replace("object", "model")).gameObject.SetActive(false);



////debug <<<
//Debug.Log("select:: MarkerName = " + MarkerName);
//foreach (var item in imageTracking.dict_active)
//{
//    Debug.Log("dict_active:: key = " + item.Key + "  value = " + item.Value);
//}
////debug >>>

// placeholder
//string label = "test01";
//string label = MarkerName;
//GameObject object_part = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//object_part.SetActive(false);


