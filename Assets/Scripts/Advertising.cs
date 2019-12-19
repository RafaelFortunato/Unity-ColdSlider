using admob;

public class Advertising
{
    public static void Init()
    {
#if UNITY_ANDROID
        Admob.Instance().initAdmob("", "insert_admob_id");
        Admob.Instance().loadInterstitial();
#endif
    }

    public static void ShowAd()
    {
#if UNITY_ANDROID
        if (!SaveGame.showAds())
            return;

        if (!Admob.Instance().isInterstitialReady())
        {
            Admob.Instance().loadInterstitial();
            return;
        }

        if (GameManager.level < 11)
            return;

        if (GameManager.level % 2 == 0)
            return;

        if (Admob.Instance().isInterstitialReady())
        {
            Admob.Instance().showInterstitial();
            Admob.Instance().loadInterstitial();
        }
#endif
    }
}