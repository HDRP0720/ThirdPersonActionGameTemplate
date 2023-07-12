using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStanceState : State
{  
  public AttackState attackState;
  public PursueTargetState pursueTargetState;

  public EnemyAttackAction[] enemyAttacks;

  protected bool randomDestinationSet = false;

  protected float verticalMovementValue = 0f;
  protected float horizontalMovementValue = 0f;

  public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
  {
    float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
    enemyAnimatorManager.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
    enemyAnimatorManager.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
    attackState.hasPerformedAttack = false;

    if (enemyManager.isInteracting) 
    {
      enemyAnimatorManager.animator.SetFloat("Vertical", 0);
      enemyAnimatorManager.animator.SetFloat("Horizontal", 0);

      return this;
    }

    if (distanceFromTarget > enemyManager.maximumAggroRadius)
      return pursueTargetState;

    if(!randomDestinationSet)
    {
      randomDestinationSet = true;
      DecideCirclingAction(enemyAnimatorManager);
    }

    HandleRotateTowardsTarget(enemyManager);    

    if(enemyManager.isPerformingAction)
      enemyAnimatorManager.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);

    if(enemyManager.currentRecoveryTime <= 0 && attackState.currentAttack != null)
    {
      randomDestinationSet = false;
      return attackState;
    }
    else
    {
      GetNewAttack(enemyManager);
    }

    return this;
  }

  protected void HandleRotateTowardsTarget(EnemyManager enemyManager)
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

  protected void DecideCirclingAction(EnemyAnimatorManager enemyAnimatorManager)
  {
    WalkAroundTarget(enemyAnimatorManager);
  }

  protected void WalkAroundTarget(EnemyAnimatorManager enemyAnimatorManager)
  {
    // verticalMovementValue = 0.5f;
    verticalMovementValue = Random.Range(-1, 1);
    if(verticalMovementValue <= 1 && verticalMovementValue > 0) 
      verticalMovementValue = 0.5f;
    else if(verticalMovementValue >= -1 && verticalMovementValue < 0)
      verticalMovementValue = -0.5f;

    // horizontalMovementValue= 0.5f;
    horizontalMovementValue = Random.Range(-1, 1);
    if(horizontalMovementValue <= 1 && horizontalMovementValue > 0)
      horizontalMovementValue = 0.5f;
    else if (horizontalMovementValue >= -1 && horizontalMovementValue < 0)
      horizontalMovementValue = -0.5f;
  }

  protected virtual void GetNewAttack(EnemyManager enemyManager)
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
          if (attackState.currentAttack != null) return;

          tempScore += enemyAttackAction.attackScore;

          if (tempScore > randomValue)
            attackState.currentAttack = enemyAttackAction;
        }
      }
    }
  }
}
