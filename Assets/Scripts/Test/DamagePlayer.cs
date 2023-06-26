using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
  public int damage = 25;
  private void OnTriggerEnter(Collider other) 
  {
    PlayerStats playerStats = other.GetComponent<PlayerStats>();

    playerStats?.TakeDamage(damage);
  }
}