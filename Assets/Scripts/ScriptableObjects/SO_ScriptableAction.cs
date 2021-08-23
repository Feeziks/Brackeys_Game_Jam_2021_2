using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SO_ScriptableAction : ScriptableObject
{
  public abstract void PerformAction(GameObject abilityManager);
}
