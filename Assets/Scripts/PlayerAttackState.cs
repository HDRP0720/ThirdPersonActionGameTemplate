using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : MonoBehaviour
{
  private AnimatorHandler animatorHandler;

  private void Awake() 
  {
    animatorHandler = GetComponent<AnimatorHandler>();
  }

  public void HandleLightAttack(WeaponItem weapon)
  {
    animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
  }

  public void HandleHeavyAttack(WeaponItem weapon)
  {
    animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
  }
}