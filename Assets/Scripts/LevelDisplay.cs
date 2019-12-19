using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDisplay : MonoBehaviour {

    public Text whiteLabel;
    public Text shadowLabel;

    // Use this for initialization
    void Start () {
        whiteLabel.text = "Level " + GameManager.level;
        shadowLabel.text = "Level " + GameManager.level;
    }
}
