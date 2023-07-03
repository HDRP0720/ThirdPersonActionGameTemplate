using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
  [Header("# Lock On Target Transform")]
  public Transform lockOnTransform;

  [Header("# Combat Colliders")]
  public BoxCollider backStabBoxCollider;
  [HideInInspector] public BackStabCollider backStabCollider;
}
