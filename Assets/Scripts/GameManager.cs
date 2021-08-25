using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public GameplayTimeStatus timeState;


  #region Unity Methods
  private void Awake()
  {
    timeState = GameplayTimeStatus.standard;
  }

  private void Start()
  {
    
  }

  private void Update()
  {
    
  }

  #endregion


}
