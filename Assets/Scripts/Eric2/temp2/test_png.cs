using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class test_png : MonoBehaviour
{
    public Image photo;
    float tao = 1f;
    Vector2 center1;
    Vector2 center2;
    Vector2 length1;
    Vector2 length2;
    float t_photo;

    // Start
    void Start()
    {
        string fname = "C:/Scratch/UserData/user2022a/CMU/course_53607_LAB/ARviation_project/ARviation/Assets/Arts/Marker2/marker31_L.png";
        center1 = new Vector2(0, 0);
        length1 = new Vector2(10f, 10f);
        center2 = new Vector2(-200f, -200f);
        length2 = new Vector2(5f, 5f);

        import_image_from_file(photo, fname, center1, length1);
    }


    // Update
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            t_photo = tao;
            StartCoroutine(move_image(photo, center1, length1, center2, length2, tao));
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            t_photo = tao;
            StartCoroutine(move_image(photo, center2, length2, center1, length1, tao));
        }
    }


    // move image
    IEnumerator move_image(Image photo, Vector2 center1, Vector2 length1, Vector2 center2, Vector2 length2, float tao)
    {
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
}
