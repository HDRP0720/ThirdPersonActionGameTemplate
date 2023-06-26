using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
  public int healthLevel = 10;
  public int maxHealth;
  public int currentHealth;

  public HealthBarUI healthBarUI;

  private AnimatorHandler animatorHandler;

  private void Awake()
  {
    animatorHandler = GetComponent<AnimatorHandler>();
  }
  private void Start() 
  {
    maxHealth = SetMaxHealthFromHealthLevel();
    currentHealth = maxHealth;

    healthBarUI.SetMaxHealth(maxHealth);
  }

  private int SetMaxHealthFromHealthLevel()
  {
    maxHealth = healthLevel * 10;

    return maxHealth;
  }

  public void TakeDamage(int damage)
  {
    currentHealth -= damage;

    healthBarUI.SetCurrentHealth(currentHealth);

    animatorHandler.PlayTargetAnimation("Damage_01", true);

    if(currentHealth <= 0)
    {
      currentHealth = 0;
      animatorHandler.PlayTargetAnimation("Dead_01", true);
    }
  }
}