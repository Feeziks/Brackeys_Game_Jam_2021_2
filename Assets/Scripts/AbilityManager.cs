using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
  private GameManager gm;
  private EventManager eManager;

  public Camera gameplayCamera;
  public GameObject aoeIndicator;

  public List<SO_Ability> allAbilities;
  public InputController inputs;
  public int currSelectedAbility;

  public Dictionary<SO_Ability, float> abilityCooldownTimers;

  public Vector3 mousePosition;
  public Ray mouseRay;

  public GameObject planet;

  #region Unity Methods
  private void Awake()
  {
    allAbilities = new List<SO_Ability>();
    abilityCooldownTimers = new Dictionary<SO_Ability, float>();
    InitializeAllAbilities();
    currSelectedAbility = -1;

    gm = FindObjectOfType(typeof(GameManager)) as GameManager;
    eManager = FindObjectOfType(typeof(EventManager)) as EventManager;
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
      aoeIndicator.SetActive(false);
    }


    UpdateAbilityCooldowns();
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

      abilityCooldownTimers[ability] = 0f;
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
    Vector3 mousePos = gameplayCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100f));
    Planet p = planet.GetComponent(typeof(Planet)) as Planet;
    Vector3 point;
    Vector3 closestPoint = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
    float minDistance = float.MaxValue;

    foreach (Collider c in p.colliders)
    {
      point = c.ClosestPoint(mousePos);
      if (Vector3.Distance(mousePos, point) < minDistance)
      {
        minDistance = Vector3.Distance(mousePos, point);
        closestPoint = point;
      }
    }

    return closestPoint;
  }

  private void DisplayAbilityAOE()
  {
    SO_Ability ability = allAbilities[currSelectedAbility];

    //Show the AOE circle with center at mouse position and radius from ability
    Vector3 pos = GetMousePositionOnPlanet();
    mousePosition = pos;
    mouseRay = new Ray(pos, -pos);
    float radius = ability.abilityRadius;

    aoeIndicator.SetActive(true);
    aoeIndicator.transform.position = pos;
    aoeIndicator.transform.localScale = new Vector3(radius, radius, radius);
  }

  private void UpdateAbilityCooldowns()
  {
    if (gm.timeState == GameplayTimeStatus.paused)
      return;
    /*
    foreach(KeyValuePair<SO_Ability, float> kvp in abilityCooldownTimers)
    {
      if(kvp.Value > 0f)
      {
        abilityCooldownTimers[kvp.Key] = kvp.Value - Time.deltaTime * (float)gm.timeState;
      }
    }
    */
    List<SO_Ability> keys = new List<SO_Ability>(abilityCooldownTimers.Keys);
    foreach(SO_Ability key in keys)
    {
      if(abilityCooldownTimers[key] > 0f)
      {
        abilityCooldownTimers[key] -= Time.deltaTime * (float)gm.timeState;
      }
    }
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

  public void ApplyAbilityToMeeplesInRadius(AbilityCastType data, GameObject abilityGO)
  {
    AbilityAffectMeepleType abilityAffectData = new AbilityAffectMeepleType(data.ability.actionDuration, data.ability.chaosInfliction, data.ability.killChance);
    RaycastHit[] hits = Physics.SphereCastAll(abilityGO.transform.position, data.ability.abilityRadius, abilityGO.transform.forward, 0.1f);

    foreach (RaycastHit h in hits)
    {
      if (h.collider.gameObject.layer == LayerMask.NameToLayer("Meeple"))
      {
        h.collider.gameObject.SendMessage("AbilityEffect", abilityAffectData);
      }
    }

  }

  public void AsteroidAbility(AbilityCastType data)
  {
    if (abilityCooldownTimers[data.ability] <= 0f)
    {
      StartCoroutine(AsteroidAbility_Coroutine(data));
      abilityCooldownTimers[data.ability] = data.ability.abilityCooldown;
    }
    else
    {
      //Tell the player they have to wait somehow
      eManager.SendMessage("OnErrorEvent", data.ability.abilityName + " Is still on cooldown");
    }
  }

  public void SnakeAbility(AbilityCastType data)
  {
    if (abilityCooldownTimers[data.ability] <= 0f)
    {
      SO_Ability_Snake snake = (SO_Ability_Snake)data.ability.action;
      GameObject snakeGO = Instantiate(snake.snakeModel);
      snakeGO.layer = LayerMask.NameToLayer("Ability");
      snakeGO.transform.parent = gameObject.transform;
      snakeGO.transform.position = mousePosition;
      snakeGO.transform.localScale = new Vector3(3, 3, 3);
      Destroy(snakeGO, data.ability.actionDuration);
      abilityCooldownTimers[data.ability] = data.ability.abilityCooldown;
      ApplyAbilityToMeeplesInRadius(data, snakeGO);
    }
    else
    {
      eManager.SendMessage("OnErrorEvent", data.ability.abilityName + " Is still on cooldown");
    }
  }

  public void SowDivisionAbility(AbilityCastType data)
  {
    if(abilityCooldownTimers[data.ability] <= 0f)
    {
      SO_Sow_Division_Action division = (SO_Sow_Division_Action)data.ability.action;
      GameObject abilityGo = Instantiate(division.model);
      abilityGo.layer = LayerMask.NameToLayer("Ability");
      abilityGo.transform.parent = gameObject.transform;
      abilityGo.transform.position = mousePosition;
      abilityGo.transform.localScale = new Vector3(3, 3, 3);
      Destroy(abilityGo, data.ability.actionDuration);
      abilityCooldownTimers[data.ability] = data.ability.abilityCooldown;
      ApplyAbilityToMeeplesInRadius(data, abilityGo);
    }
    else
    {
      eManager.SendMessage("OnErrorEvent", data.ability.abilityName + " Is still on cooldown");
    }
  }

  #endregion

  #region Coroutines

  public IEnumerator AsteroidAbility_Coroutine(AbilityCastType data)
  {
    SO_Ability_Asteroid asteroidAbility = (SO_Ability_Asteroid)data.ability.action;
    GameObject asteroidGo = Instantiate(asteroidAbility.asteroidModel);
    asteroidGo.transform.position = mousePosition * 4f;
    asteroidGo.transform.localScale = new Vector3(8f, 8f, 8f);
    asteroidGo.transform.LookAt(Vector3.zero);
    asteroidGo.layer = LayerMask.NameToLayer("Ability");
    float velocity = Vector3.Distance(mousePosition, asteroidGo.transform.position) / data.ability.actionDuration;


    yield return new WaitForSecondsRealtime(data.ability.delayBeforeAction);
    float startTime = Time.realtimeSinceStartup;
    float endTime = startTime + data.ability.actionDuration;
    
    while(Time.realtimeSinceStartup < endTime)
    {
      asteroidGo.transform.position += asteroidGo.transform.forward * velocity * Time.deltaTime;
      yield return null;
    }

    ApplyAbilityToMeeplesInRadius(data, asteroidGo);

    Destroy(asteroidGo, data.ability.actionDuration);
    yield return null;
  }

  #endregion



}
