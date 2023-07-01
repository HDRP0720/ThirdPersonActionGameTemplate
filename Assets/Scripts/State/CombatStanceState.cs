using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStanceState : State
{
  public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
  {
    // TODO: Check for attack range
    // TODO: Circle player potentially or walk around them
    // TODO: if in attack range, return attack State
    // TODO: if in a cooldown after attacking, return this state and continue circling player
    // TODO: if player runs out of range, return the pursue target state
    return this;
  }
}
