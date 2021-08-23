using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
  public Vector2 movement;
  public bool click;


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
}
