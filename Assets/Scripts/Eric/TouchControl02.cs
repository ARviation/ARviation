using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl02 : MonoBehaviour
{
    // Variables
    public ImageTracking imageTracking;
    public GameObject UI_icon;
    public Inventory inventory;
    AudioSource source;

    public Camera mainCamera;
    public Vector3 offset_object;

    // Start
    void Start()
    {
        // UI_object
        UI_icon.SetActive(false);

        // sound
        source = gameObject.AddComponent<AudioSource>();
        AudioClip clip = Resources.Load<AudioClip>("AudioClip/sound_ui_hover");
        source.clip = clip;
    }


    // button icon
    public void button_icon()
    {
        if (inventory.is_UI_busy) return;
        source.Play();
        UI_icon.SetActive(true);
        inventory.is_UI_busy = true;
        // show object
        ////Debug <<<
        //Debug.Log("label = " + label);
        //for (int i = 0; i < transform.parent.childCount; i++)
        //{
        //    GameObject child = transform.parent.GetChild(i).gameObject;
        //    print("name = " + child.name);
        //}
        ////Debug >>>
        string label = extract_label(transform.parent.name);
        GameObject go = transform.parent.Find(label).gameObject;
        go.transform.position = mainCamera.transform.position + mainCamera.transform.rotation * offset_object;
        go.transform.rotation = mainCamera.transform.rotation;
        go.SetActive(true);
    }


    // button delete
    public void button_delete()
    {
        //print("delete");
        source.Play();
        UI_icon.SetActive(false);
        string MarkerName = transform.parent.name.Replace("part_", "");
        imageTracking.dict_active[MarkerName] = true;
        RectTransform rect = GetComponent<RectTransform>();
        inventory.release(rect.anchoredPosition);
        inventory.is_UI_busy = false;
        Destroy(transform.parent.gameObject);
    }


    // button cancel
    public void button_cancel()
    {
        source.Play();
        UI_icon.SetActive(false);
        inventory.is_UI_busy = false;
        // hide object
        string label = extract_label(transform.parent.name);
        GameObject go = transform.parent.Find(label).gameObject;
        go.SetActive(false);
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

/////////////////////////////////////////////////////////////////////////////////////////////////////////
///





//// Update
//void Update()
//{

//}


////debug <<<
//Debug.Log("delete:: MarkerName = " + MarkerName);
//foreach (var item in imageTracking.dict_active)
//{
//    Debug.Log("dict_active:: key = " + item.Key + "  value = " + item.Value);
//}
////debug >>>