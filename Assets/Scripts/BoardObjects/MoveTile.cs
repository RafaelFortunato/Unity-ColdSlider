using UnityEngine;
using System.Collections;

public class MoveTile : BoardTile
{
	public Direction direction;

	public void setNewDirection(Orb orb)
	{
		Debug.Log("Moving");
		
		orb.isAnimating = false;

		switch(direction)
		{
			case BoardNode.Direction.kUp:
				Debug.Log("New Direction: Up");
				orb.moveUp();
				break;
				
			case BoardNode.Direction.kRight:
				Debug.Log("New Direction: Right");
				orb.moveRight();
				break;
				
			case BoardNode.Direction.kDown:
				Debug.Log("New Direction: Down");
				orb.moveDown();
				break;
				
			case BoardNode.Direction.kLeft:
				Debug.Log("New Direction: Left");
				orb.moveLeft();
				break;
		}
	}
}
