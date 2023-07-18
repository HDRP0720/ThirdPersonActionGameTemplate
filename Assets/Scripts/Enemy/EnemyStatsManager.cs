using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsManager : CharacterStatsManager
{
  [Header("# Enemy Health UI")]
  public EnemyHealthBarUI enemyHealthBarUI;

  public bool isBoss = false;

  private EnemyBossManager enemyBossManager;
  private EnemyAnimatorManager enemyAnimatorManager;

  private void Awake()
  {
    enemyBossManager = GetComponent<EnemyBossManager>();
    enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();

    maxHealth = SetMaxHealthFromHealthLevel();
    currentHealth = maxHealth;
  }
  private void Start() 
  {
    if(!isBoss)
      enemyHealthBarUI.SetMaxHealth(maxHealth);
  }

  private int SetMaxHealthFromHealthLevel()
  {
    maxHealth = healthLevel * 10;

    return maxHealth;
  }

  public override void TakeDamageWithoutAnimation(int damage)
  {
    base.TakeDamageWithoutAnimation(damage);

    if (!isBoss)
      enemyHealthBarUI.SetHealth(currentHealth);
    else if (isBoss && enemyBossManager != null)
      enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
  }

  public void BreakGuard()
  {
    enemyAnimatorManager.PlayTargetAnimation("Break Guard", true);
  }

  public override void TakeDamage(int damage, string damageAnimation = "Damage_01")
  {
    base.TakeDamage(damage, damageAnimation);
    
    if(isDead) return;   
    
    currentHealth -= damage;

    if(!isBoss)    
      enemyHealthBarUI.SetHealth(currentHealth);    
    else if(isBoss && enemyBossManager != null)    
      enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
    
    enemyAnimatorManager.PlayTargetAnimation("Damage_01", true);

    if(currentHealth <= 0)
      HandleDeath();
  }
  private void HandleDeath()
  {
    currentHealth = 0;
    enemyAnimatorManager.PlayTargetAnimation("Dead_01", true);
    isDead = true;
  }
}