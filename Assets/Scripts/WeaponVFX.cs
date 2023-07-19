using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WeaponVFX : MonoBehaviour
{
  [Header("# Weapon VFX")]
  public VisualEffect weaponSlash;

  public void PlayWeaponVFX()
  {
    weaponSlash.Stop();

    if(weaponSlash.pause == false)
    {
      weaponSlash.Play();
    }
  }
}
