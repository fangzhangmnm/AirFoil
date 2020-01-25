using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AircraftControlSimple : MonoBehaviour
{
    public bool axesToCenter = false;
    public Transform LeftAileron, RightAileron, HorizontalAileron, VerticalAileron;
    public Thruster thruster;
    public Rigidbody body;
    public float rollSensitivity = 15f;
    public float pitchSensitivity = 15f;
    public float yawSensitivity = 15f;
    public Text UIText;
    public Transform CenterOfMass = null;
    AirFoil[] airFoils;
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
        float h = Input.GetAxis("Horizontal");
        float mx = Mathf.Clamp((Input.mousePosition.x*2-Screen.width)/Screen.height, -1, 1);
        float my = Mathf.Clamp(Input.mousePosition.y / Screen.height * 2 - 1, -1, 1);
        if (axesToCenter) { h = mx = my = 0; }

        LeftAileron.localRotation = Quaternion.Euler(-mx * rollSensitivity, 0, 0);
        RightAileron.localRotation = Quaternion.Euler(mx * rollSensitivity, 0, 0);
        HorizontalAileron.localRotation = Quaternion.Euler(-my * pitchSensitivity, 0, 0);
        VerticalAileron.localRotation = Quaternion.Euler(-h * yawSensitivity, 0, 0);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            thruster.throttle = Mathf.Clamp01(thruster.throttle + 0.1f);
        if (Input.GetKeyDown(KeyCode.S))
            thruster.throttle = Mathf.Clamp01(thruster.throttle - 0.1f);
        Vector3 force = Vector3.zero;
        foreach (var f in airFoils)
            force += f.force;
        if(UIText!=null)
            UIText.text = string.Format("Throttle: {0:P0} {1:F1}G\nSpeed: {2:F1}\nClimb Rate:{3:F1}\nAltitude:{4:F1}\nOverload:{5:F1}G",
                thruster.throttle,thruster.thrust/body.mass/9.81, 
                body.velocity.magnitude,
                body.velocity.y,
                body.position.y,
                Vector3.Dot(transform.up, force) / body.mass / 9.81f);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.TransformPoint(body.centerOfMass), .01f);
    }
}
