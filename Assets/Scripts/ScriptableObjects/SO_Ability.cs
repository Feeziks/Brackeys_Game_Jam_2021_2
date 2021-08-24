using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Ability")]
public class SO_Ability : ScriptableObject
{
  public string abilityName;
  public Sprite abilitySprite;
  public float abilityCooldown;

  public GameObject abilityManager;
  public SO_ScriptableAction action;
}
