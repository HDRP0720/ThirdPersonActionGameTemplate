using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsManager : MonoBehaviour
{
  public GameObject currentVFX;
  public GameObject instaniatedItemPrefab;
  public int amountToBeHealed;

  private PlayerStats playerStats;
  private WeaponSlotManager weaponSlotManager;

  private void Awake() 
  {
    playerStats = GetComponentInParent<PlayerStats>();
    weaponSlotManager = GetComponent<WeaponSlotManager>();
  }

  public void HealPlayerFromEffect()
  {
    playerStats.HealPlayer(amountToBeHealed);
    GameObject healVFX = Instantiate(currentVFX, playerStats.transform);
    Destroy(instaniatedItemPrefab, 1.5f);
    StartCoroutine(LoadPrevWeapons());
  }

  private IEnumerator LoadPrevWeapons()
  {
    yield return new WaitForSeconds(1.5f);
    weaponSlotManager.LoadBothWeaponsOnSlots();
  }
}