using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
  public WeaponItem attackingWeapon;

  private WeaponHolderSlot leftHandSlot;
  private WeaponHolderSlot rightHandSlot;

  private DamageCollider leftHandDamageCollider;
  private DamageCollider rightHandDamageCollider;  

  private QuickSlotsUI quickSlotsUI;
  private PlayerStats playerStats;

  private void Awake() 
  {
    quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
    playerStats = GetComponent<PlayerStats>();
    
    WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
    foreach(WeaponHolderSlot weaponSlot in weaponHolderSlots)
    {
      if(weaponSlot.isLeftHandSlot)
        leftHandSlot = weaponSlot;
      else if(weaponSlot.isRightHandSlot)
        rightHandSlot = weaponSlot;      
    }
  }

  public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
  {
    if(isLeft)
    {
      leftHandSlot.LoadWeaponModel(weaponItem);
      LoadLeftWeaponDamageCollider();
      quickSlotsUI.UpdateWeaponQuickSlotUI(true, weaponItem);
    }
    else
    {
      rightHandSlot.LoadWeaponModel(weaponItem);
      LoadRightWeaponDamageCollider();
      quickSlotsUI.UpdateWeaponQuickSlotUI(false, weaponItem);
    }
  }

  #region Handle Weapon's Damage Collider
  private void LoadLeftWeaponDamageCollider()
  {
    leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
  }

  private void LoadRightWeaponDamageCollider()
  {
    rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
  }

  public void OpenLeftHandDamageCollider()
  {
    leftHandDamageCollider.EnableDamageCollider();
  }

  public void OpenRightHandDamageCollider()
  {
    rightHandDamageCollider.EnableDamageCollider();
  }

  public void CloseLeftHandDamageCollider()
  {
    leftHandDamageCollider.DisableDamageCollider();
  }

  public void CloseRightHandDamageCollider()
  {
    rightHandDamageCollider.DisableDamageCollider();
  }
  #endregion

  #region Handle Weapon's Stamina Drainage
  public void DrainStaminaLightAttack()
  {
    playerStats.TakeStamina(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
  }

  public void DrainStaminaHeavyAttack()
  {
    playerStats.TakeStamina(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
  }
  #endregion


}