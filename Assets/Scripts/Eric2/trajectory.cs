using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trajectory : MonoBehaviour
{
    // Variables
    float R = 1.5f;
    float L1 = 2f;
    float L2 = 4f;
    float h = 2f;
    float v = 1f;
    float tol = 0.01f;
    public int state;
    public float t;

    Vector3 position;
    float theta;
    float phi;

    float pi;


    float T1, T2, T3, T4;
    float t1, t2, t3, t4;
    float T5, T6, T7, T8, T9;
    float t5, t6, t7, t8, t9;
    float w;

    // Start
    void Start()
    {
        w = v / R;
        pi = Mathf.PI;

        T1 = L1 / v;
        T2 = L2 / v;
        T3 = R / v;
        T4 = 2 * pi * R / v;
        t1 = T1;
        t2 = T1 + T2;
        t3 = T1 + T2 + T3;
        t4 = T1 + T2 + T3 + T4;

        T5 = R / v;
        T6 = 0.75f * 2 * pi * R / v;
        T7 = 0.25f * 2 * pi * R / v;
        T8 = L2 / v;
        T9 = L1 / v;
        t5 = T5;
        t6 = T5 + T6;
        t7 = T5 + T6 + T7;
        t8 = T5 + T6 + T7 + T8;
        t9 = T5 + T6 + T7 + T8 + T9;

        state = -1;

    }

    // Update
    void Update()
    {
        //Debug.Log("state = " + state);

        // launch
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (state == -1)
            {
                state = 1;
                t = 0;
                return;
            }
        }

        // return
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (state == 2)
            {
                state = 3;
                return;
            }
        }


        if (state == 1)
        {
            launch();
        }

        if (state == 2 || state == 3)
        {
            fly();
        }

        if (state == 4)
        {
            landing();
        }

        if (state > 0)
        {
            t += Time.deltaTime;
            transform.localPosition = position;
            transform.localEulerAngles = new Vector3(theta / pi * 180, -phi / pi * 180, 0);
        }
    }


    // launch
    void launch()
    {
        if (t < t1)
        {
            position = new Vector3(0, 0, v * t);
        }
        else if (t < t2)
        {
            float t_ = t - t1;
            float beta = (v * t_) / L2 * pi;
            position = new Vector3(0, h / 2 * (1 - Mathf.Cos(beta)), L1 + v * t_);
            theta = -Mathf.Atan(Mathf.Sin(beta));
        }
        else if (t < t3)
        {
            float t_ = t - t2;
            position = new Vector3(0, h, L1 + L2 + v * t_);
        }
        else
        {
            state = 2;
            return;
        }
    }


    // fly
    void fly()
    {
        float t_ = t - t3;
        phi = w * t_;
        float period = phi / (2 * pi);
        float delta = period - Mathf.Floor(period);
        if (state == 3 & delta < tol)
        {
            state = 4;
            t = 0;
            return;
        }
        position = new Vector3(-R + R * Mathf.Cos(phi), h, L1 + L2 + R + R * Mathf.Sin(phi));
    }


    // landing
    void landing()
    {
        if (t < t5)
        {
            position = new Vector3(0, h, v * t + L1 + L2 + R);
        }
        else if (t < t6)
        {
            float t_ = t - t5;
            phi = w * t_;
            position = new Vector3(-R + R * Mathf.Cos(phi), h, L1 + L2 + 2 * R + R * Mathf.Sin(phi));
        }
        else if (t < t7)
        {
            float t_ = t - t6;
            var phi1 = pi / 2 - w * t_;
            position = new Vector3(-R + R * Mathf.Cos(phi1), h, L1 + L2 + R * Mathf.Sin(phi1));
            phi = pi + phi1;
        }
        else if (t < t8)
        {
            float t_ = t - t7;
            float beta = pi - (v * t_) / L2 * pi;
            position = new Vector3(0, h / 2 * (1 - Mathf.Cos(beta)), L1 + L2 - v * t_);
            theta = Mathf.Atan(Mathf.Sin(beta));
        }
        else if (t < t9)
        {
            float t_ = t - t8;
            position = new Vector3(0, 0, L1 - v * t_);
        }

    }



}
