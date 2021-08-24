using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{

  public List<SO_Ability> allAbilities;
  public InputController inputs;

  private SO_Ability[] currentAbilities;

  #region Unity Methods
  private void Awake()
  {
    allAbilities = new List<SO_Ability>();
    InitializeAllAbilities();

    currentAbilities = new SO_Ability[6];
  }

  private void Start()
  {
    
  }

  private void Update()
  {
    //Check our inputs for user clicks
    HandleUserInput();
  }


  #endregion

  #region Helpers

  private void InitializeAllAbilities()
  {
    Object[] temp = Resources.LoadAll("Abilities", typeof(SO_Ability));

    foreach(Object tempAbility in temp)
    {
      SO_Ability ability = (SO_Ability)tempAbility;
      ability.abilityManager = gameObject;
      allAbilities.Add(ability);
    }
  }

  private void HandleUserInput()
  {
    if(inputs.click)
    {
      int abilityIdx = GetAbilityIndex();
      if(abilityIdx == -1)
      {
        Debug.Log("Must select an ability before casting!");
        return;
      }

      inputs.click = false;
      Debug.Log("Casting spell: " + abilityIdx);
      inputs.abilities[abilityIdx] = false;
    }
  }

  private int GetAbilityIndex()
  {
    int none = -1;
    for(int i = 0; i < inputs.abilities.Length; i++)
    {
      if (inputs.abilities[i])
        return i;
    }
    return none;
  }

  #endregion


  #region Functions For Send Message

  public void AsteroidAbility(AbilityCastType data)
  {
    StartCoroutine(AsteroidAbility_Coroutine(data));
  }

  #endregion


  #region Coroutines

  public IEnumerator AsteroidAbility_Coroutine(AbilityCastType data)
  {

    Debug.Log("Performing Asteroid Ability");
    yield return new WaitForSecondsRealtime(1f);
    Debug.Log("Asteroid Action Performed");
    yield return null;
  }

  #endregion



}
