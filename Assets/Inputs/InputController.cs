using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
  public Vector2 movement;
  public bool click;
  public bool[] abilities = new bool[6];


  public void OnMovement(InputAction.CallbackContext context)
  {
    movement = context.ReadValue<Vector2>();
  }

  public void OnClick(InputAction.CallbackContext context)
  {
    if(context.started) //Only works when click is initiated, will need to determine if we need a seperate "click held" event
    {
      click = true;
    }
  }

  public void OnAbilityHotkey_1(InputAction.CallbackContext context)
  {
    if(context.started)
    {
      ClearAbilities();
      abilities[0] = true;
    }
  }

  public void OnAbilityHotkey_2(InputAction.CallbackContext context)
  {
    if (context.started)
    {
      ClearAbilities();
      abilities[1] = true;
    }
  }

  public void OnAbilityHotkey_3(InputAction.CallbackContext context)
  {
    if (context.started)
    {
      ClearAbilities();
      abilities[2] = true;
    }
  }

  public void OnAbilityHotkey_4(InputAction.CallbackContext context)
  {
    if (context.started)
    {
      ClearAbilities();
      abilities[3] = true;
    }
  }

  public void OnAbilityHotkey_5(InputAction.CallbackContext context)
  {
    if (context.started)
    {
      ClearAbilities();
      abilities[4] = true;
    }
  }

  public void OnAbilityHotkey_6(InputAction.CallbackContext context)
  {
    if (context.started)
    {
      ClearAbilities();
      abilities[5] = true;
    }
  }

  public void ClearAbilities()
  {
    for(int i = 0; i < abilities.Length; i++)
    {
      abilities[i] = false;
    }
  }
}
