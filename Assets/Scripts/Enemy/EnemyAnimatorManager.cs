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

  // animation event
  public void AwardSoulsOnDeath()
  {
    SoulCountsUI soulCountsUI = FindObjectOfType<SoulCountsUI>();

    PlayerStats playerStats = FindObjectOfType<PlayerStats>();
    if (playerStats != null)
    {
      playerStats.AddSouls(enemyStats.soulsAwardedOnDeath);

      if (soulCountsUI != null)
      {
        soulCountsUI.SetSoulCount(playerStats.soulCount);
      }
    }   
  }

  public void EnableIsParrying()
  {
    enemyManager.isParrying = true;
  }

  public void DisableIsParrying()
  {
    enemyManager.isParrying = false;
  }

  public void EnableCanBeRiposted()
  {
    enemyManager.canBeRiposted = true;
  }

  public void DisableCanBeRiposted()
  {
    enemyManager.canBeRiposted = false;
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
