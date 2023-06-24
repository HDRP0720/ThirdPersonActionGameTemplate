using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
  public static CameraHandler instance;

  public Transform targetTransform;
  public Transform cameraTransform;
  public Transform cameraPivotTransform;

  public float lookSpeed = 0.08f;
  public float followSpeed = 0.1f;
  public float privotSpeed = 0.03f;

  public float minimumPivot = -35;
  public float maximumPivot = 35;

  public float cameraSphereRadius = 0.2f;
  public float cameraCollisionOffset = 0.2f;
  public float minimumCollisionOffset = 0.2f;

  private Transform myTranform;
  private Vector3 cameraTransformPosition;
  private LayerMask ignoreLayers;  

  private float defaultPosition;
  private float targetPosition;
  private float lookAngle;
  private float pivotAngle;

  private Vector3 cameraFollowVelocity = Vector3.zero;

  private void Awake() 
  {
    instance = this;
    myTranform = transform;
    defaultPosition = cameraTransform.localPosition.z;
    ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);    
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
}