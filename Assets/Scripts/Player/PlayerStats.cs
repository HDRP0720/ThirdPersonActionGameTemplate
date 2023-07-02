using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
  public HealthBarUI healthBarUI;
  public StaminaBarUI staminaBarUI;

  private AnimatorHandler animatorHandler;

  private void Awake()
  {
    animatorHandler = GetComponent<AnimatorHandler>();
  }
  private void Start() 
  {
    // Set Health Stats
    maxHealth = SetMaxHealthFromHealthLevel();
    currentHealth = maxHealth;   

    healthBarUI.SetMaxHealth(maxHealth);
    healthBarUI.SetCurrentHealth(currentHealth);

    // Set Stamina Stats
    maxStamina = SetMaxStaminaFromStaminaLevel();
    currentStamina = maxStamina;

    staminaBarUI.SetMaxStamina(maxStamina);
    staminaBarUI.SetCurrentStamina(currentStamina);
  }

  private int SetMaxHealthFromHealthLevel()
  {
    maxHealth = healthLevel * 10;

    return maxHealth;
  }

  private int SetMaxStaminaFromStaminaLevel()
  {
    maxStamina = staminaLevel * 10;

    return maxStamina;
  }

  public void TakeDamage(int damage)
  {
    if(isDead) return;

    currentHealth -= damage;

    healthBarUI.SetCurrentHealth(currentHealth);

    animatorHandler.PlayTargetAnimation("Damage_01", true);

    if(currentHealth <= 0)
    {
      currentHealth = 0;
      animatorHandler.PlayTargetAnimation("Dead_01", true);
      isDead = true;
    }
  }

  public void TakeStamina(int amount)
  {
    currentStamina -= amount;

    staminaBarUI.SetCurrentStamina(currentStamina);
  }
}