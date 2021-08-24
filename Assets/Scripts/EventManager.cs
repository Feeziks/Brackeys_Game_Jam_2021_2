using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
  public delegate void AbilityCastAction(AbilityCastType data);
  public static event AbilityCastAction abilityCastEvent;

  public delegate void ScoreChangeAction(int value);
  public static event ScoreChangeAction scoreChangeEvent;




  //Use send message on the event manager to call this and initiate events
  public void OnAbilityCast(AbilityCastType data)
  {
    abilityCastEvent(data);
  }

  public void OnScoreChange(int value)
  {
    scoreChangeEvent(value);
  }
}
