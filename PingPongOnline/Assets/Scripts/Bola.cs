//using System;
using System.Collections;
using UnityEngine;
using UnityEngine.LightTransport;

public class Bola : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float delay = 3;
    [SerializeField] private float speed = 5;
   
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        PontuarOM.OnPonto += BolaPontuou;
    }

    void OnDisable()
    {
        PontuarOM.OnPonto -= BolaPontuou;
    }


    void BolaPontuou(int index)
    {
        //Debug.Log($"PLAYER {index} PONTUOU!!!");

        rb.position = new Vector2(0, 0);
        rb.linearVelocityX = 0;
        rb.linearVelocityY = 0;
        rb.angularVelocity = 0;
        StartCoroutine(StartGame());
        
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(delay);

        Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-0.3f, 0.3f));

        rb.linearVelocity = direction * speed;
    }
    


}
