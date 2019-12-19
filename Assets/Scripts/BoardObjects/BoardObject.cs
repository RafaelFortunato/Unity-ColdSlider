using UnityEngine;
using System.Collections;

public class BoardObject : BoardNode
{
	public float fadeDuration = 0.5f;

	// Tipo
	public OrbType orbType;
	public enum OrbType
	{
		kTypeBlock = 0,
		kTypeOrb,
		kTypeStar,
	}
	
	// Cor
	public OrbColor orbColor;
	public enum OrbColor
	{
		kColorNull = 0,
		kColorRed,
		kColorBlue,
		kColorGreen,
		kColorCyan,
		kColorPink,
		kColorPurple,
		kColorOrange,
		kColorYellow,
		kColorMulti,
	}

	// Use this for initialization
	new virtual protected void Start ()
	{
		base.Start();
		//getBoard().boardMatrix[posX, posY] = this;

		if (orbColor != OrbColor.kColorNull)
		{
			getBoard().objectsToBeDestroyed++;
		}

		//float myScale = 0.5f;
		//transform.localScale = new Vector3(myScale, myScale, myScale);
		//GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
		//GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/" + getSpriteName() + "_0" );
	}

	public void Fade()
	{	
		boardNodeAnimation = new FadeAnimation(this, fadeDuration);
		getBoard().boardMatrix[posX, posY] = null;
		getBoard().canMove = true;
		isAnimating = true;
	}
	
	public void FadeEnd()
	{
		Destroy(gameObject);
		getBoard().objectDestroyed();
	}
}
