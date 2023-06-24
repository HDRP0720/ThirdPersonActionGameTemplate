using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Make New Weapon Item")]
public class WeaponItem : Item
{
  public GameObject weaponPrefab;
  public bool isUnarmed;
}
