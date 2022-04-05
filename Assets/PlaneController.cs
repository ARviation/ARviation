using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
   [SerializeField] private Placeable[] _placeables;
   private bool _openHighlight = false;

   private void Start()
   {
      _placeables = FindObjectsOfType<Placeable>();
   }

   private void Update()
   {
      if (_openHighlight)
      {
         foreach (Placeable placeable in _placeables)
         {
            if (!placeable.GetIsPlace())
            {
               placeable.OpenHighlight();
            }
         }
      }
   }
}