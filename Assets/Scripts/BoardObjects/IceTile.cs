using UnityEngine;
using System.Collections;

public class IceTile : BoardTile
{
	// Use this for initialization
	void Start ()
	{
		base.Start();
		GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Random.Range(0.5f, 0.8f));
		/*getBoard().tileMatrix[posX, posY] = this;*/
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
