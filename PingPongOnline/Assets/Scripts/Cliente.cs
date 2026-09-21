using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Globalization;

public class UdpClientTwoClients : MonoBehaviour
{

    private string ip = " ";
    UdpClient client;

    Thread receiveThread;

    IPEndPoint serverEP;

    int myId = -1;

    // =========================
    // OBJETOS
    // =========================

    public GameObject localCube;
    public GameObject remoteCube;

    public GameObject ball;

    public bool isHost;

    // =========================
    // POSIÇÕES RECEBIDAS
    // =========================

    Vector3 remotePos = Vector3.zero;
    Vector3 remoteBallPos = Vector3.zero;

    // =========================
    // PLACAR
    // =========================

    public int scorePlayer1 = 0;
    public int scorePlayer2 = 0;

    void Start()
    {
        client = new UdpClient();

        serverEP = new IPEndPoint(
            IPAddress.Parse(
                ip
            ),
            5001
        );

        client.Connect(serverEP);

        receiveThread = new Thread(ReceiveData);
        receiveThread.IsBackground = true;
        receiveThread.Start();

        // Identifica o cliente
        SendMessageToServer("HELLO");

        isHost =
            GameManager.instance.isHost;

        // =========================
        // REFERÊNCIAS DOS PLAYERS
        // =========================

        if (isHost)
        {
            localCube =
                GameObject.Find("Player1");

            remoteCube =
                GameObject.Find("Player2");
        }
        else
        {
            localCube =
                GameObject.Find("Player2");

            remoteCube =
                GameObject.Find("Player1");
        }

        // =========================
        // REFERÊNCIA DA BOLA
        // =========================

        ball =
            GameObject.Find("Ball");

        remotePos =
            remoteCube.transform.position;

        remoteBallPos =
            ball.transform.position;
    }

    void Update()
    {
        // =====================================
        // MOVIMENTO DO PLAYER LOCAL
        // =====================================

        float h =
            Input.GetAxis("Horizontal");

        float v =
            Input.GetAxis("Vertical");

        localCube.transform.Translate(
            new Vector3(h, v, 0) *
            Time.deltaTime *
            5
        );

        // =====================================
        // ENVIA POSIÇÃO DO PLAYER
        // =====================================

        string playerMessage =
            "POS:" +
            localCube.transform.position.x.ToString(
                "F2",
                CultureInfo.InvariantCulture
            ) +
            ";" +
            localCube.transform.position.y.ToString(
                "F2",
                CultureInfo.InvariantCulture
            );

        SendMessageToServer(playerMessage);

        // =====================================
        // ENVIA POSIÇÃO DA BOLA
        // =====================================

        string ballMessage =
            "BALL:" +
            ball.transform.position.x.ToString(
                "F2",
                CultureInfo.InvariantCulture
            ) +
            ";" +
            ball.transform.position.y.ToString(
                "F2",
                CultureInfo.InvariantCulture
            );

        SendMessageToServer(ballMessage);

        // =====================================
        // ATUALIZA PLAYER REMOTO
        // =====================================

        remoteCube.transform.position =
            Vector3.Lerp(
                remoteCube.transform.position,
                remotePos,
                Time.deltaTime * 10f
            );

        // =====================================
        // ATUALIZA BOLA
        // =====================================

        ball.transform.position =
            Vector3.Lerp(
                ball.transform.position,
                remoteBallPos,
                Time.deltaTime * 10f
            );
    }

    // =====================================
    // RECEBIMENTO
    // =====================================

    void ReceiveData()
    {
        IPEndPoint remoteEP =
            new IPEndPoint(
                IPAddress.Any,
                0
            );

        while (true)
        {
            try
            {
                byte[] data =
                    client.Receive(
                        ref remoteEP
                    );

                string msg =
                    Encoding.UTF8.GetString(data);

                // =================================
                // ID
                // =================================

                if (msg.StartsWith("ASSIGN:"))
                {
                    myId =
                        int.Parse(
                            msg.Substring(7)
                        );

                    Debug.Log(
                        "[Cliente] Meu ID = " +
                        myId
                    );
                }

                // =================================
                // PLAYER
                // =================================

                else if (msg.StartsWith("POS:"))
                {
                    string[] parts =
                        msg.Substring(4)
                           .Split(';');

                    if (parts.Length == 3)
                    {
                        int id =
                            int.Parse(parts[0]);

                        // Só atualiza o outro player
                        if (id != myId)
                        {
                            float x =
                                float.Parse(
                                    parts[1],
                                    CultureInfo.InvariantCulture
                                );

                            float y =
                                float.Parse(
                                    parts[2],
                                    CultureInfo.InvariantCulture
                                );

                            remotePos =
                                new Vector3(
                                    x,
                                    y,
                                    0
                                );
                        }
                    }
                }

                // =================================
                // BOLA
                // =================================

                else if (msg.StartsWith("BALL:"))
                {
                    string[] parts =
                        msg.Substring(5)
                           .Split(';');

                    if (parts.Length == 2)
                    {
                        float x =
                            float.Parse(
                                parts[0],
                                CultureInfo.InvariantCulture
                            );

                        float y =
                            float.Parse(
                                parts[1],
                                CultureInfo.InvariantCulture
                            );

                        remoteBallPos =
                            new Vector3(
                                x,
                                y,
                                0
                            );
                    }
                }

                // =================================
                // SCORE
                // =================================

                else if (msg.StartsWith("SCORE:"))
                {
                    string[] parts =
                        msg.Substring(6)
                           .Split(';');

                    if (parts.Length == 2)
                    {
                        scorePlayer1 =
                            int.Parse(parts[0]);

                        scorePlayer2 =
                            int.Parse(parts[1]);

                        Debug.Log(
                            "Placar: " +
                            scorePlayer1 +
                            " x " +
                            scorePlayer2
                        );
                    }
                }
            }
            catch
            {
                // Socket fechado ou pacote inválido
            }
        }
    }

    // =====================================
    // ENVIA MENSAGEM
    // =====================================

    void SendMessageToServer(string message)
    {
        byte[] data =
            Encoding.UTF8.GetBytes(message);

        client.Send(
            data,
            data.Length
        );
    }

    // =====================================
    // ENVIA PLACAR
    // =====================================

    public void SendScore(
        int player1Score,
        int player2Score
    )
    {
        string message =
            "SCORE:" +
            player1Score +
            ";" +
            player2Score;

        SendMessageToServer(message);
    }

    // =====================================
    // ENCERRAMENTO
    // =====================================

    void OnApplicationQuit()
    {
        if (receiveThread != null)
            receiveThread.Abort();

        if (client != null)
            client.Close();
    }
}