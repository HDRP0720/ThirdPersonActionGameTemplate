using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
  private Animator animator;

  private void Awake()
  {
    animator = GetComponent<Animator>();
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

  public void TakeDamage(int damage)
  {
    if(isDead) return;
    
    currentHealth -= damage;
    animator.Play("Damage_01");

    if(currentHealth <= 0)
    {
      currentHealth = 0;
      animator.Play("Dead_01");
      isDead = true;
    }
  }
}