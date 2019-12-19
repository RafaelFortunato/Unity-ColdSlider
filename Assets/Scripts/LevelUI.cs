using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelUI : MonoBehaviour {

    public void restartLevel()
    {
        SceneManager.LoadScene("Level");
    }

    public void home()
    {
        MusicManager.playMenuMusic();
        SceneManager.LoadScene("NewMap");
    }
}
