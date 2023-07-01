using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
  public LayerMask detectionLayer;

  public PursueTargetState pursueTargetState;

  // TODO: Look for a potential target
  // TODO: Switch to the pursue target state if target is found
  // TODO: if not return this state
  public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
  {
    #region Handle Enemy Target Detection
    Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);
    for (int i = 0; i < colliders.Length; i++)
    {
      CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();
      if (characterStats != null)
      {
        Vector3 targetDirection = characterStats.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
        if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetetionAngle)
        {
          enemyManager.currentTarget = characterStats;         
        }
      }
    }
    #endregion
    
    #region Handle Switching To Next State 
    if(enemyManager.currentTarget != null)
    {
      return pursueTargetState;
    }
    else
    {
      return this;
    }
    #endregion
  }
}