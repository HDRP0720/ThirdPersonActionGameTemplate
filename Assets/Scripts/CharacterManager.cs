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

  [Header("# Interation Flag")]
  public bool isInteracting;

  [Header("# Combat Flags")]
  public bool canBeRiposted;
  public bool canBeParried;
  public bool canDoCombo;
  public bool isParrying;
  public bool isBlocking;
  public bool isInvulnerable;
  public bool isUsingRightHand;
  public bool isUsingLeftHand;

  [Header("# Movement Flags")]
  public bool isRotatingWithRootMotion;
  public bool canRotate;
  public bool isSprinting;
  public bool isInAir;
  public bool isGrounded;

  [Header("# Spell Flags")]
  public bool isFiringSpell;

  // Damage will be inflicted during an animation event
  // Used in backstab or riposte animations
  public int pendingCriticalDamage;
}
