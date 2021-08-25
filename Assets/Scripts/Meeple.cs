using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meeple : MonoBehaviour
{
  public GameObject planet;

  private float sphereCastRadius = 5f;
  private float meepleGroundOffset = 1f;
  private LayerMask meepleLayer;

  // Start is called before the first frame update
  void Start()
  {
    meepleLayer = LayerMask.NameToLayer("Meeple");
    gameObject.layer = meepleLayer;
  }

  // Update is called once per frame
  void Update()
  {
    OrientToPlanet();
    FormTribes();
    Build();
  }


  #region Meeple Actions

  private void OrientToPlanet()
  {
    Vector3 directionToPlanet = -1f * transform.position;
    directionToPlanet.Normalize();
    RaycastHit raycastHit;

    if(Physics.Raycast(transform.position, directionToPlanet, out raycastHit, 100f))
    {
      transform.up = raycastHit.normal;
      transform.position = raycastHit.point + transform.up * meepleGroundOffset;
    }
  }

  private void FormTribes()
  {
    //Try to create groups of 5 or so meeples

    //Use sphere cast all to find nearby meeples
    RaycastHit[] raycastHits = Physics.SphereCastAll(transform.position, sphereCastRadius, transform.forward, 30f);

    foreach(RaycastHit rayHit in raycastHits)
    {
      if(rayHit.collider.gameObject.layer == meepleLayer && rayHit.collider.gameObject != gameObject)
      {
        Debug.Log(gameObject.name + " is nearby to " + rayHit.collider.gameObject.name);
      }
    }

  }

  private void Build()
  {
    //If meeples are part of a group they should build, the more meeples present the faster they build
  }

  #endregion

}
