using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
  public int healthLevel = 10;
  public int maxHealth;
  public int currentHealth;

  public int staminaLevel = 10;
  public int maxStamina;
  public int currentStamina;

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
    currentHealth -= damage;

    healthBarUI.SetCurrentHealth(currentHealth);

    animatorHandler.PlayTargetAnimation("Damage_01", true);

    if(currentHealth <= 0)
    {
      currentHealth = 0;
      animatorHandler.PlayTargetAnimation("Dead_01", true);
    }
  }

  public void TakeStamina(int amount)
  {
    currentStamina -= amount;

    staminaBarUI.SetCurrentStamina(currentStamina);
  }
}