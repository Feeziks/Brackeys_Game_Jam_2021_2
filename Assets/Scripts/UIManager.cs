using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

  #region Properties

  private GameObject hovered;
  private AbilityManager abilityManager;
  
  private Dictionary<GameObject, int> abilityGameObjectToIndex;
  private Dictionary<GameObject, SO_Ability> abilityGameObjectToAbility;
  public List<GameObject> abilityGameObjects;
  public List<int> abilityIndices;
  public List<SO_Ability> abilities;

  public GameObject toolTipGO;
  public TextMeshProUGUI toolTipHeader;
  public TextMeshProUGUI ToolTipBody;

  public RectTransform chaosOrderPanel;
  public RectTransform chaosBar;
  public RectTransform orderBar;

  public TextMeshProUGUI scoreText;
  private int score;

  #endregion

  #region Unity Methods

  private void Start()
  {
    hovered = null;
    abilityManager = FindObjectOfType(typeof(AbilityManager)) as AbilityManager;
    abilityGameObjectToAbility = new Dictionary<GameObject, SO_Ability>();
    abilityGameObjectToIndex = new Dictionary<GameObject, int>();

    int idx = 0;
    foreach(GameObject go in abilityGameObjects)
    {
      abilityGameObjectToIndex[go] = abilityIndices[idx];
      abilityGameObjectToAbility[go] = abilities[idx];
      idx++;
    }

    UpdateAbilitySprites();

    score = 0;
    UpdateScoreText(score);
  }

  private void Awake()
  {
    
  }

  private void Update()
  {
    if (EventSystem.current.IsPointerOverGameObject())
    {
      hovered = GetHoveredObject(GetPointerRaycastResults());
      DisplayToolTip(hovered);
      if(Input.GetMouseButtonDown(0))
      {
        abilityManager.SendMessage("SelectAbilityFromUI", abilityGameObjectToIndex[hovered]);
      }

    }
    else
    {
      hovered = null;
      toolTipGO.SetActive(false);
    }
  }

  #endregion

  #region Events

  private void OnEnable()
  {
    EventManager.abilityCastEvent += OnAbilityCast;
    EventManager.scoreChangeEvent += UpdateScoreText;
  }

  private void OnDisable()
  {
    EventManager.abilityCastEvent -= OnAbilityCast;
    EventManager.scoreChangeEvent -= UpdateScoreText;

  }

  private void OnAbilityCast(AbilityCastType data)
  {
    //Handle ability realted UI things

    //Show that ability is on cooldown etc
  }

  #endregion

  #region Tool Tips

  //Find if we are hovering over an ability
  private GameObject GetHoveredObject(List<RaycastResult> raycasts)
  {
    for(int index = 0; index < raycasts.Count; index++)
    {
      if(raycasts[index].gameObject.layer == LayerMask.NameToLayer("UI"))
      {
        return raycasts[index].gameObject;
      }
    }

    return null;
  }

  private List<RaycastResult> GetPointerRaycastResults()
  {
    PointerEventData eventData = new PointerEventData(EventSystem.current);
    eventData.position = Input.mousePosition;
    List<RaycastResult> result = new List<RaycastResult>();
    EventSystem.current.RaycastAll(eventData, result);
    return result;
  }

  public void DisplayToolTip(GameObject abilityGameObject)
  {
    if(abilityGameObject == null)
        return;

    if (!abilityGameObjectToAbility.ContainsKey(abilityGameObject))
      return;

    toolTipGO.SetActive(true);
    //RectTransform rt = toolTipGO.GetComponent(typeof(RectTransform)) as RectTransform;
    //rt.anchoredPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    toolTipGO.transform.position = Input.mousePosition;
    toolTipHeader.text = abilityGameObjectToAbility[abilityGameObject].abilityName;
    ToolTipBody.text = abilityGameObjectToAbility[abilityGameObject].abilityToolTip;
    ToolTipBody.text += "\n" + "Cooldown: " + abilityGameObjectToAbility[abilityGameObject].abilityCooldown;
  }

  #endregion

  #region Chaos / Order and Score

  public void UpdateChaosAndOrder(float chaos, float order)
  {
    //1f == full; 0f == empty;

    chaosBar.sizeDelta = new Vector2(chaosOrderPanel.rect.width / chaos, chaosOrderPanel.rect.height);
    orderBar.sizeDelta = new Vector2(chaosOrderPanel.rect.width / chaos, chaosOrderPanel.rect.height);
  }

  public void UpdateScoreText(int scoreChange)
  {
    score += scoreChange;
    scoreText.text = score.ToString();
  }

  #endregion

  #region helpers

  private void UpdateAbilitySprites()
  {
    foreach(GameObject go in abilityGameObjects)
    {
      Image im = go.GetComponent(typeof(Image)) as Image;

      im.sprite = abilityGameObjectToAbility[go].abilitySprite;
    }
  }

  #endregion
}
