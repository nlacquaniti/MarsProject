using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CarMovement : MonoBehaviour 
{
    [Header("Potenza motore")]
    [Range (500, 5000)]
    [SerializeField] private float motorTorqueForce;
    [Header("Intensità sterzata")]
    [Range(10, 50)]
    [SerializeField] private float steerAngleAmount;
    [Header("Potenza ammortizzatori")]
    [Range(1000, 5000)]
    [SerializeField] private float damperIntensity;
    [Header("Elasticità ammortizzatori")]
    [Range(100, 20000)]
    [SerializeField] private float damperSpring;
    [Header("Altezza baricentro")]
    [Range(-1, 5)]
    [SerializeField] private float centerOfMassPosition;

    [Header("Gestione carburante")]
    [SerializeField] private float m_MaxFuel;
    [SerializeField] private float m_ReductionRate;
    [SerializeField] private float m_CurrentFuel;

    [Space (10)]
    [Header("Joypad/Keyboard switch")]
    [SerializeField] private enControls controls;

    [Space (20)]
    [Header("Programmers Stuff")]
    [SerializeField] private WheelCollider[] wheels;
    [SerializeField] private GameObject[] wheelModels;

    private GameObject centerOfMass;
    private Vector3 massLocation;
    private enum enControls {Joypad, Keyboard};
    private enum enWheels {FL = 0, FR = 1, RL = 2, RR = 3}
    private float averageRotation;
    private Rigidbody rb;

    public float CurrentFuel { get { return m_CurrentFuel; } }
    public float MaxFuel { get { return m_MaxFuel; } }
    public float ReductionRate { get { return m_ReductionRate; } }
    //private Text txtSpeed;


    void Start ()
	{
        m_CurrentFuel = m_MaxFuel;
        CenterOfMassSettings();
        DamperSettings();
        //txtSpeed = GameObject.Find("txtSpeed").GetComponent<Text>();
	}

    void Update ()
	{
        averageRotation = wheels[(int)enWheels.FL].motorTorque + wheels[(int)enWheels.FR].motorTorque + wheels[(int)enWheels.RL].motorTorque + wheels[(int)enWheels.RR].motorTorque;
        ControlSwitch(controls);
        FuelReduction();
        //SetSpeedText();
	}

    void ControlSwitch (enControls controls)
    {
        switch (controls)
        {
            case enControls.Joypad:
                {
                    if (Input.GetAxis("Accelerate") < 0 && averageRotation > 0)
                    {
                        wheels[(int)enWheels.FL].brakeTorque = Mathf.Abs(Input.GetAxis("Accelerate") * motorTorqueForce);
                        wheels[(int)enWheels.FR].brakeTorque = Mathf.Abs(Input.GetAxis("Accelerate") * motorTorqueForce);
                        wheels[(int)enWheels.RL].brakeTorque = Mathf.Abs(Input.GetAxis("Accelerate") * motorTorqueForce);
                        wheels[(int)enWheels.RR].brakeTorque = Mathf.Abs(Input.GetAxis("Accelerate") * motorTorqueForce);
                    }
                    else
                    {
                        wheels[(int)enWheels.FL].motorTorque = Input.GetAxis("Accelerate") * motorTorqueForce;
                        wheels[(int)enWheels.FR].motorTorque = Input.GetAxis("Accelerate") * motorTorqueForce;
                        wheels[(int)enWheels.RL].motorTorque = Input.GetAxis("Accelerate") * motorTorqueForce;
                        wheels[(int)enWheels.RR].motorTorque = Input.GetAxis("Accelerate") * motorTorqueForce;
                    }
                    break;
                }
            case enControls.Keyboard:
                {
                    if (Input.GetAxis("Vertical") < 0 && averageRotation > 0)
                    {
                        wheels[(int)enWheels.FL].brakeTorque = Mathf.Abs(Input.GetAxis("Vertical") * motorTorqueForce);
                        wheels[(int)enWheels.FR].brakeTorque = Mathf.Abs(Input.GetAxis("Vertical") * motorTorqueForce);
                        wheels[(int)enWheels.RL].brakeTorque = Mathf.Abs(Input.GetAxis("Vertical") * motorTorqueForce);
                        wheels[(int)enWheels.RR].brakeTorque = Mathf.Abs(Input.GetAxis("Vertical") * motorTorqueForce);
                    }
                    else
                    {
                        wheels[(int)enWheels.FL].motorTorque = Input.GetAxis("Vertical") * motorTorqueForce;
                        wheels[(int)enWheels.FR].motorTorque = Input.GetAxis("Vertical") * motorTorqueForce;
                        wheels[(int)enWheels.RL].motorTorque = Input.GetAxis("Vertical") * motorTorqueForce;
                        wheels[(int)enWheels.RR].motorTorque = Input.GetAxis("Vertical") * motorTorqueForce;
                    }
                    break;
                }
        }

        wheels[(int)enWheels.FL].steerAngle = Input.GetAxis("Horizontal") * steerAngleAmount;
        wheels[(int)enWheels.FR].steerAngle = Input.GetAxis("Horizontal") * steerAngleAmount;

        wheelModels[(int)enWheels.FL].transform.Rotate(0, -wheels[(int)enWheels.FL].rpm / 60 * 360 * Time.deltaTime, 0);
        wheelModels[(int)enWheels.FR].transform.Rotate(0, -wheels[(int)enWheels.FR].rpm / 60 * 360 * Time.deltaTime, 0);
        wheelModels[(int)enWheels.RL].transform.Rotate(0, -wheels[(int)enWheels.RL].rpm / 60 * 360 * Time.deltaTime, 0);
        wheelModels[(int)enWheels.RR].transform.Rotate(0, -wheels[(int)enWheels.RR].rpm / 60 * 360 * Time.deltaTime, 0);

        ApplyValueToWheelModel(wheels[(int)enWheels.FL], wheelModels[(int)enWheels.FL]);
        ApplyValueToWheelModel(wheels[(int)enWheels.FR], wheelModels[(int)enWheels.FR]);
        ApplyValueToWheelModel(wheels[(int)enWheels.RL], wheelModels[(int)enWheels.RL]);
        ApplyValueToWheelModel(wheels[(int)enWheels.RR], wheelModels[(int)enWheels.RR]);
    }

    void DamperSettings ()
    {
        JointSpring damperSettings = new JointSpring();
        damperSettings.damper = damperIntensity;
        damperSettings.spring = damperSpring;

        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].suspensionSpring = damperSettings;
        }
    }

    void CenterOfMassSettings ()
    {
        rb = GetComponent<Rigidbody>();
        centerOfMass = GameObject.Find("CenterOfMass");
        massLocation = new Vector3(0, centerOfMassPosition, 0);
        centerOfMass.transform.localPosition = massLocation;
        rb.centerOfMass = centerOfMass.transform.localPosition;
    }

	void ApplyValueToWheelModel (WheelCollider wheel, GameObject wheelModel)
	{
		Vector3 rot = wheelModel.transform.localEulerAngles;
		rot.y = wheel.steerAngle;
		wheelModel.transform.localEulerAngles = rot;

		RaycastHit hit;
		if (Physics.Raycast(wheel.transform.position, -wheel.transform.up, out hit, wheel.suspensionDistance + wheel.radius)) 
		{
			wheelModel.transform.position = hit.point + wheel.transform.up * wheel.radius;
		} 
		else
		{
			wheelModel.transform.position = wheel.transform.position - (wheel.transform.up * wheel.suspensionDistance);
		}
	}

    void SetSpeedText()
    {
        //txtSpeed.text = "Km/h: " +  Mathf.Round(rb.velocity.magnitude*3).ToString();
    }

    private void FuelReduction()
    {
        m_CurrentFuel -= m_ReductionRate * Time.deltaTime;
    }
}
