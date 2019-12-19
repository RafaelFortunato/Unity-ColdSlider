using UnityEngine;
using System.Collections;

public class FadeAnimation : BoardNodeAnimation
{
	private const float fadeTo = 0.0f;

	public FadeAnimation (BoardObject boardObject, float duration) : base(boardObject, duration)
	{}
	
	public override void Animate ()
	{
		float currentTime = (Time.time - startTime) / duration;
		boardNode.transform.localScale = new Vector2(Mathf.SmoothStep(1, fadeTo, currentTime), Mathf.SmoothStep(1, fadeTo, currentTime));

		if (currentTime >= 1.0f)
		{
			boardNode.GetComponent<BoardObject>().FadeEnd();
		}
	}
}
