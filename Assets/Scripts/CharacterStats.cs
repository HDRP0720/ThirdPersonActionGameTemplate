using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
  [Header("# Health Parameters")]
  public int healthLevel = 10;
  public int maxHealth;
  public int currentHealth;

  [Header("# Mana Parameters")]
  public int manaLevel = 10;
  public float maxMana;
  public float currentMana;

  [Header("# Stamina Parameters")]
  public int staminaLevel = 10;
  public float maxStamina;
  public float currentStamina;

  [Header("# Armor Absorptions")]
  public float damageAbsorptionHead;
  public float damageAbsorptionBody;
  public float damageAbsorptionHand;
  public float damageAbsorptionLegs;

  // TODO: Magic Absorption
  // TODO: Fire Absorption
  // TODO: Water Absorption
  // TODO: Lightning Absorption
  // TODO: Darkness Absorption

  [Space] public int soulCount = 0;

  public bool isDead;

  public virtual void TakeDamage(int damage, string damageAnimation = "Damage_01") 
  {
    float totalDamageAbsorptionRate = 1 - 
      (1 - damageAbsorptionHead / 100) * 
      (1 - damageAbsorptionBody / 100) *
      (1 - damageAbsorptionHand / 100) *
      (1 - damageAbsorptionLegs / 100);

    damage = Mathf.RoundToInt(damage - (damage * totalDamageAbsorptionRate));    
    Debug.Log($"Total Damage Absorption: {totalDamageAbsorptionRate}"); // Check

    int finalDamage = damage; // + fireDamage + magicDamage +....
    Debug.Log($"Final Damage : {finalDamage}");

    currentHealth -= finalDamage;
    if(currentHealth <= 0)
    {
      currentHealth = 0;
      isDead = true;
    }
  }
}