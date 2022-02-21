using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Inventory : MonoBehaviour
{
    // Variables
    [SerializeField] GameObject template;

    public int N1;
    public int N2;
    public Vector2 center0;
    public Vector2 delta;
    public Vector2 button_size;
    List<Vector2> center_list;

    public bool is_UI_busy = false;


    // Start
    void Start()
    {
        center_list = new List<Vector2> { };
        for (int i1 = 0; i1 < N1; i1++)
        {
            for (int i2 = 0; i2 < N2; i2++)
            {
                float x = center0.x + i1 * delta.x;
                float y = center0.y + i2 * delta.y;
                var center = new Vector2(x, y);
                center_list.Add(center);
            }
        }
    }


    //// Update
    //void Update()
    //{
        
    //}


    // add new part
    public void addNewPart(string label, GameObject object_part)
    {
        // create part
        GameObject part = Instantiate(template);
        part.transform.parent = this.transform;
        part.name = "part_" + label;
        part.SetActive(true);
        RectTransform rect0 = part.GetComponent<RectTransform>();
        rect0.anchoredPosition = new Vector2(0, 0);
        rect0.sizeDelta = new Vector2(100, 100);

        // part/icon
        GameObject icon = part.transform.Find("icon").gameObject;
        RectTransform rect = icon.GetComponent<RectTransform>();        
        rect.anchoredPosition = request();
        rect.sizeDelta = button_size;        
        Image image = part.transform.Find("icon").GetComponent<Image>();
        Sprite sprite = Resources.Load<Sprite>("Arts/" + label.Replace("marker_", "icon_"));
        image.sprite = sprite;

        // object_part
        object_part.SetActive(false);
        object_part.transform.parent = part.transform;
    }


    // request
    public Vector2 request()
    {
        Vector2 center;
        if (center_list.Count > 0)
        {
            center = center_list[center_list.Count - 1];
            center_list.RemoveAt(center_list.Count - 1);
        }
        else
        {
            center = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
        }
        return center;
    }


    // release
    public void release(Vector2 center)
    {
        if (center.x != float.PositiveInfinity && center.y != float.PositiveInfinity)
        {
            center_list.Add(center);
        }
    }

}


///////////////////////////////////////////////////////////////////////////////////////////////////////


////debug <<<
//Sprite sprite2 = import_sprite_from_file("ARmarkers/marker00_RiversOfSteel.jpg");
//print("ok");
////debug >>>

////debug <<<
//Sprite sprite2 = import_sprite_from_file("Assets/ARmarkers/marker00_RiversOfSteel.jpg");
//image.sprite = sprite2;
////debug >>>
//Sprite sprite = Resources.Load<Sprite>("icons/icon_" + label);
//Sprite sprite = Resources.Load<Sprite>("icons/" + label);
//Debug.Log("label = " + label);


//print("add");
//var center_new = request();
//print("center_new = " + center_new.x + " " + center_new.y);

//Sprite import_sprite_from_file(string fname)
//{
//    //Texture2D tex2d = Resources.Load<Texture2D>(fname);
//    Texture2D tex2d = new Texture2D(1, 1);
//    var fileContent = File.ReadAllBytes(fname);
//    tex2d.LoadImage(fileContent);
//    if (!tex2d)
//    {
//        print("fail to read texture");
//        return null;
//    }

//    //if (!tex2d)
//    //{
//    //    m_image.sprite = null;
//    //    return null;
//    //}
//    //m_image.sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), Vector2.zero);
//    //RectTransform rt = m_image.GetComponent<RectTransform>();
//    //rt.transform.localPosition = center;
//    //rt.transform.localScale = length;
//    //print(tex2d.width + " " + tex2d.height);
//    Sprite sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), Vector2.zero);
//    //print("dxxx");
//    return sprite;
//}





//debug <<<
//rect.anchoredPosition = new Vector2(-350, -1000);
//rect.sizeDelta = new Vector2(300, 300);


//object_part.GetComponent<MeshRenderer>().material.color = Color.black;
//object_part.transform.position = new Vector3(1, 0, 0);


////debug <<<
//public Image im;
////debug >>>
///

//Image image;
//Sprite sprite;
//image = transform.Find("part_template").transform.Find("icon").GetComponent<Image>();
//sprite = Resources.Load<Sprite>("icons/icon_test01");
//image.sprite = sprite;

//string label = "test01";
//GameObject object_part = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//object_part.SetActive(false);

//addNewPart(label, object_part);

////debug <<<
//Sprite sprite2 = import_sprite_from_file("Assets/ARmarkers/marker00_RiversOfSteel.jpg");
//im.sprite = sprite2;
//print("ok");
////debug >>>
