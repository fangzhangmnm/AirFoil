using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AircraftControlSimple : MonoBehaviour
{
    public Transform LeftAileron, RightAileron, HorizontalAileron, VerticalAileron;
    public Thruster thruster;
    public Rigidbody body;
    public float rollSensitivity = 15f;
    public float pitchSensitivity = 15f;
    public float yawSensitivity = 15f;
    public Text UIText;
    public Transform CenterOfMass = null;
    AirFoil[] airFoils;
    [Range(0, 1)]
    public float throttleInput;
    [Range(-1,1)]
    public float rollInput, pitchInput, yawInput;
    public bool playerInput = false;
    private void Start()
    {
        airFoils = GetComponentsInChildren<AirFoil>();
        if (CenterOfMass)
        {
            body.centerOfMass = transform.InverseTransformPoint(CenterOfMass.position);
        }
    }
    private void FixedUpdate()
    {
        thruster.throttle = throttleInput;
        LeftAileron.localRotation = Quaternion.Euler(rollInput * rollSensitivity, 0, 0);
        RightAileron.localRotation = Quaternion.Euler(-rollInput * rollSensitivity, 0, 0);
        HorizontalAileron.localRotation = Quaternion.Euler(-pitchInput * pitchSensitivity, 0, 0);
        VerticalAileron.localRotation = Quaternion.Euler(-yawInput * yawSensitivity, 0, 0);
    }
    private void Update()
    {
        if (playerInput)
        {
            if (Input.GetKeyDown(KeyCode.W))
                throttleInput = Mathf.Clamp01(throttleInput + 0.1f);
            if (Input.GetKeyDown(KeyCode.S))
                throttleInput = Mathf.Clamp01(throttleInput - 0.1f);

            yawInput = Input.GetAxis("Horizontal");
            rollInput = -Mathf.Clamp((Input.mousePosition.x * 2 - Screen.width) / Screen.height, -1, 1);
            pitchInput = Mathf.Clamp(Input.mousePosition.y / Screen.height * 2 - 1, -1, 1);

        }
        if (UIText != null)
        {
            Vector3 force = Vector3.zero;
            foreach (var f in airFoils)
                force += f.force;
            UIText.text = string.Format("Throttle: {0:P0} {1:F1}G\nSpeed: {2:F1}\nClimb Rate:{3:F1}\nAltitude:{4:F1}\nOverload:{5:F1}G",
                thruster.throttle, thruster.thrust / body.mass / 9.81,
                body.velocity.magnitude,
                body.velocity.y,
                body.position.y,
                Vector3.Dot(transform.up, force) / body.mass / 9.81f);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.TransformPoint(body.centerOfMass), .01f);
    }
}
