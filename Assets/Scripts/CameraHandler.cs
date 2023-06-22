using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
  public static CameraHandler instance;

  public Transform targetTransform;
  public Transform cameraTransform;
  public Transform cameraPivotTransform;

  public float lookSpeed = 0.1f;
  public float followSpeed = 0.1f;
  public float privotSpeed = 0.03f;

  public float minimumPivot = -35;
  public float maximumPivot = 35;

  private Transform myTranform;
  private Vector3 cameraTransformPosition;
  private LayerMask ignoreLayers;  

  private float defaultPosition;
  private float lookAngle;
  private float pivotAngle;

  private void Awake() 
  {
    instance = this;
    myTranform = transform;
    defaultPosition = cameraTransform.localPosition.z;
    ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);    
  }

  public void FollowTarget(float delta)
  {
    Vector3 targetPosition = Vector3.Lerp(myTranform.position, targetTransform.position, delta / followSpeed);
    myTranform.position = targetPosition;
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
