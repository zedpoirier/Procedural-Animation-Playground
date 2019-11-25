// https://www.weaverdev.io/tutorials/procedural-animation/
using UnityEngine;
public class GeckoController : MonoBehaviour
{
    [SerializeField] Transform target;

    [Header("Head Data")]
    [SerializeField] Transform headBone;
    [SerializeField] float headMaxTurnAngle;
    [SerializeField] float headTrackingSpeed;

    [Header("Eye Data")]
    [SerializeField] Transform leftEyeBone;
    [SerializeField] Transform rightEyeBone;
    [SerializeField] float eyeTrackingSpeed;
    [SerializeField] float leftEyeMaxYRotation;
    [SerializeField] float leftEyeMinYRotation;
    [SerializeField] float rightEyeMaxYRotation;
    [SerializeField] float rightEyeMinYRotation;

    void LateUpdate()
    {
        HeadTrackingUpdate();
        EyeTrackingUpdate();
    }

    void HeadTrackingUpdate()
    {
        Vector3 targetLookDir = target.position - headBone.position;

        // Apply angle limit clamp
        targetLookDir = Vector3.RotateTowards(
            Vector3.forward,
            targetLookDir,
            Mathf.Deg2Rad * headMaxTurnAngle,
            0 // We don't care about the length here, so we leave it at zero
        );

        // Get the local rotation by using LookRotation on a local directional vector
        Quaternion targetHeadRotation = Quaternion.LookRotation(
            targetLookDir,
            transform.up);

        // Apply smoothing
        headBone.localRotation = Quaternion.Slerp(
            headBone.localRotation,
            targetHeadRotation,
            1 - Mathf.Exp(-headTrackingSpeed * Time.deltaTime)
        );
    }

    void EyeTrackingUpdate()
    {
        Quaternion targetEyeRotation = Quaternion.LookRotation(
            target.position - headBone.position,
            transform.up);

        leftEyeBone.rotation = Quaternion.Slerp(
            leftEyeBone.rotation,
            targetEyeRotation,
            1 - Mathf.Exp(-eyeTrackingSpeed * Time.deltaTime));

        rightEyeBone.rotation = Quaternion.Slerp(
            rightEyeBone.rotation,
            targetEyeRotation,
            1 - Mathf.Exp(-eyeTrackingSpeed * Time.deltaTime));

        float leftEyeCurrentYRotation = leftEyeBone.localEulerAngles.y;
        float rightEyeCurrentYRotation = rightEyeBone.localEulerAngles.y;

        // Move the rotation to a -180 ~ 180 range
        if (leftEyeCurrentYRotation > 180)
        {
            leftEyeCurrentYRotation -= 360;
        }
        if (rightEyeCurrentYRotation > 180)
        {
            rightEyeCurrentYRotation -= 360;
        }

        // Clamp the Y axis rotation
        float leftEyeClampedYRotation = Mathf.Clamp(
            leftEyeCurrentYRotation,
            leftEyeMinYRotation,
            leftEyeMaxYRotation
        );
        float rightEyeClampedYRotation = Mathf.Clamp(
            rightEyeCurrentYRotation,
            rightEyeMinYRotation,
            rightEyeMaxYRotation
        );

        // Apply the clamped Y rotation without changing the X and Z rotations
        leftEyeBone.localEulerAngles = new Vector3(
            leftEyeBone.localEulerAngles.x,
            leftEyeClampedYRotation,
            leftEyeBone.localEulerAngles.z
        );
        rightEyeBone.localEulerAngles = new Vector3(
            rightEyeBone.localEulerAngles.x,
            rightEyeClampedYRotation,
            rightEyeBone.localEulerAngles.z
        );
    }
}
