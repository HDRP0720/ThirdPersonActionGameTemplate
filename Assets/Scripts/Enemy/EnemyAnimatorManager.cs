using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : AnimatorManager
{
  private EnemyManager enemyManager;
  private EnemyStats enemyStats;

  private void Awake() 
  {
    enemyManager = GetComponent<EnemyManager>();
    enemyStats = GetComponent<EnemyStats>();

    animator = GetComponent<Animator>();
  }

  public override void TakeCriticalDamageAnimationEvent()
  {
    enemyStats.TakeDamageWithoutAnimation(enemyManager.pendingCriticalDamage);
    enemyManager.pendingCriticalDamage = 0;
  }

  private void OnAnimatorMove()
  {
    float delta = Time.deltaTime;

    enemyManager.enemyRigidBody.drag = 0;

    Vector3 deltaPosition = animator.deltaPosition;
    deltaPosition.y = 0;
    Vector3 velocity = deltaPosition / delta;

    enemyManager.enemyRigidBody.velocity = velocity;
  }
}
