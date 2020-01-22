using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AirFoil : MonoBehaviour
{
    Rigidbody body;
    public BoxCollider shapeReferenceCollider = null;
    //http://www.aerospaceweb.org/question/airfoils/q0150b.shtml
    public AnimationCurve liftCoefficient;
    public AnimationCurve dragCoefficient;
    public AnimationCurve aerodynamicCenter;
    public float chordLength = 1;
    public float sectionLength = 1;
    public float damping = 0f;
    public float maxSpeed = 15f;
    [ReadOnly,SerializeField] private float angleOfAttack = 0;
    [ReadOnly, SerializeField] private float planarFlowSpeed = 0;
    [ReadOnly, SerializeField] private float lift = 0;
    [ReadOnly, SerializeField] private float drag = 0;
    [ReadOnly] public Vector3 force;
    private Vector3 oldForce = Vector3.zero;
    private void Start()
    {
        OnValidate();
        body = GetComponentInParent<Rigidbody>();
        //body.maxAngularVelocity = 3;
    }
    private void OnDrawGizmosSelected()
    {
        OnValidate();
    }
    private void OnValidate()
    {
        if (shapeReferenceCollider)
        {
            chordLength = shapeReferenceCollider.size.z * shapeReferenceCollider.transform.localScale.z;
            sectionLength = shapeReferenceCollider.size.x * shapeReferenceCollider.transform.localScale.x;
        }
    }
    private void FixedUpdate()
    {
        Vector3 foilVelocity = body.GetPointVelocity(transform.position);
        Vector3 airVelocity = Vector3.zero;
        Vector3 localFlow = transform.InverseTransformDirection(airVelocity - foilVelocity);
        localFlow.x = 0;
        planarFlowSpeed = localFlow.magnitude;
        angleOfAttack = planarFlowSpeed > 0 ? -Mathf.Atan2(-localFlow.y, -localFlow.z) * Mathf.Rad2Deg : 0;
        float airDensity = 1.225f;
        float coeff1 = 0.5f * airDensity * planarFlowSpeed * planarFlowSpeed * chordLength*sectionLength;
        float coeff2 = 0.5f * airDensity * Mathf.Pow(Mathf.Clamp(planarFlowSpeed,0,maxSpeed),2)  * chordLength * sectionLength;

        drag = dragCoefficient.Evaluate(angleOfAttack) * coeff1;
        lift = liftCoefficient.Evaluate(angleOfAttack) * coeff2;
        Vector3 localForce = Quaternion.Euler(angleOfAttack,0,0)*new Vector3(0, lift, -drag);
        force = transform.TransformDirection(localForce);
        force = force * (1 - damping) + oldForce * damping;
        oldForce = force;

        body.AddForceAtPosition(force, transform.position);

        Debug.DrawRay(transform.position, transform.TransformDirection(localFlow).normalized, Color.blue);
        Debug.DrawRay(transform.position, transform.TransformDirection(localForce), Color.green);
        Debug.DrawRay(body.position, body.velocity.normalized, Color.red);
    }
}

