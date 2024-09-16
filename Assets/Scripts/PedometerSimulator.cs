using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PedometerSimulator : MonoBehaviour
{
    // Define varaibles
    public float walkingStepFrequency = 1.0f;
    public float runningStepFrequency = 2.5f; 
    public int maxWalkingStepsPerMinute = 100; 
    public int maxRunningStepsPerMinute = 180; 

    public PedometerManager pedoManager;
    
    //Define parameters
    private float timeSinceLastStep = 0.0f;
    private int totalSteps = 0;
    private float stepInterval;
    private bool isWalking = false; 
    private bool isRunning = false;

    private bool isMoving = false;
    private float elapsedTime = 0.0f;
    private float lastStartTime = 0.0f;

    public Text stepCounterText;
    public Text elapsedTimeText;

    public float ElapsedTime
    {
        get
        {
            return elapsedTime; // Return the current elapsed time
        }
    }

    // turning total steps accessibel even though its private
    public int TotalSteps
    {
        get
        {
            return totalSteps; // Return the current total steps
        }
    }

    void Start()
    {
        try
        {
            // Ensure pedoManager is assigned
            if (pedoManager == null)
            {
                Debug.LogError("PedometerManager is not assigned.");
                return;
            }

            stepInterval = 1.0f / walkingStepFrequency; // Calculate step interval for walking
            UpdateStepCounterUI(); // Update the step counter UI
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
            HandleInput(); // Handle user input for walking or running

            timeSinceLastStep += Time.deltaTime; // Update time since last step

            // Simulate walking if walking
            if (isWalking && timeSinceLastStep >= stepInterval)
            {
                SimulateWalking();
                timeSinceLastStep = 0.0f; // Reset step timer
            }

            // Simulate running if running
            if (isRunning && timeSinceLastStep >= stepInterval)
            {
                SimulateRunning();
                timeSinceLastStep = 0.0f; // Reset step timer
            }

            // Update UI elements
            UpdateStepCounterUI();
            UpdateElapsedTime();
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
            // Start walking when "W" key is pressed
            if (Input.GetKeyDown(KeyCode.W))
            {
                isWalking = true;
                isRunning = false;
                stepInterval = 1.0f / walkingStepFrequency;
                Debug.Log("Started walking.");
                StartMovementTimer(); // Start or resume the movement timer
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                isRunning = true;
                isWalking = false;
                stepInterval = 1.0f / runningStepFrequency;
                Debug.Log("Started running.");
                StartMovementTimer(); // Start or resume the movement timer
            }

            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.R))
            {
                isWalking = false;
                isRunning = false;
                Debug.Log("Stopped moving.");
                StopMovementTimer(); // Stop the movement timer
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

            // Calculate steps based on a random factor and step frequency
            float randomFactor = Random.Range(0.4f, 2.2f);
            int stepsThisInterval = Mathf.RoundToInt(randomFactor * (pedoManager.stepLength * walkingStepFrequency));
            stepsThisInterval = Mathf.Clamp(stepsThisInterval, maxWalkingStepsPerMinute / 60, 2);
            totalSteps += stepsThisInterval; // Update total steps
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
            totalSteps += stepsThisInterval; // Update total steps
            Debug.Log("Running - Steps this interval: " + stepsThisInterval + " Total: " + totalSteps.ToString());
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error in SimulateRunning method: " + ex.Message);
        }
    }

    void StartMovementTimer()
    {
        if (!isMoving)
        {
            isMoving = true;
            lastStartTime = Time.time; // Capture the time when movement starts or resumes
        }
    }

    void StopMovementTimer()
    {
        if (isMoving)
        {
            isMoving = false;
            elapsedTime += Time.time - lastStartTime; // Accumulate the elapsed time when stopping
        }
    }

    void UpdateElapsedTime()
    {
        if (isMoving && elapsedTimeText != null)
        {
            // Calculate and display the total elapsed time including current session
            float currentElapsedTime = elapsedTime + (Time.time - lastStartTime);
            elapsedTimeText.text = "Time: " + currentElapsedTime.ToString("F2") + " seconds";
        }
        else if (elapsedTimeText != null)
        {
            // Display the accumulated elapsed time if not moving
            elapsedTimeText.text = "Time: " + elapsedTime.ToString("F2") + " seconds";
        }
    }

    public void UpdateStepCounterUI()
    {
        try
        {
            if (stepCounterText != null)
            {
                stepCounterText.text = totalSteps.ToString(); // Update step counter UI
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

    public void ResetTotalStep()
    {
        totalSteps = 0;
        isMoving = false;
        elapsedTime = 0.0f;
        lastStartTime = 0.0f;
    }
}
