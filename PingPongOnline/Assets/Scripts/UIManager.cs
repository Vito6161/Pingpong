using UnityEngine;
using System.Collections;
using System;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    private GameManager manager;

    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //SetText(0);
    }
    void OnEnable()
    {
        PontuarOM.OnPonto += StartDelay;
    }
    void OnDisable()
    {
        PontuarOM.OnPonto -= StartDelay;
    }

    void StartDelay(int index)
    {
        StartCoroutine(SetText());
    }

    IEnumerator SetText()
    {
        yield return new WaitForSeconds(.5f);

        score.text = $"Player 1: {manager.pontosP1} | Player 2: {manager.pontosP2}";
    }
}
