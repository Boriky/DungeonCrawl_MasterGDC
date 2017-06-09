using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTiltController : MonoBehaviour {
    public bool m_keyboardControls;
    // Smoothly tilts a transform towards a target rotation.
    public float m_smoothTilt = 3.0f;
    public float m_tiltAngle = 30.0f;

    private Camera m_mainCamera;

    private float m_xStart = 0.0f;
    private float m_zStart = 0.0f;

    void Start()
    {
        AccelerometerCalibration();
        m_mainCamera = Camera.main;
    }

    void Update()
    {
        float tiltAroundZ;
        float tiltAroundX;
        if (m_keyboardControls)
        {
            tiltAroundZ = Input.GetAxis("Horizontal") * m_tiltAngle;
            tiltAroundX = Input.GetAxis("Vertical") * m_tiltAngle;
        }
        else
        {
            tiltAroundZ = (Input.acceleration.z /*- m_zStart*/) * m_tiltAngle;
            tiltAroundX = (Input.acceleration.y /*- m_xStart*/) * m_tiltAngle;
        }
        Transform cameraTransform = m_mainCamera.transform;

        // Get the Z and X axes of the Camera
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // Project and normalize the Z and X axes on the level plane
        Vector3 cameraForwardProj = Vector3.ProjectOnPlane(cameraForward, Vector3.up);
        cameraForwardProj.Normalize();
        Vector3 cameraRightProj = Vector3.ProjectOnPlane(cameraRight, Vector3.up);
        cameraRightProj.Normalize();
        
        // Rotate the level based on the axes of the Camera
        Quaternion rotationAroundZ = Quaternion.AngleAxis(-tiltAroundZ, cameraForwardProj);
        Quaternion rotationAroundX = Quaternion.AngleAxis(tiltAroundX, cameraRightProj);

        Quaternion targetRotation = rotationAroundZ * rotationAroundX;

        // Interpolate towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * m_smoothTilt);
    }

    void AccelerometerCalibration() 
    {
        m_xStart = Input.acceleration.x;
        m_zStart = Input.acceleration.z;
    }
}
