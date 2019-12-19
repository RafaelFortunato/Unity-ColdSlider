using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButtonScript : MonoBehaviour {

    public int levelNumber;
    public Image star;
    public Text whiteLabel;
    public Text shadowLabel;

    private bool locked = true;

	// Use this for initialization
	void Start () 
    {
        whiteLabel.text = "" + levelNumber;
        shadowLabel.text = "" + levelNumber;

        if (levelNumber <= 1 || SaveGame.isLevelCompleted(levelNumber - 1))
        {
            createUnlockedLevel();
        }
        else
        {
            createLockedLevel();
        }
    }

    public void startLevel()
    {
        Debug.Log("Level Clicked: " + levelNumber);
        if (!locked)
        {
            GameManager.level = levelNumber;
            MusicManager.playLevelMusic();
            SceneManager.LoadScene("Level");
        }
    }
	
    public void createUnlockedLevel()
    {
        locked = false;
        star.enabled = true;

        if (SaveGame.gotStarInLevel(levelNumber))
        {
            Sprite enabledStar = Resources.Load<Sprite>("Buttons/LevelStarTrue");
            star.sprite = enabledStar;
        }
    }

    public void createLockedLevel()
    {
        GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 1);
    }
}
