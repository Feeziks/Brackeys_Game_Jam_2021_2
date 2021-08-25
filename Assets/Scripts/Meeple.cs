using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Meeple : MonoBehaviour
{
  public GameObject planet;

  private float sphereCastRadius = 5f;
  private float meepleGroundOffset = 1f;
  private LayerMask meepleLayer;

  private int minTribeSizeToBuild = 5;
  private int tribeSizeLimit = 10;
  private List<GameObject> myTribe;

  private float moveSpeed = 5f;
  private float tribeStopDistance = 0.25f;

  // Start is called before the first frame update
  void Start()
  {
    meepleLayer = LayerMask.NameToLayer("Meeple");
    gameObject.layer = meepleLayer;

    myTribe = new List<GameObject>();
  }

  // Update is called once per frame
  void Update()
  {
    OrientToPlanet();
    FormTribes();
    CoalesceTribes();
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
    if (myTribe.Count >= tribeSizeLimit)
      return;

    //Use sphere cast all to find nearby meeples
    RaycastHit[] temp = Physics.SphereCastAll(transform.position, sphereCastRadius, transform.forward, 30f);
    List<RaycastHit> raycastHits = new List<RaycastHit>(temp);
    raycastHits = raycastHits.OrderBy(rh => rh.point.magnitude).ToList();

    foreach(RaycastHit rayHit in raycastHits)
    {
      if(rayHit.collider.gameObject.layer == meepleLayer && rayHit.collider.gameObject != gameObject)
      {
        if (myTribe.Count < tribeSizeLimit)
        {
          myTribe.Add(rayHit.collider.gameObject);
        }
      }
    }

  }

  private void CoalesceTribes()
  {
    if (myTribe.Count == 0)
      return;

    //Move towards the center of the tribes - but must remain on the surface of the sphere
    //TODO: Better way to determine the center of the tribe that is a point on the surface
    Vector3 tribeCenter = Vector3.zero;
    foreach(GameObject go in myTribe)
    {
      tribeCenter += go.transform.position;
    }

    tribeCenter /= myTribe.Count;
    tribeCenter = PushTribeCenterToSphereSurface(tribeCenter);

    //transform.LookAt(tribeCenter);

    if(Vector3.Distance(transform.position, tribeCenter) > tribeStopDistance)
      transform.position += tribeCenter.normalized * moveSpeed * Time.deltaTime;


  }

  public Vector3 PushTribeCenterToSphereSurface(Vector3 center)
  {
    Vector3 ret = Vector3.zero;

    //Closest point on the spheres surface is the vector from sphere center to point normalized and multiplied by sphere radius
    ret = (center - Vector3.zero).normalized;
    ret *= planet.transform.localScale.x;

    return ret;
  }

  private void Build()
  {
    //If meeples are part of a group they should build, the more meeples present the faster they build
    if(myTribe.Count != 0)
    {
      //Do some building
    }
  }

  #endregion

  public void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.red;

    foreach(GameObject go in myTribe)
    {
      Gizmos.DrawLine(transform.position, go.transform.position);
    }
  }

}
