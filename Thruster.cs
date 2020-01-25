using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    //https://wenku.baidu.com/view/f3701926854769eae009581b6bd97f192279bf9f.html
    //https://www.zhihu.com/question/316834572
    //https://www.grc.nasa.gov/WWW/K-12/airplane/propanl.html
    //https://www.grc.nasa.gov/WWW/K-12/airplane/propth.html
    public float diameter = 0.2f;//8 inch
    public float pitch = 0.15f;//6 inch
    public float propellerInertia = 0.01f *0.15f*0.15f / 12;
    public int rotationDirection = 1;
    public float propellerEfficiency = 0.9f;
    public float dragTorqueCoefficient = 1f;//0 for turbojet

    public float maxRPM = 6000f;//15 m/s
    [Range(0, 1)] public float throttle = 0;

    [ReadOnly, SerializeField] private float airSpeed;
    [ReadOnly, SerializeField] private float geometricalMaxSpeed;
    [ReadOnly, SerializeField] private float geometricalSpeed;
    [ReadOnly, SerializeField] private float airDensity;
    [ReadOnly, SerializeField] public float rpm;
    [ReadOnly, SerializeField] public float thrust;
    [ReadOnly, SerializeField] public float dragTorque;
    [ReadOnly, SerializeField] public float precessionTorque;

    Rigidbody body;
    private void Start()
    {
        body = GetComponentInParent<Rigidbody>();
    }
    private void OnValidate()
    {
        geometricalMaxSpeed = maxRPM / 60 * pitch;
    }
    private void FixedUpdate()
    {
        Vector3 pointVelocity = body.GetPointVelocity(transform.position);
        Vector3 airVelocity = Vector3.zero;
        Vector3 localFlow = transform.InverseTransformDirection(airVelocity - pointVelocity);
        airSpeed = -localFlow.z;
        airDensity = AirDensitySetting.getDensity(transform.position.y);//1.225f
        rpm = maxRPM * throttle;
        float rotationSpeed = rpm / 30 * Mathf.PI;

        //suppose rpm is large enough so aoa is small enough that outflow speend is just geometricalSpeed
        //also suppose 
        geometricalSpeed = rpm / 60 * pitch;
        float area = Mathf.PI * diameter * diameter / 4;
        thrust = airDensity * area * 2* geometricalSpeed*(geometricalSpeed - airSpeed);
        //thrust can be lesser than zero
        
        dragTorque = thrust * pitch/(2*Mathf.PI)/ propellerEfficiency * -rotationDirection* dragTorqueCoefficient;
        Vector3 angularMomentum = rotationSpeed * propellerInertia* rotationDirection* transform.forward;
        Vector3 precessionTorqueVector= -Vector3.Cross(body.angularVelocity, angularMomentum);
        precessionTorque = precessionTorqueVector.magnitude;
        Vector3 totalTorque = precessionTorqueVector + dragTorque * transform.forward;

        body.AddForceAtPosition(transform.forward * thrust, transform.position);
        body.AddTorque(totalTorque);
    }
}
