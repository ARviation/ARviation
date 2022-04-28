using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonTransitioner_UDLR : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // Variables
    public bool interactable;
    public bool active;
    public string mode;
    bool interactable_;
    bool active_;
    float v = 50f;
    Color color1 = Color.red;
    Color color2 = Color.white;
    Color color3 = Color.gray;
    ControlPad controlPad;

    Image m_Image = null;


    // Awake
    void Awake()
    {
        m_Image = GetComponent<Image>();
        controlPad = transform.parent.GetComponent<ControlPad>();
        interactable = true;
        interactable_ = true;
        active = true;
        active_ = true;
    }


    // Update
    void Update()
    {
        if ((interactable != interactable_) | (active != active_))
        {
            interactable_ = interactable;
            active_ = active;
            if (interactable & active)
            {
                m_Image.color = color2;
            }
            else
            {
                m_Image.color = color3;
            }
        }
    }


    // Down
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!interactable | !active) return;
        controlPad.is_button_pressed = true;
        m_Image.color = color1;
        InvokeRepeating("act", 0, 0.1f);

    }


    // Up
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!interactable | !active) return;
        controlPad.is_button_pressed = false;
        m_Image.color = color2;
        CancelInvoke("act");
    }


    // act
    void act()
    {
        if (mode == "y+")
        {
            controlPad.y += v * Time.deltaTime;
            controlPad.y = Mathf.Min(1f, controlPad.y);
        }
        else if (mode == "y-")
        {
            controlPad.y -= v * Time.deltaTime;
            controlPad.y = Mathf.Max(-1f, controlPad.y);
        }
        else if (mode == "x+")
        {
            controlPad.x += v * Time.deltaTime;
            controlPad.x = Mathf.Min(1f, controlPad.x);
        }
        else if (mode == "x-")
        {
            controlPad.x -= v * Time.deltaTime;
            controlPad.x = Mathf.Max(-1f, controlPad.x);
        }
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
