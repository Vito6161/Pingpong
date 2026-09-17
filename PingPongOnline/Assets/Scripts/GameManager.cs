using System;
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public int pontosP1, pontosP2;

    void OnEnable()
    {
        PontuarOM.OnPonto += Pontuou;
    }



    void Pontuou(int index)
    {
        switch(index)
        {
            case 1:
                pontosP1++;
                break;
            case 2:
                pontosP2++;
                break;
            default:
                Debug.Log("Player não encontrado. Erro no serializefield da parede pntuar");
                break;
        }

        Debug.Log($"Player 1: {pontosP1} ||| Player 2: {pontosP2}");
    }
}
