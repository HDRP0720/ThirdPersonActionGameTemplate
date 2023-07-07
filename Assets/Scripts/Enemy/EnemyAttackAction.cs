using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TPA/Enemy Actions/Make New Attack Action")]
public class EnemyAttackAction : EnemyAction
{
  public bool canCombo;
  public EnemyAttackAction comboAction;
  public int attackScore = 3;
  public float recoveryTime = 2f;

  public float maximumAttackAngle = 35f;
  public float minimumAttackAngle = -35f;

  public float minDistanceToAttack = 0f;
  public float maxDistanceToAttack = 3f;
}
