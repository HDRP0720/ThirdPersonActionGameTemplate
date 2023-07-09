using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
  [Header("# Kill Reward for Player")]
  public int soulsAwardedOnDeath = 50;

  [Header("# Enemy Health UI")]
  public EnemyHealthBarUI enemyHealthBarUI;

  private EnemyAnimatorManager enemyAnimatorManager;

  private void Awake()
  {    
    enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
  }
  private void Start() 
  {
    maxHealth = SetMaxHealthFromHealthLevel();
    currentHealth = maxHealth; 

    enemyHealthBarUI.SetMaxHealth(maxHealth);
  }

  private int SetMaxHealthFromHealthLevel()
  {
    maxHealth = healthLevel * 10;

    return maxHealth;
  }

  public void TakeDamageWithoutAnimation(int damage)
  {
    if (isDead) return;

    currentHealth -= damage;

    enemyHealthBarUI.SetHealth(currentHealth);

    if (currentHealth <= 0)
    {
      currentHealth = 0;
      isDead = true;
    }
  }

  public void TakeDamage(int damage)
  {
    if(isDead) return;
    
    currentHealth -= damage;

    enemyHealthBarUI.SetHealth(currentHealth);
    
    enemyAnimatorManager.PlayTargetAnimation("Damage_01", true);

    if(currentHealth <= 0)
    {
      HandleDeath();
    }
  }
  private void HandleDeath()
  {
    currentHealth = 0;
    enemyAnimatorManager.PlayTargetAnimation("Dead_01", true);
    isDead = true;
  }
}