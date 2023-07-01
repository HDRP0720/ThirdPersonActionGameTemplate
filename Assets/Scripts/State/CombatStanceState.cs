using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStanceState : State
{
  public AttackState attackState;
  public PursueTargetState pursueTargetState;

  // TODO: Check for attack range
  // TODO: Circle player potentially or walk around them
  // TODO: if in attack range, return attack State
  // TODO: if in a cooldown after attacking, return this state and continue circling player
  // TODO: if player runs out of range, return the pursue target state
  public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
  {
    enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

    if(enemyManager.currentRecoveryTime <= 0 && enemyManager.distanceFromTarget <= enemyManager.maximumAttackRange)
      return attackState;
    else if(enemyManager.distanceFromTarget > enemyManager.maximumAttackRange)
      return pursueTargetState;
    else
      return this;
  }
}
