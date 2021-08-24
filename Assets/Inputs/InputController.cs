using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
  public Vector2 movement;
  public bool click;
  private AbilityManager abilityManager;

  private void Start()
  {
    abilityManager = FindObjectOfType(typeof(AbilityManager)) as AbilityManager;
  }

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
      abilityManager.SendMessage("HotkeyPressed", (int)-1);
    }
  }

  public void OnAbilityHotkey_2(InputAction.CallbackContext context)
  {
    if (context.started)
    {
      abilityManager.SendMessage("HotkeyPressed", 1);
    }
  }

  public void OnAbilityHotkey_3(InputAction.CallbackContext context)
  {
    if (context.started)
    {
      abilityManager.SendMessage("HotkeyPressed", 2);
    }
  }

  public void OnAbilityHotkey_4(InputAction.CallbackContext context)
  {
    if (context.started)
    {
      abilityManager.SendMessage("HotkeyPressed", 3);
    }
  }

  public void OnAbilityHotkey_5(InputAction.CallbackContext context)
  {
    if (context.started)
    {
      abilityManager.SendMessage("HotkeyPressed", 4);
    }
  }

  public void OnAbilityHotkey_6(InputAction.CallbackContext context)
  {
    if (context.started)
    {
      abilityManager.SendMessage("HotkeyPressed", 5);
    }
  }

}
