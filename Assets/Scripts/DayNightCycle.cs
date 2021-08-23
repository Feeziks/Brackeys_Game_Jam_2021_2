using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
  public Light sun;
  public Light moon;
  public Vector3 center;

  public float rotationSpeed;

  private void Start()
  {
    
  }


  private void Update()
  {
    //Rotate the lights

    //TODO: Pause / time speed changes

    sun.transform.RotateAround(center, Vector3.up, rotationSpeed * Time.deltaTime);
    moon.transform.RotateAround(center, Vector3.up, rotationSpeed * Time.deltaTime);

  }
}
