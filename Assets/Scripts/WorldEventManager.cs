using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEventManager : MonoBehaviour
{
  public BossHealthBarUI bossHealthBarUI;
  public EnemyBossManager bossManager;

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

    // TODO: Activate Fog Wall
  }

  public void BossHasBeenDefeated()
  {
    bossHasBeenDefeated = true;
    bossFightIsActive = false;

    // TODO: Deactivate Fog Wall
  }
}
