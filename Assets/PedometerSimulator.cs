using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PedometerSimulator : MonoBehaviour
{
    //Variables
    public float walkingStepFrequency = 1.0f;
    public float runningStepFrequency = 2.5f;
    public int maxWalkingStepsPerMinute = 100;
    public int maxRunningStepsPerMinute = 180;

    //Parameters
    public PedometerManager pedoManager;
    private float timeSinceLastStep = 0.0f;
    private int totalSteps = 0;
    private float stepInterval;
    private bool isWalking = false;
    private bool isRunning = false;

    public Text stepCounterText;

    public int TotalSteps
    {
        get
        {
            return totalSteps; // using property method to get public access to totalSteps from other scripts
        }
    }

    void Start()
    {
        try
        {
            // Ensures pedoManager is assigned
            if (pedoManager == null)
            {
                Debug.LogError("PedometerManager is not assigned.");
                return;
            }

            stepInterval = 1.0f / walkingStepFrequency;

            UpdateStepCounterUI();
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error in Start method: " + ex.Message);
        }
    }

    void Update()
    {
        try
        {
            HandleInput();

            timeSinceLastStep += Time.deltaTime;

            // If walking, simulate walking
            if (isWalking && timeSinceLastStep >= stepInterval)
            {
                SimulateWalking();
                timeSinceLastStep = 0.0f;
            }

            // If running, simulate running
            if (isRunning && timeSinceLastStep >= stepInterval)
            {
                SimulateRunning();
                timeSinceLastStep = 0.0f;
            }

            UpdateStepCounterUI();
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error in Update method: " + ex.Message);
        }
    }

    void HandleInput()
    {
        try
        {
            // Walking with the "W" key
            if (Input.GetKeyDown(KeyCode.W))
            {
                isWalking = true;
                isRunning = false;
                stepInterval = 1.0f / walkingStepFrequency;
                Debug.Log("Started walking.");
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                isRunning = true;
                isWalking = false;
                stepInterval = 1.0f / runningStepFrequency;
                Debug.Log("Started running.");
            }

            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.R))
            {
                isWalking = false;
                isRunning = false;
                Debug.Log("Stopped moving.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error in HandleInput method: " + ex.Message);
        }
    }

    void SimulateWalking()
    {
        try
        {
            if (pedoManager == null) throw new System.NullReferenceException("PedometerManager is not assigned.");

            float randomFactor = Random.Range(0.4f, 2.2f);
            int stepsThisInterval = Mathf.RoundToInt(randomFactor * (pedoManager.stepLength * walkingStepFrequency));
            stepsThisInterval = Mathf.Clamp(stepsThisInterval, maxWalkingStepsPerMinute / 60, 2);
            totalSteps += stepsThisInterval;
            Debug.Log("Walking - Steps this interval: " + stepsThisInterval + " Total: " + totalSteps.ToString());
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error in SimulateWalking method: " + ex.Message);
        }
    }

    void SimulateRunning()
    {
        try
        {
            if (pedoManager == null) throw new System.NullReferenceException("PedometerManager is not assigned.");

            float randomFactor = Random.Range(0.4f, 2.2f);
            int stepsThisInterval = Mathf.RoundToInt(randomFactor * (pedoManager.stepLength * runningStepFrequency));
            stepsThisInterval = Mathf.Clamp(stepsThisInterval, 2, maxRunningStepsPerMinute / 60);
            totalSteps += stepsThisInterval;
            Debug.Log("Running - Steps this interval: " + stepsThisInterval + " Total: " + totalSteps.ToString());
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error in SimulateRunning method: " + ex.Message);
        }
    }

    //Method for UI Text Count
    public void UpdateStepCounterUI()
    {
        try
        {
            if (stepCounterText != null)
            {
                stepCounterText.text = totalSteps.ToString();
            }
            else
            {
                Debug.LogError("Step Counter Text is not assigned.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error in UpdateStepCounterUI method: " + ex.Message);
        }
    }

    public void ResetTotalStep() //Method to reset total step count
    {
        totalSteps = 0;
    }
}
