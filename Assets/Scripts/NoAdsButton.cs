using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoAdsButton : MonoBehaviour {

    void Start()
    {
        if (!SaveGame.showAds())
        {
            removeButton();
        }
    }

    public void buttonClicked()
    {
        StoreManager.Instance.BuyNoAds();
    }

    public void removeButton()
    {
        gameObject.SetActive(false);
    }
        
}
