using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UserConfigManager : MonoBehaviour
{
    public UserConfiguration userConfig;
    public InputField usernameInputField;
    public InputField heightInputField;
    public Slider accelSlider;
    public Toggle genderToggle;
    public Text errorMessage;

    private string filePath;

    void Awake() // better if we want to lead the configuration before the start method
    {
        //used Path.Combine to account for different OS's having different path seperators

        filePath = Path.Combine(Application.persistentDataPath, "userConfig.json");
        LoadConfiguration();

    }

    public void SaveConfiguration()
    {
        userConfig.userName = usernameInputField.text;

        float heightValue;
        if(float.TryParse(heightInputField.text, out heightValue))
        {
            userConfig.height = heightValue;
        }

        else
        {
            errorMessage.text = "Invalid height";
            return;
        }

        userConfig.isMale = genderToggle.isOn;

        userConfig.accelLimit = accelSlider.value;

        if (!userConfig.IsValidConfiguration())
        {
            errorMessage.text = "Make sure the boxes are filled correctly";
            return;
        }

        SaveToJSON();

        errorMessage.text = "Configuration saved successfully";
    }

    private void SaveToJSON()
    {
        string json = JsonUtility.ToJson(userConfig, true);

        // Write the JSON string to the file
        File.WriteAllText(filePath, json);

    }

    public void LoadConfiguration()
    {
        if (File.Exists(filePath))
        {
            // Read the JSON string from the file
            string json = File.ReadAllText(filePath);

            // Deserialize the JSON back into a UserConfiguration object
            JsonUtility.FromJsonOverwrite(json, userConfig);

            // Populate the UI fields with the loaded configuration
            usernameInputField.text = userConfig.userName;
            heightInputField.text = userConfig.height.ToString();
            genderToggle.isOn = userConfig.isMale;
            accelSlider.value = userConfig.accelLimit;
        }
        else
        {
            errorMessage.text = "No saved configuration found.";
        }
    }
}