using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class vehicleMovement : MonoBehaviour
{
    public float motorForce = 1500f;      // Force applied to move the car forward/backward
    public float brakeForce = 3000f;      // Force applied to stop the car
    public float maxSteerAngle = 30f;     // Maximum steering angle

    [Header("Wheel Colliders")]
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    [Header("Wheel Meshes")]
    public Transform frontLeftMesh;
    public Transform frontRightMesh;
    public Transform rearLeftMesh;
    public Transform rearRightMesh;

    private float currentAcceleration = 0f;
    private float currentSteeringAngle = 0f;

    private Rigidbody _rb;
    
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    void HandleMotor()
    {
        currentAcceleration = motorForce * inputController.Instance.MoveInput.y;   // Move forward or backward
        rearLeftWheel.motorTorque = currentAcceleration;
        rearRightWheel.motorTorque = currentAcceleration;

        if (inputController.Instance.BrakeInput)
        {
            frontLeftWheel.brakeTorque = brakeForce;
            frontRightWheel.brakeTorque = brakeForce;
            rearLeftWheel.brakeTorque = brakeForce;
            rearRightWheel.brakeTorque = brakeForce;
        }
        else
        {
            frontLeftWheel.brakeTorque = 0;
            frontRightWheel.brakeTorque = 0;
            rearLeftWheel.brakeTorque = 0;
            rearRightWheel.brakeTorque = 0;
        }
    }

    void HandleSteering()
    {
        currentSteeringAngle = maxSteerAngle * inputController.Instance.MoveInput.x;   // Steer left or right
        frontLeftWheel.steerAngle = currentSteeringAngle;
        frontRightWheel.steerAngle = currentSteeringAngle;
    }

    void UpdateWheels()
    {
        UpdateWheelPose(frontLeftWheel, frontLeftMesh);
        UpdateWheelPose(frontRightWheel, frontRightMesh);
        UpdateWheelPose(rearLeftWheel, rearLeftMesh);
        UpdateWheelPose(rearRightWheel, rearRightMesh);
    }

    void UpdateWheelPose(WheelCollider collider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        collider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }
}
