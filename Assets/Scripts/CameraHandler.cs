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

  [Header("# Camera movement")]
  public float lookSpeed = 0.01f;
  public float followSpeed = 0.1f;
  public float privotSpeed = 0.03f;

  public float minimumPivot = -35;
  public float maximumPivot = 35;

  public float cameraSphereRadius = 0.2f;
  public float cameraCollisionOffset = 0.2f;
  public float minimumCollisionOffset = 0.2f;

  [Header("For Lock-On System")]
  public List<CharacterManager> availableTargets = new List<CharacterManager>();
  public Transform nearestLockOnTarget;
  public Transform currentLockOnTarget;
  public Transform leftLockOnTarget;
  public Transform rightLockOnTarget;

  public float maximumLockOnDistance = 30f;

  private Transform myTranform;
  private Vector3 cameraTransformPosition;

  private float defaultPosition;
  private float targetPosition;
  private float lookAngle;
  private float pivotAngle;

  private Vector3 cameraFollowVelocity = Vector3.zero;

  private InputHandler inputHandler;

  private void Awake() 
  {
    instance = this;
    myTranform = transform;
    defaultPosition = cameraTransform.localPosition.z;
    ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);

    targetTransform = FindObjectOfType<PlayerManager>().transform;
    inputHandler = FindObjectOfType<InputHandler>();
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
      float velocity = 0f;

      Vector3 dir = currentLockOnTarget.position - transform.position;
      dir.Normalize();
      dir.y = 0;

      Quaternion targetRotation = Quaternion.LookRotation(dir);
      transform.rotation = targetRotation;

      dir = currentLockOnTarget.position - cameraPivotTransform.position;
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
    float shortestDistanceOfLeftTarget = Mathf.Infinity;
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

        if(characterManager.transform.root != targetTransform.transform.root 
          && viewableAngle > -50 && viewableAngle < 50 
          && distanceFromTarget <= maximumLockOnDistance)
        {
          availableTargets.Add(characterManager);
        }
      }
    }

    for (int k = 0; k < availableTargets.Count; k++)
    {
      float distanceFromTarget = Vector3.Distance(targetTransform.position, availableTargets[k].transform.position);
      if(distanceFromTarget < shotestDistance)
      {
        shotestDistance = distanceFromTarget;
        nearestLockOnTarget = availableTargets[k].lockOnTransform;
      }

      if(inputHandler.lockOnFlag)
      {
        Vector3 relativeEnemyPosition = currentLockOnTarget.InverseTransformPoint(availableTargets[k].transform.position);
        var distanceFromLeftTarget = currentLockOnTarget.transform.position.x - availableTargets[k].transform.position.x;
        var distanceFromRightTarget = currentLockOnTarget.transform.position.x + availableTargets[k].transform.position.x;

        if(relativeEnemyPosition.x > 0.00 && distanceFromLeftTarget < shortestDistanceOfLeftTarget)
        {
          shortestDistanceOfLeftTarget = distanceFromLeftTarget;
          leftLockOnTarget = availableTargets[k].lockOnTransform;
        }

        if (relativeEnemyPosition.x < 0.00 && distanceFromLeftTarget < shortestDistanceOfRightTarget)
        {
          shortestDistanceOfRightTarget = distanceFromLeftTarget;
          rightLockOnTarget = availableTargets[k].lockOnTransform;
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
}