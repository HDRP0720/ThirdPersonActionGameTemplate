using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
  public WeaponItem attackingWeapon;

  public DamageCollider leftHandDamageCollider;
  public DamageCollider rightHandDamageCollider;

  public WeaponHolderSlot leftHandSlot;
  public WeaponHolderSlot rightHandSlot;
  private WeaponHolderSlot backSlot; 

  private QuickSlotsUI quickSlotsUI;

  private PlayerManager playerManager;
  private InputHandler inputHandler;  
  private PlayerAnimatorManager playerAnimatorManager;
  private PlayerInventory playerInventory;
  private PlayerStats playerStats;

  private void Awake()
  {
    quickSlotsUI = FindObjectOfType<QuickSlotsUI>();

    playerManager = GetComponentInParent<PlayerManager>();
    inputHandler = GetComponentInParent<InputHandler>();    
    playerAnimatorManager = GetComponentInParent<PlayerAnimatorManager>();
    playerInventory = GetComponentInParent<PlayerInventory>();
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
        playerAnimatorManager.animator.CrossFade(weaponItem.TH_Idle, 0.2f);
      }
      else
      {
        playerAnimatorManager.animator.CrossFade("Both Arms Empty", 0.2f);
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
    leftHandDamageCollider.currentWeaponDamage = playerInventory.leftWeapon.baseDamage;
  }

  private void LoadRightWeaponDamageCollider()
  {
    rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    rightHandDamageCollider.currentWeaponDamage = playerInventory.rightWeapon.baseDamage;
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