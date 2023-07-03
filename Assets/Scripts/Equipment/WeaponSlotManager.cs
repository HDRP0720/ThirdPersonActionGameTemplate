using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
  public WeaponItem attackingWeapon;

  private WeaponHolderSlot leftHandSlot;
  private WeaponHolderSlot rightHandSlot;
  private WeaponHolderSlot backSlot;

  private DamageCollider leftHandDamageCollider;
  private DamageCollider rightHandDamageCollider;  

  private QuickSlotsUI quickSlotsUI;

  private InputHandler inputHandler;
  private PlayerManager playerManager;
  private AnimatorHandler animatorHandler;
  private PlayerStats playerStats;

  private void Awake() 
  {
    quickSlotsUI = FindObjectOfType<QuickSlotsUI>();

    inputHandler = GetComponentInParent<InputHandler>();
    playerManager = GetComponentInParent<PlayerManager>();
    animatorHandler = GetComponentInParent<AnimatorHandler>();
    playerStats = GetComponentInParent<PlayerStats>();   
    
    WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
    foreach(WeaponHolderSlot weaponSlot in weaponHolderSlots)
    {
      if(weaponSlot.isLeftHandSlot)
        leftHandSlot = weaponSlot;
      else if(weaponSlot.isRightHandSlot)
        rightHandSlot = weaponSlot;
      else if(weaponSlot.isBackSlot)
        backSlot = weaponSlot;
    }
  }

  public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
  {
    if(isLeft)
    {
      leftHandSlot.currentWeapon = weaponItem;
      leftHandSlot.LoadWeaponModel(weaponItem);
      LoadLeftWeaponDamageCollider();
      quickSlotsUI.UpdateWeaponQuickSlotUI(true, weaponItem);
    }
    else
    {
      if(inputHandler.twoHandFlag)
      {
        backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
        leftHandSlot.UnloadWeaponAndDestroy();       
        animatorHandler.animator.CrossFade(weaponItem.TH_Idle, 0.2f);
      }
      else
      {
        animatorHandler.animator.CrossFade("Both Arms Empty", 0.2f);
        backSlot.UnloadWeaponAndDestroy();
      }

      rightHandSlot.currentWeapon = weaponItem;
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

  public void OpenDamageCollider()
  {
    if(playerManager.isUsingRightHand)
    {
      rightHandDamageCollider.EnableDamageCollider();
    }
    else if(playerManager.isUsingLeftHand)
    {
      leftHandDamageCollider.EnableDamageCollider();
    }
  }

  public void CloseDamageCollider()
  {
    leftHandDamageCollider.DisableDamageCollider();
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