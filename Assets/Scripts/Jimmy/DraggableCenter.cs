using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace ARviation
{
  public class DraggableCenter : MonoBehaviour
  {
    public GameObject targetObj;
    public Transform camPivot, camTarget, camRoot;

    private float distance = 10.0f;
    private float rot = 0f;

    private float xAngle = 0.0f;
    private float yAngle = 0.0f;
    private float xAngleTmp = 0.0f;
    private float yAngleTmp = 0.0f;

    private void Start()
    {
      xAngle = 0.0f;
      yAngle = 0.0f;
      this.transform.rotation = Quaternion.Euler(yAngle, xAngle, 0.0f);
    }

    private void Update()
    {
    }
  }
}