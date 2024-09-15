using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "UserConfiguration", menuName = "User/Config", order = 1)]
public class UserConfiguration : ScriptableObject
{
    public string userName = "Your Name";
    public bool isMale = true;
    public float height = 1.75f;

    public bool IsValidConfiguration() //Ensures the user does not go past a range for the paramenters
    {
        return !string.IsNullOrEmpty(userName) && height > 1.0f && height < 2.5f;
    }
}
