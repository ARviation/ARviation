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

    float R = 1.5f;
    float L1 = 2f;
    float L2 = 4f;
    float h = 2f;
    float speed = 1f;
    float omega;
    float tol = 0.01f;
    float pi;

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
        //var job1 = new job();
        //job1.action = "go straight";
        //job1.parameters = new List<float> { 1f };
        //job_list.Add(job1);

        job_list.Add(new job() { action = "go straight", parameters = new List<float> { L1 } });
        job_list.Add(new job() { action = "take off", parameters = new List<float> { h, L2 } });
        job_list.Add(new job() { action = "go straight", parameters = new List<float> { R } });

        job_list.Add(new job() { action = "landing", parameters = new List<float> { h, L2 } });

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

        omega = speed / R;
        pi = Mathf.PI;

    }


    // Update
    void Update()
    {
        Debug.Log("action = " + action + "  len(job_list) = " + job_list.Count);

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

        // action: go straight
        if (action == "go straight")
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
            transform.Find("offset").transform.Find("orientation").transform.localEulerAngles = new Vector3(-theta/pi*180, 0, 0);
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
            velocity.y = speed * gamma / Mathf.Sqrt(1 + gamma * gamma);
            velocity.z = speed * 1 / Mathf.Sqrt(1 + gamma * gamma);
            transform.position += velocity * Time.deltaTime;
            transform.Find("offset").transform.Find("orientation").transform.localEulerAngles = new Vector3(-theta / pi * 180, 0, 0);
            t += Time.deltaTime;
            return;
        }




    }
}


///////////////////////////////////////////////////////////////////////////////////////////////////////

//print("trans: " + transform.position.x + "  " + transform.position.y + "  " + transform.position.z);
//print("d = " + delta.magnitude);