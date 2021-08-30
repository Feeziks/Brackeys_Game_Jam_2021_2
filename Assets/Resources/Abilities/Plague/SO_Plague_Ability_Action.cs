using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Abilities/Plague")]
public class SO_Plague_Ability_Action : SO_ScriptableAction
{
  public GameObject model;
  public override void PerformAction(GameObject abilityManager, SO_Ability callingAbility)
  {
    AbilityCastType temp = new AbilityCastType(Vector3.zero, Time.realtimeSinceStartup, callingAbility);
    abilityManager.SendMessage("PlagueAbility", temp);
  }
}
