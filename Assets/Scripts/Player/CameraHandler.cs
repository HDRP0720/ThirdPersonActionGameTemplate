using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
  public static CameraHandler instance;

  [Header("# For Player")]
  public Transform targetTransform;
  public Transform cameraTransform;
  public Transform cameraPivotTransform;
  public LayerMask ignoreLayers;
  public LayerMask environmentLayer;

  [Header("# Camera movement")]
  public float lookSpeed = 0.02f;
  public float followSpeed = 0.1f;
  public float privotSpeed = 0.03f;

  public float minimumPivot = -35;
  public float maximumPivot = 35;

  public float cameraSphereRadius = 0.2f;
  public float cameraCollisionOffset = 0.2f;
  public float minimumCollisionOffset = 0.2f;
  public float lockedPivotPosition = 2.25f;
  public float unlockedPivotPosition = 1.65f;

  [Header("For Lock-On System")]
  public List<CharacterManager> availableTargets = new List<CharacterManager>();
  public CharacterManager nearestLockOnTarget;
  public CharacterManager currentLockOnTarget;
  public CharacterManager leftLockOnTarget;
  public CharacterManager rightLockOnTarget;

  public float maximumLockOnDistance = 30f;

  private Transform myTranform;
  private Vector3 cameraTransformPosition;

  private float defaultPosition;
  private float targetPosition;
  private float lookAngle;
  private float pivotAngle;

  private Vector3 cameraFollowVelocity = Vector3.zero;

  private InputHandler inputHandler;
  private PlayerManager playerManager;

  private void Awake() 
  {
    instance = this;
    myTranform = transform;
    defaultPosition = cameraTransform.localPosition.z;
    ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10 | 1 << 13);

    targetTransform = FindObjectOfType<PlayerManager>().transform;
    inputHandler = FindObjectOfType<InputHandler>();
    playerManager = FindObjectOfType<PlayerManager>();
  }
  private void Start() 
  {
    environmentLayer = LayerMask.NameToLayer("Environment");
  }

  public void FollowTarget(float delta)
  {
    // Vector3 targetPosition = Vector3.Lerp(myTranform.position, targetTransform.position, delta / followSpeed);

    Vector3 targetPosition = Vector3.SmoothDamp(myTranform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
    myTranform.position = targetPosition;

    HandleCameraCollisions(delta);
  }
  private void HandleCameraCollisions(float delta)
  {
    targetPosition = defaultPosition;
    RaycastHit hit;
    Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
    direction.Normalize();
    if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition), ignoreLayers))
    {
      float distance = Vector3.Distance(cameraPivotTransform.position, hit.point);
      targetPosition = -(distance - cameraCollisionOffset);
    }

    if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
      targetPosition = -minimumCollisionOffset;

    cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
    cameraTransform.localPosition = cameraTransformPosition;
  }

  public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
  {
    if(!inputHandler.lockOnFlag && currentLockOnTarget == null)
    {
      lookAngle += (mouseXInput * lookSpeed) / delta;
      pivotAngle -= (mouseYInput * privotSpeed) / delta;
      pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

      Vector3 rotation = Vector3.zero;
      rotation.y = lookAngle;
      Quaternion targetRotation = Quaternion.Euler(rotation);
      myTranform.rotation = targetRotation;

      rotation = Vector3.zero;
      rotation.x = pivotAngle;

      targetRotation = Quaternion.Euler(rotation);
      cameraPivotTransform.localRotation = targetRotation;
    }
    else
    {
      // float velocity = 0f;

      Vector3 dir = currentLockOnTarget.transform.position - transform.position;
      dir.Normalize();
      dir.y = 0;

      Quaternion targetRotation = Quaternion.LookRotation(dir);
      transform.rotation = targetRotation;

      dir = currentLockOnTarget.transform.position - cameraPivotTransform.position;
      dir.Normalize();

      targetRotation = Quaternion.LookRotation(dir);
      Vector3 eulerAngle = targetRotation.eulerAngles;
      eulerAngle.y = 0;
      cameraPivotTransform.localEulerAngles = eulerAngle;
    }    
  }

  public void HandleLockOn()
  {
    float shotestDistance = Mathf.Infinity;
    float shortestDistanceOfLeftTarget = -Mathf.Infinity;
    float shortestDistanceOfRightTarget = Mathf.Infinity;

    Collider[] colliders = Physics.OverlapSphere(targetTransform.position, 26);

    for (int i = 0; i < colliders.Length; i++)
    {
      CharacterManager characterManager = colliders[i].GetComponent<CharacterManager>();
      if(characterManager != null)
      {
        Vector3 lockTargetDirection = characterManager.transform.position - targetTransform.position;
        float distanceFromTarget = Vector3.Distance(targetTransform.position, characterManager.transform.position);
        float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);
        RaycastHit hit;

        if(characterManager.transform.root != targetTransform.transform.root 
          && viewableAngle > -50 && viewableAngle < 50 
          && distanceFromTarget <= maximumLockOnDistance)
        {
          if(Physics.Linecast(playerManager.lockOnTransform.position, characterManager.lockOnTransform.position, out hit))
          {
            Debug.DrawLine(playerManager.lockOnTransform.position, characterManager.lockOnTransform.position);
            if(hit.transform.gameObject.layer == environmentLayer)
            {
              // TODO: cannot lock onto target, object in the way
            }
            else
            {
              availableTargets.Add(characterManager);
            }
          }
        }
      }
    }

    for (int k = 0; k < availableTargets.Count; k++)
    {
      float distanceFromTarget = Vector3.Distance(targetTransform.position, availableTargets[k].transform.position);
      if(distanceFromTarget < shotestDistance)
      {
        shotestDistance = distanceFromTarget;
        nearestLockOnTarget = availableTargets[k];
      }

      if(inputHandler.lockOnFlag)
      {
        // Vector3 relativeEnemyPosition = currentLockOnTarget.transform.InverseTransformPoint(availableTargets[k].transform.position);
        // var distanceFromLeftTarget = currentLockOnTarget.transform.position.x - availableTargets[k].transform.position.x;
        // var distanceFromRightTarget = currentLockOnTarget.transform.position.x + availableTargets[k].transform.position.x;
        Vector3 relativeEnemyPosition = inputHandler.transform.InverseTransformPoint(availableTargets[k].transform.position);
        var distanceFromLeftTarget = relativeEnemyPosition.x;
        var distanceFromRightTarget = relativeEnemyPosition.x;

        if(relativeEnemyPosition.x <= 0.00 && distanceFromLeftTarget > shortestDistanceOfLeftTarget && availableTargets[k] != currentLockOnTarget)
        {
          shortestDistanceOfLeftTarget = distanceFromLeftTarget;
          leftLockOnTarget = availableTargets[k];
        }
        else if (relativeEnemyPosition.x >= 0.00 && distanceFromLeftTarget < shortestDistanceOfRightTarget && availableTargets[k] != currentLockOnTarget)
        {
          shortestDistanceOfRightTarget = distanceFromLeftTarget;
          rightLockOnTarget = availableTargets[k];
        }
      }
    }
  }

  public void ClearLockOnTargets()
  {
    availableTargets.Clear();
    currentLockOnTarget = null;
    nearestLockOnTarget = null;
  }

  public void SetCameraHeight()
  {
    Vector3 velocity = Vector3.zero;
    Vector3 newLockedPosition = new Vector3(0, lockedPivotPosition);
    Vector3 newUnlockedPosition = new Vector3(0, unlockedPivotPosition);

    if(currentLockOnTarget != null)    
      cameraPivotTransform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.localPosition, newLockedPosition, ref velocity, Time.deltaTime);    
    else    
      cameraPivotTransform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.localPosition, newUnlockedPosition, ref velocity, Time.deltaTime);    
  }
}