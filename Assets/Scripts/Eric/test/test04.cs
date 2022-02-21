using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test04 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Image image;
        Sprite sprite;
        image = transform.Find("icon").GetComponent<Image>();
        print("ok001");
        print("image = " + image);
        sprite = Resources.Load<Sprite>("icons/icon_test");
        image.sprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
