using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Make New Weapon Item")]
public class WeaponItem : Item
{
  public GameObject weaponPrefab;
  public bool isUnarmed;

  [Header("Idle Animations")]
  public string TH_Idle;

  [Header("One Handed Attack Animations")]
  public string OH_Light_Attack_1;
  public string OH_Light_Attack_2;

  [Space] public string OH_Heavy_Attack_1;

  [Header("Stamina Costs")]
  public int baseStamina;
  public float lightAttackMultiplier;
  public float heavyAttackMultiplier;
}