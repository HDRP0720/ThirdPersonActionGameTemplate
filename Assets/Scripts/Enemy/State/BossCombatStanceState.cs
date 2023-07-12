using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombatStanceState : CombatStanceState
{
  [Header("# Second Phase Attacks")]
  public bool hasPhaseShifted;
  public EnemyAttackAction[] secondPhaseEnemyAttacks;

  protected override void GetNewAttack(EnemyManager enemyManager)
  {
    if(hasPhaseShifted)
    {
      Vector3 targetsDirection = enemyManager.currentTarget.transform.position - transform.position;
      float viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
      float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);

      int maxScore = 0;
      for (int i = 0; i < secondPhaseEnemyAttacks.Length; i++)
      {
        EnemyAttackAction enemyAttackAction = secondPhaseEnemyAttacks[i];
        if (distanceFromTarget <= enemyAttackAction.maxDistanceToAttack
          && distanceFromTarget >= enemyAttackAction.minDistanceToAttack)
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
      for (int i = 0; i < secondPhaseEnemyAttacks.Length; i++)
      {
        EnemyAttackAction enemyAttackAction = secondPhaseEnemyAttacks[i];
        if (distanceFromTarget <= enemyAttackAction.maxDistanceToAttack
          && distanceFromTarget >= enemyAttackAction.minDistanceToAttack)
        {
          if (viewableAngle <= enemyAttackAction.maximumAttackAngle
            && viewableAngle >= enemyAttackAction.minimumAttackAngle)
          {
            if (attackState.currentAttack != null) return;

            tempScore += enemyAttackAction.attackScore;

            if (tempScore > randomValue)
              attackState.currentAttack = enemyAttackAction;
          }
        }
      }
    }
    else
    {
      base.GetNewAttack(enemyManager);
    }
  }
}
