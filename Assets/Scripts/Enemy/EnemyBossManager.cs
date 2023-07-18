using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossManager : MonoBehaviour
{
  // TODO: Handle Switching Phase
  // TODO: Handle Attack Pattern
  public string bossName;

  [Header("# Second Phase VFX")]
  public GameObject particleFX;


  private BossHealthBarUI bossHealthBarUI;
  private EnemyStatsManager enemyStats;
  private EnemyAnimatorManager enemyAnimatorManager;
  private BossCombatStanceState bossCombatStanceState;

  private void Awake() 
  {
    bossHealthBarUI = FindObjectOfType<BossHealthBarUI>();
    enemyStats = GetComponent<EnemyStatsManager>();
    enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
    bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
  }
  private void Start() 
  {
    bossHealthBarUI.SetBossName(bossName);
    bossHealthBarUI.SetBossMaxHealth(enemyStats.maxHealth);
  }

  public void UpdateBossHealthBar(int currentHealth, int maxHealth)
  {
    bossHealthBarUI.SetBossCurrentHealth(currentHealth);

    if (currentHealth <= maxHealth / 2 && !bossCombatStanceState.hasPhaseShifted)
    {
      bossCombatStanceState.hasPhaseShifted = true;
      ShiftToSecondPhase();
    }
  }

  public void ShiftToSecondPhase()
  {
    enemyAnimatorManager.animator.SetBool("isInvulnerable", true);
    enemyAnimatorManager.animator.SetBool("isPhaseShifting", true);

    enemyAnimatorManager.PlayTargetAnimation("Phase Shift", true);
  
    bossCombatStanceState.hasPhaseShifted = true;
  }

}