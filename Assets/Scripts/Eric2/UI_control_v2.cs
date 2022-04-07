using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_control_v2 : MonoBehaviour
{
    // Variables
    public string state;
    public GameObject button_launch;
    public GameObject button_return;
    public GameObject slider_control;
    public GameObject button_quit;
    public GameObject button_take_photo;
    public GameObject screenshot;
    public GameObject quit_inquiry_screen;

    float dumping_rate = 0.01f;


    // Start
    void Start()
    {
        state = "idling";
        StartCoroutine(slider_dumping());
        quit_inquiry_screen.SetActive(false);
    }


    // Update
    void Update()
    {
        if (state == "idling")
        {
            button_launch.SetActive(true);
            button_launch.GetComponent<Button>().interactable = true;
            button_return.SetActive(false);
            slider_control.SetActive(false);
        }
        if (state == "free flight")
        {
            button_launch.SetActive(false);
            button_return.SetActive(true);
            button_return.GetComponent<Button>().interactable = true;
            slider_control.SetActive(true);
            slider_control.GetComponent<Slider>().interactable = true;
        }
        if (state == "fixed traj")
        {
            button_launch.SetActive(true);
            button_launch.GetComponent<Button>().interactable = false;
            button_return.SetActive(false);
            slider_control.SetActive(false);
        }

    }


    // slider dumping
    IEnumerator slider_dumping()
    {
        while (true)
        {
            if (slider_control.GetComponent<Slider>().value > 0)
            {
                slider_control.GetComponent<Slider>().value = Mathf.Max(0, slider_control.GetComponent<Slider>().value - dumping_rate);
            }
            yield return null;
        }
    }


    // button quit task
    public void button_quit_task()
    {
        // show quit inquiry screen
        quit_inquiry_screen.SetActive(true);
        // disable all clickables
        button_launch.GetComponent<Button>().interactable = false;
        button_return.GetComponent<Button>().interactable = false;
        slider_control.GetComponent<Slider>().interactable = false;
        button_quit.GetComponent<Button>().interactable = false;
        button_take_photo.GetComponent<Button>().interactable = false;
    }


    // button quit_yes task
    public void button_quit_yes_task()
    {
        // hide quit inqiry screen
        quit_inquiry_screen.SetActive(false);
        // send email
        screenshot.GetComponent<screenshot>().button_send_email();
        // disable UI
        gameObject.SetActive(false);
        // quit
        Application.Quit();
    }


    // button quit_no task
    public void button_quit_no_task()
    {
        // hide quit inqiry screen
        quit_inquiry_screen.SetActive(false);
        // enable all clickables
        button_launch.GetComponent<Button>().interactable = true;
        button_return.GetComponent<Button>().interactable = true;
        slider_control.GetComponent<Slider>().interactable = true;
        button_quit.GetComponent<Button>().interactable = true;
        button_take_photo.GetComponent<Button>().interactable = true;
    }

}
