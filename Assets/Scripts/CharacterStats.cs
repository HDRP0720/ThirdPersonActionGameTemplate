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

  [Space] public int soulCount = 0;

  public bool isDead;

  public virtual void TakeDamage(int damage, string damageAnimation = "Damage_01") 
  {

  }
}