using UnityEngine;
using System.Collections;

public class BoardNodeAnimation
{
	protected BoardNode boardNode;
	protected float duration;
	protected float startTime;

	public BoardNodeAnimation (BoardNode boardNode, float duration)
	{
		this.boardNode = boardNode;
		this.duration = duration;
		startTime = Time.time;
	}

	// Override Me
	public virtual void Animate(){}

	public Board getBoard()
	{
		return GameObject.Find("Board").GetComponent<Board>();
	}
}
