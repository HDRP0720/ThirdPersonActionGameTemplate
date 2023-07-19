using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : AnimatorManager
{
  private EnemyBossManager enemyBossManager;
  private EnemyVFXManager enemyVFXManager;
  private Rigidbody enemyRigidbody;

  protected override void Awake() 
  {
    base.Awake();

    animator = GetComponent<Animator>();
    enemyBossManager = GetComponent<EnemyBossManager>();
    enemyVFXManager = GetComponent<EnemyVFXManager>();
    enemyRigidbody = GetComponent<Rigidbody>();
  }

  // animation events
  public void AwardSoulsOnDeath()
  {
    SoulCountsUI soulCountsUI = FindObjectOfType<SoulCountsUI>();

    PlayerStatsManager playerStats = FindObjectOfType<PlayerStatsManager>();
    if (playerStats != null)
    {
      playerStats.AddSouls(characterStatsManager.soulsAwardedOnDeath);

      if (soulCountsUI != null)
      {
        soulCountsUI.SetSoulCount(playerStats.soulCount);
      }
    }   
  }

  public void InstantiateBossParticleVFX()
  {
    BossFXTransform bossFXTransform = GetComponentInChildren<BossFXTransform>();

    GameObject phaseFX = Instantiate(enemyBossManager.particleFX, bossFXTransform.transform);
  }
  
  public void PlayWeaponVFX()
  {
    enemyVFXManager.PlayWeaponVFX(false);
  }

  private void OnAnimatorMove()
  {
    float delta = Time.deltaTime;

    enemyRigidbody.drag = 0;

    Vector3 deltaPosition = animator.deltaPosition;
    deltaPosition.y = 0;
    Vector3 velocity = deltaPosition / delta;

    enemyRigidbody.velocity = velocity;

    if(characterManager.isRotatingWithRootMotion)
      characterManager.transform.rotation *= animator.deltaRotation;    
  }
}
