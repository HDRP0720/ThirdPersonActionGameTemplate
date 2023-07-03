using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : MonoBehaviour
{
  private PlayerManager playerManager;
  private PlayerInventory playerInventory;
  private AnimatorHandler animatorHandler;
  private InputHandler inputHandler;
  private WeaponSlotManager weaponSlotManager;

  public string lastAttack;

  private void Awake() 
  {
    playerManager = GetComponentInParent<PlayerManager>();
    playerInventory = GetComponentInParent<PlayerInventory>();
    animatorHandler = GetComponentInParent<AnimatorHandler>();
    inputHandler = GetComponentInParent<InputHandler>();
    
    weaponSlotManager= GetComponent<WeaponSlotManager>();
  }

  public void HandleLightAttack(WeaponItem weapon)
  {
    weaponSlotManager.attackingWeapon = weapon;

    if(inputHandler.twoHandFlag)
    {
      animatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_1, true);
      lastAttack = weapon.TH_Light_Attack_1;
    }
    else
    {
      animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
      lastAttack = weapon.OH_Light_Attack_1;
    }
  }

  public void HandleHeavyAttack(WeaponItem weapon)
  {
    weaponSlotManager.attackingWeapon = weapon;

    if (inputHandler.twoHandFlag)
    {
      animatorHandler.PlayTargetAnimation(weapon.TH_Heavy_Attack_1, true);
      lastAttack = weapon.TH_Heavy_Attack_1;
    }
    else
    {
      animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
      lastAttack = weapon.OH_Heavy_Attack_1;
    }  
  }

  public void HandleWeaponCombo(WeaponItem weapon)
  {
    if(inputHandler.comboFlag)
    {
      animatorHandler.animator.SetBool("canDoCombo", false);      

      if (lastAttack == weapon.OH_Light_Attack_1)      
        animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);
      else if(lastAttack == weapon.TH_Light_Attack_1)
        animatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_2, true);
    }    
  }  

  #region Input Actions
  public void HandleLightAction()
  {
    if(playerInventory.rightWeapon.isMeleeWeapon)
    {
      PerformLightMeleeAction();
    }
    else if(playerInventory.rightWeapon.isSpellCaster || playerInventory.rightWeapon.isFaithCaster || playerInventory.rightWeapon.isPyroCaster)
    {
      PerformLightMagicAction(playerInventory.rightWeapon);
    }   
  }
  #endregion

  #region Attack Actions
  private void PerformLightMeleeAction()
  {
    if (playerManager.canDoCombo)
    {
      inputHandler.comboFlag = true;
      HandleWeaponCombo(playerInventory.rightWeapon);
      inputHandler.comboFlag = false;
    }
    else
    {
      if (playerManager.isInteracting) return;

      if (playerManager.canDoCombo) return;

      animatorHandler.animator.SetBool("isUsingRightHand", true);
      HandleLightAttack(playerInventory.rightWeapon);
    }
  }

  private void PerformLightMagicAction(WeaponItem weapon)
  {
    if(weapon.isFaithCaster)
    {
      if(playerInventory.currentSpell != null && playerInventory.currentSpell.isFaithSpell)
      {
        // TODO: CHeck for MP
        // TODO: Attempt to cast spell
      }
    }
  }
  #endregion
}