using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossManager : MonoBehaviour
{
  // TODO: Handle Switching Phase
  // TODO: Handle Attack Pattern
  public string bossName;  

  private BossHealthBarUI bossHealthBarUI;
  private EnemyStats enemyStats;

  private void Awake() 
  {
    bossHealthBarUI = FindObjectOfType<BossHealthBarUI>();
    enemyStats = GetComponent<EnemyStats>();
  }
  private void Start() 
  {
    bossHealthBarUI.SetBossName(bossName);
    bossHealthBarUI.SetBossMaxHealth(enemyStats.maxHealth);
  }

  public void UpdateBossHealthBar(int currentHealth)
  {
    bossHealthBarUI.SetBossCurrentHealth(currentHealth);
  }
}