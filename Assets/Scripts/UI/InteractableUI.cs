using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractableUI : MonoBehaviour
{
  [Header("Interaction")]
  public GameObject interactionPopup;
  public TMP_Text interactableTextField;

  [Header("Item")]
  public GameObject itemPopup;
  public TMP_Text itemTextField;
  public Image itemImage;
}