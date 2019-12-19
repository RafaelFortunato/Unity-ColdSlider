using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

        Advertising.Init();
        PlayGamesManager.Init();
        SceneManager.LoadScene("NewMap");
    }
}
