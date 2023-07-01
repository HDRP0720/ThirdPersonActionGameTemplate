using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : CharacterManager
{
  [Header("Enemy Settings")]
  public float detectionRadius = 20f;
  public float minimumDetectionAngle = -50f;
  public float maximumDetetionAngle = 50f;

  public EnemyAttackAction[] enemyAttacks;
  public EnemyAttackAction currentAttack;

  public float currentRecoveryTime = 0;

  public bool isPerformingAction;

  private EnemyMoveState enemyMoveState;
  private EnemyAnimatorManager enemyAnimatorManager;

  private void Awake() 
  {
    enemyMoveState = GetComponent<EnemyMoveState>();
    enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
  }
  private void FixedUpdate() 
  {
    HandleCurrentAction();
  }
  private void Update() 
  {
    HandleRecoveryTimer();
  }

  private void HandleCurrentAction()
  {
    if(enemyMoveState.currentTarget != null)
    {
      enemyMoveState.distanceFromTarget = Vector3.Distance(enemyMoveState.currentTarget.transform.position, transform.position);
    }
   
    if(enemyMoveState.currentTarget == null)
    {
      enemyMoveState.HandleDetection();
    }
    else if(enemyMoveState.distanceFromTarget > enemyMoveState.stoppingDistance)
    {
      enemyMoveState.HandleMoveToTarget();
    }
    else if(enemyMoveState.distanceFromTarget <= enemyMoveState.stoppingDistance)
    {
      AttackTarget();
    }
  }

  private void HandleRecoveryTimer()
  {
    if(currentRecoveryTime > 0)    
      currentRecoveryTime -= Time.deltaTime;    

    if(isPerformingAction)
    {
      if(currentRecoveryTime <= 0)      
        isPerformingAction = false;      
    }
  }

  #region Enemy Attacks
  private void AttackTarget()
  {
    if(isPerformingAction) return;

    if(currentAttack == null)
    {
      GetNewAttack();
    }
    else
    {
      isPerformingAction = true;
      currentRecoveryTime = currentAttack.recoveryTime;
      enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
      currentAttack = null;
    }
  }
  
  private void GetNewAttack()
  {
    Vector3 targetsDirection = enemyMoveState.currentTarget.transform.position - transform.position;
    float viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
    enemyMoveState.distanceFromTarget = Vector3.Distance(enemyMoveState.currentTarget.transform.position, transform.position);

    int maxScore = 0;
    for (int i = 0; i < enemyAttacks.Length; i++)
    {
      EnemyAttackAction enemyAttackAction = enemyAttacks[i];      
      if(enemyMoveState.distanceFromTarget <= enemyAttackAction.maxDistanceToAttack
        && enemyMoveState.distanceFromTarget >= enemyAttackAction.minDistanceToAttack)
      {
        if(viewableAngle <= enemyAttackAction.maximumAttackAngle
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
      if (enemyMoveState.distanceFromTarget <= enemyAttackAction.maxDistanceToAttack
        && enemyMoveState.distanceFromTarget >= enemyAttackAction.minDistanceToAttack)
      {
        if (viewableAngle <= enemyAttackAction.maximumAttackAngle
          && viewableAngle >= enemyAttackAction.minimumAttackAngle)
        {
          if(currentAttack != null) return;

          tempScore += enemyAttackAction.attackScore;

          if(tempScore > randomValue)
            currentAttack = enemyAttackAction;
        }
      }
    }
  }
  #endregion
}
