using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
  private LayerMask backStabLayer = 1 << 13;
  private LayerMask riposteLayer = 1 << 14;

  private CameraHandler cameraHandler;
  private InputHandler inputHandler;

  private PlayerManager playerManager;
  private PlayerInventoryManager playerInventoryManager;
  private PlayerStatsManager playerStatsManager;
  private PlayerAnimatorManager playerAnimatorManager;
  private PlayerEquipmentManager playerEquipmentManager;
  private PlayerWeaponSlotManager playerWeaponSlotManager;
  private PlayerVFXManager playerVFXManager;

  public string lastAttack;

  private void Awake() 
  {
    cameraHandler = FindObjectOfType<CameraHandler>();
    inputHandler = GetComponent<InputHandler>();

    playerManager = GetComponent<PlayerManager>();
    playerInventoryManager = GetComponent<PlayerInventoryManager>();    
    playerStatsManager = GetComponent<PlayerStatsManager>(); 
    playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
    playerEquipmentManager = GetComponent<PlayerEquipmentManager>();    
    playerWeaponSlotManager= GetComponent<PlayerWeaponSlotManager>();
    playerVFXManager = GetComponent<PlayerVFXManager>();
  }

  public void HandleLightAttack(WeaponItem weapon)
  {
    if (playerStatsManager.currentStamina <= 0) return;

    playerWeaponSlotManager.attackingWeapon = weapon;

    if(inputHandler.twoHandFlag)
    {
      playerAnimatorManager.PlayTargetAnimation(weapon.TH_Light_Attack_1, true);
      lastAttack = weapon.TH_Light_Attack_1;
    }
    else
    {
      playerAnimatorManager.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
      lastAttack = weapon.OH_Light_Attack_1;
    }
  }

  public void HandleHeavyAttack(WeaponItem weapon)
  {
    if (playerStatsManager.currentStamina <= 0) return;

    playerWeaponSlotManager.attackingWeapon = weapon;

    if (inputHandler.twoHandFlag)
    {
      playerAnimatorManager.PlayTargetAnimation(weapon.TH_Heavy_Attack_1, true);
      lastAttack = weapon.TH_Heavy_Attack_1;
    }
    else
    {
      playerAnimatorManager.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
      lastAttack = weapon.OH_Heavy_Attack_1;
    }  
  }

  public void HandleWeaponCombo(WeaponItem weapon)
  {
    if (playerStatsManager.currentStamina <= 0) return;

    if(inputHandler.comboFlag)
    {
      playerAnimatorManager.animator.SetBool("canDoCombo", false);      

      if (lastAttack == weapon.OH_Light_Attack_1)      
        playerAnimatorManager.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);
      else if(lastAttack == weapon.TH_Light_Attack_1)
        playerAnimatorManager.PlayTargetAnimation(weapon.TH_Light_Attack_2, true);
    }    
  }  

  #region Input Actions
  public void HandleLightAction()
  {
    if(playerInventoryManager.rightWeapon.isMeleeWeapon)
    {
      PerformLightMeleeAction();
    }
    else if(playerInventoryManager.rightWeapon.isSpellCaster || playerInventoryManager.rightWeapon.isFaithCaster || playerInventoryManager.rightWeapon.isPyroCaster)
    {
      PerformLightMagicAction(playerInventoryManager.rightWeapon);
    }   
  }

  public void HandleBlockingAction()
  {
    PerformBlockingAction();
  }

  public void HandleParryAction()
  {
    if(playerInventoryManager.leftWeapon.isShield)
    {
      PerformParryAction(inputHandler.twoHandFlag);
    }
    else if(playerInventoryManager.leftWeapon.isMeleeWeapon)
    {
      // TODO: do a light attack;
    }
  }
  #endregion

  #region Attack Actions
  private void PerformLightMeleeAction()
  {
    if (playerManager.canDoCombo)
    {
      inputHandler.comboFlag = true;
      HandleWeaponCombo(playerInventoryManager.rightWeapon);
      inputHandler.comboFlag = false;
    }
    else
    {
      if (playerManager.isInteracting) return;

      if (playerManager.canDoCombo) return;

      playerAnimatorManager.animator.SetBool("isUsingRightHand", true);
      HandleLightAttack(playerInventoryManager.rightWeapon);
    }
    StartCoroutine(PlayWeaponVFX());
  }

  private IEnumerator PlayWeaponVFX()
  {
    yield return new WaitForSeconds(0.5f);
    playerVFXManager.PlayWeaponVFX(false);
  }

  private void PerformLightMagicAction(WeaponItem weapon)
  {
    if(playerManager.isInteracting) return;

    if(weapon.isFaithCaster)
    {
      if(playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isFaithSpell)
      {
        if(playerStatsManager.currentMana >= playerInventoryManager.currentSpell.manaCost)
        { 
          playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager);
        }
        else
        {
          playerAnimatorManager.PlayTargetAnimation("Shrug", true);
        }
      }
    }
    else if(weapon.isPyroCaster)
    {
      if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isPyroSpell)
      {
        if (playerStatsManager.currentMana >= playerInventoryManager.currentSpell.manaCost)
        {
          playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager);
        }
        else
        {
          playerAnimatorManager.PlayTargetAnimation("Shrug", true);
        }
      }  
    }
  }

  private void PerformParryAction(bool isTwoHanding)
  {
    if(playerManager.isInteracting) return;

    if(isTwoHanding)
    {
      // TODO: if player use two-handed weapon, perform parry art for right weapon
    }
    else
    {
      playerAnimatorManager.PlayTargetAnimation(playerInventoryManager.leftWeapon.parry_art, true);
    }
  }

  private void SuccessfullyCastSpell()
  {
    playerInventoryManager.currentSpell.SucessfullyCastSpell(playerAnimatorManager, playerStatsManager, cameraHandler, playerWeaponSlotManager);
    playerAnimatorManager.animator.SetBool("isFiringSpell", true);
  }
  #endregion

  #region Defense Action
  private void PerformBlockingAction()
  {
    if(playerManager.isInteracting) return;

    if(playerManager.isBlocking) return;

    playerAnimatorManager.PlayTargetAnimation("BlockStart", false, true);
    playerEquipmentManager.OpenBlockingCollider();
    playerManager.isBlocking = true;
  }
  #endregion

  public void AttemptBackStabOrRiposte()
  {
    if (playerStatsManager.currentStamina <= 0) return;
    
    RaycastHit hit;
    if(Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, 
      transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
    {
      CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
      DamageCollider rightweapon = playerWeaponSlotManager.rightHandDamageCollider;

      if(enemyCharacterManager != null)
      {
        // TODO: Manipulate position -> rotation -> animation
        playerManager.transform.position = enemyCharacterManager.backStabCollider.specialAttackerTransform.position;

        Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
        rotationDirection = hit.transform.position - playerManager.transform.position;
        rotationDirection.y = 0;
        rotationDirection.Normalize();

        Quaternion tr = Quaternion.LookRotation(rotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
        playerManager.transform.rotation = targetRotation;

        int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightweapon.currentWeaponDamage;
        enemyCharacterManager.pendingCriticalDamage = criticalDamage;

        playerAnimatorManager.PlayTargetAnimation("BackStab", true);
        enemyCharacterManager.GetComponent<AnimatorManager>().PlayTargetAnimation("BackStabbed", true);
      }
    }
    else if(Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position,
      transform.TransformDirection(Vector3.forward), out hit, 3.0f, riposteLayer))
    {
      CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
      DamageCollider rightweapon = playerWeaponSlotManager.rightHandDamageCollider;

      if(enemyCharacterManager != null && enemyCharacterManager.canBeRiposted)
      {
        playerManager.transform.position = enemyCharacterManager.riposteCollider.specialAttackerTransform.position;

        Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
        rotationDirection = hit.transform.position - playerManager.transform.position;
        rotationDirection.y = 0;
        rotationDirection.Normalize();

        Quaternion tr = Quaternion.LookRotation(rotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
        playerManager.transform.rotation = targetRotation;

        int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightweapon.currentWeaponDamage;
        enemyCharacterManager.pendingCriticalDamage = criticalDamage;

        playerAnimatorManager.PlayTargetAnimation("Riposte", true);
        enemyCharacterManager.GetComponent<AnimatorManager>().PlayTargetAnimation("Riposted", true);
      }  
    }
  }
}