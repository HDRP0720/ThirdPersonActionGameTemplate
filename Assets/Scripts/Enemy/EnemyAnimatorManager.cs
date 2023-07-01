using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : AnimatorManager
{
  private EnemyMoveState enemyMoveState;
  private Rigidbody enemyRigidBody;

  private void Awake() 
  {
    animator = GetComponent<Animator>();
    enemyMoveState = GetComponent<EnemyMoveState>();
    enemyRigidBody = GetComponent<Rigidbody>();
  }

  private void OnAnimatorMove()
  {
    float delta = Time.deltaTime;

    enemyRigidBody.drag = 0;

    Vector3 deltaPosition = animator.deltaPosition;
    deltaPosition.y = 0;
    Vector3 velocity = deltaPosition / delta;

    enemyRigidBody.velocity = velocity;
  }
}
