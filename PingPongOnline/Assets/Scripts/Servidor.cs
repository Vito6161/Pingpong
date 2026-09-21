using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

public class UdpServerTwoClients : MonoBehaviour
{
    UdpClient server;
    IPEndPoint anyEP;
    Thread receiveThread;

    Dictionary<string, int> clientIds =
        new Dictionary<string, int>();

    int nextId = 1;

    void Start()
    {
        server = new UdpClient(5001);

        anyEP = new IPEndPoint(
            IPAddress.Any,
            0
        );

        receiveThread = new Thread(ReceiveData);
        receiveThread.IsBackground = true;
        receiveThread.Start();

        Debug.Log("Servidor iniciado na porta 5001");
    }

    void ReceiveData()
    {
        while (true)
        {
            try
            {
                byte[] data = server.Receive(ref anyEP);

                string msg =
                    Encoding.UTF8.GetString(data);

                string key =
                    anyEP.Address + ":" + anyEP.Port;

                // Novo cliente
                if (!clientIds.ContainsKey(key))
                {
                    clientIds[key] = nextId++;

                    string assignMsg =
                        "ASSIGN:" + clientIds[key];

                    byte[] assignData =
                        Encoding.UTF8.GetBytes(assignMsg);

                    server.Send(
                        assignData,
                        assignData.Length,
                        anyEP
                    );

                    Debug.Log(
                        "Novo cliente: " +
                        key +
                        " ID: " +
                        clientIds[key]
                    );
                }

                int id = clientIds[key];

                // =========================
                // POSIÇÃO DOS PLAYERS
                // =========================

                if (msg.StartsWith("POS:"))
                {
                    string coords =
                        msg.Substring(4);

                    string broadcast =
                        "POS:" +
                        id +
                        ";" +
                        coords;

                    Broadcast(broadcast);
                }

                // =========================
                // POSIÇÃO DA BOLA
                // =========================

                else if (msg.StartsWith("BALL:"))
                {
                    string coords =
                        msg.Substring(5);

                    string broadcast =
                        "BALL:" +
                        coords;

                    Broadcast(broadcast);
                }

                // =========================
                // PONTUAÇÃO
                // =========================

                else if (msg.StartsWith("SCORE:"))
                {
                    string score =
                        msg.Substring(6);

                    string broadcast =
                        "SCORE:" +
                        score;

                    Broadcast(broadcast);
                }
            }
            catch
            {
                // Evita que a thread morra caso
                // o socket seja fechado.
            }
        }
    }

    void Broadcast(string message)
    {
        byte[] data =
            Encoding.UTF8.GetBytes(message);

        foreach (var kvp in clientIds)
        {
            string[] parts =
                kvp.Key.Split(':');

            IPEndPoint ep =
                new IPEndPoint(
                    IPAddress.Parse(parts[0]),
                    int.Parse(parts[1])
                );

            server.Send(
                data,
                data.Length,
                ep
            );
        }
    }

    void OnApplicationQuit()
    {
        if (receiveThread != null)
            receiveThread.Abort();

        if (server != null)
            server.Close();
    }
}