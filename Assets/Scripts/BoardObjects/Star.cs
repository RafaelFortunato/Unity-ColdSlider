using UnityEngine;
using System.Collections;

public class Star : BoardObject
{

    override protected void Start()
    {
        base.Start();

        if (SaveGame.gotStarInLevel(GameManager.level))
        {
            // Black Star
            Debug.Log("Black Star");
            //GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BlackStar");
        }

    }

    public void starCollected(Orb orb)
    {
        float delay = Mathf.Abs(((orb.transform.localPosition.x - this.transform.localPosition.x) + (orb.transform.localPosition.y - this.transform.localPosition.y)) + 1) * Orb.moveSpeed;
        boardNodeAnimation = new StarCollectedAnimation(this, delay);
        isAnimating = true;
    }

    public void StarCollectedAnimationEnd()
    {
        Debug.Log("StarCollectedAnimationEnd");
        getBoard().starCollected = true;

        // Se a orbe naquela posi��o ainda � a estrela, ent�o setamos a posi��o para null,
        // isso evita a posi��o ficar nula quando uma orbe para em cima da estrela
        BoardObject obj = getBoard().boardMatrix[posX, posY];
        if (obj != null && obj.orbType == OrbType.kTypeStar)
            getBoard().boardMatrix[posX, posY] = null;

        Destroy(gameObject);
    }
}

public class StarCollectedAnimation : BoardNodeAnimation
{
    public StarCollectedAnimation(Star star, float duration) : base(star, duration)
    { }

    public override void Animate()
    {
        float currentTime = (Time.time - startTime) / duration;
        if (currentTime >= 1.0f)
        {
            boardNode.GetComponent<Star>().StarCollectedAnimationEnd();
        }
    }
}
