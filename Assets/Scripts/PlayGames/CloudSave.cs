using UnityEngine;
using System;
using System.Collections.Generic;
//gpg
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
//for encoding
using System.Text;
//for extra save ui
using UnityEngine.SocialPlatforms;
//for text, remove
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine.SceneManagement;

public class CloudSave
{

    private static CloudSave _instance;
    public static CloudSave Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CloudSave();
            }
            return _instance;
        }
    }

    //keep track of saving or loading during callbacks.
    private bool m_saving;
    //save name. This name will work, change it if you like.
    private static string m_saveName = "game_save_name";
    //This is the saved file. Put this in seperate class with other variables for more advanced setup. Remember to change merging, toBytes and fromBytes for more advanced setup.
    // private string saveString = "";

    //check with GPG (or other*) if user is authenticated. *e.g. GameCenter
    private bool Authenticated
    {
        get
        {
            return Social.Active.localUser.authenticated;
        }
    }

    //merges loaded bytearray with old save
    private void ProcessCloudData(byte[] cloudData)
    {
        Debug.Log("ProcessCloudData");

        if (cloudData == null)
        {
            Debug.Log("No data saved to the cloud yet...");
            return;
        }
        Debug.Log("Decoding cloud data from bytes.");
        string progress = FromBytes(cloudData);
        Debug.Log("Merging with existing game progress.");
        MergeWith(progress);
    }

    //load save from cloud
    public void SyncWithCloud()
    {
#if UNITY_ANDROID
        Debug.Log("Loading game progress from the cloud.");

        if (!Authenticated)
            return;

        m_saving = false;
        ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(
            m_saveName, //name of file.
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime,
            SavedGameOpened);
        Debug.Log("LoadFromCloud FIM");
#endif
    }

    //overwrites old file or saves a new one
    private void SaveToCloud()
    {
#if UNITY_ANDROID
        Debug.Log("SaveToCloud");
        if (Authenticated)
        {
            Debug.Log("Saving progress to the cloud... filename: " + m_saveName);
            m_saving = true;
            //save to named file
            ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(
                m_saveName, //name of file. If save doesn't exist it will be created with this name
                DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime,
                SavedGameOpened);
        }
        else
        {
            Debug.Log("Not authenticated!");
        }
        Debug.Log("SaveToCloud FIM");
#endif
    }

    //save is opened, either save or load it.
    private void SavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
#if UNITY_ANDROID
        Debug.Log("SavedGameOpened");

        //check success
        if (status == SavedGameRequestStatus.Success)
        {
            //saving
            if (m_saving)
            {
                //read bytes from save
                byte[] data = ToBytes();
                //create builder. here you can add play time, time created etc for UI.
                SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
                SavedGameMetadataUpdate updatedMetadata = builder.Build();
                //saving to cloud
                ((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(game, updatedMetadata, data, SavedGameWritten);
                Debug.Log("Commited Update: " + status);
                //loading
            }
            else
            {
                Debug.Log("SavedGame.ReadBinaryData");
                ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(game, SavedGameLoaded);

                SaveToCloud();
            }
            //error
        }
        else
        {
            Debug.LogWarning("Error opening game: " + status);
        }
#endif
    }

    //callback from SavedGameOpened. Check if loading result was successful or not.
    private void SavedGameLoaded(SavedGameRequestStatus status, byte[] data)
    {
        Debug.Log("SavedGameLoaded");

        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("SaveGameLoaded, success=" + status);
            ProcessCloudData(data);
        }
        else
        {
            Debug.LogWarning("Error reading game: " + status);
        }
    }

    //callback from SavedGameOpened. Check if saving result was successful or not.
    private void SavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("Game " + game.Description + " written");
        }
        else
        {
            Debug.LogWarning("Error saving game: " + status);
        }
    }

    //merge local save with cloud save. Here is where you change the merging betweeen cloud and local save for your setup.
    private void MergeWith(string other)
    {
        if (other != "")
        {
            LoadSaveData(other);
        }
        else
        {
            Debug.Log("Loaded save string doesn't have any content");
        }
    }

    //return saveString as bytes
    private byte[] ToBytes()
    {
        byte[] bytes = Encoding.UTF8.GetBytes(GetSaveData());
        return bytes;
    }

    private string GetSaveData()
    {
        //Debug.Log("GetSaveData");
        JSONNode node = new JSONClass();
        node["level"].AsInt = SaveGame.GetLastLevelUnlocked();

        for (int level = 1; level <= 50; level++)
        {
            node["stars"]["" + level].AsBool = level <= 10 ? true : false;
        }

        Debug.Log("Returning " + node.ToString());
        return node.ToString();
    }

    private void LoadSaveData(string data)
    {
        Debug.Log("Loaded save data: " + data);

        JSONNode node = JSONNode.Parse(data);
        int level = node["level"].AsInt;

        int oldLevel = SaveGame.GetLastLevelUnlocked();

        for (int i = SaveGame.GetLastLevelUnlocked(); i < level; i++)
        {
            SaveGame.completedLevel(i, false);
        }

        for (int i = 1; i <= 50; i++)
        {
            if (node["stars"]["" + i].AsBool)
                SaveGame.completedLevel(i, true);
        }

        if (oldLevel != SaveGame.GetLastLevelUnlocked())
        {
            if (SceneManager.GetActiveScene().name == "NewMap")
                SceneManager.LoadScene("NewMap");
        }

        PlayGamesManager.SyncAchievements();

        /*
        var N = JSON.Parse(the_JSON_string);
        var versionString = N["version"].Value;        // versionString will be a string containing "1.0"
        var versionNumber = N["version"].AsFloat;      // versionNumber will be a float containing 1.0
        var name = N["data"]["sampleArray"][2]["name"];// name will be a string containing "sub object"
        */
    }

    //take bytes as arg and return string
    private string FromBytes(byte[] bytes)
    {
        string decodedString = Encoding.UTF8.GetString(bytes);
        return decodedString;
    }
}
