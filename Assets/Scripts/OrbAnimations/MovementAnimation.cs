using UnityEngine;
using System.Collections;

public class MovementAnimation : BoardNodeAnimation
{
	private Vector2 from;
	private Vector2 to;

	public MovementAnimation (BoardObject boardObject, Vector2 to, float duration) : base(boardObject, duration)
	{
		getBoard().canMove = false;
		this.from = boardObject.transform.position;
		this.to = to;
	}

	public override void Animate ()
	{
		//Debug.Log("FromX: " + from.x + ", ToX: " + to.x);
		//Debug.Log("FromY: " + from.y + ", ToY: " + to.y);

		float currentTime = (Time.time - startTime) / duration;
		boardNode.transform.position = new Vector2(Mathf.SmoothStep(from.x, to.x, currentTime), Mathf.SmoothStep(from.y, to.y, currentTime));

		/*
		if (currentTime >= 0.75f && !Board.canMove)
		{
			Board.canMove = true;
		}
		*/

		if (currentTime >= 1)
		{
			boardNode.GetComponent<Orb>().MovingEnd();
		}
	}
}
