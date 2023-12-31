using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager
{
  public float staminaRegenerationAmount = 1;
  public float staminaRegenTimer = 0;

  private PlayerManager playerManager;
  private PlayerAnimatorManager playerAnimatorManager;

  private HealthBarUI healthBarUI;
  private ManaBarUI manaBarUI;
  private StaminaBarUI staminaBarUI;

  private void Awake()
  {
    playerManager = GetComponent<PlayerManager>();
    playerAnimatorManager = GetComponent<PlayerAnimatorManager>();

    healthBarUI = FindObjectOfType<HealthBarUI>();
    manaBarUI = FindObjectOfType<ManaBarUI>();
    staminaBarUI = FindObjectOfType<StaminaBarUI>();
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

  public override void HandlePoiseResetTimer()
  {
    if (poiseResetTimer > 0)
    {
      poiseResetTimer -= Time.deltaTime;
    }
    else if(poiseResetTimer <= 0 && !playerManager.isInteracting)
    {
      totalPoiseDefence = armorPoiseBonus;
    }
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

  public override void TakeDamageWithoutAnimation(int damage)
  {
    base.TakeDamageWithoutAnimation(damage);
    healthBarUI.SetCurrentHealth(currentHealth);
  }

  public override void TakeDamage(int damage, string damageAnimation ="Damage_01")
  {
    base.TakeDamage(damage, damageAnimation);    

    if(playerManager.isInvulnerable) return;  

    healthBarUI.SetCurrentHealth(currentHealth);

    playerAnimatorManager.PlayTargetAnimation(damageAnimation, true);

    if(currentHealth <= 0)
    {
      currentHealth = 0;
      playerAnimatorManager.PlayTargetAnimation("Dead_01", true);
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