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

        try
        {
            // Load the last saved date from existing data, if availab
            List<PedometerData> existingData = LoadExistingData();

            if (existingData.Count > 0)
            {
                // Get the last entry's timestamp and parse it into DateTime
                string lastTimestamp = existingData[existingData.Count - 1].timestamp;

                if (!DateTime.TryParseExact(lastTimestamp, "dd-MM", null, System.Globalization.DateTimeStyles.None, out lastSavedDate))
                {
                    Debug.LogWarning("Failed to parse last saved date. Starting from today's date.");
                    lastSavedDate = DateTime.UtcNow; // Use current date if parsing fails
                }
            }
            else
            {
 
                lastSavedDate = DateTime.UtcNow;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error during Awake: " + ex.Message);
            lastSavedDate = DateTime.UtcNow; // Set default date in case of error
        }
    }

    public void SaveData()
    {
        if (pedoSimulator == null || pedoManager == null)
        {
            Debug.LogError("PedometerSimulator or PedometerManager is not assigned.");
            return;
        }

        try
        {
            // Increment the saved date by one day on each save
            lastSavedDate = lastSavedDate.AddDays(1);

            PedometerData data = new PedometerData
            {
                timestamp = lastSavedDate.ToString("dd-MM"),
                totalSteps = pedoSimulator.TotalSteps,
                walkDistance = pedoManager.WalkDistance,
                caloriesBurned = pedoManager.CaloriesBurned,
            };

            // Load existing data and append the new entry
            List<PedometerData> dataList = LoadExistingData();
            dataList.Add(data);

            // Serialize to list
            string json = JsonUtility.ToJson(new PedometerDataList { dataList = dataList }, true);
            File.WriteAllText(filePath, json);

            Debug.Log("Data saved to " + filePath + " with timestamp: " + data.timestamp);

            // Reset steps after saving
            pedoSimulator.ResetTotalStep();
        }
        catch (Exception ex)
        {
            Debug.LogError("Error while saving data: " + ex.Message);
        }
    }

    public void ClearAllData()
    {
        try
        {
            // Overwrite the file with an empty list
            PedometerDataList emptyDataList = new PedometerDataList { dataList = new List<PedometerData>() };
            string json = JsonUtility.ToJson(emptyDataList, true);
            File.WriteAllText(filePath, json);

            Debug.Log("All data cleared from " + filePath);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error while clearing data: " + ex.Message);
        }
    }

    public List<PedometerData> LoadExistingData() // Method is responsible for loading
    {
        try
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath); // Read content of file
                PedometerDataList dataList = JsonUtility.FromJson<PedometerDataList>(json); // Deserialize
                return dataList.dataList ?? new List<PedometerData>(); // Return data or empty list
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error while loading data: " + ex.Message);
        }

        return new List<PedometerData>();
    }
}

[Serializable]
public class PedometerDataList
{
    public List<PedometerData> dataList;
}
