using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : CharacterManager
{
  [Header("Enemy Settings")]
  public float detectionRadius = 20f;
  public float minimumDetectionAngle = -50f;
  public float maximumDetetionAngle = 50f;

  public bool isPerformingAction;

  private EnemyMoveState enemyMoveState; 

  private void Awake() 
  {
    enemyMoveState = GetComponent<EnemyMoveState>();
  }
  private void FixedUpdate() 
  {
    HandleCurrentAction();
  }

  private void HandleCurrentAction()
  {
    if(enemyMoveState.currentTarget == null)
    {
      enemyMoveState.HandleDetection();
    }
    else
    {
      enemyMoveState.HandleMoveToTarget();
    }
  }
}
