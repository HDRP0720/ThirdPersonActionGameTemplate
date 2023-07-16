using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
  public bool enabledDamageColliderOnStartUp = false;

  [Header("# Damage")]
  public int currentWeaponDamage = 25;

  [Header("# Poise")]
  public float poiseBreak;
  public float offensivePoiseBonus;

  [HideInInspector] public CharacterManager characterManager;

  private Collider damageCollider;

  private void Awake()
  {
    damageCollider = GetComponent<Collider>();
    damageCollider.gameObject.SetActive(true);
    damageCollider.isTrigger = true;
    damageCollider.enabled = enabledDamageColliderOnStartUp; 
  }

  public void EnableDamageCollider()
  {
    damageCollider.enabled = true;
  }

  public void DisableDamageCollider()
  {
    damageCollider.enabled = false;
  }

  private void OnTriggerEnter(Collider other) 
  {
    if(other.tag == "Player")
    {
      CharacterManager playerManager = other.GetComponent<CharacterManager>();
      BlockingCollider shieldCollider = other.transform.GetComponentInChildren<BlockingCollider>();
      PlayerStats playerStats = other.GetComponent<PlayerStats>();   

      if(playerManager != null)
      {
        if(playerManager.isParrying)
        {
          characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
          return;
        }
        else if(shieldCollider != null && playerManager.isBlocking)
        {
          float damageAfterBlocking = currentWeaponDamage - (currentWeaponDamage * shieldCollider.blockingDamageAbsorption);
          if(playerStats != null)
          {
            playerStats.TakeDamage(Mathf.RoundToInt(damageAfterBlocking), "BlockGuard");
            return;
          }
        }
      }
      
      if(playerStats != null)
      {
        playerStats.poiseResetTimer = playerStats.totalPoiseResetTime;
        playerStats.totalPoiseDefence = playerStats.totalPoiseDefence - poiseBreak;
        Debug.Log($"Player's Poise is currently {playerStats.totalPoiseDefence}");
        if (playerStats.totalPoiseDefence > poiseBreak)
        {
          playerStats.TakeDamageWithoutAnimation(currentWeaponDamage);     
        }
        else
        {
          playerStats.TakeDamage(currentWeaponDamage);
        }
      }
    }

    if(other.tag == "Enemy")
    {
      CharacterManager enemyCharacterManager = other.GetComponent<CharacterManager>();
      if (enemyCharacterManager != null)
      {
        if (enemyCharacterManager.isParrying)
        {
          characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
          return;
        }
      }

      EnemyStats enemyStats = other.GetComponent<EnemyStats>();
      if(enemyStats != null)
      {
        enemyStats.poiseResetTimer = enemyStats.totalPoiseResetTime;
        enemyStats.totalPoiseDefence = enemyStats.totalPoiseDefence - poiseBreak;
        Debug.Log($"Enemy Poise is currently {enemyStats.totalPoiseDefence}");

        if(enemyStats.isBoss)
        {
          if (enemyStats.totalPoiseDefence > poiseBreak)
          {
            enemyStats.TakeDamageWithoutAnimation(currentWeaponDamage);
          }
          else
          {
            enemyStats.TakeDamageWithoutAnimation(currentWeaponDamage);
            enemyStats.BreakGuard();
          }
        }
        else
        {
          if (enemyStats.totalPoiseDefence > poiseBreak)          
            enemyStats.TakeDamageWithoutAnimation(currentWeaponDamage);          
          else          
            enemyStats.TakeDamage(currentWeaponDamage);          
        }
        
      }   
    }   
  }
}
