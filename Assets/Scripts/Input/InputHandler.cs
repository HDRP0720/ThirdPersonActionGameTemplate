using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
  public float horizontal;
  public float vertical;
  public float moveAmount;
  public float mouseX;
  public float mouseY;

  public bool roll_Input;
  public bool twoHand_Input;
  public bool lightAttack_Input;
  public bool heavyAttack_Input;

  public bool loot_Input;
  public bool jump_Input;
  public bool inventory_Input;
  public bool drink_Input;

  public bool criticalAttack_Input;
  public bool parry_Input;
  public bool block_Input;

  public bool lockOn_Input;
  public bool lockOnLeft_Input;
  public bool lockOnRight_Input;

  public bool d_Pad_Up;
  public bool d_Pad_Down;  
  public bool d_Pad_Left;
  public bool d_Pad_Right;

  public bool rollFlag;
  public bool twoHandFlag;
  public bool sprintFlag;
  public bool comboFlag;
  public bool inventoryFlag;
  public bool lockOnFlag;

  public float rollInputTimer;

  public Transform criticalAttackRayCastStartPoint;

  private PlayerControls inputActions;

  private PlayerManager playerManager;
  private PlayerStatsManager playerStats;
  private PlayerInventoryManager playerInventory;

  private PlayerAnimatorManager playerAnimatorManager;
  private PlayerEffectsManager playerEffectsManager;
  private PlayerCombatManager playerAttackState;
  private PlayerWeaponSlotManager weaponSlotManager;
  private BlockingCollider blockingCollider;

  private CameraHandler cameraHandler;
  private UIManager uiManager;

  private Vector2 movementInput;
  private Vector2 cameraInput;

  private void Awake() 
  {   
    playerManager = GetComponent<PlayerManager>();    
    playerInventory = GetComponent<PlayerInventoryManager>();
    playerStats = GetComponent<PlayerStatsManager>();
    playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
    playerEffectsManager = GetComponent<PlayerEffectsManager>();
    playerAttackState = GetComponent<PlayerCombatManager>();
    weaponSlotManager = GetComponent<PlayerWeaponSlotManager>();

    blockingCollider = GetComponentInChildren<BlockingCollider>();

    cameraHandler = FindObjectOfType<CameraHandler>();
    uiManager = FindObjectOfType<UIManager>();
  }

  private void OnEnable() 
  {
    if(inputActions == null)
    {
      inputActions = new PlayerControls();
      inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
      inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

      inputActions.PlayerActions.Roll.performed += i => roll_Input = true;
      inputActions.PlayerActions.Roll.canceled += i => roll_Input = false;
      
      inputActions.PlayerActions.TwoHand.performed += i => twoHand_Input = true;
      inputActions.PlayerActions.LightAttack.performed += i => lightAttack_Input = true;
      inputActions.PlayerActions.HeavyAttack.performed += i => heavyAttack_Input = true;
      inputActions.PlayerActions.ShieldBlock.performed += i => block_Input = true;
      inputActions.PlayerActions.ShieldBlock.canceled += i => block_Input = false;

      inputActions.PlayerQuickslots.DPadLeft.performed += i => d_Pad_Left = true;
      inputActions.PlayerQuickslots.DPadRight.performed += i => d_Pad_Right = true;

      inputActions.PlayerActions.Loot.performed += i => loot_Input = true;
      inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
      inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;
      inputActions.PlayerActions.CriticalAttack.performed += i => criticalAttack_Input = true;
      inputActions.PlayerActions.Parry.performed += i => parry_Input = true;
      inputActions.PlayerActions.Drink.performed += i => drink_Input = true;

      inputActions.PlayerActions.LockOn.performed += i => lockOn_Input = true;
      inputActions.PlayerActions.LockOnLeftTarget.performed += i => lockOnLeft_Input = true;
      inputActions.PlayerActions.LockOnRightTarget.performed += i => lockOnRight_Input = true;
    }

    inputActions.Enable();
  }
  private void OnDisable() 
  {
    inputActions.Disable();
  }

  public void TickInput(float delta)
  {
    HandleMoveInput(delta);
    HandleRollInput(delta);
    HandleCombatInput(delta);
    HandleQuickSlotsInput();
    HandleInventoryInput();
    HandleLockOnInput();
    HandleTwoHandInput();
    HandleCriticalAttackInput();
    HandleUseConsumableInput();
  }

  private void HandleMoveInput(float delta)
  {
    horizontal = movementInput.x;
    vertical = movementInput.y;

    moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

    mouseX = cameraInput.x;
    mouseY = cameraInput.y;    
  }

  private void HandleRollInput(float delta)
  {
    if(roll_Input)
    {
      rollInputTimer += delta;

      if(playerStats.currentStamina <= 0)
      {
        roll_Input = false;
        sprintFlag = false;
      }

      if(moveAmount > 0.5f && playerStats.currentStamina > 0)      
        sprintFlag = true;      
    }
    else
    {
      sprintFlag = false;

      if(rollInputTimer > 0 && rollInputTimer < 0.5f)      
        rollFlag = true;      

      rollInputTimer = 0;
    }  
  }

  private void HandleCombatInput(float delta)
  {
    if(lightAttack_Input)
    {
      playerAttackState.HandleLightAction();
    }

    if(heavyAttack_Input)
    {
      if (playerManager.isInteracting) return;

      if (playerManager.canDoCombo) return;

      playerAnimatorManager.animator.SetBool("isUsingRightHand", true);
      playerAttackState.HandleHeavyAttack(playerInventory.rightWeapon);
    }

    if(block_Input)
    {
      playerAttackState.HandleBlockingAction();
    }
    else
    {
      playerManager.isBlocking = false;
      if(blockingCollider.blockingCollider.enabled)
      {
        blockingCollider.DisableBlockingCollider();
      }
    }

    if(parry_Input)
    {
      if(twoHandFlag)
      {
        // TODO: Handle weapon arts
      }
      else
      {
        playerAttackState.HandleParryAction();
      } 
    }
  }

  private void HandleQuickSlotsInput()
  {
    if(d_Pad_Right)
    {
      playerInventory.ChangeRightWeapon();
    }
    else if(d_Pad_Left)
    {
      playerInventory.ChangeLeftWeapon();
    }
  }

  private void HandleInventoryInput()
  {
    if(inventory_Input)
    {
      inventoryFlag = !inventoryFlag;
      if(inventoryFlag)
      {
        uiManager.OpenSelectableWindow();
        uiManager.UpdateUI();
        uiManager.hudWindow.SetActive(false);
      }
      else
      {
        uiManager.CloseSelectableWindow();
        uiManager.CloseAllInventoryWindows();
        uiManager.hudWindow.SetActive(true);
      }
    }
  }

  private void HandleLockOnInput()
  {
    if(lockOn_Input && !lockOnFlag)
    {
      lockOn_Input = false;
      cameraHandler.HandleLockOn();
      if(cameraHandler.nearestLockOnTarget != null)
      {
        cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
        lockOnFlag = true;
      }
    }
    else if(lockOn_Input && lockOnFlag)
    {
      lockOn_Input = false;
      lockOnFlag = false;

      cameraHandler.ClearLockOnTargets();
    }

    if(lockOnFlag && lockOnLeft_Input)
    {
      lockOnLeft_Input = false;
      cameraHandler.HandleLockOn();
      if(cameraHandler.leftLockOnTarget !=null)
      {
        cameraHandler.currentLockOnTarget = cameraHandler.leftLockOnTarget;
      }
    }

    if (lockOnFlag && lockOnRight_Input)
    {
      lockOnRight_Input = false;
      cameraHandler.HandleLockOn();
      if (cameraHandler.rightLockOnTarget != null)
      {
        cameraHandler.currentLockOnTarget = cameraHandler.rightLockOnTarget;
      }
    }

    cameraHandler.SetCameraHeight();
  }

  private void HandleTwoHandInput()
  {
    if(twoHand_Input)
    {
      twoHand_Input = false;
      twoHandFlag = !twoHandFlag;

      if(twoHandFlag)
      {
        // TODO: Change to Two Hand mode
        weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
      }
      else
      {
        // TODO: Change to one Hand mode
        weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);
        weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
      }
    }
  }

  private void HandleCriticalAttackInput()
  {
    if(criticalAttack_Input)
    {
      criticalAttack_Input = false;

      playerAttackState.AttemptBackStabOrRiposte();
    }
  }

  private void HandleUseConsumableInput()
  {
    if(drink_Input)
    {
      drink_Input = false;
      playerInventory.currentConsumableItem.AttemptToConsumeItem(playerAnimatorManager, weaponSlotManager, playerEffectsManager);
    }
  }
}