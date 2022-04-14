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
    public GameObject scan_prompt_screen;
    public ImageTracking_v3 imageTracking;
    public GameObject msg_sending_email;
    public GameObject scanning_line;

    float dumping_rate = 0.01f;
    float v_scan = 600f;
    float L_scan_max = 300f;


    // Start
    void Start()
    {
        state = "scanning";
        //StartCoroutine(slider_dumping());
        quit_inquiry_screen.SetActive(false);
        msg_sending_email.SetActive(false);
    }


    // Update
    void Update()
    {
        //Debug.Log("state = " + state);
        if (state == "scanning")
        {   
            // scanning line
            RectTransform rt = scanning_line.GetComponent<RectTransform>();
            Vector2 scanning_line_center = rt.transform.localPosition;
            float L_scan = scanning_line_center.y;
            L_scan -= v_scan * Time.deltaTime;
            if (L_scan < -L_scan_max) L_scan = L_scan_max;
            //Debug.Log("L_scan = " + L_scan);
            scanning_line_center.y = L_scan;
            rt.transform.localPosition = scanning_line_center;

            button_quit.SetActive(false);
            button_take_photo.SetActive(false);
            button_launch.SetActive(false);
            button_return.SetActive(false);
            slider_control.SetActive(false);
            if (imageTracking.isMarkerDetected)
            {
                scan_prompt_screen.SetActive(false);
                button_quit.SetActive(true);
                button_take_photo.SetActive(true);
                button_launch.SetActive(true);
                state = "idling";
            }
            return;
        }
        if (state == "idling")
        {            
            button_launch.SetActive(true);
            button_launch.GetComponent<Button>().interactable = true;
            button_return.SetActive(false);
            slider_control.SetActive(false);
            return;
        }
        if (state == "free flight")
        {
            button_launch.SetActive(false);
            button_return.SetActive(true);
            button_return.GetComponent<Button>().interactable = true;
            slider_control.SetActive(true);
            slider_control.GetComponent<Slider>().interactable = true;
            return;
        }
        if (state == "fixed traj")
        {
            button_launch.SetActive(true);
            button_launch.GetComponent<Button>().interactable = false;
            button_return.SetActive(false);
            slider_control.SetActive(false);
            return;
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
        // sound
        SFXmanager.playsound("click");
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
        //// hide quit inqiry screen
        //quit_inquiry_screen.SetActive(false);
        //// show msg
        //text_msg_sending_email.SetActive(true);
        //// send email
        //screenshot.GetComponent<screenshot_v2>().button_send_email();
        //// disable UI
        //gameObject.SetActive(false);
        //// quit
        //Application.Quit();
        // sound
        SFXmanager.playsound("click");
        StartCoroutine(quit_task());
    }


    // button quit_no task
    public void button_quit_no_task()
    {
        // sound
        SFXmanager.playsound("click");
        // hide quit inqiry screen
        quit_inquiry_screen.SetActive(false);
        // enable all clickables
        button_launch.GetComponent<Button>().interactable = true;
        button_return.GetComponent<Button>().interactable = true;
        slider_control.GetComponent<Slider>().interactable = true;
        button_quit.GetComponent<Button>().interactable = true;
        button_take_photo.GetComponent<Button>().interactable = true;
    }


    // quit task
    IEnumerator quit_task()
    {
        // hide quit inqiry screen
        quit_inquiry_screen.SetActive(false);
        // send email
        int N_photo = screenshot.GetComponent<screenshot_v2>().N_photo;
        if (N_photo > 0)
        {
            // show msg
            msg_sending_email.SetActive(true);
            yield return null;
            // send email
            screenshot.GetComponent<screenshot_v2>().button_send_email();
        }
        // quit
        Debug.Log("quit flying scene");
        GameManager.Instance.ChangeSceneToEnd();
        // gameObject.SetActive(false);
    }
}
