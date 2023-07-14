using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDamageCollider : DamageCollider
{
  public GameObject impactParticle;
  public GameObject projectileParticle;
  public GameObject muzzleParticle;

  private bool hasCollided = false;
  private Rigidbody rb;
  private CharacterStats spellTarget;
  private Vector3 impactNormal; // used to rotate impactParticle

  private void Awake() 
  {
    rb = GetComponent<Rigidbody>();
  }
  private void Start() 
  {
    projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation);
    projectileParticle.transform.parent = transform;

    if(muzzleParticle)
    {
      muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation);
      Destroy(muzzleParticle, 2f);
    }
  }

  private void OnCollisionEnter(Collision other)
  {
    if(!hasCollided)
    {
      spellTarget = other.transform.GetComponent<CharacterStats>();
      if(spellTarget != null)      
        spellTarget.TakeDamage(currentWeaponDamage);
      
      hasCollided = true;
      impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal));

      Destroy(projectileParticle);
      Destroy(impactParticle, 5f);
      Destroy(gameObject, 5f);
    }
  }
}