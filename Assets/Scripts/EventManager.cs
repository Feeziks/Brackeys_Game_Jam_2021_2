using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
  public delegate void AbilityCastAction(AbilityCastType data);
  public static event AbilityCastAction abilityCastEvent;






  //Use send message on the event manager to call this and initiate events
  public void OnAbilityCast(AbilityCastType data)
  {
    abilityCastEvent(data);
  }
}
