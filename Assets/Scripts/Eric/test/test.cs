using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Variables
    public int N1;
    public int N2;
    public Vector2 center0;
    public Vector2 delta;

    List<Vector2> center_list;

    Vector2 button_center;

    // Start
    void Start()
    {
        center_list = new List<Vector2> { };
        for (int i1=0; i1<N1; i1++)
        {
            for (int i2=0; i2<N2; i2++)
            {
                float x = center0.x + i1 * delta.x;
                float y = center0.y + i2 * delta.y;
                var center = new Vector2(x, y);
                center_list.Add(center);
            }
        }
        //print("n = " + center_list.Count);
        //foreach (var center in center_list) print(center);
    }


    // Update
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            button_center = request();
            print("request center = (" + button_center.x + " " + button_center.y + ")");
            print("length = " + center_list.Count);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            release(button_center); 
            print("release center = (" + button_center.x + " " + button_center.y + ")");
            print("length = " + center_list.Count);
            button_center.x = float.PositiveInfinity;
            button_center.y = float.PositiveInfinity;            
        }
    }


    // request
    public Vector2 request()
    {
        Vector2 center;
        if (center_list.Count > 0)
        {
            center = center_list[0];
            center_list.RemoveAt(0);
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
