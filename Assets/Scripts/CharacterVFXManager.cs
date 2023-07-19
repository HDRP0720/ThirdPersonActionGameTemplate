using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVFXManager : MonoBehaviour
{
  public WeaponVFX leftWeaponVFX;
  public WeaponVFX rightWeaponVFX; 

  public virtual void PlayWeaponVFX(bool isLeft)
  {
    if(isLeft == false)
    {
      if(rightWeaponVFX != null)
        rightWeaponVFX.PlayWeaponVFX();
    }
    else
    {
      if(leftWeaponVFX != null)
        leftWeaponVFX.PlayWeaponVFX();
    }
  }
}
