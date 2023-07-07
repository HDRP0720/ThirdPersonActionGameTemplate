using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
  public int currentWeaponDamage = 25;

  [HideInInspector] public CharacterManager characterManager;

  private Collider damageCollider;

  private void Awake()
  {
    damageCollider = GetComponent<Collider>();
    damageCollider.gameObject.SetActive(true);
    damageCollider.isTrigger = true;
    damageCollider.enabled = false; 
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
      CharacterManager playerCharacterManager = other.GetComponent<CharacterManager>();
      BlockingCollider shieldCollider = other.transform.GetComponentInChildren<BlockingCollider>();
      PlayerStats playerStats = other.GetComponent<PlayerStats>();

      if(playerCharacterManager != null)
      {
        if(playerCharacterManager.isParrying)
        {
          characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
          return;
        }
        else if(shieldCollider != null && playerCharacterManager.isBlocking)
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
        playerStats.TakeDamage(currentWeaponDamage);      
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
        enemyStats.TakeDamage(currentWeaponDamage);
    }   
  }
}
