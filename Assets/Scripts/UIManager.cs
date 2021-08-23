using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

  #region Properties


  #endregion



  #region Events

  private void OnEnable()
  {
    EventManager.abilityCastEvent += OnAbilityCast;
  }

  private void OnDisable()
  {
    EventManager.abilityCastEvent -= OnAbilityCast;
  }

  private void OnAbilityCast(AbilityCastType data)
  {
    //Handle ability realted UI things

    //Show that ability is on cooldown etc
  }

  #endregion



}
