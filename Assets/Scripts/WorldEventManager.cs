using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEventManager : MonoBehaviour
{
  public BossHealthBarUI bossHealthBarUI;
  public EnemyBossManager bossManager;
  public List<FogWall> fogWalls;

  public bool bossFightIsActive;
  public bool bossHasBeenAwakened;
  public bool bossHasBeenDefeated;

  private void Awake() 
  {
    bossHealthBarUI = FindObjectOfType<BossHealthBarUI>();
  }

  public void ActivateBossFight()
  {
    bossFightIsActive = true;
    bossHasBeenAwakened = true;
    bossHealthBarUI.SetBossHealthBarToActive();
 
    foreach (var fogWall in fogWalls)
    {
      fogWall.ActivateFogWall();
    }
  }

  public void BossHasBeenDefeated()
  {
    bossHasBeenDefeated = true;
    bossFightIsActive = false;

    foreach (var fogWall in fogWalls)
    {
      fogWall.DeactivateFogWall();
    }
  }
}
