using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Ability")]
public class SO_Ability : ScriptableObject
{
  public string abilityName;
  public Sprite abilitySprite;
  public string abilityToolTip;
  public float abilityCooldown;

  public SO_ScriptableAction action;
}
