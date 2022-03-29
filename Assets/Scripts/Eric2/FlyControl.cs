using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyControl : MonoBehaviour
{
    // Variables
    public struct job
    {
        public string action;
        public List<float> parameters;
    }

    float R = 2f;
    float L1 = 2f;
    float L2 = 4f;
    float h = 2f;
    float speed = 1f;
    float omega;
    float pi;
    float alpha0 = 1f;
    float roll_max = 15; //degree

    public string action;
    List<float> parameters;
    public List<job> job_list;
    public float alpha;
    Vector3 velocity;


    float t;
    Vector3 action_origin;


    // Start
    void Start()
    {
        action = null;
        job_list = new List<job>();

        alpha = alpha0;
        omega = speed / R;
        pi = Mathf.PI;

        StartCoroutine(fly_control_keyboard());
    }


    // Update
    void Update()
    {
        //Debug.Log("action = " + action + "  len(job_list) = " + job_list.Count);

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

        // action: straight
        if (action == "straight")
        {
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

        // action: take off
        if (action == "take off")
        {
            if (t == 0) action_origin = transform.position;
            var delta = transform.position - action_origin;
            if (Mathf.Abs(delta.z) >= parameters[1])
            {
                action = null;
                return;
            }

            float h = parameters[0];
            float L = parameters[1];
            float z = delta.z;
            float gamma = h / 2 * Mathf.Sin(z / L * pi) * pi / L;
            float theta = Mathf.Atan(gamma);
            velocity.x = 0;
            velocity.y = speed * gamma / Mathf.Sqrt(1 + gamma * gamma);
            velocity.z = speed * 1 / Mathf.Sqrt(1 + gamma * gamma);
            transform.position += velocity * Time.deltaTime;
            transform.Find("orientation").transform.localEulerAngles = new Vector3(-theta/pi*180, 0, 0);
            t += Time.deltaTime;
            return;
        }

        // action: landing
        if (action == "landing")
        {
            if (t == 0) action_origin = transform.position;
            var delta = transform.position - action_origin;
            if (Mathf.Abs(delta.z) >= parameters[1])
            {
                action = null;
                return;
            }

            float h = parameters[0];
            float L = parameters[1];
            float z = delta.z;
            float gamma = - h / 2 * Mathf.Sin(z / L * pi) * pi / L;
            float theta = Mathf.Atan(gamma);
            velocity.x = 0;
            velocity.y = - speed * gamma / Mathf.Sqrt(1 + gamma * gamma);
            velocity.z = - speed * 1 / Mathf.Sqrt(1 + gamma * gamma);
            transform.position += velocity * Time.deltaTime;
            transform.Find("orientation").transform.localEulerAngles = new Vector3(theta / pi * 180, 0, 0);
            t += Time.deltaTime;
            return;
        }

        // action: free flight
        if (action == "free flight")
        {
            velocity = speed * transform.forward;
            transform.position += velocity * Time.deltaTime;
            transform.Rotate(0, -omega*alpha * Time.deltaTime / pi * 180, 0);
            transform.Find("orientation").transform.localEulerAngles = new Vector3(0, 0, alpha * roll_max);
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
            transform.Find("orientation").transform.localEulerAngles = new Vector3(0, 0, alpha * roll_max);
            t += Time.deltaTime;
            if (t >= t_max)
            {
                action = null;
                return;
            }
            return;
        }
    }


    // take off
    void take_off()
    {
        alpha = alpha0;
        transform.localEulerAngles = Vector3.zero;
        transform.Find("orientation").transform.localEulerAngles = Vector3.zero;
        action = null;
        job_list = new List<job>();
        job_list.Add(new job() { action = "straight", parameters = new List<float> { L1 } });
        job_list.Add(new job() { action = "take off", parameters = new List<float> { h, L2 } });
        job_list.Add(new job() { action = "straight", parameters = new List<float> { R } });
        job_list.Add(new job() { action = "free flight", parameters = new List<float> { } });
    }


    // landing
    void landing()
    {
        // q0, theta0
        alpha = alpha0;
        Vector3 r0 = transform.position + Quaternion.AngleAxis(-90f*Mathf.Sign(alpha0), Vector3.up) * velocity.normalized * R;
        Vector3 r0_star = new Vector3(-R, h, L1+L2+2*R);
        Vector3 Delta = r0 - r0_star;
        (float _, int q0) = Atan3(Delta);
        (float theta0, int _) = Atan3(transform.position - r0);

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
        job_list.Add(new job() { action = "circle", parameters = new List<float> { alpha0, d0 } });
        job_list.Add(new job() { action = "straight", parameters = new List<float> { d1 } });
        job_list.Add(new job() { action = "circle", parameters = new List<float> { alpha0, 0.25f } });
        job_list.Add(new job() { action = "straight", parameters = new List<float> { d2 } });
        job_list.Add(new job() { action = "circle", parameters = new List<float> { alpha0, 0.25f *q1 } });
        job_list.Add(new job() { action = "circle", parameters = new List<float> { -alpha0, 0.25f } });
        job_list.Add(new job() { action = "landing", parameters = new List<float> { h, L2 } });
        job_list.Add(new job() { action = "straight", parameters = new List<float> { L1 } });
    }


    // keyboard fly control	
    IEnumerator fly_control_keyboard()
    {
        while (true)
        {
            // take off
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (action == null)
                {
                    take_off();
                    yield return null;
                }
            }

            // landing
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (action == "free flight")
                {
                    landing();
                    yield return null;
                }
            }

            // turn right
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (action == "free flight")
                {
                    alpha = Mathf.Max(-1f, alpha - 0.1f);
                    yield return null;
                }                    
            }

            // turn left
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (action == "free flight")
                {
                    alpha = Mathf.Min(1f, alpha + 0.1f);
                    yield return null;
                }
            }

            // other
            yield return null;
        }
    }


    // Atan3
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


}


///////////////////////////////////////////////////////////////////////////////////////////////////////

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