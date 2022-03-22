using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using System.Net.Mail;

public class screenshot : MonoBehaviour
{
    // Variables
    public GameObject Canvas;
    public AudioSource source;
    public string receiver_email = "ARviationAlbum@gmail.com";
    float t_pause = 1f;
    List<string> photo_file_list = new List<string>();

    // Start
    void Start()
    {
        // clear folder
        string[] fileEntries = Directory.GetFiles(Application.persistentDataPath);
        foreach (string fileName in fileEntries)
        {
            File.Delete(fileName);
            Debug.Log("delete file " + fileName);
        }


        //// sound
        //source = gameObject.AddComponent<AudioSource>();
        //AudioClip clip = Resources.Load<AudioClip>("AudioClip/sound_camera_snap");
        //source.clip = clip;
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    // button_take_photo
    public void button_take_photo()
    {
        string screenshotName = "screenshot" + System.DateTime.Now.ToString("_yyyy_MM_dd_hh_mm_ss") + ".png";
        //string folderPath = "C:/Scratch/UserData/user2022a/CMU/course_53607_LAB/ARviation_project/experiment_screenshot/photos/";
        //string folderPath = Application.persistentDataPath + "/";
        //folderPath = "";
        //Canvas.SetActive(false);
        source.volume = 0.3f;
        source.Play();
        StartCoroutine(UI_pause(t_pause));
        ScreenCapture.CaptureScreenshot(screenshotName);
        //while (!File.Exists(screenshotName)) { };
        //Canvas.SetActive(true);
        Debug.Log("screenshot");
        photo_file_list.Add(screenshotName);
        //string path_full = Application.persistentDataPath;
        //print("picture saved to " + path_full);
    }

    // UI pause
    IEnumerator UI_pause(float t_pause)
    {
        Canvas.SetActive(false);
        yield return new WaitForSeconds(t_pause);
        Canvas.SetActive(true);
    }


    // send email
    public void button_send_email()
    {
        MailMessage mail = new MailMessage();
        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
        mail.From = new MailAddress("ARviationAlbum@gmail.com");
        mail.To.Add(receiver_email);
        mail.Subject = "ARviation Photos";
        mail.Body = "Dear guests, please see the attached photos. We hope you enjoy the ARviation experience!";

        foreach (string photo_file in photo_file_list)
        {
            System.Net.Mail.Attachment attachment;
            attachment = new System.Net.Mail.Attachment(Application.persistentDataPath + "/" + photo_file);
            mail.Attachments.Add(attachment);
        }        

        SmtpServer.Port = 587;
        SmtpServer.Credentials = new NetworkCredential("ARviationAlbum@gmail.com", "esngpvhpxmgnbsdh");
        SmtpServer.EnableSsl = true;

        SmtpServer.Send(mail);
    }
}


///////////////////////////////////////////////////////////////////////////////////////////////////////
