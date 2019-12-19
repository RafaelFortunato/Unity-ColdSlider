using UnityEngine;
using System.Collections;

public class Cracked : BoardTile
{
	// bool broke = false;

	public new void Start()
	{
		GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
		base.Start();
	}

	public override bool PassingOver(Orb orb)
	{
        float delay = Mathf.Abs(((orb.transform.localPosition.x - this.transform.localPosition.x) + (orb.transform.localPosition.y - this.transform.localPosition.y)) + 1) * Orb.moveSpeed;
        boardNodeAnimation = new CrackedBreakingAnimation(this, delay);
        isAnimating = true;
        return true;
        /*
		if (broke)
		{
			Debug.Log("Broked");

			return true;
		}
		else
		{
			//orb.GetComponent<Orb>().crackingTile = this;

			float delay = Mathf.Abs(((orb.transform.localPosition.x - this.transform.localPosition.x) + (orb.transform.localPosition.y - this.transform.localPosition.y)) + 1) * Orb.moveSpeed;
			boardNodeAnimation = new CrackedBreakingAnimation(this, delay);
			isAnimating = true;

			broke = true;
			return false;
		}
         * */
	}

	public void BreakingAnimationEnd()
	{
		GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.0f);
        getBoard().tileMatrix[posX, posY] = null;
        getBoard().boardMatrix[posX, posY] = null;
        Destroy(gameObject);
	}
}
