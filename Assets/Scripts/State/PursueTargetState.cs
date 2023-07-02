using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueTargetState : State
{
  public CombatStanceState combatStanceState;

  // TODO: Chase the target
  // TODO: If within attack range, return combat stance state
  // TODO: if target is out of range, return this state and continue to chase target
  public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
  {
    if (enemyManager.isPerformingAction) 
    {
      enemyAnimatorManager.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
      return this;
    }  

    Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
    float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
    float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

    if (distanceFromTarget > enemyManager.maximumAttackRange)    
      enemyAnimatorManager.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);

    HandleRotateTowardsTarget(enemyManager);

    enemyManager.navMeshAgent.transform.localPosition = Vector3.zero;
    enemyManager.navMeshAgent.transform.localRotation = Quaternion.identity;

    if(distanceFromTarget <= enemyManager.maximumAttackRange)    
      return combatStanceState;    
    else    
      return this;    
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
