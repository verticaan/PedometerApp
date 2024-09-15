using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PedometerSave : MonoBehaviour
{
    public PedometerSimulator pedoSimulator;
    public PedometerManager pedoManager;

    private string filePath;
    private DateTime lastSavedDate;

    void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "pedometerData.json");

        // Load the last saved date from existing data, if available
        List<PedometerData> existingData = LoadExistingData();
        if (existingData.Count > 0)
        {
            // Get the last entry's timestamp and parse it into DateTime
            string lastTimestamp = existingData[existingData.Count - 1].timestamp;
            lastSavedDate = DateTime.ParseExact(lastTimestamp, "dd-MM", null);
        }
        else
        {
            // If no data exists, start from today's date
            lastSavedDate = DateTime.UtcNow;
        }
    }

    public void SaveData()
    {
        // Increment the saved date by one day on each save
        lastSavedDate = lastSavedDate.AddDays(1);

        // Create an instance of my Class and reference it
        PedometerData data = new PedometerData
        {
            // Use the incremented date as the timestamp
            timestamp = lastSavedDate.ToString("dd-MM"),
            totalSteps = pedoSimulator.TotalSteps,
            walkDistance = pedoManager.WalkDistance,
            caloriesBurned = pedoManager.CaloriesBurned,
        };

        // Read the existing data from the file
        List<PedometerData> dataList = LoadExistingData();

        // Append the new data
        dataList.Add(data);

        // Serialize the list to JSON
        string json = JsonUtility.ToJson(new PedometerDataList { dataList = dataList }, true);

        // Save the JSON to a file
        File.WriteAllText(filePath, json);
        Debug.Log("Data saved to " + filePath + " with timestamp: " + data.timestamp);

        // Reset steps after saving
        pedoSimulator.ResetTotalStep();
    }

    public void ClearAllData()
    {
        // Overwrite the file with an empty list
        PedometerDataList emptyDataList = new PedometerDataList { dataList = new List<PedometerData>() };
        string json = JsonUtility.ToJson(emptyDataList, true);

        // Save the empty JSON to the file
        File.WriteAllText(filePath, json);
        Debug.Log("All data cleared from " + filePath);
    }

    public List<PedometerData> LoadExistingData() // Method is responsible for loading the existing pedometer data
    {
        if (File.Exists(filePath)) // Check if file exists
        {
            string json = File.ReadAllText(filePath); // Read content of file
            PedometerDataList dataList = JsonUtility.FromJson<PedometerDataList>(json); // Deserialize the JSON file to an object list
            return dataList.dataList; // Return the list of data
        }
        return new List<PedometerData>(); // Return empty list if no file exists
    }
}

[Serializable]
public class PedometerDataList
{
    public List<PedometerData> dataList;
}
