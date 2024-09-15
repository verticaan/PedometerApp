using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BarChart : MonoBehaviour
{
    public Bar barPrefab;
    public PedometerSave pedometerSave;

    List<Bar> bars = new List<Bar>();
    float chartHeight;


    void Start()
    {
        chartHeight = Screen.height + GetComponent<RectTransform>().sizeDelta.y;

        List<PedometerData> pedometerData = pedometerSave.LoadExistingData();

        if (pedometerData == null || pedometerData.Count == 0)
        {
            Debug.LogWarning("No pedometer data available to display.");
            return; // Exit early if no data is available
        }

        int maxSteps = pedometerData.Max(data => data.totalSteps);

        float[] values = new float[pedometerData.Count];
        for (int i = 0; i < pedometerData.Count; i++)
        {
            values[i] = pedometerData[i].totalSteps;
        }

        DisplayGraph(pedometerData, maxSteps);
    }

    void DisplayGraph(List<PedometerData> data, int maxSteps)
    {
        for (int i = 0; i < data.Count; i++)
        {
            Bar newBar = Instantiate<Bar>(barPrefab, transform);
            RectTransform rt = newBar.bar.GetComponent<RectTransform>();

            float normalizedValue = ((float)data[i].totalSteps / (float)maxSteps) * 5f; //tried to get a normalised value based on the max value of the totalsteps list

            rt.sizeDelta = new Vector2(rt.sizeDelta.x, -chartHeight * normalizedValue); //used that normalised value to plot the bar chart

            if (newBar.stepText != null)
            {
                newBar.stepText.text = data[i].totalSteps.ToString();
            }

            // Set the timestamp text
            if (newBar.dateText != null)
            {
                newBar.dateText.text = data[i].timestamp.ToString();
            }
        }
    }

}
