using UnityEngine;
using System.Collections;

public class Portal : BoardTile
{
	/*
	public Direction direction;
	public Portal twin;

	/*
	public override bool PassingOver(Orb orb)
	{
		return false;
	}

	public void Teleport(Orb orb)
	{
		Debug.Log("Teleporting");

		orb.teleported = false;
		orb.isAnimating = false;

		Board.boardMatrix[orb.posX, orb.posY] = null;

		orb.posX = twin.posX;
		orb.posY = twin.posY;

		Board.boardMatrix[orb.posX, orb.posY] = orb;

		orb.transform.position = twin.transform.position;

		int newDirection = orb.movementDirection.GetHashCode() - direction.GetHashCode() + twin.direction.GetHashCode();

		newDirection = newDirection % 4;

		if (newDirection < 0)
		{
			newDirection += 4;
		}

		switch(newDirection)
		{
			case 0:
				Debug.Log("New Direction: Up");
				orb.moveUp();
				break;

			case 1:
				Debug.Log("New Direction: Right");
				orb.moveRight();
				break;

			case 2:
				Debug.Log("New Direction: Down");
				orb.moveDown();
				break;

			case 3:
				Debug.Log("New Direction: Left");
				orb.moveLeft();
				break;
		}

	}
	*/
}
