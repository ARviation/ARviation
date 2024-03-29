using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyControl_v6 : MonoBehaviour
{
    /// <summary>
    /// author:: Yu (Eric) Zhu, 2022/05/06, yuzhu2@andrew.cmu.edu
    /// usage:: control how an airplane flies, key code
    /// usage:: job_list is a list of jobs, each job is an animation
    /// usage:: run through the job list with Update
    /// </summary>


    // Parameters
    float R = 2f;                                          // minimum yaw radius
    float L1 = 2f;                                         // take off acceleration length
    float L2 = 4f;                                         // take off climbing length
    float h = 2f;                                          // take off height
    float h_min = 1f;                                      // minimum height
    float h_max = 3f;                                      // maximum height
    float d_max = 50f;                                     // maximum distance
    float speed = 1f;                                      // flying speed
    float roll_max = 15;                                   // maximum rolling angle in degree
    float roll_eta = 0.01f;                                // rolling dumping rate
    float theta_max = 15;                                  // maximum pitch angle in degree
    float propeller_rotation_speed = 2000f;                // propeller rotation speed


    // Variables
    public struct job
    {
        public string action;
        public List<float> parameters;
    }
    float distance;
    Vector3 born_origin;
    bool isBoundaryCheck = true;
    float omega;
    float pi;
    public AudioSource source;
    GameObject CanvasObj;
    public GameObject landing_gears;
    public GameObject propeller;
    public GameObject trail_white;
    public GameObject trail_rainbow;
    public Sprite sprite_trail_rainbow;
    public Sprite sprite_trail_none;
    public string action;
    List<float> parameters;
    public List<job> job_list;
    public float alpha;
    float alpha0 = 1f;
    Vector3 velocity;
    bool is_fixed_traj = false;
    int trail_index = 0;
    float height0, height1;
    ControlPad control_pad;
    Button button_launch;
    Button button_return;
    Button button_trail;
    float t;
    Vector3 action_origin;


    // Awake
    void Awake()
    {
        // init
        action = null;
        job_list = new List<job>();
        alpha = 0;
        omega = speed / R;
        pi = Mathf.PI;

        // find
        CanvasObj = GameObject.Find("Canvas").gameObject;
        control_pad = GameObject.Find("Canvas").transform.Find("ControlPad").GetComponent<ControlPad>();
        button_launch = GameObject.Find("Canvas").transform.Find("button_launch").GetComponent<Button>();
        button_return = GameObject.Find("Canvas").transform.Find("button_return").GetComponent<Button>();
        button_trail = GameObject.Find("Canvas").transform.Find("button_trail").GetComponent<Button>();
        button_launch.onClick.AddListener(button_launch_task);
        button_return.onClick.AddListener(button_return_task);
        button_trail.onClick.AddListener(button_trail_task);

        // sound
        source = gameObject.AddComponent<AudioSource>();
        AudioClip clip = Resources.Load<AudioClip>("AudioClip/sound_engine");
        source.clip = clip;
        source.loop = true;
        source.playOnAwake = false;

        // trail
        trail_white.SetActive(false);
        trail_rainbow.SetActive(false);
        
        // height
        height0 = transform.position.y;

        // born origin
        born_origin = transform.position;
    }


    // Update
    void Update()
    {
        //Debug.Log("action = " + action + "  len(job_list) = " + job_list.Count);

        // distance control
        distance = (transform.position - born_origin).magnitude;
        if (distance > d_max & isBoundaryCheck)
        {
            button_return_task();
            button_return.Select();
            isBoundaryCheck = false;
            //Debug.Log("select button return");
        }

        // update UI state
        update_UI_state();

        // roll dumping
        roll_angle_dumping();             

        // yaw control
        if (action == "free flight") alpha = - control_pad.x;

        // pitch/roll control
        if (action == "free flight")
        {
            float height = transform.position.y - height0;
            float theta;
            if ((height < h_min & control_pad.y < 0) | (height > h_max & control_pad.y > 0))
            {
                theta = 0;                
            }
            else
            {
                theta = theta_max * control_pad.y / 180 * pi;
            }
            velocity.x = 0;
            velocity.y = speed * Mathf.Sin(theta);
            velocity.z = speed * Mathf.Cos(theta);
            velocity = transform.rotation * velocity;
            transform.position += velocity * Time.deltaTime;
            Vector3 orientation = transform.Find("orientation").transform.localEulerAngles;
            orientation.x = -theta / pi * 180;
            transform.Find("orientation").transform.localEulerAngles = orientation;
        }

        // new action
        if (action == null)
        {
            if (job_list.Count == 0) return;
            var job = job_list[0];
            job_list.RemoveAt(0);
            action = job.action;
            parameters = job.parameters;            
            t = 0;
            return;
        }

        // propeller
        Vector3 angles = propeller.transform.localEulerAngles;
        angles.z += propeller_rotation_speed * Time.deltaTime;
        propeller.transform.localEulerAngles = angles;

        // action: straight
        if (action == "straight")
        {
            alpha = 0;
            if (t == 0) action_origin = transform.position;
            var delta = transform.position - action_origin;
            if (delta.magnitude >= parameters[0])
            {
                action = null;
                return;
            }
            velocity = transform.forward * speed;
            transform.position += velocity * Time.deltaTime;
            t += Time.deltaTime;
            return;
        }

        // action: accelerate
        if (action == "accelerate")
        {
            alpha = 0;
            float t_max = Mathf.Sqrt(2 * Mathf.Abs(parameters[0] / parameters[1]));
            if (t >= t_max)
            {
                action = null;
                return;
            }
            velocity += transform.forward * parameters[1] * Time.deltaTime;
            transform.position += velocity * Time.deltaTime;
            t += Time.deltaTime;
            return;
        }

        // action: take off
        if (action == "take off")
        {
            alpha = 0;
            if (t == 0) 
            { 
                action_origin = transform.position;
                height0 = transform.position.y;
            }
            var delta = transform.position - action_origin;
            delta = transform.InverseTransformDirection(delta);
            if (Mathf.Abs(delta.z) >= parameters[1])
            {
                action = null;
                return;
            }
            if (Mathf.Abs(delta.z) >= parameters[1]/2)
            {
                landing_gears.SetActive(false);   // hide landing gears
            }
            float h = parameters[0];
            float L = parameters[1];
            float z = delta.z;
            float gamma = h / 2 * Mathf.Sin(z / L * pi) * pi / L;
            float theta = Mathf.Atan(gamma);
            velocity.x = 0;
            velocity.y = speed * gamma / Mathf.Sqrt(1 + gamma * gamma);
            velocity.z = speed * 1 / Mathf.Sqrt(1 + gamma * gamma);
            velocity = transform.rotation * velocity;
            transform.position += velocity * Time.deltaTime;
            Vector3 orientation = transform.Find("orientation").transform.localEulerAngles;
            orientation.x = -theta / pi * 180;
            transform.Find("orientation").transform.localEulerAngles = orientation;
            t += Time.deltaTime;
            return;
        }

        // action: landing
        if (action == "landing")
        {
            alpha = 0;
            if (t == 0)
            {
                action_origin = transform.position;
                height1 = transform.position.y;
            }
            var delta = transform.position - action_origin;
            delta = transform.InverseTransformDirection(delta);
            if (Mathf.Abs(delta.z) >= parameters[1])
            {
                action = null;
                return;
            }
            if (Mathf.Abs(delta.z) >= parameters[1] / 2)
            {
                landing_gears.SetActive(true);   // show landing gears
            }
            //float h = parameters[0];
            float h = height1 - height0;
            float L = parameters[1];
            float z = delta.z;
            float gamma = - h / 2 * Mathf.Sin(z / L * pi) * pi / L;
            float theta = Mathf.Atan(gamma);
            velocity.x = 0;
            velocity.y = speed * gamma / Mathf.Sqrt(1 + gamma * gamma);
            velocity.z = speed * 1 / Mathf.Sqrt(1 + gamma * gamma);
            velocity = transform.rotation * velocity;
            transform.position += velocity * Time.deltaTime;
            Vector3 orientation = transform.Find("orientation").transform.localEulerAngles;
            orientation.x = -theta / pi * 180;
            transform.Find("orientation").transform.localEulerAngles = orientation;
            t += Time.deltaTime;
            return;
        }

        // action: free flight
        if (action == "free flight")
        {
            velocity = speed * transform.forward;
            transform.position += velocity * Time.deltaTime;
            transform.Rotate(0, -omega * alpha * Time.deltaTime / pi * 180, 0);
            return;
        }

        // action: circle
        if (action == "circle")
        {
            alpha = parameters[0];
            float t_max = 2 * pi / Mathf.Abs(omega * alpha) * parameters[1];
            velocity = speed * transform.forward;
            transform.position += velocity * Time.deltaTime;
            transform.Rotate(0, -omega * alpha * Time.deltaTime / pi * 180, 0);
            t += Time.deltaTime;
            if (t >= t_max)
            {
                action = null;
                return;
            }
            return;
        }

        // action: set traj flag
        if (action == "lock traj")
        {
            float tf = parameters[0];
            is_fixed_traj = (tf > 0);
            if (is_fixed_traj)
            {
                trail_white.SetActive(false);
                trail_rainbow.SetActive(false);
            }
            action = null;
            return;
        }

        // action: landing gears animation
        if (action == "landing gears animation")
        {
            float tf_ = parameters[0];
            landing_gears.SetActive(tf_ > 0);
            action = null;
            return;
        }

        // action: engine sound
        if (action == "engine sound")
        {
            float tf_ = parameters[0];
            if (tf_ > 0)
            {
                source.Play();
            }
            else
            {
                source.Stop();
            }
            action = null;
            return;
        }
    }



    // take off
    public void take_off()
    {
        SFXmanager.playsound("voiceover-3");
        alpha = 0;
        trail_index = 0;
        button_trail.GetComponent<Image>().sprite = sprite_trail_rainbow;
        isBoundaryCheck = true;
        control_pad.Reset();
        velocity = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
        transform.localPosition = Vector3.zero;
        transform.Find("orientation").transform.localEulerAngles = Vector3.zero;
        action = null;
        job_list = new List<job>();
        job_list.Add(new job() { action = "lock traj", parameters = new List<float> { 1f } });
        job_list.Add(new job() { action = "engine sound", parameters = new List<float> { 1f } });
        job_list.Add(new job() { action = "accelerate", parameters = new List<float> { L1, speed*speed/(2*L1) } });        
        job_list.Add(new job() { action = "take off", parameters = new List<float> { h, L2 } });
        job_list.Add(new job() { action = "straight", parameters = new List<float> { R } });
        job_list.Add(new job() { action = "lock traj", parameters = new List<float> { -1f } });
        job_list.Add(new job() { action = "free flight", parameters = new List<float> { } });
    }


    // landing
    public void landing()
    {
        // voiceover-4
        SFXmanager.playsound("voiceover-4");

        // q0, theta0
        alpha = alpha0;
        Vector3 r0 = transform.position + Quaternion.AngleAxis(-90f * Mathf.Sign(alpha0), Vector3.up) * velocity.normalized * R;
        r0 = transform.parent.InverseTransformDirection(r0 - transform.parent.position);
        Vector3 pos = Quaternion.AngleAxis(-90f * Mathf.Sign(alpha0), Vector3.up) * velocity.normalized * R * (-1);
        pos = transform.parent.InverseTransformDirection(pos);
        Vector3 r0_star = new Vector3(-R, h, L1 + L2 + 2 * R);
        Vector3 Delta = r0 - r0_star;
        (float _, int q0) = Atan3(Delta);
        (float theta0, int _) = Atan3(pos);

        // d0, d1, d2
        float theta = pi / 2 * q0;
        float d0 = (theta - theta0) / (2*pi);
        if (d0 < 0) d0 = d0 + 1;
        float d1 = 0;
        float d2 = 0;
        if ( q0 == 1 | q0 == 3)
        {            
            d1 = Mathf.Abs(Delta.x);
            d2 = Mathf.Abs(Delta.z);
        }
        if (q0 == 2 | q0 == 4)
        {
            d1 = Mathf.Abs(Delta.z);
            d2 = Mathf.Abs(Delta.x);
        }
        int q1 = (6 - q0) % 4;    

        // job_list
        action = null;
        job_list = new List<job>();
        job_list.Add(new job() { action = "lock traj", parameters = new List<float> { 1f } });
        job_list.Add(new job() { action = "circle", parameters = new List<float> { alpha0, d0 } });
        job_list.Add(new job() { action = "straight", parameters = new List<float> { d1 } });
        job_list.Add(new job() { action = "circle", parameters = new List<float> { alpha0, 0.25f } });
        job_list.Add(new job() { action = "straight", parameters = new List<float> { d2 } });
        job_list.Add(new job() { action = "circle", parameters = new List<float> { alpha0, 0.25f * q1 } });
        job_list.Add(new job() { action = "circle", parameters = new List<float> { -alpha0, 0.25f } });
        job_list.Add(new job() { action = "landing", parameters = new List<float> { h, L2 } });
        job_list.Add(new job() { action = "accelerate", parameters = new List<float> { L1, - speed * speed / (2 * L1) } });
        job_list.Add(new job() { action = "engine sound", parameters = new List<float> { -1f } });
        job_list.Add(new job() { action = "lock traj", parameters = new List<float> { -1f } });
    }


    // roll angle dumping
    void roll_angle_dumping()
    {
        if (action == null) return;

        Vector3 orientation = transform.Find("orientation").transform.localEulerAngles;
        float roll_angle_ = orientation.z;

        float angle_offset = 0;
        if (roll_angle_ > 90) angle_offset = -360;
        roll_angle_ += angle_offset;

        float roll_angle = alpha * roll_max;
        roll_angle_ = roll_eta * roll_angle + (1 - roll_eta) * roll_angle_;

        roll_angle_ -= angle_offset;

        orientation.z = roll_angle_;
        transform.Find("orientation").transform.localEulerAngles = orientation;
    }


    // update_UI_state
    void update_UI_state()
    {
        if (action == null & !is_fixed_traj)
        {
            CanvasObj.GetComponent<UI_control_v3>().state = "idling";
        }
        if (action != null & !is_fixed_traj)
        {
            CanvasObj.GetComponent<UI_control_v3>().state = "free flight";
        }
        if (action != null & is_fixed_traj)
        {
            CanvasObj.GetComponent<UI_control_v3>().state = "fixed traj";
        }
    }
       

    // tan3
    (float theta3, int q3) Atan3(Vector3 vv)
    {
        float x = vv.x;
        float z = vv.z;
        float theta3;
        int q3;
        theta3 = Mathf.Atan2(z, x);
        if (theta3 <= 0) theta3 += 2*pi;
        q3 = (int) Mathf.Ceil(theta3 / (pi / 2));
        return (theta3, q3);
    }


    // button_launch_task
    public void button_launch_task()
    {
        Debug.Log("button_launch_task");
        if (action == null)
        {
            SFXmanager.playsound("click");
            take_off();
        }
    }


    // button_land_task
    public void button_return_task()
    {
        Debug.Log("button_return_task");
        if (action == "free flight")
        {
            SFXmanager.playsound("click");
            landing();
        }
    }


    // button_trail_task
    public void button_trail_task()
    {
        trail_index = 1 - trail_index;
        if (trail_index == 0)
        {
            button_trail.GetComponent<Image>().sprite = sprite_trail_rainbow;
            trail_rainbow.SetActive(false);
        }
        else
        {
            button_trail.GetComponent<Image>().sprite = sprite_trail_none;
            trail_rainbow.SetActive(true);
        }
    }

}


///////////////////////////////////// trash ///////////////////////////////////////
//// update_UI_state
//IEnumerator update_UI_state()
//{
//    while (true)
//    {
//        //Debug.Log("update_UI_state:: action = " + action + "  is_fixed_traj = " + is_fixed_traj);
//        //if (!CanvasObj) Debug.Log("cannot find CanvasObj!!!");
//        if (action == null & !is_fixed_traj)
//        {
//            //debug <<<
//            GameObject atom = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//            atom.transform.position = new Vector3(1, 1, 1);
//            atom.transform.localScale = new Vector3(1, 1, 1) * 0.1f;
//            atom.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
//            atom.GetComponent<MeshRenderer>().material.color = Color.black;
//            //debug >>>
//            CanvasObj.GetComponent<UI_control_v2>().state = "idling";
//        }

//        //debug <<<
//        if (action == null & is_fixed_traj)
//        {
//            //debug <<<
//            GameObject atom = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//            atom.transform.position = new Vector3(1, 1, 1);
//            atom.transform.localScale = new Vector3(1, 1, 1) * 0.1f;
//            atom.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
//            atom.GetComponent<MeshRenderer>().material.color = Color.white;
//            //debug >>>
//            CanvasObj.GetComponent<UI_control_v2>().state = "idling";
//        }
//        //debug >>>

//        if (action != null & !is_fixed_traj)
//        {
//            //debug <<<
//            GameObject atom = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//            atom.transform.position = new Vector3(1, 1, 1);
//            atom.transform.localScale = new Vector3(1, 1, 1) * 0.1f;
//            atom.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
//            atom.GetComponent<MeshRenderer>().material.color = Color.blue;
//            //debug >>>
//            CanvasObj.GetComponent<UI_control_v2>().state = "free flight";
//        }
//        if (action != null & is_fixed_traj)
//        {
//            //debug <<<
//            GameObject atom = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//            atom.transform.position = new Vector3(1, 1, 1);
//            atom.transform.localScale = new Vector3(1, 1, 1) * 0.1f;
//            atom.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
//            atom.GetComponent<MeshRenderer>().material.color = Color.green;
//            //debug >>>
//            CanvasObj.GetComponent<UI_control_v2>().state = "fixed traj";
//        }
//        yield return null;
//    }
//}



//// button launch
//public void button_launch()
//{
//    if (action == null)
//    {
//        take_off();
//    }
//}


//// button return
//public void button_return()
//{
//    if (action == "free flight" & alpha == alpha0)
//    {
//        landing();
//    }
//}


//Debug.Log("Delta = " + Delta.x + "  " + Delta.z + "   q0 = " + q0);

////debug <<<
//GameObject atom = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//atom.transform.localPosition = r0_star;
//atom.transform.localScale = new Vector3(1, 1, 1) * 0.1f;
//atom.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
//atom.GetComponent<MeshRenderer>().material.color = Color.red;
//atom.transform.parent = transform.parent;

//GameObject atom2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//atom2.transform.localPosition = r0;
//atom2.transform.localScale = new Vector3(1, 1, 1) * 0.1f;
//atom2.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
//atom2.GetComponent<MeshRenderer>().material.color = Color.blue;
//atom2.transform.parent = transform.parent;
////debug >>>


//Debug.Log("q0 = " + q0 + "  q1 = " + q1 + "  theta = " + theta + "  theta0 = " + theta0 + "  d0 = " + d0);

//job_list.Add(new job() { action = "circle", parameters = new List<float> { alpha0, 1.25f } });
//job_list.Add(new job() { action = "circle", parameters = new List<float> { -alpha0, 0.25f } });

//job_list.Add(new job() { action = "straight", parameters = new List<float> { L1 } });
//job_list.Add(new job() { action = "take off", parameters = new List<float> { h, L2 } });
//job_list.Add(new job() { action = "straight", parameters = new List<float> { R } });
//job_list.Add(new job() { action = "free flight", parameters = new List<float> { } });
//job_list.Add(new job() { action = "landing", parameters = new List<float> { h, L2 } });


//Debug.Log("circle:: t = " + t + "  t_max = " + t_max + "  para1 = " + parameters[0] + "  para2 = " + parameters[1] + "  omega = " + omega + "  alpha = " + alpha);

//int M = 10;
//pi = Mathf.PI;
//for (int k = 0; k<M; k++)
//{
//    float tt = 2 * pi / M * k;
//    float x = Mathf.Cos(tt);
//    float y = Mathf.Sin(tt);
//    (float tt_, int qq_) = Atan3(x, y);
//    Debug.Log("theta = " + tt + "  theta_ = " + tt_ + "  qq_ = " + qq_ + "  k_ = " + (tt_/(2*pi)*M));
//}

//pi = Mathf.PI;
//float tt = pi / 6;
//float x = Mathf.Cos(tt);
//float z = Mathf.Sin(tt);
//var vv = new Vector3(x, 0, z)*2;
//var uu = Quaternion.AngleAxis(-90, Vector3.up) * vv.normalized;
//float x_ = uu.x;
//float z_ = uu.z;
//Debug.Log("x = " + x + " z = " + z + " x_ =" + x_ + " z_ = " + z_);
//Debug.Log("vv = " + vv.magnitude);



//Debug.Log("  ratio = " + (theta3 / (pi / 2)) + "  q3 = " + q3);
//if (theta3 < 0)
//{
//    theta3 = theta3 + pi;
//}
//if (theta3 < pi/2)
//{
//    q3 = 1;
//}
//else if (theta3 < pi)
//{
//    q3 = 2;
//}
//else if (theta3 < pi*3/2)
//{
//    q3 = 3;
//}
//else
//{
//    q3 = 4;
//}


//print("trans: " + transform.position.x + "  " + transform.position.y + "  " + transform.position.z);
//print("d = " + delta.magnitude);

//for (int i = 0; i < job_list.Count; i++ )
//{
//    var job = job_list[i];
//    print("action = " + job.action + "  para = " + job.parameters[0]);
//}

//var job_new = job_list[0];
//job_list.RemoveAt(0);
//print("action = " + job_new.action + "  para = " + job_new.parameters[0]);
//print("job_list length = " + job_list.Count);

//job_list.RemoveAt(0);
//print("action = " + job_new.action + "  para = " + job_new.parameters[0]);
//print("job_list length = " + job_list.Count);


//if (t == 0) action_origin = transform.position;
//var delta = transform.position - action_origin;
//if (delta.magnitude >= parameters[0])
//{
//    action = null;
//    return;
//}




////Debug.Log("forward = " + transform.forward.x + "  " + transform.forward.y + "  " + transform.forward.z);
////debug<<<
//Vector3 center = transform.position + new Vector3(-R, h, L1 + L2 + R);
//GameObject atom2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//atom2.transform.position = center;
//atom2.transform.localScale = new Vector3(1, 1, 1) * 0.1f;
//atom2.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
//atom2.GetComponent<MeshRenderer>().material.color = Color.green;
////debug>>>
///



//var direction = transform.rotation * transform.forward;
////velocity = speed * transform.forward;
//velocity = speed * direction;   
//transform.localPosition += velocity * Time.deltaTime;
//transform.Rotate(0, -omega * alpha * Time.deltaTime / pi * 180, 0);

//var direction = transform.InverseTransformDirection(transform.forward);
//velocity = speed * direction;
//transform.localPosition += velocity * Time.deltaTime;
////transform.Rotate(0, -omega * alpha * Time.deltaTime / pi * 180, 0);
//transform.localRotation = Quaternion.Euler(0, -omega * alpha * Time.deltaTime, 0);
//debug >>>


//transform.Find("orientation").transform.localEulerAngles = new Vector3(0, 0, alpha * roll_max);



//Vector3 r0 = transform.position + Quaternion.AngleAxis(-90f*Mathf.Sign(alpha0), Vector3.up) * velocity.normalized * R;
//debug <<<
//Vector3 r0 = transform.localPosition + Quaternion.AngleAxis(-90f * Mathf.Sign(alpha0), Vector3.up) * velocity.normalized * R;
//Vector3 r0 = transform.localPosition + transform.rotation * Quaternion.AngleAxis(-90f * Mathf.Sign(alpha0), Vector3.up) * velocity.normalized * R;


//Debug.Log("transform.position = " + transform.position.x + " " + transform.position.y + " " + transform.position.z);
//Debug.Log("velocity.normalized = " + velocity.normalized.x + " " + velocity.normalized.y + " " + velocity.normalized.z);
//Debug.Log("r0 = " + r0.x + " " + r0.y + " " + r0.z);

//debug >>>

//debug <<<
//Vector3 r0_star = new Vector3(-R, h, L1+L2+2*R);
//Vector3 r0_star = transform.parent.position + transform.parent.rotation * new Vector3(-R, h, L1 + L2 + 2 * R);
//debug >>>

//debug <<<


//GameObject atom = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//atom.transform.position = r0;
//atom.transform.localScale = new Vector3(1, 1, 1) * 0.1f;
//atom.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
//atom.GetComponent<MeshRenderer>().material.color = Color.red;

//GameObject atom1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//atom1.transform.position = r0_star;
//atom1.transform.localScale = new Vector3(1, 1, 1) * 0.1f;
//atom1.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
//atom1.GetComponent<MeshRenderer>().material.color = Color.blue;


//debug <<<
//(float _, int q0) = Atan3(transform.InverseTransformDirection(Delta));
//(float theta0, int _) = Atan3(transform.InverseTransformDirection(transform.position - r0));
//debug >>>


//if (CanvasObj)
//{
//    //debug <<<
//    GameObject atom = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//    atom.transform.position = new Vector3(1, 1, 1);
//    atom.transform.localScale = new Vector3(1, 1, 1) * 0.1f;
//    atom.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
//    atom.GetComponent<MeshRenderer>().material.color = Color.red;
//    //debug >>>
//    Debug.Log("CanvasObj name = " + CanvasObj.name);
//}
//else
//{
//    //debug <<<
//    GameObject atom = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//    atom.transform.position = new Vector3(1, 1, 1);
//    atom.transform.localScale = new Vector3(1, 1, 1) * 0.1f;
//    atom.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
//    atom.GetComponent<MeshRenderer>().material.color = Color.blue;
//    //debug >>>
//    Debug.Log("not find CanvasObj");
//}
//StartCoroutine(fly_control_keyboard());
//StartCoroutine(roll_angle_dumping());
//StartCoroutine(update_UI_state());



//// action: change alpha
//if (action == "change alpha")
//{
//    alpha = parameters[0];
//    action = null;
//    return;
//}



//// action: sync slicer
//if (action == "sync slider")
//{
//    // Debug.Log("sync slider: alpha = " + alpha);
//    slider_alpha.value = (1 - alpha) / 2;
//    action = null;
//    return;
//}



//// keyboard fly control	
//IEnumerator fly_control_keyboard()
//{
//    while (true)
//    {
//        // take off
//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            if (action == null)
//            {
//                take_off();
//                yield return null;
//            }
//        }

//        // landing
//        if (Input.GetKeyDown(KeyCode.Return))
//        {
//            if (action == "free flight" & alpha == alpha0)
//            {
//                landing();
//                yield return null;
//            }
//        }

//        // turn right
//        if (Input.GetKeyDown(KeyCode.RightArrow))
//        {
//            if (action == "free flight")
//            {
//                alpha = Mathf.Max(-1f, alpha - 0.1f);
//                yield return null;
//            }                    
//        }

//        // turn left
//        if (Input.GetKeyDown(KeyCode.LeftArrow))
//        {
//            if (action == "free flight")
//            {
//                alpha = Mathf.Min(1f, alpha + 0.1f);
//                yield return null;
//            }
//        }

//        // other
//        yield return null;
//    }
//}


//// roll angle dumping
//IEnumerator roll_angle_dumping()
//{
//    while (true)
//    {

//        if (action == null) yield return null;

//        Vector3 orientation = transform.Find("orientation").transform.localEulerAngles;
//        float roll_angle_ = orientation.z;

//        float angle_offset = 0;
//        if (roll_angle_ > 90) angle_offset = -360;
//        roll_angle_ += angle_offset;

//        float roll_angle = alpha * roll_max;
//        roll_angle_ = roll_eta * roll_angle + (1 - roll_eta) * roll_angle_;

//        roll_angle_ -= angle_offset;

//        orientation.z = roll_angle_;
//        transform.Find("orientation").transform.localEulerAngles = orientation;
//        yield return null;
//    }
//}



//// update_UI_state
//IEnumerator update_UI_state()
//{
//    while (true)
//    {
//        if (GameObject.Find("Canvas"))
//        {
//            if (action == null & !is_fixed_traj)
//            {
//                GameObject.Find("Canvas").GetComponent<UI_control_v2>().state = "idling";
//            }
//            if (action != null & !is_fixed_traj)
//            {
//                GameObject.Find("Canvas").GetComponent<UI_control_v2>().state = "free flight";
//            }
//            if (action != null & is_fixed_traj)
//            {
//                GameObject.Find("Canvas").GetComponent<UI_control_v2>().state = "fixed traj";
//            }
//        }
//        yield return null;
//    }
//}

//is_trail = !is_trail;
//trail.SetActive(is_trail);
//trail_index = (trail_index + 1) % 3;
//if (trail_index == 0)
//{
//    trail_white.SetActive(false);
//    trail_rainbow.SetActive(false);
//}
//if (trail_index == 1)
//{
//    trail_white.SetActive(true);
//    trail_rainbow.SetActive(false);
//}
//if (trail_index == 2)
//{
//    trail_white.SetActive(false);
//    trail_rainbow.SetActive(true);
//}