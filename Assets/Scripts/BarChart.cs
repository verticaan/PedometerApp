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
        // Calculate the total height for the chart based on device screen height and UI element size
        chartHeight = Screen.height + GetComponent<RectTransform>().sizeDelta.y;

        List<PedometerData> pedometerData = pedometerSave.LoadExistingData(); //Load pedometer data from json list

        if (pedometerData == null || pedometerData.Count == 0) //check if data is available and shows error
        {
            Debug.LogWarning("No pedometer data available to display."); // Exit early if no data is available
            return; 
        }

        int maxSteps = pedometerData.Max(data => data.totalSteps); //get max number of steps stored in json file

        //An array that store step values
        float[] values = new float[pedometerData.Count];
        for (int i = 0; i < pedometerData.Count; i++)
        {
            values[i] = pedometerData[i].totalSteps;
        }

        DisplayGraph(pedometerData, maxSteps);
    }

    void DisplayGraph(List<PedometerData> data, int maxSteps) //displays bar graph based on pedometer data
    {
        for (int i = 0; i < data.Count; i++)
        {
            //spawns a new bar from prefab
            Bar newBar = Instantiate<Bar>(barPrefab, transform);
            RectTransform rt = newBar.bar.GetComponent<RectTransform>(); //get rect transform to modify bar size

            float normalizedValue = ((float)data[i].totalSteps / (float)maxSteps) * 5f; //tried to get a normalised value based on the max value of the totalsteps list

            rt.sizeDelta = new Vector2(rt.sizeDelta.x, -chartHeight * normalizedValue); //used that normalised value to plot the bar chart

            if (newBar.stepText != null) //set step text anchored above bar
            {
                newBar.stepText.text = data[i].totalSteps.ToString();
            }

            // Set the timestamp text achored below graph
            if (newBar.dateText != null)
            {
                newBar.dateText.text = data[i].timestamp.ToString();
            }
        }
    }

}
