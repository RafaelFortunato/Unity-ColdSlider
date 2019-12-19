using UnityEngine;
using System.Collections;

public class GameManager
{
    // Singleton, objeto que sera criado somente uma vez
    // na primeira chamada a essa classe
    private static GameManager singleton = null;

    // Variaveis comuns
    float multiResScale;

    public static int level = 1;
    public static bool mute = false;

    // Metodo estatico que retorna sempre a mesma instancia
    // da classe, se ela ainda nao existe, entao ela e criada
    public static GameManager getInstance()
    {
        if (singleton == null)
        {
            singleton = new GameManager();
        }

        return singleton;
    }

    // Inicializaçao
    GameManager()
    {
        
    }

    void calcMultiResScale()
    {
        // Proporçao entre largura e altura do dispositivo atual
        float deviceRatio = (float)Screen.width / (float)Screen.height;

        // Menor proporçao entre largura e altura entre os dispositivos (iPad)
        float lowestRatio = 0.6666667f;

        // Diferença
        float multiResSize = lowestRatio / deviceRatio;

        // Nossa scala final
        multiResScale = (Screen.width / 768.0f) * multiResSize;
    }

    // Metodos comuns 
    public float getScale()
    {
        return multiResScale;
    }
}
