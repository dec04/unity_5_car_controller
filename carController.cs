using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public Transform visualLeftWheel;
    public Transform visualRightWheel;
    public bool motor;
    public bool steering;
}

[System.Serializable]
public class CarInfo
{
    public float currentTorque;
    public float currentSpeed;
    public float currentSteerAngle;
}

public class CarController : MonoBehaviour
{
    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public CarInfo carInfos;

    public void MoveVisualWheels(WheelCollider collider, Transform wheelTransform)
    {
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        wheelTransform.transform.position = position;
        wheelTransform.transform.rotation = rotation;
    }

    public void FixedUpdate()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        Rigidbody rb = GetComponent<Rigidbody>();
        carInfos.currentTorque = motor;
        carInfos.currentSpeed = Vector3.Dot(rb.velocity, transform.forward);


        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }

            MoveVisualWheels(axleInfo.leftWheel, axleInfo.visualLeftWheel);
            MoveVisualWheels(axleInfo.rightWheel, axleInfo.visualRightWheel);
        }

    }
}
