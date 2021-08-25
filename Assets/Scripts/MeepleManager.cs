using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeepleManager : MonoBehaviour
{
  public Mesh meepleMesh;
  public GameObject planet;

  public float meepleSpawnTimer;
  private List<GameObject> spawnedMeeples;

  private GameManager gm;

  // Start is called before the first frame update
  void Start()
  {
    spawnedMeeples = new List<GameObject>();

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
    //Return a random position on the surface of the sphere? Try to be close to previous meeples? Also dont place them into the ocean 
    Vector3 randomPointOnSphereSurface = Random.onUnitSphere * planet.transform.localScale.x;


    return randomPointOnSphereSurface;
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

        MeshFilter mf = newMeeple.AddComponent(typeof(MeshFilter)) as MeshFilter;
        mf.mesh = meepleMesh;
        MeshRenderer mr = newMeeple.AddComponent(typeof(MeshRenderer)) as MeshRenderer;

        CapsuleCollider cc = newMeeple.AddComponent(typeof(CapsuleCollider)) as CapsuleCollider;

        Meeple m = newMeeple.AddComponent(typeof(Meeple)) as Meeple;
        m.planet = planet;

        spawnedMeeples.Add(newMeeple);
        yield return new WaitForSeconds(meepleSpawnTimer / (float)gm.timeState);
      }
    }
    yield return null;
  }

  #endregion
}
