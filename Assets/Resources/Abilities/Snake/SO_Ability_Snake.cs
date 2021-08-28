using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Abilities/Snakes")]
public class SO_Ability_Snake : SO_ScriptableAction
{
  public GameObject snakeModel;

  public override void PerformAction(GameObject abilityManager, SO_Ability callingAbility)
  {
    AbilityCastType temp = new AbilityCastType(Vector3.zero, Time.realtimeSinceStartup, callingAbility);
    abilityManager.SendMessage("SnakeAbility", temp);
  }
}
