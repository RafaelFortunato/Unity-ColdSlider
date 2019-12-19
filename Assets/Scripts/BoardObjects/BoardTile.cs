using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BoardTile : BoardNode
{
	// Tipo
	public TileType tileType;
	public enum TileType
	{
		kTileNull = 0,
		kTileNormalIce,
		kTileCracked,
		kTilePortal,
		KTileMove,
	}

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (getBoard().boardMatrix[posX, posY] != null)
        {
            Orb currentOrb = getBoard().boardMatrix[posX, posY] as Orb;
            if (currentOrb)
            {
                currentOrb.OnMouseDown();
                return;
            }
        }

        getBoard().NodeClicked(this);
    }

    void OnMouseDrag()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (getBoard().boardMatrix[posX, posY] != null)
        {
            Orb currentOrb = getBoard().boardMatrix[posX, posY] as Orb;
            if (currentOrb)
            {
                currentOrb.OnMouseDrag();
            }
        }
    }

    // Use this for initialization
    protected new void Start ()
	{
		base.Start();

		/*
		if (getBoard().tileMatrix[posX, posY] == null || getBoard().tileMatrix[posX, posY].tileType == TileType.kTileNormalIce)
		{
			getBoard().tileMatrix[posX, posY] = this;
		}
		*/
	}

	// Override Me
	virtual public bool PassingOver(Orb orb)
	{
		return false;
	}

	/*
	virtual public void Fade()
	{	
	}
	
	public void FadeEnd()
	{
	}
	*/
}
