using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{


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
