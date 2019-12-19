using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Orb : BoardObject
{
	// Drag
	private Vector3 initialTouch;
	private bool canDrag = false;

	// Constantes que definem a velocidade com que a orbe se move
	// e a quantidade de pixels que o jogador precisa arrastar o dedo
	// para a orbe começar a se mover
	public const float moveSpeed = 0.10f;
	private const int moveOffset = 30;

	// Outro objeto da Board com que essa orbe venha a interagir
	BoardNode actionNode = null;

	// Estados dessa orb
	public OrbStatus orbStatus = OrbStatus.kNormal;
	public enum OrbStatus
	{
		kNormal = 0,
		kMatching,
		kFalling,
		kRedirecting,
	}

	// Unity CallBacks
	public void OnMouseDown()
	{
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        initialTouch = Input.mousePosition;
		if (!isAnimating && getBoard().canMove)
		{
            getBoard().NodeClicked(this);

            canDrag = true;
		}
	}

    // Chamado quando o jogador move o dedo por cima de uma orbe. A posiçao inicial do toque que foi guardada no OnMouseDown()
    // e comparada com a posiçao atual, se a diferença em x ou em y for maior do que a variavel moveOffSet, entao movemos
    // a orbe para a direçao correta
    public void OnMouseDrag()
	{
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (canDrag)
		{
			if (initialTouch.x - Input.mousePosition.x > moveOffset)
				moveLeft();

			else if (initialTouch.x - Input.mousePosition.x < -moveOffset)
				moveRight();

			else if (initialTouch.y - Input.mousePosition.y > moveOffset)
				moveDown();

			else if (initialTouch.y - Input.mousePosition.y < -moveOffset)
				moveUp();
		}
	}

	// Faz com que a orbe se movimente para a esquerda
	public void moveLeft()
	{
		Debug.Log("Left");
		Move(-1, 0);
	}

	// Faz com que a orbe se movimente para a direita
	public void moveRight()
	{
		Debug.Log("Right");
		Move(1, 0);
	}

	// Faz com que a orbe se movimente para baixo
	public void moveDown()
	{
		Debug.Log("Down");
		Move(0, -1);
	}

	// Faz com que a orbe se movimente para cima
	public void moveUp()
	{
		Debug.Log("Up");
		Move(0, 1);
	}

	// Metodo mais importante da orbe, chamado pelos metodos acima, faz com que a orbe se movimente em
	// uma direcao e realize checagens em relacao a cada tile e objeto que esta no caminho, os parametros definem
	// a direcao, incrementX corresponde a horizontal enquanto incrementY corresponde a vertical, e eles 
	// representam o movimento a cada loop dentro da matriz a partir da posicao atual da orbe, portanto a posiçao X e Y
	// da orbe sera somada com incrementX e incrementY respectivamente, e um deles deve ser 0, porque a orbe nao pode
	// se mover na diagonal
	void Move(int incrementX, int incrementY)
	{
        getBoard().NodeClicked(null);

		// O player nao pode mais mover essa orbe por enquanto
		canDrag = false;

		// Resetando o Action Node
		actionNode = null;

		// A orbe por enquanto esta normal
		orbStatus = OrbStatus.kNormal;

		// Essas variaveis vao guardar a posiçao X e Y para onde a orbe vai se mover no final do proximo loop
		int toX = posX;
		int toY = posY;

        {
            // Checa o tipo do tile atual
            // Cracked 
            BoardTile currentTile = getBoard().tileMatrix[toX, toY];
            if (currentTile.tileType == BoardTile.TileType.kTileCracked)
            {
                currentTile.PassingOver(this);
            }
        }

		// Loop que soma a posiçao da orbe com as variaveis passadas por parametro,
		// e aqui que a logica de colisao com tiles e objetos acontece
		while (true)
		{
			// Tile Atual
			BoardTile currentTile = getBoard().tileMatrix[toX, toY];

			// Tiles
			if (currentTile)
			{
				// Checa o tipo do tile atual
                // Cracked 
                //if (currentTile.tileType == BoardTile.TileType.kTileCracked)
                //{
                //    currentTile.PassingOver(this);
                //}
					
				if (currentTile.tileType == BoardTile.TileType.KTileMove)
				{
					// Move Tile
					Debug.Log("Move Tile");

					// Checa se a orbe esta atualmente em cima do MoveTile, se ela estiver ela pode se mover
					// para qualquer direçao sem ser afetada por ele, se nao estiver, ela sera movida para uma nova
					// direcao no fim desse movimento
					if ((posX != toX || posY != toY))
					{
						// Redirecionando a orbe

						// Salvando o MoveTile no objeto de interaçao, para futuramente pegar a direcao
						// para qual ele aponta.
						actionNode = currentTile.GetComponent<MoveTile>();

						// Muda o status da orbe para Redirecting
						orbStatus = OrbStatus.kRedirecting;

						// Para o loop
						break;
					}
				}
			}

			// Limites da Board

			if (toX + incrementX < 0 || toX + incrementX >= Board.kBoardWidth ||
			    toY + incrementY < 0 || toY + incrementY >= Board.kBoardHeight)
			{
				// Fim dos limites da matriz da Board
				Debug.Log("Limite da Board");
				break;
			}

			if (!getBoard().tileMatrix[toX + incrementX, toY + incrementY])
			{
				// Dentro dos limites da matriz da board,
				// porem os tiles de gelo ja acabaram,
				// portanto esse e o limite do movimento.
				Debug.Log("Limite do Gelo");
				break;
			}

			// No caso dos objetos, nos checamos pelo objeto da casa a frente, porque o da casa atual
			// seria obviamente a propria orbe
			BoardObject objectAhead = getBoard().boardMatrix[toX + incrementX, toY + incrementY];

			// Objetos
			if (objectAhead)
			{
				// Checa o tipo do objeto afrente

				if (objectAhead.orbType == OrbType.kTypeOrb || objectAhead.orbType == OrbType.kTypeBlock)
				{
					// O objeto a frente e uma orbe ou blocos comum
					Debug.Log("Orbe ou bloco");

					// Checa a cor do objeto a frente para saber se combina com a cor dessa orbe
                    if (objectAhead.orbType == OrbType.kTypeBlock && checkMatch(objectAhead.orbColor))
					{
						// As cores se combinam, portanto deu match
						Debug.Log("Match");

                        if (currentTile.tileType == BoardTile.TileType.kTileCracked)
                        {
                            currentTile.PassingOver(this);
                        }

                        // Salva o objeto a frente no objeto de interacao, para futuramente
                        // aplicar a animaçao de fade nele tambem
                        actionNode = objectAhead;

						// Muda o status da orbe para Matching
						orbStatus = OrbStatus.kMatching;
					}	

					// Como colidiu com um objeto solido, sae do loop
					break;
				}
				else if (objectAhead.orbType == OrbType.kTypeStar)
				{
					// A orbe passou por uma estrela
					Debug.Log("Pegou estrela");

					// Pega a estrela e continua o loop, pois a estrela nao e um objeto solido
					objectAhead.GetComponent<Star>().starCollected(this);
				}
                
			}

            if (currentTile.tileType == BoardTile.TileType.kTileCracked)
            {
                currentTile.PassingOver(this);
            }

            // Nenhuma colisao aconteceu, portanto continua checando a proxima posiçao
            toX += incrementX;
			toY += incrementY;
		}

		// Se o tile onde a orbe parou for um Cracked Tile, entao ela cae
        /*
		if (getBoard().tileMatrix[toX, toY].tileType == BoardTile.TileType.kTileCracked)
		{
			// Muda o status da orbe para Falling
			orbStatus = OrbStatus.kFalling;
		}
        */
		// Se a posiçao para onde a orbe tem que se mover for a mesma da posicao inicial
		// significa que ela nem se moveu, portanto nao tera animaçao
		if (toX == posX && toY == posY)
		{
			// Posiçao final e igual a posiçao inicial
			Debug.Log("Pos Igual");

			// Chamando o metodo que e executado no fim da animaçao de movimento
			MovingEnd();

			// Parando esse metodo, porque nao haveria animaçao de movimento
			return;
		}

		// Criando a animaçao de movimento ate a posiçao final
		createMovementAnimation(toX, toY);
	}

	void createMovementAnimation(int toX, int toY)
	{
		// Ativando esse boolean que indica que a orbe esta no meio de uma animaçao
		isAnimating = true;

		// Calculando a duraçao da animaçao baseado na distancia entre a posiçao inicial e a final em relaçao a constante moveSpeed
		float duration = Mathf.Abs((posX - toX) + (posY - toY)) * moveSpeed;

		// Criando e exectando a animaçao de movimento da orbe
		boardNodeAnimation = new MovementAnimation(this, new Vector2(toX + Board.kBoardOffSetX, toY + Board.kBoardOffSetY), duration);
		
		// Setando a posiçao em que a orbe estava na matriz para nula
		getBoard().boardMatrix[posX, posY] = null;

		// Setando a posiçao final da orbe na matriz para ela mesma
		getBoard().boardMatrix[toX, toY] = this;

		// Atualizando as variaveis que controlam a posiçao atual da orbe
		posX = toX;
		posY = toY;

		// Tocando o efeito sonoro da orbe deslizando pelo gelo
		PlaySound("Slide");
	}

	// Toca o som passado por parametro direto da pasta Audio/Sounds
	void PlaySound(string soundName)
	{
        if (GameManager.mute)
            return;

        // Carregando o som
        GetComponent<AudioSource>().clip = (AudioClip)Resources.Load("Audio/Sounds/" + soundName);

		// Tocando o som
		GetComponent<AudioSource>().Play();
	}

	// Metodo que checa se a cor passada por parametro combina com a 
	// cor dessa orbe, sendo cores exatamente iguals ou uma delas sendo multi
	// contanto que nenhuma delas tenha cor nula (Branca).
	bool checkMatch(OrbColor otherColor)
	{
		// Checando se as cores combinam
		if (orbColor == otherColor || orbColor == OrbColor.kColorMulti || otherColor == OrbColor.kColorMulti)
		{
			// Checando se nenhuma das orbes possui a cor nula
			if (orbColor != OrbColor.kColorNull && otherColor != OrbColor.kColorNull)
			{
				// Houve match
				return true;
			}
		}
	
		// Nao houve match
		return false;
	}

	// Chamado no fim da animaçao de movimento da Orbe
	public void MovingEnd()
	{
		// Checando o status da orbe, se ela deu match, caiu, foi redirecionada ou se simplesmente 
		// se movimentou de forma comum

		if (orbStatus == OrbStatus.kMatching)
		{
			// Houve match

			// aplicando a animaçao de fade nessa orbe e 
			// no boardObject (Orbe ou Bloco) que ela colidiu
			actionNode.GetComponent<BoardObject>().Fade();
			Fade();

			// Tocando o efeito sonoro de colisao
			PlaySound("Hit");
		}
		else if (orbStatus == OrbStatus.kFalling)
		{
			// Caiu em um buraco

			// Se o actionNode nao for nulo, significa que a orbe
			// caiu no buraco porque bateu em uma orbe ou bloco 
			// da mesma cor, portanto aplicamos o fade nela tbm
			if (actionNode != null)
			{
				actionNode.GetComponent<BoardObject>().Fade();
			}


			// Aplicando a animaçao da orbe sumindo
			Fade();
		}
		else if (orbStatus == OrbStatus.kRedirecting)
		{
			// Passou por um cima de um moveTile

			// Fazendo a orbe se mover para a direçao do MoveTile
			actionNode.GetComponent<MoveTile>().setNewDirection(this);
		}
		else
		{
			// Fim do movimento comum, sem match, sem queda e sem redirecionamento

			// A orbe nao esta mais animando
			isAnimating = false;

			// O jogador pode mover novamente
			getBoard().canMove = true;

			// Setando o objeto de animaçao para nulo
			boardNodeAnimation = null;

			// Tocando o efeito sonoro de colisao
			PlaySound("Hit");
		}
	}
}
