using UnityEngine;
using System.Collections;

public class BoardNode : MonoBehaviour
{
	// Posiçao na Matriz
	public int posX;
	public int posY;

	// Animaçoes
	public BoardNodeAnimation boardNodeAnimation = null;
	public bool isAnimating = false;

	// Direçoes
	public enum Direction
	{
		kUp = 0,
		kRight,
		kDown,
		kLeft,
	}

	protected void Start ()
	{
		//posX = (int)transform.localPosition.x;
		//posY = (int)transform.localPosition.y;
	}

	void Update ()
	{
		if (isAnimating && boardNodeAnimation != null)
		{
			boardNodeAnimation.Animate();
		}
	}

	public Board getBoard()
	{
		return GameObject.Find("Board").GetComponent<Board>();
	}
}
