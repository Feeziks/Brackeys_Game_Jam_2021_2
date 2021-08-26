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
  private Tribe myTribe;

  private List<GameObject> neighbors;

  private float moveSpeed = 5f;
  private Vector3 targetLocation;
  private float targetPositionOffset = 2f;
  private float tribeStopDistance = 2f;
  private float minSeperationDistance = 2f;

  private Vector3 tribeCenter;
  private Vector3 seperation;

  public bool currentlyBuilding = false;


  // Start is called before the first frame update
  void Start()
  {
    meepleLayer = LayerMask.NameToLayer("Meeple");
    gameObject.layer = meepleLayer;
    neighbors = new List<GameObject>();

    targetLocation = transform.position;
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
    //Get Neighbors in the area
    RaycastHit[] temp = Physics.SphereCastAll(transform.position, sphereCastRadius, transform.forward, 1f);
    List<RaycastHit> raycastHits = new List<RaycastHit>(temp);
    raycastHits = raycastHits.OrderBy(rh => rh.point.magnitude).ToList();

    foreach(RaycastHit rayHit in raycastHits)
    {
      if(rayHit.collider.gameObject.layer == meepleLayer && rayHit.collider.gameObject != gameObject)
      {
        if(!neighbors.Contains(rayHit.collider.gameObject))
        {
          Meeple other = rayHit.collider.gameObject.GetComponent(typeof(Meeple)) as Meeple;
          if (!other.currentlyBuilding)
            neighbors.Add(rayHit.collider.gameObject);
        }
      }
    }




    /*
    if (myTribe != null && myTribe.joinable == false)
      return;

    //Use sphere cast all to find nearby meeples
    RaycastHit[] temp = Physics.SphereCastAll(transform.position, sphereCastRadius, transform.forward, 30f);
    List<RaycastHit> raycastHits = new List<RaycastHit>(temp);
    raycastHits = raycastHits.OrderBy(rh => rh.point.magnitude).ToList();

    foreach(RaycastHit rayHit in raycastHits)
    {
      if(rayHit.collider.gameObject.layer == meepleLayer && rayHit.collider.gameObject != gameObject)
      {
        Meeple otherMeeple = rayHit.collider.gameObject.GetComponent(typeof(Meeple)) as Meeple;

        if (otherMeeple.GetTribe() != null && otherMeeple.GetTribe().joinable && myTribe == null)
        {
          myTribe = otherMeeple.GetTribe();
          myTribe.JoinTribe(this);
        }
        else if(otherMeeple.GetTribe() == null && myTribe == null) //Neither of us have a tribe -> create a new one and attempt to add this neighbor to it
        {
          myTribe = new Tribe();
          if(otherMeeple.SetTribe(myTribe))
          {
            myTribe.JoinTribe(this);
            myTribe.JoinTribe(otherMeeple);
          }
          else //If adding fails scrap our new tribe. Guess we will figure this out later on
          {
            myTribe = null;
          }
        }
        else if(otherMeeple.GetTribe() != null && myTribe != null) //See if we can merge our tribes
        {
          if (otherMeeple.GetTribe().Mergable(myTribe))
          {
            myTribe = otherMeeple.GetTribe().MergeTribes(myTribe);
          }
        }
      }
    }

    //If we dont have a tribe after all that move around so we can hopefully find one next time
    if(myTribe == null)
    {
      transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
    */
  }

  public Tribe GetTribe()
  {
    return myTribe;
  }

  public bool SetTribe(Tribe newTribe)
  {
    if (myTribe == null)
    {
      myTribe = newTribe;
      return true;
    }
    else
      return false;
  }

  private void CoalesceTribes()
  {
    if (currentlyBuilding) //Dont move if we are building
      return;

    tribeCenter = Vector3.zero;
    foreach(GameObject go in neighbors)
    {
      tribeCenter += go.transform.position;
    }
    tribeCenter /= neighbors.Count;
    tribeCenter = PushTribeCenterToSphereSurface(tribeCenter);

    if (Vector3.Distance(transform.position, tribeCenter) > tribeStopDistance)
      transform.position += tribeCenter.normalized * moveSpeed * Time.deltaTime;

    //Moving in random direction seems to push everyone to a pole.
    //Try to pick a random target destination and if we end up joining a group and building then we stop before getting there
    if(Mathf.Abs((transform.position - targetLocation).magnitude) < targetPositionOffset)
    {
      //Pick a new target location and a direction to walk in to get there
      targetLocation = Random.onUnitSphere * planet.transform.localScale.x;
      transform.LookAt(targetLocation);
    }
    else
    {
      transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
    

    seperation = Vector3.zero;
    foreach (GameObject go in neighbors)
    {
      if(Vector3.Distance(transform.position, go.transform.position) < minSeperationDistance)
      {
        transform.forward = Vector3.Cross(transform.position, go.transform.position).normalized;
        seperation = transform.forward * 20f;
      }
    }

    /*
    if (myTribe == null)
      return;

    //Move towards the center of the tribes - but must remain on the surface of the sphere
    //TODO: Better way to determine the center of the tribe that is a point on the surface
    
    Vector3 tribeCenter = Vector3.zero;
    foreach(Meeple m in myTribe.GetList())
    {
      GameObject go = m.gameObject;
      tribeCenter += go.transform.position;
    }

    tribeCenter /= myTribe.GetList().Count;
    tribeCenter = PushTribeCenterToSphereSurface(tribeCenter);

    //transform.LookAt(tribeCenter);

    if(Vector3.Distance(transform.position, tribeCenter) > tribeStopDistance)
      transform.position += tribeCenter.normalized * moveSpeed * Time.deltaTime;
    */
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
    if(neighbors.Count >= minTribeSizeToBuild)
    {
      //Do some building
      currentlyBuilding = true;

    }
  }

  #endregion

  public void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.red;
    /*
    if (myTribe != null)
    {
      foreach (Meeple m in myTribe.GetList())
      {
        GameObject go = m.gameObject;
        Gizmos.DrawLine(transform.position, go.transform.position);
      }
    }*/

    foreach(GameObject go in neighbors)
    {
      Gizmos.DrawLine(transform.position, go.transform.position);
    }

    Gizmos.color = Color.white;
    Gizmos.DrawLine(transform.position, tribeCenter);

    //TODO: Seperation stays on sphere surface
    Gizmos.color = Color.green;
    Gizmos.DrawLine(transform.position, seperation);
  }

}
