using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Abilities/Asteroid")]
public class SO_Ability_Asteroid : SO_ScriptableAction
{
  public GameObject asteroidModel;

  public float timeDamageRemains;

  public override void PerformAction(GameObject abilityManager, SO_Ability callingAbility)
  {
    AbilityCastType temp = new AbilityCastType(Vector3.zero, Time.realtimeSinceStartup, callingAbility);
    abilityManager.SendMessage("AsteroidAbility", temp);
  }
}
