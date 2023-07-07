using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
  [Header("# Lock On Target Transform")]
  public Transform lockOnTransform;

  [Header("# Combat Colliders")]
  public SpecialAttackCollider backStabCollider;
  public SpecialAttackCollider riposteCollider;

  [Header("# Combat Flags")]
  public bool canBeRiposted;
  public bool canBeParried;
  public bool isParrying;

  // Damage will be inflicted during an animation event
  // Used in backstab or riposte animations
  public int pendingCriticalDamage;
}
