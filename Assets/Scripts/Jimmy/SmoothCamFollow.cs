using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARviation
{
  public class SmoothCamFollow : MonoBehaviour
  {
    public Transform target;
    private new Transform camera;
    private float distance = 10.0f;

    public float posSpeed = 1.0f;
    public float rotSpeed = 1.0f;

    private void Start()
    {
      camera = GetComponent<Transform>();
    }

    private void Update()
    {
      camera.position = Vector3.Lerp(camera.position, target.position, (posSpeed * Time.deltaTime));

      camera.rotation = Quaternion.Lerp(camera.rotation, target.rotation, (rotSpeed * Time.deltaTime));
    }
  }
}