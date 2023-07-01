using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
  public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
  {
    // TODO: Select one of my attacks based on attack scores
    // TODO: if selected attack is not able to be used because of bad angle or distance, select a new attack
    // TODO: if the attack is visible, stop to move and attack our target
    // TODO: set recovery timer and return the combat stance state
    return this;
  }
}
