using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TPA/Items/Make New Weapon Item")]
public class WeaponItem : Item
{
  public GameObject weaponPrefab;
  public bool isUnarmed;

  [Header("# Weapon Damage")]
  public int baseDamage = 25;
  public int criticalDamageMultiplier = 4;

  [Header("# Idle Animations")]
  public string TH_Idle;

  [Header("# One Handed Attack Animations")]
  public string OH_Light_Attack_1;
  public string OH_Light_Attack_2;

  [Space] public string OH_Heavy_Attack_1;

  [Header("# Two Handed Attack Animations")]
  public string TH_Light_Attack_1;
  public string TH_Light_Attack_2;

  [Space] public string TH_Heavy_Attack_1;

  [Header("Weapon Art for parry")]
  public string parry_art;

  [Header("# Stamina Costs")]
  public int baseStamina;
  public float lightAttackMultiplier;
  public float heavyAttackMultiplier;

  [Header("# Weapon Type")]
  public bool isSpellCaster;
  public bool isFaithCaster;
  public bool isPyroCaster;
  public bool isMeleeWeapon;
  public bool isShield;
}