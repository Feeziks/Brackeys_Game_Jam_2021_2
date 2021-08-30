using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
  public delegate void AbilityCastAction(AbilityCastType data);
  public static event AbilityCastAction abilityCastEvent;

  public delegate void ScoreChangeAction(int value);
  public static event ScoreChangeAction scoreChangeEvent;

  public delegate void ErrorAction(string errorText);
  public static event ErrorAction errorEvent;

  public delegate void GameOverAction(int score);
  public static event GameOverAction gameOverEvent;

  public delegate void ChaosOrderChange(float chaos, float order);
  public static event ChaosOrderChange chaosOrderChangeEvent;


  //Use send message on the event manager to call this and initiate events
  public void OnAbilityCast(AbilityCastType data)
  {
    abilityCastEvent(data);
  }

  public void OnScoreChange(int value)
  {
    scoreChangeEvent(value);
  }

  public void OnErrorEvent(string errorText)
  {
    errorEvent(errorText);
  }

  public void OnGameOver(int finalScore)
  {
    gameOverEvent(finalScore);
  }

  public void OnChaosOrderChange(float chaos, float order)
  {
    chaosOrderChangeEvent(chaos, order);
  }
}
