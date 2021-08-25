using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCastType
{
  public Vector3 location;
  public float timeInitiated;
  public SO_Ability ability;

  public AbilityCastType(Vector3 l, float tI, SO_Ability a)
  {
    location = l;
    timeInitiated = tI;
    ability = a;
  }
}


public enum GameplayTimeStatus
{
  paused    = 0,
  standard  = 1,
  dbl       = 2,
  triple    = 3
}
