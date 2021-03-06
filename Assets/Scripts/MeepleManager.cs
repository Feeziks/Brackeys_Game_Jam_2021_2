using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeepleManager : MonoBehaviour
{
  public Mesh meepleMesh;
  public GameObject planet;
  public Vector3 planetCenter;

  public float meepleSpawnTimer;
  private List<GameObject> spawnedMeeples;

  private List<Vector3> previousSpawnPoints;
  private float sphereLimiterRadius = 6f;

  public Material meepleMaterial;

  private GameManager gm;

  // Start is called before the first frame update
  void Start()
  {
    spawnedMeeples = new List<GameObject>();
    previousSpawnPoints = new List<Vector3>();

    gm = FindObjectOfType(typeof(GameManager)) as GameManager;

    StartCoroutine("SpawnMeeples");
  }

  private void OnDisable()
  {
    StopCoroutine("SpawnMeeples");
  }

  // Update is called once per frame
  void Update()
  {

  }

  private Vector3 GetNewMeeplePosition()
  {
    Vector3 spawnPosition;
    if (spawnedMeeples.Count < 6)
    {
      bool nearOtherBoid = false;
      do
      {
        nearOtherBoid = false;
        spawnPosition = planetCenter + Random.onUnitSphere * 210f;

        foreach (Vector3 v in previousSpawnPoints)
        {
          if (Vector3.Distance(spawnPosition, v) < (sphereLimiterRadius * 2f))
          {
            nearOtherBoid = true;
          }
        }
      } while (nearOtherBoid);
      
    }
    else
    {
      //Every nth meeple should be a part of another meeples group
      if(spawnedMeeples.Count % 4 == 0)
      {
        spawnPosition = previousSpawnPoints[(int)Random.Range(0f, spawnedMeeples.Count)];
      }
      //Otherwise spawn randomly, could be part of a group, could not be
      else
      {
        spawnPosition = planetCenter + Random.onUnitSphere * 210f;

        foreach (Vector3 v in previousSpawnPoints)
        {
          if (Vector3.Distance(spawnPosition, v) < (sphereLimiterRadius * 2f))
          {
            spawnPosition = v;
            break;
          }
        }
      }
    }

    previousSpawnPoints.Add(spawnPosition);
    return spawnPosition;
  }

  public void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(planetCenter, 2f);
  }

  #region Co-routines
  IEnumerator SpawnMeeples()
  {
    while(true)
    {
      if(gm.timeState != GameplayTimeStatus.paused)
      {
        GameObject newMeeple = new GameObject("Meeple_" + (spawnedMeeples.Count + 1).ToString());
        newMeeple.transform.parent = transform;
        newMeeple.transform.position = GetNewMeeplePosition();
        newMeeple.transform.localScale = new Vector3(500f, 500f, 500f);

        MeshFilter mf = newMeeple.AddComponent(typeof(MeshFilter)) as MeshFilter;
        mf.mesh = meepleMesh;
        MeshRenderer mr = newMeeple.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        mr.material = meepleMaterial;

        CapsuleCollider cc = newMeeple.AddComponent(typeof(CapsuleCollider)) as CapsuleCollider;

        Meeple m = newMeeple.AddComponent(typeof(Meeple)) as Meeple;
        m.planet = planet;
        m.planetCenter = planetCenter;

        spawnedMeeples.Add(newMeeple);
      }

      if(gm.timeState == GameplayTimeStatus.paused)
      {
        yield return null;
      }
      else
      {
        yield return new WaitForSeconds(meepleSpawnTimer / (float)gm.timeState);
      }
    }
    yield return null;
  }

  #endregion
}
