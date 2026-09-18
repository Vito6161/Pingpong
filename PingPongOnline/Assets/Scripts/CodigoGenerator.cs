using System.Net;
using UnityEngine;
using System;
using System.Collections.Generic;

public class GetLocalIP : MonoBehaviour
{
    public string ipDoPC;
   // public string ipConvertido;

   public static GetLocalIP instance;

   void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    Dictionary<char, int> letras = new Dictionary<char, int>()
    {
        { 'a', 1 },
        { 'b', 2 },
        { 'c', 3 },
        { 'd', 4 },
        { 'e', 5 },
        { 'f', 6 },
        { 'g', 7 },
        { 'h', 8 },
        { 'i', 9 },
        { 'j', 10 },
        { 'k', 11 },
        { 'l', 12 },
        { 'm', 13 },
        { 'n', 14 },
        { 'o', 15 },
        { 'p', 16 },
        { 'q', 17 },
        { 'r', 18 },
        { 's', 19 },
        { 't', 20 },
        { 'u', 21 },
        { 'v', 22 },
        { 'w', 23 },
        { 'x', 24 },
        { 'y', 25 },
        { 'z', 26 },
        {'0', 0}
        
    };

    Dictionary<int, char> numeros = new Dictionary<int, char>()
    {
        { 1, 'a' },
        { 2, 'b' },
        { 3, 'c' },
        { 4, 'd' },
        { 5, 'e' },
        { 6, 'f' },
        { 7, 'g' },
        { 8, 'h' },
        { 9, 'i' },
        { 10, 'j' },
        { 11, 'k' },
        { 12, 'l' },
        { 13, 'm' },
        { 14, 'n' },
        { 15, 'o' },
        { 16, 'p' },
        { 17, 'q' },
        { 18, 'r' },
        { 19, 's' },
        { 20, 't' },
        { 21, 'u' },
        { 22, 'v' },
        { 23, 'w' },
        { 24, 'x' },
        { 25, 'y' },
        { 26, 'z' },
        {0, '0'}
    };

    void Start()
    {
        ipDoPC = GetLocalIPAddress();
    }

    public string GetLocalIPAddress()
    {
        string hostName = Dns.GetHostName();
        IPAddress[] addresses = Dns.GetHostEntry(hostName).AddressList;

        foreach (IPAddress address in addresses)
        {
            if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return address.ToString();
            }
        }

        return "IP não encontrado";
    }

    public string GenerateCode(string ipToCode)
    {
        string[] splitIp = ipToCode.Split(".");
        string[] ipConverted = new string[4];
        string pontos = "";

        for (int i = 0; i < 4; i++)
        {
            string bloco = splitIp[i];
            string convertido = "";
            pontos += numeros[splitIp[i].Length];

            if (int.Parse(bloco) <= 26)
            {
                convertido = numeros[int.Parse(bloco)].ToString();
            }
            else
            {
                for (int d = 0; d < bloco.Length; d += 2)
                {
                    int tamanho = Mathf.Min(2, bloco.Length - d);

                    int numero = int.Parse(bloco.Substring(d, tamanho));

                    convertido += numeros[numero];
                }
            }

            ipConverted[i] = convertido;
        }

        return pontos + ipConverted[0] + ipConverted[1] + ipConverted[2] + ipConverted[3];
        
    }

    public string BreakCode(string ipCode)
    {
        string ipQuebrado = "";
        string ipConvertido = ipCode.Substring(4);
        string pontos = ipCode.Substring(0, 3);
        string pontosConvertido = "";

        int posicao = 0;
        bool temp = true;

        for(int i = 0; i < ipConvertido.Length; i++)
        {
            ipQuebrado += letras[ipConvertido[i]].ToString();
        }

        for(int i = 0; i < pontos.Length; i++)
        {
            pontosConvertido += letras[pontos[i]].ToString();

            
        }

        for(int i = 0; i < pontos.Length; i++)
        {    
            if(posicao == 0)
            {
                posicao = posicao + int.Parse(pontosConvertido[i].ToString());
            }
            else
            {
                posicao = posicao + int.Parse(pontosConvertido[i].ToString()) + 1;
            }

            ipQuebrado = ipQuebrado.Insert(posicao, ".");
        }

        return ipQuebrado;
    }
}