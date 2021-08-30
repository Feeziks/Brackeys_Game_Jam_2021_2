using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public GameplayTimeStatus timeState;
  public float scoreFrequency = 1f; //auto add score every n seconds

  public float chaos;
  public float order;

  private EventManager eManager;
  private UIManager uiManager;

  #region Unity Methods
  private void Awake()
  {
    timeState = GameplayTimeStatus.standard;
    eManager = FindObjectOfType(typeof(EventManager)) as EventManager;
    uiManager = FindObjectOfType(typeof(UIManager)) as UIManager;
  }

  private void Start()
  {
    StartCoroutine("ScoreCoroutine");
  }

  private void Update()
  {
    float orderPercent = order / (chaos + order);
    if(orderPercent >= 1f)
    {
      GameOver();
    }
  }

  #endregion

  private void GameOver()
  {
    int score = uiManager.GetScore();
    eManager.OnGameOver(score);
    timeState = GameplayTimeStatus.paused;
  }


  public void UpdateChaosOrder(float c, float o)
  {
    chaos += c;
    order += o;
    eManager.OnChaosOrderChange(c, o);
  }

  public void UpdateGameSpeed(int value)
  {
    timeState = (GameplayTimeStatus)value;
  }

  #region Coroutines

  //Coroutine for score you get over time
  public IEnumerator ScoreCoroutine()
  {
    while(true)
    {
      if(timeState != GameplayTimeStatus.paused)
      {
        eManager.SendMessage("OnScoreChange", 10);
        yield return new WaitForSeconds(scoreFrequency / (float)timeState);
      }
      else
      {
        yield return null;
      }
    }
  }

  #endregion

}
