using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
  public Light sun;
  public Light moon;
  public Vector3 center;

  public float rotationSpeed;

  private GameManager gm;

  private void Start()
  {
    gm = FindObjectOfType(typeof(GameManager)) as GameManager;
  }


  private void Update()
  {
    //Rotate the lights

    //TODO: Pause / time speed changes
    if (gm.timeState != GameplayTimeStatus.paused)
    {
      sun.transform.RotateAround(center, Vector3.up, rotationSpeed * Time.deltaTime * (float)gm.timeState);
      moon.transform.RotateAround(center, Vector3.up, rotationSpeed * Time.deltaTime * (float)gm.timeState);
    }

  }
}
