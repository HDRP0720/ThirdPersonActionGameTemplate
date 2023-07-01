using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : AnimatorManager
{
  private EnemyManager enemyManager;

  private void Awake() 
  {
    enemyManager = GetComponent<EnemyManager>();
    animator = GetComponent<Animator>();
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