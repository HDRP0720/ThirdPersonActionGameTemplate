using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
{
  public WeaponItem rightHandWeapon;
  public WeaponItem leftHandWeapon;

  private EnemyStatsManager enemyStatsManager;
  private EnemyVFXManager enemyVFXManager;

  private void Awake()
  {
    enemyStatsManager = GetComponent<EnemyStatsManager>();
    enemyVFXManager = GetComponent<EnemyVFXManager>();
    
    LoadWeaponHolderSlots();
  }
  private void Start() 
  {
    LoadWeaponsOnBothHands();
  }

  private void LoadWeaponHolderSlots()
  {
    WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
    foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
    {
      if (weaponSlot.isLeftHandSlot)
        leftHandSlot = weaponSlot;
      else if (weaponSlot.isRightHandSlot)
        rightHandSlot = weaponSlot;
    }
  }

  public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
  {
    if(isLeft)
    {
      leftHandSlot.currentWeapon = weaponItem;
      leftHandSlot.LoadWeaponModel(weaponItem);
      LoadWeaponDamageCollider(true);
    }
    else
    {
      rightHandSlot.currentWeapon = weaponItem;
      rightHandSlot.LoadWeaponModel(weaponItem);
      LoadWeaponDamageCollider(false);
    }
  }

  public void LoadWeaponsOnBothHands()
  {
    if(rightHandWeapon != null)
      LoadWeaponOnSlot(rightHandWeapon, false);

    if(leftHandWeapon != null)
      LoadWeaponOnSlot(leftHandWeapon, true);
  }

  public void LoadWeaponDamageCollider(bool isLeft)
  {
    if(isLeft)
    {
      leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
      leftHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
      enemyVFXManager.leftWeaponVFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponVFX>();
    }
    else
    {
      rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
      rightHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
      enemyVFXManager.rightWeaponVFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponVFX>();
    }
  }

  public void OpenDamageCollider()
  {
    rightHandDamageCollider.EnableDamageCollider();
  }

  public void CloseDamageCollider()
  {
    rightHandDamageCollider.DisableDamageCollider();
  }

  public void DrainStaminaLightAttack()
  {

  }

  public void DrainStaminaHeavyAttack()
  {

  }

  public void EnableCombo()
  {
    // animator.SetBool("canDoCombo", true);
  }

  public void DisableCombo()
  {
    // animator.SetBool("canDoCombo", false);
  }

  public void GrantAttackPoiseBonus()
  {
    enemyStatsManager.totalPoiseDefence = enemyStatsManager.totalPoiseDefence + enemyStatsManager.offensivePoiseBonus;
  }

  public void ResetAttackPoiseBonus()
  {
    enemyStatsManager.totalPoiseDefence = enemyStatsManager.armorPoiseBonus;
  }
}