using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
  public RotateTowardsTargetState rotateTowardsTargetState;
  public CombatStanceState combatStanceState;
  public PursueTargetState pursueTargetState;

  public EnemyAttackAction currentAttack;

  public bool hasPerformedAttack = false;

  private bool willDoComboingOnNext = false;

  public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStats, EnemyAnimatorManager enemyAnimatorManager)
  {
    HandleRotateTowardsTargetWhileAttack(enemyManager);

    float disdtanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
    if(disdtanceFromTarget > enemyManager.maximumAggroRadius)
      return pursueTargetState;

    if(willDoComboingOnNext && enemyManager.canDoCombo)
    {   
      AttackTargetWithCombo(enemyManager, enemyAnimatorManager);   
    }

    if(!hasPerformedAttack)
    {
      AttackTarget(enemyManager, enemyAnimatorManager);
      RollForComboChance(enemyManager);
    }

    if(willDoComboingOnNext && hasPerformedAttack)
    {
      return this;
    }

    return rotateTowardsTargetState;    
  }

  private void AttackTarget(EnemyManager enemyManager, EnemyAnimatorManager enemyAnimatorManager)
  {
    enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
    enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
    hasPerformedAttack = true;
  }

  private void AttackTargetWithCombo(EnemyManager enemyManager, EnemyAnimatorManager enemyAnimatorManager)
  {
    willDoComboingOnNext = false;
    enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
    enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
    currentAttack = null;
  }

  private void HandleRotateTowardsTargetWhileAttack(EnemyManager enemyManager)
  {
    if (enemyManager.canRotate && enemyManager.isInteracting)
    {
      // Rotate manually
      Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
      direction.y = 0;
      direction.Normalize();

      if (direction == Vector3.zero)
        direction = transform.forward;

      Quaternion targetRotation = Quaternion.LookRotation(direction);
      transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
    }
    
  }

  private void RollForComboChance(EnemyManager enemyManager)
  {
    float comboChance = Random.Range(0, 100);
    if(enemyManager.allowEnemyToPerformCombos && comboChance <= enemyManager.comboChancePercentage)
    {
      if(currentAttack.comboAction != null)
      {
        willDoComboingOnNext = true;
        currentAttack = currentAttack.comboAction;
      }
      else
      {
        willDoComboingOnNext = false;
        currentAttack = null;
      }
    }
  }
}