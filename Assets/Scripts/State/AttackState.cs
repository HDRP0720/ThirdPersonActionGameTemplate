using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
  public CombatStanceState combatStanceState;

  public EnemyAttackAction[] enemyAttacks;
  public EnemyAttackAction currentAttack;

  // TODO: Select one of my attacks based on attack scores
  // TODO: if selected attack is not able to be used because of bad angle or distance, select a new attack
  // TODO: if the attack is visible, stop to move and attack our target
  // TODO: set recovery timer and return the combat stance state
  public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
  {
    if (enemyManager.isPerformingAction) 
      return combatStanceState;

    Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

    if(currentAttack != null)
    {
      // TODO: if too close to perform current attack, then get a new attack
      if(enemyManager.distanceFromTarget < currentAttack.minDistanceToAttack)
        return this;
      else if(enemyManager.distanceFromTarget < currentAttack.maxDistanceToAttack)
      {
        if(enemyManager.viewableAngle <= currentAttack.maximumAttackAngle
          && enemyManager.viewableAngle >= currentAttack.minimumAttackAngle)
        {
          if(enemyManager.currentRecoveryTime <= 0 && !enemyManager.isPerformingAction)
          {
            enemyAnimatorManager.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            enemyAnimatorManager.animator.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
            enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
            enemyManager.isPerformingAction = true;
            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
            currentAttack = null;

            return combatStanceState;
          }
        }
      }
    }
    else
    {
      GetNewAttack(enemyManager);
    }

    return combatStanceState;
  }

  private void GetNewAttack(EnemyManager enemyManager)
  {
    Vector3 targetsDirection = enemyManager.currentTarget.transform.position - transform.position;
    float viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
    enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);

    int maxScore = 0;
    for (int i = 0; i < enemyAttacks.Length; i++)
    {
      EnemyAttackAction enemyAttackAction = enemyAttacks[i];
      if (enemyManager.distanceFromTarget <= enemyAttackAction.maxDistanceToAttack
        && enemyManager.distanceFromTarget >= enemyAttackAction.minDistanceToAttack)
      {
        if (viewableAngle <= enemyAttackAction.maximumAttackAngle
          && viewableAngle >= enemyAttackAction.minimumAttackAngle)
        {
          maxScore += enemyAttackAction.attackScore;
        }
      }
    }

    int randomValue = Random.Range(0, maxScore);
    int tempScore = 0;
    for (int i = 0; i < enemyAttacks.Length; i++)
    {
      EnemyAttackAction enemyAttackAction = enemyAttacks[i];
      if (enemyManager.distanceFromTarget <= enemyAttackAction.maxDistanceToAttack
        && enemyManager.distanceFromTarget >= enemyAttackAction.minDistanceToAttack)
      {
        if (viewableAngle <= enemyAttackAction.maximumAttackAngle
          && viewableAngle >= enemyAttackAction.minimumAttackAngle)
        {
          if (currentAttack != null) return;

          tempScore += enemyAttackAction.attackScore;

          if (tempScore > randomValue)
            currentAttack = enemyAttackAction;
        }
      }
    }
  }  
}
