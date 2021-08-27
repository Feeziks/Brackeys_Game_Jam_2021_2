using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
  public Camera gameplayCamera;
  public GameObject aoeProjector;

  public List<SO_Ability> allAbilities;
  public InputController inputs;

  private SO_Ability[] currentAbilities;
  public int currSelectedAbility;

  #region Unity Methods
  private void Awake()
  {
    allAbilities = new List<SO_Ability>();
    InitializeAllAbilities();

    currentAbilities = new SO_Ability[6];
    currSelectedAbility = -1;
  }

  private void Start()
  {
    
  }

  private void Update()
  {
    //Check our inputs for user clicks
    HandleUserInput();

    //Display the AOE of the currently selected ability
    if(currSelectedAbility != -1)
    {
      DisplayAbilityAOE();
    }
    else
    {
      aoeProjector.SetActive(false);
    }
  }


  #endregion

  #region Helpers

  private void InitializeAllAbilities()
  {
    Object[] temp = Resources.LoadAll("Abilities", typeof(SO_Ability));

    foreach(Object tempAbility in temp)
    {
      SO_Ability ability = (SO_Ability)tempAbility;
      allAbilities.Add(ability);
    }
  }

  private void HandleUserInput()
  {
    if(inputs.click)
    {
      inputs.click = false;
      if(currSelectedAbility == -1)
      {
        Debug.Log("Must select an ability before casting!\nPlease use hotkeys 1 through 6 or select the ability through the UI");
        return;
      }

      //If our raycast target is NOT the planet dont cast
      Vector3 HitPosition = GetMousePositionOnPlanet();
      if (HitPosition != Vector3.zero)
      {
        Debug.Log("Casting spell: " + currSelectedAbility + "\nAt position: " + HitPosition);
        allAbilities[currSelectedAbility].action.PerformAction(gameObject, allAbilities[currSelectedAbility]);
        currSelectedAbility = -1;
      }
    }
  }

  private Vector3 GetMousePositionOnPlanet()
  {
    Vector3 mousePos = gameplayCamera.ScreenToWorldPoint(Input.mousePosition);
    Ray r = new Ray(mousePos, -1f * mousePos);
    RaycastHit hit;
    if(Physics.Raycast(r, out hit, 1000))
    {
      return hit.transform.position;
    }

    return Vector3.zero;
  }

  private void DisplayAbilityAOE()
  {
    SO_Ability ability = allAbilities[currSelectedAbility];

    //Show the AOE circle with center at mouse position and radius from ability
    Vector3 pos = GetMousePositionOnPlanet();
    float radius = ability.abilityRadius;

    aoeProjector.SetActive(true);
    aoeProjector.transform.position = pos + (-1f * pos) * 10f;
    aoeProjector.transform.LookAt(Vector3.zero);

  }

  #endregion

  #region Functions for Cast from UI
  public void SelectAbilityFromUI(int abilityNum)
  {
    currSelectedAbility = abilityNum;
  }

  #endregion

  #region Functions For Send Message

  public void HotkeyPressed(int abilityIdx)
  {
    //I dont know why this gross work around is required but I cant send a message with int 0 for some reason
    if (abilityIdx == -1)
      abilityIdx = 0;
    currSelectedAbility = abilityIdx;
  }

  public void AsteroidAbility(AbilityCastType data)
  {
    StartCoroutine(AsteroidAbility_Coroutine(data));
  }

  #endregion

  #region Coroutines

  public IEnumerator AsteroidAbility_Coroutine(AbilityCastType data)
  {
    SO_Ability_Asteroid asteroidAbility = (SO_Ability_Asteroid)data.ability.action;
    GameObject asteroidGo = Instantiate(asteroidAbility.asteroidModel);
    asteroidGo.transform.position = gameplayCamera.transform.position + gameplayCamera.transform.forward * -1f * 20f; //Should Spawn behind the camera
    asteroidGo.transform.LookAt(Vector3.zero);
    float velocity = Vector3.Distance(Vector3.zero, asteroidGo.transform.position) / data.ability.actionDuration;


    yield return new WaitForSecondsRealtime(data.ability.delayBeforeAction);
    float startTime = Time.realtimeSinceStartup;
    float endTime = startTime + data.ability.actionDuration;
    
    while(Time.realtimeSinceStartup < endTime)
    {
      asteroidGo.transform.position += asteroidGo.transform.forward * velocity * Time.deltaTime;
      yield return null;
    }

    yield return null;
  }

  #endregion



}
