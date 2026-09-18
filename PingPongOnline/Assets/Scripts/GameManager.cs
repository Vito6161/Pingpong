using System;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int pontosP1 = 0, pontosP2 = 0;
    public bool isHost;   

    [SerializeField] private GameObject menu1, menu2, menu3;
    [SerializeField] private TextMeshProUGUI codigoText;
    [SerializeField] private TMP_InputField inputField;
    private string textoDigitado;

    public string ip; 
    public string codigo;

    void OnEnable()
    {
        PontuarOM.OnPonto += Pontuou;
    }

    void OnDisable()
    {
        PontuarOM.OnPonto -= Pontuou;
    }

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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

    }

    public void Host()
    {
        ip = GetLocalIP.instance.GetLocalIPAddress();
        codigo = GetLocalIP.instance.GenerateCode(ip);
        codigoText.text = codigo;

        Debug.Log(codigo);

        isHost = true;

        menu1.SetActive(false);
        menu2.SetActive(true);
    }

    public void Join()
    {
        isHost = false;

        menu1.SetActive(false);
        menu3.SetActive(true);

    }

    public void AtualizarTexto()
    {
        textoDigitado = inputField.text;
        textoDigitado.Replace(" ", "");
        ip = GetLocalIP.instance.BreakCode(textoDigitado);
    }
}
