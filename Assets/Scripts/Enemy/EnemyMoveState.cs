using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveState : MonoBehaviour
{
  public LayerMask detectionLayer;
  
  public CapsuleCollider characterCollider;
  public CapsuleCollider characterCollisionBlockerCollider;

  private EnemyManager enemyManager;
  private EnemyAnimatorManager enemyAnimatorManager;

  private void Awake() 
  {
    enemyManager = GetComponent<EnemyManager>();
    enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
  }
  private void Start() 
  {
    Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
  }
}
