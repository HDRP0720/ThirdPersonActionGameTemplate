using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
{
  [Header("# Current Attacking Weapon")]
  public WeaponItem attackingWeapon;

  private QuickSlotsUI quickSlotsUI;

  private PlayerManager playerManager;
  private InputHandler inputHandler;  
  private PlayerAnimatorManager playerAnimatorManager;
  private PlayerInventoryManager playerInventory;
  private PlayerStatsManager playerStats;
  private PlayerVFXManager playerVFXManager;

  private void Awake()
  {
    quickSlotsUI = FindObjectOfType<QuickSlotsUI>();

    playerManager = GetComponent<PlayerManager>();
    inputHandler = GetComponent<InputHandler>();    
    playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
    playerInventory = GetComponent<PlayerInventoryManager>();
    playerStats = GetComponent<PlayerStatsManager>();
    playerVFXManager = GetComponent<PlayerVFXManager>();
    
    LoadWeaponHolderSlots();
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
      else if (weaponSlot.isBackSlot)
        backSlot = weaponSlot;
    }
  }

  public void LoadBothWeaponsOnSlots()
  {
    LoadWeaponOnSlot(playerInventory.rightWeapon, false);
    LoadWeaponOnSlot(playerInventory.leftWeapon, true);
  }

  public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
  {
    if(weaponItem != null)
    {
      if (isLeft)
      {
        leftHandSlot.currentWeapon = weaponItem;
        leftHandSlot.LoadWeaponModel(weaponItem);
        LoadLeftWeaponDamageCollider();
        quickSlotsUI.UpdateWeaponQuickSlotUI(true, weaponItem);
      }
      else
      {
        if (inputHandler.twoHandFlag)
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
    else
    {
      weaponItem = unarmedWeapon;
      if(isLeft)
      {
        playerInventory.leftWeapon = unarmedWeapon;

        leftHandSlot.currentWeapon = weaponItem;
        leftHandSlot.LoadWeaponModel(weaponItem);
        LoadLeftWeaponDamageCollider();
        quickSlotsUI.UpdateWeaponQuickSlotUI(true, weaponItem);
      }
      else
      {
        playerInventory.rightWeapon = unarmedWeapon;

        rightHandSlot.currentWeapon = weaponItem;
        rightHandSlot.LoadWeaponModel(weaponItem);
        LoadRightWeaponDamageCollider();
        quickSlotsUI.UpdateWeaponQuickSlotUI(false, weaponItem);
      }
    }   
  }

  #region Handle Weapon's Damage Collider
  private void LoadLeftWeaponDamageCollider()
  {
    leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    leftHandDamageCollider.currentWeaponDamage = playerInventory.leftWeapon.baseDamage;
    leftHandDamageCollider.poiseBreak = playerInventory.leftWeapon.poiseBreak;
    playerVFXManager.leftWeaponVFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponVFX>();
  }

  private void LoadRightWeaponDamageCollider()
  {
    rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    rightHandDamageCollider.currentWeaponDamage = playerInventory.rightWeapon.baseDamage;
    rightHandDamageCollider.poiseBreak = playerInventory.rightWeapon.poiseBreak;
    playerVFXManager.rightWeaponVFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponVFX>();
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
    if(rightHandDamageCollider != null)
      rightHandDamageCollider.DisableDamageCollider();

    if(leftHandDamageCollider != null)
      leftHandDamageCollider.DisableDamageCollider();
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

  #region Handle Weapon's Poise Bonus
  public void GrantAttackPoiseBonus()
  {
    playerStats.totalPoiseDefence = playerStats.totalPoiseDefence + attackingWeapon.offensivePoiseBonus;
  }

  public void ResetAttackPoiseBonus()
  {
    playerStats.totalPoiseDefence = playerStats.armorPoiseBonus;
  }
  #endregion
}