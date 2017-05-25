using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTiltController : MonoBehaviour {

    // Smoothly tilts a transform towards a target rotation.
    public float m_smoothTilt = 3.0f;
    public float m_tiltAngle = 30.0f;

    void Update() {
        float tiltAroundZ = Input.GetAxis("Horizontal") * m_tiltAngle;
        float tiltAroundX = Input.GetAxis("Vertical") * m_tiltAngle;
        var target = Quaternion.Euler(tiltAroundX, 0, -tiltAroundZ);
        // Damper towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * m_smoothTilt);
    }
}
