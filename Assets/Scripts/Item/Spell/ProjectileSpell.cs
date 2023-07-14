using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TPA/Spells/Make New Projectile Spell")]
public class ProjectileSpell : SpellItem
{
  [Header("Projectile Damage")]
  public float baseDamage;

  [Header("Projectile Physics")]
  public float projectileForwardVelocity;
  public float projectileUpwardVelocity;
  public float projectileMass;
  public bool isEffectedByGravity;

  private Rigidbody rigidbody;

  public override void AttemptToCastSpell(PlayerAnimatorManager animatorHandler, PlayerStats playerStats, WeaponSlotManager weaponSlotManager)
  {
    base.AttemptToCastSpell(animatorHandler, playerStats, weaponSlotManager);

    GameObject warmUpSpellVFX = Instantiate(spellWarmUpFX, weaponSlotManager.rightHandSlot.transform);
    // warmUpSpellVFX.gameObject.transform.localScale = new Vector3(100, 100, 100);
    animatorHandler.PlayTargetAnimation(spellAnimation, true);
  }

  public override void SucessfullyCastSpell(PlayerAnimatorManager animatorHandler, PlayerStats playerStats, CameraHandler cameraHandler, WeaponSlotManager weaponSlotManager)
  {
    base.SucessfullyCastSpell(animatorHandler, playerStats, cameraHandler, weaponSlotManager);
    GameObject castSpellVFX = Instantiate(spellCastFX, weaponSlotManager.rightHandSlot.transform.position, cameraHandler.cameraPivotTransform.rotation);
    rigidbody = castSpellVFX.GetComponent<Rigidbody>();
    // spellDamageCollider = castSpellVFX.GetComponent<SpellDamageCollider>();

    if(cameraHandler.currentLockOnTarget != null)
    {
      castSpellVFX.transform.LookAt(cameraHandler.currentLockOnTarget.transform);
    }
    else
    {
      castSpellVFX.transform.rotation = Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x, playerStats.transform.eulerAngles.y, 0);
    }

    rigidbody.AddForce(castSpellVFX.transform.forward * projectileForwardVelocity);
    rigidbody.AddForce(castSpellVFX.transform.up * projectileUpwardVelocity);
    rigidbody.useGravity = isEffectedByGravity;
    rigidbody.mass = projectileMass;

    castSpellVFX.transform.parent = null;
  }
}
