using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Abilities/Asteroid")]
public class SO_Ability_Asteroid : SO_ScriptableAction
{
  public Mesh asteroidModel;
  public float delayBeforeCast;
  public float timeToImpact;

  public float timeDamageRemains;
  public float damageRadius;

  public override void PerformAction(GameObject abilityManager)
  {
    AbilityCastType temp = null;
    abilityManager.SendMessage("AsteroidAbility", temp);
  }
}
