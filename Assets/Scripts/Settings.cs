using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {

    public void CreateSettings()
    {
        Debug.Log("CreateSettings");
        gameObject.SetActive(true);
    }

    public void Login()
    {
        PlayGamesManager.ConnectToGoogleServices();
    }

    public void ShowAchievements()
    {
        PlayGamesManager.ShowAchievements();
    }

    public void ShowLeaderboard()
    {
        PlayGamesManager.ShowLeaderboard();
    }

    public void CloseButton()
    {
        gameObject.SetActive(false);
    }
    
}
