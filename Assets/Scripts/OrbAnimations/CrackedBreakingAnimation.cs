using UnityEngine;
using System.Collections;

public class CrackedBreakingAnimation : BoardNodeAnimation
{
	public CrackedBreakingAnimation (Cracked cracked, float duration) : base(cracked, duration)
	{}
	
	public override void Animate ()
	{
		float currentTime = (Time.time - startTime) / duration;
		if (currentTime >= 1.0f)
		{
			boardNode.GetComponent<Cracked>().BreakingAnimationEnd();
		}
	}
}
