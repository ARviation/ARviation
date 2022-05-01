using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextSentenceButton : MonoBehaviour
{
  private bool _canTriggerFeedback = true;
  private Vector3 originPosition;

  private void Start()
  {
    originPosition = gameObject.transform.position;
  }

  public void StartShakingEffect()
  {
    StartCoroutine(ShakingEffect());
  }

  private IEnumerator ShakingEffect()
  {
    float offset = 5.0f;
    while (true)
    {
      var position = gameObject.transform.position;
      Vector3 newPosition = new Vector3(position.x, position.y + offset, position.z);
      gameObject.transform.position = newPosition;
      offset *= -1;
      yield return new WaitForSeconds(0.2f);
      // yield return null;
    }
  }
}