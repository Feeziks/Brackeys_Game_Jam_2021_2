using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tribe
{

  private List<Meeple> meeples;

  private int minTribeSizeToBuild = 5;
  private int tribeSizeLimit = 10;

  public bool readyToBuild;
  public bool joinable;
  public bool full;


  public Tribe()
  {
    meeples = new List<Meeple>();
    readyToBuild = false;
    joinable = true;
    full = false;
  }

  public bool JoinTribe(Meeple m)
  {
    if(meeples.Count < tribeSizeLimit && joinable)
    {
      meeples.Add(m);

      if (meeples.Count == tribeSizeLimit)
        joinable = false;

      if (meeples.Count >= minTribeSizeToBuild)
        readyToBuild = true;
      else
        readyToBuild = false;

      return true;
    }
    return false;
  }

  public bool LeaveTribe(Meeple m)
  {
    if(meeples.Contains(m))
    {
      meeples.Remove(m);

      joinable = true;

      if (meeples.Count >= minTribeSizeToBuild)
        readyToBuild = true;
      else
        readyToBuild = false;

      return true;
    }
    return false;
  }

  public bool Mergable(Tribe otherTribe)
  {
    if (meeples.Count + otherTribe.GetList().Count <= tribeSizeLimit)
      return true;
    else
      return false;
  }

  public Tribe MergeTribes(Tribe otherTribe) //TODO: This doesnt work. Probably a sympton of this bad idea
  {
    if (meeples.Count + otherTribe.GetList().Count > tribeSizeLimit)
      return null;
    
    //Larget tribe is the "retained" tribe
    if(otherTribe.GetList().Count > meeples.Count)
    {
      foreach (Meeple m in meeples)
      {
        otherTribe.JoinTribe(m);
      }

      return otherTribe;
    }
    else
    {
      foreach(Meeple m in otherTribe.GetList())
      {
        JoinTribe(m);
      }

      return this;
    }
  }

  public List<Meeple> GetList()
  {
    return meeples;
  }

}
