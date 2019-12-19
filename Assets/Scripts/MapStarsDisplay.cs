using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapStarsDisplay : MonoBehaviour {

    public Text whiteLabel;
    
    // Use this for initialization
    void Start ()
    {
        var text = SaveGame.getTotalStars() + " / 50";
        whiteLabel.text = text;
    }
}
