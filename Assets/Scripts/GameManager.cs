using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
  public GameplayTimeStatus timeState;
  public float scoreFrequency = 1f; //auto add score every n seconds

  public float chaos;
  public float order;

  private EventManager eManager;
  private UIManager uiManager;

  private bool tutorial;
  private int tutorialIdx = 0;

  public GameObject tutorialPanel;
  public TextMeshProUGUI tutorialText;

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
    timeState = GameplayTimeStatus.paused;
    tutorial = true;
  }

  private void Update()
  {
    float orderPercent = order / (chaos + order);
    if(orderPercent >= 1f)
    {
      GameOver();
    }
    if(tutorial)
    {
      HandleTutorial();
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

  public void NextTutorialButton()
  {
    tutorialIdx++;
  }

  public void HandleTutorial()
  {
    switch(tutorialIdx)
    {
      case 0:
        tutorialText.text = "";
        tutorialText.text += "Welcome Loki!\n";
        tutorialText.text += "This planet has brand new inhabitants attempting to form order among themselves\n";
        tutorialText.text += "As the chaotic trickster you are; this simply cannot be allowed\n";
        tutorialText.text += "(Press the arrow to continue)\n";
        break;
      case 1:
        tutorialText.text = "";
        tutorialText.text += "The inhabitants of this world will slowly form groups and develop order\n";
        tutorialText.text += "Its your task to delay this for as long as possible\n";
        tutorialText.text += "Make use of your abilities to ensure there is never more order than chaos\n";
        break;
      case 2:
        tutorialText.text = "";
        tutorialText.text += "Hover over an ability to see its description\n";
        tutorialText.text += "Click on the ability to select it for casting\n";
        tutorialText.text += "Alternativley use the hotkeys [1 - 6] to select the respective ability\n";
        tutorialText.text += "Move your mouse over the planet to see the abilities area of effect\n";
        break;
      case 3:
        tutorialText.text = "";
        tutorialText.text += "Click to cast the ability - Try to hit as many inhabitants as possible!\n";
        tutorialText.text += "Each inhabitant you effect produces more chaos and prevents them from creating order for a short time\n";
        tutorialText.text += "Some abilities can even remove inhabitants - preventing order gain until they respawn\n";
        break;
      case 4:
        tutorialText.text = "";
        tutorialText.text += "Move around the planet with WASD\n";
        tutorialText.text += "Zoom in or out with the scroll wheel\n";
        tutorialText.text += "Press ESC at anytime to quit";
        break;
      case 5:
        tutorialText.text = "";
        tutorialText.text += "Order and chaos are displayed in the top left of the screen\n";
        tutorialText.text += "Blue represents the amount of order\n";
        tutorialText.text += "Red  represents the amount of chaos\n";
        break;
      case 6:
        tutorialText.text = "";
        tutorialText.text += "Your score is shown in the top right\n";
        tutorialText.text += "You gain score passivley and by causing chaos\n\n";
        tutorialText.text += "You can select the game speed in the top left\n";
        tutorialText.text += "Pause - Normal - Double - Triple speed options are available\n";
        break;
      case 7:
        tutorialText.text = "";
        tutorialText.text += "Have fun and cause as much chaos as possible!\n";
        tutorialText.text += "\n";
        tutorialText.text += "Youtube: Feeziks Gaming \n";
        tutorialText.text += "Youtube: Feeziks GameDev \n";
        tutorialText.text += "Itch.IO: Feeziks \n";
        break;
      case 8:
        tutorialPanel.SetActive(false);
        tutorial = false;
        break;
      default:
        break;
    }
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
