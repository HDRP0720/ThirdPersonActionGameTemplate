using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
  public int soulsAwardedOnDeath = 50;

  private EnemyAnimatorManager enemyAnimatorManager;

  private void Awake()
  {    
    enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
  }
  private void Start() 
  {
    maxHealth = SetMaxHealthFromHealthLevel();
    currentHealth = maxHealth; 
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