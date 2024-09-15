using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PedometerData
{
    public string timestamp;
    public int totalSteps;
    public float walkDistance;
    public float caloriesBurned;
}
