using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Pontuar : MonoBehaviour
{
    [SerializeField] private int playerGol; // quem faz gol aqui. Ex: a parede oposta ao player 1 recebe o index 1

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Bola"))
        {
            PontuarOM.OnPontuou(playerGol);
        }
    }
}

public class PontuarOM
{
    public static event Action<int> OnPonto;

    public static void OnPontuou(int index)
    {
        OnPonto?.Invoke(index);
    }
}
