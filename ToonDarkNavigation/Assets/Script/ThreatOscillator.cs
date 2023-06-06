using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreatOscillator : MonoBehaviour
{
    [SerializeField] private AnimationCurve animCurve;
    private float currIntensity = 0f;
    private bool isUpdating = true;
    private float currTime = 0f;
    [SerializeField] private float speedMultiplier = 1f;
    // Start is called before the first frame update
    void Start()
    {
        currTime = Random.Range(0f,1f);
    }

    // Update is called once per frame
    void Update()
    {
        currTime += Time.deltaTime*speedMultiplier;
        if(currTime > 1f)
        {
            currTime -= 1f;
        }
        currIntensity = animCurve.Evaluate(currTime);
    }

    public float GetIntensity()
    {
        return currIntensity;
    }

    public void SetSpeedMultiplier(float speedIntensity)
    {
        speedMultiplier = 0.2f + (Mathf.Clamp(speedIntensity, 0f, 1f) * 0.8f);
    }
}
