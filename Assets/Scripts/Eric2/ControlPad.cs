using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlPad : MonoBehaviour
{
    /// <summary>
    /// author:: Yu (Eric) Zhu, 2022/05/06, yuzhu2@andrew.cmu.edu
    /// usage:: direction control buttons, L/R/U/D
    /// usage:: if no button pressed, the airplane will relax to the neutral pose
    /// usage:: L/R is enabled when |y| < tol
    /// usage:: U/D is enabled when |x| < tol
    /// </summary>


    // parameters
    float v0 = 1f;                                         // relaxation speed
    float tol = 0.01f;                                     // zero tolerance


    // Variables
    public float x;
    public float y;
    public bool is_button_pressed;


    // Start
    void Start()
    {
        is_button_pressed = false;
    }


    // Update
    void Update()
    {
        // relaxation
        if (!is_button_pressed)
        {
            if (x > 0)
            {
                x = Mathf.Max(0, x - v0 * Time.deltaTime);
            }
            if (x < 0)
            {
                x = Mathf.Min(0, x + v0 * Time.deltaTime);
            }
            if (y > 0)
            {
                y = Mathf.Max(0, y - v0 * Time.deltaTime);
            }
            if (y < 0)
            {
                y = Mathf.Min(0, y + v0 * Time.deltaTime);
            }
        }

        // button activation
        bool tf1 = (Mathf.Abs(x) < tol);
        transform.Find("Button_U").GetComponent<ButtonTransitioner_UDLR>().interactable = tf1;
        transform.Find("Button_D").GetComponent<ButtonTransitioner_UDLR>().interactable = tf1;
        bool tf2 = (Mathf.Abs(y) < tol);
        transform.Find("Button_L").GetComponent<ButtonTransitioner_UDLR>().interactable = tf2;
        transform.Find("Button_R").GetComponent<ButtonTransitioner_UDLR>().interactable = tf2;
    }


    // reset
    public void Reset()
    {
        Debug.Log("reset");
        is_button_pressed = false;
        x = 0;
        y = 0;
        transform.Find("Button_U").GetComponent<ButtonTransitioner_UDLR>().tf = false;
        transform.Find("Button_D").GetComponent<ButtonTransitioner_UDLR>().tf = false;
        transform.Find("Button_L").GetComponent<ButtonTransitioner_UDLR>().tf = false;
        transform.Find("Button_R").GetComponent<ButtonTransitioner_UDLR>().tf = false;
    }
}
