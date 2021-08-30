using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Meeple : MonoBehaviour
{
  public GameObject planet;
  public Vector3 planetCenter;

  private Vector3 sphereLimiterCenter;
  private float sphereLimiterRadius = 6f;

  private float meepleGroundOffset = 1f;
  private LayerMask meepleLayer;

  private int minTribeSizeToBuild = 5;
  private List<GameObject> neighbors;

  private float movementTimer = 5f;
  private Vector3 targetLocation;
  private float lastMoveTime;
  private float moveSpeed = 1f;

  public bool currentlyBuilding = false;

  public Dictionary<int, float> numNeighborsToOrder;
  public float orderTimer = 5f;
  public float lastOrderTime;

  private GameManager gm;

  private float unableToBuildTimer;

  // Start is called before the first frame update
  void Start()
  {
    meepleLayer = LayerMask.NameToLayer("Meeple");
    gameObject.layer = meepleLayer;
    neighbors = new List<GameObject>();

    targetLocation = transform.position;
    sphereLimiterCenter = transform.position;
    lastMoveTime = Time.realtimeSinceStartup;

    numNeighborsToOrder = new Dictionary<int, float>();
    numNeighborsToOrder[5] = 1;
    numNeighborsToOrder[6] = 1;
    numNeighborsToOrder[7] = 2;
    numNeighborsToOrder[8] = 2;
    numNeighborsToOrder[9] = 3;
    numNeighborsToOrder[10] = 4;

    gm = FindObjectOfType(typeof(GameManager)) as GameManager;
  }

  // Update is called once per frame
  void Update()
  {
    OrientToPlanet();
    FormTribes();
    Movement();
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
      transform.forward = raycastHit.normal;
      transform.position = raycastHit.point + transform.up * meepleGroundOffset;
    }
  }

  private void FormTribes()
  {
    //Check if any other Meeples have spawned inside our sphere
    RaycastHit[] hits = Physics.SphereCastAll(sphereLimiterCenter, sphereLimiterRadius, transform.forward, 0.1f);

    neighbors.Clear();

    foreach(RaycastHit hit in hits)
    {
      if(hit.collider.gameObject != gameObject && hit.collider.gameObject != planet)
      {
        neighbors.Add(hit.collider.gameObject);
      }
    }
  }

  private void Movement()
  {
    //Pick another spot within the sphere limiter to move to randomly every moveTimer seconds
    if (Time.realtimeSinceStartup - movementTimer > (lastMoveTime / (float)gm.timeState))
    {
      targetLocation = sphereLimiterCenter + Random.insideUnitSphere * sphereLimiterRadius;
      targetLocation = PlacePointOnPlanetSurface(targetLocation) + transform.up * meepleGroundOffset;
      transform.LookAt(targetLocation);
      lastMoveTime = Time.realtimeSinceStartup;
    }

    if(transform.position != targetLocation && gm.timeState != GameplayTimeStatus.paused)
    {
      Vector3 direction = (targetLocation - transform.position).normalized;
      transform.position += direction * moveSpeed * Time.deltaTime * (float)gm.timeState;
    }   
  }

  public Vector3 PlacePointOnPlanetSurface(Vector3 center)
  {
    //Closest point on the spheres surface is the vector from sphere center to point normalized and multiplied by sphere radius
    Vector3 ret = (center - planetCenter).normalized;
    ret *= planet.transform.localScale.x / 2f;

    return ret;
  }

  private void Build()
  {
    //If meeples are part of a group they should build, the more meeples present the faster they build
    if(neighbors.Count >= minTribeSizeToBuild && unableToBuildTimer <= 0f && gm.timeState != GameplayTimeStatus.paused)
    {
      //Do some building
      currentlyBuilding = true;
      if(Time.realtimeSinceStartup - orderTimer > lastOrderTime)
      {
        float orderToAdd = 0f;
        if(neighbors.Count > 10)
        {
          orderToAdd = numNeighborsToOrder[10];
        }
        else
        {
          orderToAdd = numNeighborsToOrder[neighbors.Count];
        }

        gm.UpdateChaosOrder(0f, orderToAdd);
      }
    }
  }

  #endregion

  public void AbilityEffect(AbilityAffectMeepleType data)
  {
    unableToBuildTimer = data.duration;
    gm.UpdateChaosOrder(data.chaosInflicted, 0f);
    if(Random.Range(0, 100) < data.killChance)
    {
      Destroy(gameObject, data.duration);
    }
    StartCoroutine("UnableToBuildCounter");
  }

  private IEnumerator UnableToBuildCounter()
  {
    while(unableToBuildTimer > 0f)
    {
      unableToBuildTimer -= Time.deltaTime * (float)gm.timeState;
      yield return null;
    }

    unableToBuildTimer = 0f;
  }

  public void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.red;
    foreach(GameObject go in neighbors)
    {
      Gizmos.DrawLine(transform.position, go.transform.position);
    }

    Gizmos.color = Color.blue;
    Gizmos.DrawWireSphere(sphereLimiterCenter, sphereLimiterRadius);

    Gizmos.color = Color.black;
    Gizmos.DrawLine(transform.position, targetLocation);
  }

}
