using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : MonoBehaviour
{
  private LayerMask backStabLayer = 1 << 13;

  private PlayerManager playerManager;
  private PlayerInventory playerInventory;
  private PlayerStats playerStats;
  private PlayerAnimatorManager animatorHandler;
  private InputHandler inputHandler;
  private WeaponSlotManager weaponSlotManager;

  public string lastAttack;

  private void Awake() 
  {
    playerManager = GetComponentInParent<PlayerManager>();
    playerInventory = GetComponentInParent<PlayerInventory>();
    playerStats = GetComponentInParent<PlayerStats>();
    animatorHandler = GetComponent<PlayerAnimatorManager>();
    inputHandler = GetComponentInParent<InputHandler>();
    
    weaponSlotManager= GetComponent<WeaponSlotManager>();
  }

  public void HandleLightAttack(WeaponItem weapon)
  {
    if (playerStats.currentStamina <= 0) return;

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
    if (playerStats.currentStamina <= 0) return;

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
    if (playerStats.currentStamina <= 0) return;

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
    if(playerManager.isInteracting) return;

    if(weapon.isFaithCaster)
    {
      if(playerInventory.currentSpell != null && playerInventory.currentSpell.isFaithSpell)
      {
        // TODO: CHeck for MP
        if(playerStats.currentMana >= playerInventory.currentSpell.manaCost)
        {
          // TODO: Attempt to cast spell
          playerInventory.currentSpell.AttemptToCastSpell(animatorHandler, playerStats);
        }
        else
        {
          animatorHandler.PlayTargetAnimation("Shrug", true);
        }
      }
    }
  }

  private void SuccessfullyCastSpell()
  {
    playerInventory.currentSpell.SucessfullyCastSpell(animatorHandler, playerStats);
  }
  #endregion

  public void AttemptBackStabOrRiposte()
  {
    if (playerStats.currentStamina <= 0) return;
    
    RaycastHit hit;
    if(Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, 
      transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
    {
      CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
      DamageCollider rightweapon = weaponSlotManager.rightHandDamageCollider;

      if(enemyCharacterManager != null)
      {
        // TODO: Manipulate position -> rotation -> animation
        playerManager.transform.position = enemyCharacterManager.backStabCollider.backStabberStandPoint.position;

        Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
        rotationDirection = hit.transform.position - playerManager.transform.position;
        rotationDirection.y = 0;
        rotationDirection.Normalize();

        Quaternion tr = Quaternion.LookRotation(rotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
        playerManager.transform.rotation = targetRotation;

        int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightweapon.currentWeaponDamage;
        enemyCharacterManager.pendingCriticalDamage = criticalDamage;

        animatorHandler.PlayTargetAnimation("BackStab", true);
        enemyCharacterManager.GetComponent<AnimatorManager>().PlayTargetAnimation("BackStabbed", true);
      }
    }
  }
}