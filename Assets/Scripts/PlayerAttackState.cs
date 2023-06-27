using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : MonoBehaviour
{
  private AnimatorHandler animatorHandler;
  private InputHandler inputHandler;
  private WeaponSlotManager weaponSlotManager;

  public string lastAttack;

  private void Awake() 
  {
    animatorHandler = GetComponent<AnimatorHandler>();
    inputHandler = GetComponent<InputHandler>();
    weaponSlotManager= GetComponent<WeaponSlotManager>();
  }

  public void HandleLightAttack(WeaponItem weapon)
  {
    weaponSlotManager.attackingWeapon = weapon;

    animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
    lastAttack = weapon.OH_Light_Attack_1;
  }

  public void HandleWeaponCombo(WeaponItem weapon)
  {
    if(inputHandler.comboFlag)
    {
      animatorHandler.animator.SetBool("canDoCombo", false);

      if (lastAttack == weapon.OH_Light_Attack_1)      
        animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);      
    }    
  }

  public void HandleHeavyAttack(WeaponItem weapon)
  {
    weaponSlotManager.attackingWeapon = weapon;

    animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
    lastAttack = weapon.OH_Heavy_Attack_1;
  }
}