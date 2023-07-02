using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
  public CombatStanceState combatStanceState;

  public EnemyAttackAction[] enemyAttacks;
  public EnemyAttackAction currentAttack;

  public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
  {
    if (enemyManager.isPerformingAction) 
      return combatStanceState;

    HandleRotateTowardsTarget(enemyManager);

    Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
    float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

    if(currentAttack != null)
    {    
      if(distanceFromTarget < currentAttack.minDistanceToAttack)
      {
        return this;
      }   
      else if(distanceFromTarget < currentAttack.maxDistanceToAttack)
      {
        if(viewableAngle <= currentAttack.maximumAttackAngle
          && viewableAngle >= currentAttack.minimumAttackAngle)
        {
          if(enemyManager.currentRecoveryTime <= 0 && !enemyManager.isPerformingAction)
          {
            enemyAnimatorManager.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            // enemyAnimatorManager.animator.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
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
    float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);

    int maxScore = 0;
    for (int i = 0; i < enemyAttacks.Length; i++)
    {
      EnemyAttackAction enemyAttackAction = enemyAttacks[i];
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
    for (int i = 0; i < enemyAttacks.Length; i++)
    {
      EnemyAttackAction enemyAttackAction = enemyAttacks[i];
      if (distanceFromTarget <= enemyAttackAction.maxDistanceToAttack
        && distanceFromTarget >= enemyAttackAction.minDistanceToAttack)
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

  private void HandleRotateTowardsTarget(EnemyManager enemyManager)
  {
    if (enemyManager.isPerformingAction)
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
    else
    {
      // Rotate with pathfinding (navmesh)
      Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
      Vector3 targetVelocity = enemyManager.enemyRigidBody.velocity;

      enemyManager.navMeshAgent.enabled = true;
      enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
      enemyManager.enemyRigidBody.velocity = targetVelocity;
      enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
    }
  }
}
