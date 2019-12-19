using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour {

    public void Start()
    {
        changeSprite();
    }

	public void buttonSoundClicked()
    {
        GameManager.mute = !GameManager.mute;
        MusicManager.soundClicked();

        changeSprite();
    }

    private void changeSprite()
    {
        var imgComp = GetComponent<Image>();
        if (GameManager.mute)
            imgComp.sprite = Resources.Load<Sprite>("Buttons/HomeButton");
        else
            imgComp.sprite = Resources.Load<Sprite>("Buttons/GreenButton");
    }
}
