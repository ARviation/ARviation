using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using System.Net.Mail;
using UnityEngine.UI;

public class screenshot_v2 : MonoBehaviour
{
    // Variables
    public GameObject Canvas;
    public AudioSource source;
    public string receiver_email = "ARviationAlbum@gmail.com";
    public GameObject button_yes;
    public GameObject button_no;
    public GameObject image_photo;
    float t_pause = 1f;
    List<string> photo_file_list = new List<string>();

    public Image photo;
    float tao1 = 2f;
    float tao2 = 1f;
    Vector2 center1, length1;
    Vector2 center2, length2;
    Vector2 center3, length3;
    float t_photo;
    string screenshotName;


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

        // photo parameters
        center1 = new Vector2(0, 0);
        length1 = new Vector2(15f, 9.45f);
        center2 = new Vector2(600f, 100f);
        length2 = new Vector2(4f, 3f);
        center3 = new Vector2(1100f, 100f);
        length3 = new Vector2(0f, 0f);

        // disable UI
        image_photo.SetActive(false);
    }


    // button_take_photo
    public void button_take_photo()
    {
        screenshotName = "screenshot" + System.DateTime.Now.ToString("_yyyy_MM_dd_hh_mm_ss") + ".png";
        string screenshotName_full = Application.persistentDataPath + "/" + screenshotName;
        //string folderPath = "C:/Scratch/UserData/user2022a/CMU/course_53607_LAB/ARviation_project/experiment_screenshot/photos/";
        //string folderPath = Application.persistentDataPath + "/";
        //folderPath = "";
        //Canvas.SetActive(false);
        source.volume = 0.3f;
        source.Play();
        StartCoroutine(UI_pause(t_pause));
        string screenshotName_;
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            screenshotName_ = screenshotName;
        }
        else
        {
            screenshotName_ = screenshotName_full;
        }
        ScreenCapture.CaptureScreenshot(screenshotName_);
        //ScreenCapture.CaptureScreenshot(screenshotName);
        //while (!File.Exists(screenshotName)) { };
        //Canvas.SetActive(true);

        StartCoroutine(show_photo(screenshotName_full, center1, length1, center2, length2, tao1));
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


    // move image
    IEnumerator move_image(Image photo, Vector2 center1, Vector2 length1, Vector2 center2, Vector2 length2, float tao)
    {
        button_yes.SetActive(false);
        button_no.SetActive(false);
        while (t_photo > 0)
        {
            float r = t_photo / tao;
            Vector2 center = r * center1 + (1 - r) * center2;
            Vector2 length = r * length1 + (1 - r) * length2;
            RectTransform rt = photo.GetComponent<RectTransform>();
            rt.transform.localPosition = center;
            rt.transform.localScale = length;
            t_photo -= Time.deltaTime;
            yield return null;
        }
        image_photo.SetActive(false);
    }


    // import image from file
    bool import_image_from_file(Image m_image, string fname, Vector2 center, Vector2 length)
    {
        //Texture2D tex2d = Resources.Load<Texture2D>(fname);
        Texture2D tex2d = new Texture2D(1, 1);
        var fileContent = File.ReadAllBytes(fname);
        tex2d.LoadImage(fileContent);
        if (!tex2d)
        {
            m_image.sprite = null;
            return false;
        }
        m_image.sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), Vector2.zero);
        RectTransform rt = m_image.GetComponent<RectTransform>();
        rt.transform.localPosition = center;
        rt.transform.localScale = length;
        return true;
    }


    // show_photo
    IEnumerator show_photo(string fname, Vector2 center1, Vector2 length1, Vector2 center2, Vector2 length2, float tao)
    {
        // wait
        while (!File.Exists(fname)) 
        {
            yield return null;
        }

        // show
        image_photo.SetActive(true);
        button_yes.SetActive(false);
        button_no.SetActive(false);
        import_image_from_file(photo, fname, center1, length1);
        yield return null;

        //// shrink
        //t_photo = tao;
        //while (t_photo > 0)
        //{
        //    float r = t_photo / tao;
        //    Vector2 center = r * center1 + (1 - r) * center2;
        //    Vector2 length = r * length1 + (1 - r) * length2;
        //    RectTransform rt = photo.GetComponent<RectTransform>();
        //    rt.transform.localPosition = center;
        //    rt.transform.localScale = length;
        //    t_photo -= Time.deltaTime;
        //    yield return null;
        //}

        // activate yes/no buttons
        button_yes.SetActive(true);
        button_no.SetActive(true);
    }


    // button_no_task
    public void button_no_task()
    {
        photo_file_list.Remove(screenshotName);
        button_yes.SetActive(false);
        button_no.SetActive(false);
        image_photo.SetActive(false);
    }


    // button_yes_task
    public void button_yes_task()
    {
        t_photo = tao2;
        StartCoroutine(move_image(photo, center2, length2, center3, length3, tao2));
    }
}


///////////////////////////////////////////////////////////////////////////////////////////////////////
