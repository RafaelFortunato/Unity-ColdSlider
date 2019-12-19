using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine.SceneManagement;

public class Board : MonoBehaviour
{
    // Matriz principal do jogo
    public const int kBoardWidth = 10;
    public const int kBoardHeight = 10;

    public const float kBoardOffSetX = -4.5f;
    public const float kBoardOffSetY = -4.5f;
    public const float kOrbSize = 85.0f;

    public BoardObject[,] boardMatrix = new BoardObject[kBoardWidth, kBoardHeight];
    public BoardTile[,] tileMatrix = new BoardTile[kBoardWidth, kBoardHeight];

    private ArrayList objectsArray = new ArrayList();

    public bool canMove = true;
    public bool starCollected = false;
    public int objectsToBeDestroyed = 0;

    private Orb currentOrb = null;
    public GameObject selectionCircle = null;

    public void Start()
    {
        createObjectsArray();
        createLevel();
    }

    void createObjectsArray()
    {
        objectsArray.Add("None");
        objectsArray.Add("Tile");

        objectsArray.Add("Star"); // 2

        // Orbs
        objectsArray.Add("RedOrb"); // 3
        objectsArray.Add("BlueOrb"); // 4
        objectsArray.Add("GreenOrb"); // 5
        objectsArray.Add("YellowOrb"); // 6
        objectsArray.Add("OrangeOrb"); // 7
        objectsArray.Add("PurpleOrb"); // 8
        objectsArray.Add("PinkOrb"); // 9
        objectsArray.Add("CyanOrb"); // 10

        // Blocks
        objectsArray.Add("RedBlock"); // 11
        objectsArray.Add("BlueBlock"); // 12
        objectsArray.Add("GreenBlock"); // 13
        objectsArray.Add("YellowBlock"); // 14
        objectsArray.Add("OrangeBlock"); // 15
        objectsArray.Add("PurpleBlock"); // 16
        objectsArray.Add("PinkBlock"); // 17
        objectsArray.Add("CyanBlock"); // 18

        objectsArray.Add("WhiteOrb"); // 19
        objectsArray.Add("WhiteBlock"); // 20

        objectsArray.Add("MultiOrb"); // 21

        objectsArray.Add("MoveDown"); // 22
        objectsArray.Add("MoveLeft"); // 23
        objectsArray.Add("MoveRight"); // 24
        objectsArray.Add("MoveUp"); // 25

        objectsArray.Add("Cracked"); // 26
    }

    void createLevel()
    {
        TextAsset json = Resources.Load<TextAsset>("Levels/" + GameManager.level);
        JSONNode node = JSONNode.Parse(json.text);

        int pos = 0;
        for (int y = 9; y >= 0; y--)
        {
            for (int x = 0; x < 10; x++)
            {
                int objectId = node["tiles"][pos++].AsInt;
                createObject(objectId, x, y);
            }

        }
    }

    void createObject(int objectId, int x, int y)
    {
        if (objectId == 0 || objectId == 20)
            return;

        // Debug.Log(objectId);

        float posX = x + kBoardOffSetX;
        float posY = y + kBoardOffSetY;


        if (!isTile(objectId))
        {
            BoardTile tile = ((GameObject)Instantiate(Resources.Load("Prefabs/Tile"))).GetComponent<BoardTile>();
            tile.transform.position = new Vector2(posX, posY);
            tile.posX = x;
            tile.posY = y;
            tileMatrix[x, y] = tile;

            BoardObject gameObject = ((GameObject)Instantiate(Resources.Load("Prefabs/" + objectsArray[objectId]))).GetComponent<BoardObject>();
            gameObject.transform.position = new Vector2(posX, posY);
            gameObject.posX = x;
            gameObject.posY = y;
            boardMatrix[x, y] = gameObject;
        }
        else
        {
            BoardTile tile = ((GameObject)Instantiate(Resources.Load("Prefabs/" + objectsArray[objectId]))).GetComponent<BoardTile>();
            tile.transform.position = new Vector2(posX, posY);
            tile.posX = x;
            tile.posY = y;
            tileMatrix[x, y] = tile;
        }
    }

    bool isTile(int objectId)
    {
        return (objectId == 1 ||
                objectId == 22 ||
                objectId == 23 ||
                objectId == 24 ||
                objectId == 25 ||
                objectId == 26);
    }

    public void objectDestroyed()
    {
        objectsToBeDestroyed--;

        Debug.Log("ObjectsToBeDestroyed: " + objectsToBeDestroyed);

        if (objectsToBeDestroyed <= 0)
        {
            gameWin();
        }
    }

    private void gameWin()
    {
        Debug.Log("Game Win");

        SaveGame.completedLevel(starCollected);

        CloudSave.Instance.SyncWithCloud();

        if (GameManager.level == 50)
        {
            // Last Level
            MusicManager.playMenuMusic();
            SceneManager.LoadScene("NewMap");
            return;
        }

        // Animaçao de passagem de nivel
        GameManager.level++;
        SceneManager.LoadScene("Level");

        Advertising.ShowAd();
    }

    public void NodeClicked(BoardNode node)
    {
        if (node == null)
        {
            currentOrb = null;
            hideSelection();
            return;
        }

        if (currentOrb == node)
        {
            currentOrb = null;
            hideSelection();
            return;
        }

        Orb nodeOrb = node as Orb;
        if (nodeOrb != null)
        {
            currentOrb = nodeOrb;
            showSelection(currentOrb.transform.position);
            return;
        }
  
        if (currentOrb != null)
        {
            if (node.posX == currentOrb.posX)
            {
                if (node.posY > currentOrb.posY)
                {
                    currentOrb.moveUp();
                }
                else if (node.posY < currentOrb.posY)
                {
                    currentOrb.moveDown();
                }
            }
            else if (node.posY == currentOrb.posY)
            {
                if (node.posX > currentOrb.posX)
                {
                    currentOrb.moveRight();
                }
                else if (node.posX < currentOrb.posX)
                {
                    currentOrb.moveLeft();
                }
            }
        }

        currentOrb = null;
        hideSelection();
    }

    public void showSelection(Vector3 vector)
    {
        selectionCircle.SetActive(true);
        selectionCircle.transform.position = vector;
    }

    public void hideSelection()
    {
        selectionCircle.SetActive(false);
    }
}
