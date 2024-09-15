using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedometerManager : MonoBehaviour
{
    public PedometerSimulator pedoSim;

    private float walkDistance = 0;
    private float caloriesBurned = 0;

    public UserConfiguration userConfig;
    public float stepLength;

    private const float maleStepConstant = 0.415f;
    private const float femaleStepConstant = 0.413f;

    private const float caloriesPerMeter = 0.05f; // Adjust this value based on your needs (This is assumed)

    public float WalkDistance
    {
        get
        {
            return walkDistance; //Making walkDistance accessible
        }
    }

    public float CaloriesBurned
    {
        get
        {
            return caloriesBurned; //Making caloriesBurned accessible
        }
    }

    void Start()
    {
        StepLengthCheck();
    }

    public void CalculateDistance()
    {
        walkDistance = pedoSim.TotalSteps * stepLength;
        CalculateCalories(); // Recalculate calories whenever distance is calculated
        //Debug.Log("Distance Walked: " + walkDistance);
        //Debug.Log("Calories Burned: " + caloriesBurned);
    }

    void Update()
    {
        CalculateDistance();
    }

    public void StepLengthCheck()
    {
        if (userConfig.isMale == true)
        {
            stepLength = maleStepConstant * userConfig.height;
            Debug.Log("Step Length for Male: " + stepLength);
        }
        else
        {
            stepLength = femaleStepConstant * userConfig.height;
            Debug.Log("Step Length for Female: " + stepLength);
        }
    }

    private void CalculateCalories()
    {
        caloriesBurned = walkDistance * caloriesPerMeter;
    }

}
