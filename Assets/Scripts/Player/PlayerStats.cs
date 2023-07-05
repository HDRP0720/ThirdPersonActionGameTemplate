using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
  public HealthBarUI healthBarUI;
  public ManaBarUI manaBarUI;
  public StaminaBarUI staminaBarUI;

  public float staminaRegenerationAmount = 1;
  public float staminaRegenTimer = 0;

  private PlayerManager playerManager;
  private PlayerAnimatorManager animatorHandler;

  private void Awake()
  {
    playerManager = GetComponent<PlayerManager>();
    animatorHandler = GetComponentInChildren<PlayerAnimatorManager>();
  }
  private void Start() 
  {
    // Set Health Stats
    maxHealth = SetMaxHealthFromHealthLevel();
    currentHealth = maxHealth;   

    healthBarUI.SetMaxHealth(maxHealth);
    healthBarUI.SetCurrentHealth(currentHealth);

    // Set Mana Stats
    maxMana = SetMaxManaFromManaLevel();
    currentMana = maxMana;

    manaBarUI.SetMaxMana(maxMana);
    manaBarUI.SetCurrentMana(currentMana);

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

  private float SetMaxManaFromManaLevel()
  {
    maxMana = manaLevel * 10;

    return maxMana;
  }

  private float SetMaxStaminaFromStaminaLevel()
  {
    maxStamina = staminaLevel * 10;

    return maxStamina;
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

    if(playerManager.isInvulnerable) return;

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

  public void RegenerateStamina()
  {
    if(playerManager.isInteracting)
    {
      staminaRegenTimer = 0;
    }
    else
    {
      staminaRegenTimer += Time.deltaTime;
      
      if (currentStamina < maxStamina && staminaRegenTimer > 1f)
      {
        currentStamina += staminaRegenerationAmount * Time.deltaTime;
        staminaBarUI.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
      }
    }   
  }

  public void DeductMana(int manaCost)
  {
    currentMana -= manaCost;

    if(currentMana < 0)
      currentMana = 0;

    manaBarUI.SetCurrentMana(currentMana);
  }

  public void HealPlayer(int healAmount)
  {
    currentHealth += healAmount;

    if(currentHealth > maxHealth)
      currentHealth = maxHealth;

    healthBarUI.SetCurrentHealth(currentHealth);
  }

  public void AddSouls(int souls)
  {
    soulCount += souls;
  }
}