using UnityEngine;
using System.Collections;

public class SaveGame
{
    private static string levelCompleted = "LevelCompleted";
    private static string levelStar = "LevelStar";
    private static string noAds = "NoAds";

    public static void noMoreAds()
    {
        PlayerPrefs.SetInt(noAds, 0);
    }

    public static bool showAds()
    {
        if (PlayerPrefs.GetInt(noAds, 1) == 1)
        {
            return true;
        }

        return false;
    }

    public static void completedLevel(bool star)
    {
        int level = GameManager.level;
        completedLevel(level, star);
    }

    public static void completedLevel(int level, bool star)
    {
        PlayerPrefs.SetInt(levelCompleted + level, 1);

        if (star)
        {
            PlayerPrefs.SetInt(levelStar + level, 1);
        }

        PlayerPrefs.Save();
    }

    public static bool isLevelCompleted(int level)
    {
        if (PlayerPrefs.GetInt(levelCompleted + level, 0) == 1)
        {
            return true;
        }

        return false;
    }

    public static bool gotStarInLevel(int level)
    {
        if (PlayerPrefs.GetInt(levelStar + level, 0) == 1)
        {
            return true;
        }

        return false;
    }

    public static int getTotalStars()
    {
        int totalStars = 0;

        for (int level = 1; level <= 60; level++)
        {
            if (gotStarInLevel(level))
            {
                totalStars++;
            }
        }

        return totalStars;
    }

    public static int GetLastLevelUnlocked()
    {
        int lastLevel = 1;
        for (int level = 2; level <= 51; level++)
        {
            if (!SaveGame.isLevelCompleted(level))
                break;

            lastLevel = level;
        }

        Debug.Log("Last Level: " + lastLevel);

        return lastLevel;
    }
}
