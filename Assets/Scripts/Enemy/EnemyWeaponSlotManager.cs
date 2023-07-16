using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponSlotManager : MonoBehaviour
{
  public WeaponItem rightHandWeapon;
  public WeaponItem leftHandWeapon;

  WeaponHolderSlot leftHandSlot;
  WeaponHolderSlot rightHandSlot;

  DamageCollider leftHandDamageCollider;
  DamageCollider rightHandDamageCollider;

  private EnemyStats enemyStats;

  private void Awake() 
  {
    enemyStats = GetComponentInParent<EnemyStats>();
    
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
    }
    else
    {
      rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
      rightHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
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
    enemyStats.totalPoiseDefence = enemyStats.totalPoiseDefence + enemyStats.offensivePoiseBonus;
  }

  public void ResetAttackPoiseBonus()
  {
    enemyStats.totalPoiseDefence = enemyStats.armorPoiseBonus;
  }
}