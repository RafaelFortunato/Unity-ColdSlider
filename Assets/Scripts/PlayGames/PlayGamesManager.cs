using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using System;
using GooglePlayGames.BasicApi;

public class PlayGamesManager
{

    public static void Init()
    {
#if UNITY_ANDROID
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .EnableSavedGames()
            .Build();
        PlayGamesPlatform.InitializeInstance(config);

        // Activate the Play Games platform. This will make it the default
        // implementation of Social.Active
        PlayGamesPlatform.Activate();

        ConnectToGoogleServices();
#endif
    }

    public static void ConnectToGoogleServices()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
                onAuthentication();
        });
    }

    public static void onAuthentication()
    {
        //CloudSave.Instance.SaveToCloud();
        CloudSave.Instance.SyncWithCloud();
        //SyncAchievements();
    }

    public static bool IsAuthenticated()
    {
        return Social.localUser.authenticated;
    }

    public static void ShowAchievements()
    {
        if (IsAuthenticated())
        {
            Social.ShowAchievementsUI();
        }
        else
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    onAuthentication();
                    Social.ShowAchievementsUI();
                }
            });
        }
    }

    public static void ShowLeaderboard()
    {
        if (IsAuthenticated())
        {
            Social.ShowLeaderboardUI();
        }
        else
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    onAuthentication();
                    Social.ShowLeaderboardUI();
                }
            });
        }
    }

    public static void UnlockAchievement(string achievementId)
    {
        Social.ReportProgress(achievementId, 100.0f, (bool success) => { });
    }

    public static void LevelComplete(int level)
    {
        // TODO: Map LevelAchievements (Int Level, String AchievementId)
        switch (level)
        {
            case 5:
                UnlockAchievement(AchievementsIds.achievement_level_5_complete);
                break;

            case 10:
                UnlockAchievement(AchievementsIds.achievement_level_10_complete);
                break;

            case 20:
                UnlockAchievement(AchievementsIds.achievement_level_20_complete);
                break;

            case 30:
                UnlockAchievement(AchievementsIds.achievement_level_30_complete);
                break;

            case 40:
                UnlockAchievement(AchievementsIds.achievement_level_40_complete);
                break;

            case 50:
                UnlockAchievement(AchievementsIds.achievement_level_50_complete);
                break;

            default:
                break;
        }

        switch (SaveGame.getTotalStars())
        {
            case 25:
                UnlockAchievement(AchievementsIds.achievement_get_25_stars);
                break;

            case 50:
                UnlockAchievement(AchievementsIds.achievement_get_50_stars);
                break;

            default:
                break;
        }

        Social.ReportScore(level, AchievementsIds.leaderboard_level_leaderboard, (bool success) => { });
    }

    public static void SyncAchievements()
    {
        if (!IsAuthenticated())
            return;

        SyncLevelAchievements();

        if (SaveGame.getTotalStars() >= 25)
        {
            UnlockAchievement(AchievementsIds.achievement_get_25_stars);

            if (SaveGame.getTotalStars() >= 50)
                UnlockAchievement(AchievementsIds.achievement_get_50_stars);
        }

        Social.ReportScore(SaveGame.GetLastLevelUnlocked(), AchievementsIds.leaderboard_level_leaderboard, (bool success) => { });
    }

    public static void SyncLevelAchievements()
    {
#if UNITY_ANDROID
        if (!SaveGame.isLevelCompleted(5))
            return;
        UnlockAchievement(AchievementsIds.achievement_level_5_complete);

        if (!SaveGame.isLevelCompleted(10))
            return;
        UnlockAchievement(AchievementsIds.achievement_level_10_complete);

        if (!SaveGame.isLevelCompleted(20))
            return;
        UnlockAchievement(AchievementsIds.achievement_level_20_complete);

        if (!SaveGame.isLevelCompleted(30))
            return;
        UnlockAchievement(AchievementsIds.achievement_level_30_complete);

        if (!SaveGame.isLevelCompleted(40))
            return;
        UnlockAchievement(AchievementsIds.achievement_level_40_complete);

        if (!SaveGame.isLevelCompleted(50))
            return;
        UnlockAchievement(AchievementsIds.achievement_level_50_complete);
#endif
    }
}
