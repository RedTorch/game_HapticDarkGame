using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class DangerDetectionController : MonoBehaviour
{
    [SerializeField] float hearingRadius = 5f;
    [SerializeField] float singleSourceIntensity = 0.5f;
    [SerializeField] private TMP_Text debugText;
    [SerializeField] private GameObject gradientLeft;
    [SerializeField] private GameObject gradientRight;

    private bool isActive = true;
    private Gamepad gamepad;
    private float intensityRight = 0f;
    private float intensityLeft = 0f;
    private GameObject[] threats;
    // Start is called before the first frame update
    void Start()
    {
        gamepad = InputSystem.GetDevice<Gamepad>();
        threats = GameObject.FindGameObjectsWithTag("Threat");
    }

    // Update is called once per frame
    void Update()
    {
        intensityRight = 0f;
        intensityLeft = 0f;
        foreach(GameObject threat in threats)
        {
            float dist = Vector3.Distance(threat.transform.position, transform.position);
            if(dist<hearingRadius)
            {
                float intensityScalar = dist/hearingRadius;
                Vector3 relativePosition = transform.InverseTransformPoint(threat.transform.position);
                float hAxis = Mathf.Clamp(relativePosition.x / dist, -1f, 1f);
                float leftBalance = Mathf.Clamp((4f-((hAxis+1f)*(hAxis+1f)))/4f, 0f, 1f);
                float rightBalance = Mathf.Clamp(((hAxis+1f)*(hAxis+1f))/4f, 0f, 1f);
                float oscillStrength = threat.GetComponent<ThreatOscillator>().GetIntensity();
                // threat.GetComponent<ThreatOscillator>().SetSpeedMultiplier(1f - (dist / hearingRadius));
                intensityLeft += leftBalance * singleSourceIntensity * oscillStrength;
                intensityRight += rightBalance * singleSourceIntensity * oscillStrength;
            }
        }
        gradientLeft.GetComponent<CanvasGroup>().alpha = intensityLeft * 10f;
        gradientRight.GetComponent<CanvasGroup>().alpha = intensityRight * 10f;
        if(debugText)
        {
            debugText.text = (intensityLeft + ", " + intensityRight);
        }
        gamepad.SetMotorSpeeds(intensityLeft, intensityRight);
    }
}
