using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

namespace Caged
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; set; }

        public string ClientName;

        public GameObject serverPrefab;
        public GameObject clientPrefab;

        public GameObject mainMenu;
        public GameObject serverMenu;
        public GameObject connectMenu;
        public GameObject ConnectingText;

        public Server server;

        public static int portNumber = 6321;

        public Text DebugText;

        public List<Client> Clients;

        void Start()
        {
            Instance = this;
            serverMenu.SetActive(false);
            connectMenu.SetActive(false);
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(DebugText.gameObject.transform.parent.gameObject);
            ClientName = PlayerPrefs.GetString("Player1Name");
        }

        public void ConnectButton()
        {
            mainMenu.SetActive(false);
            connectMenu.SetActive(true);
        }
        public void HostButton()
        {

            try
            {
                Server s = Instantiate(serverPrefab).GetComponent<Server>();
                s.Init();
                server = s;

                Client c = Instantiate(clientPrefab).GetComponent<Client>();
                Clients.Add(c);
                c.clientName = ClientName;
                if (c.clientName == "") { c.clientName = "Host"; }
                c.ConnectToServer("127.0.0.1", portNumber);
            }
            catch (Exception e)
            {
                debug(e.Message);
            }

            mainMenu.SetActive(false);
            serverMenu.SetActive(true);
        }

        public void ConnectToServerButton()
        {
            string hostAdress = GameObject.Find("HostInput").GetComponent<InputField>().text;
            if (hostAdress == "")
            {
                hostAdress = "127.0.0.1";
            }

            try
            {
                Client c = Instantiate(clientPrefab).GetComponent<Client>();
                c.clientName = ClientName;
                Clients.Add(c);
                c.ConnectToServer(hostAdress, portNumber);
                ConnectingText.SetActive(true);
                connectMenu.SetActive(false);
            }
            catch (Exception e)
            {
                debug(e.Message);
            }
        }
        public void BackButton()
        {
            mainMenu.SetActive(true);
            serverMenu.SetActive(false);
            connectMenu.SetActive(false);
            ConnectingText.SetActive(false);

            Server s = FindObjectOfType<Server>();
            if (s != null)
            {
                server = null;
                Destroy(s.gameObject);
            }

            Client c = FindObjectOfType<Client>();
            if (c != null)
            {
                Clients.Remove(c);
                c.CloseSocket();
                Destroy(c.gameObject);
            }

        }

        public void StartGame()
        {
            SceneManager.LoadScene(3);
        }

        public void InitializeGame()
        {
            NetworkPlayer Player1 = Board.Main.players[0].GetComponent<NetworkPlayer>();
            NetworkPlayer Player2 = Board.Main.players[1].GetComponent<NetworkPlayer>();
            Clients[0].player = Player1;
            Clients[1].player = Player2;
            Clients[0].Opponent = Clients[1];
            Clients[1].Opponent = Clients[0];

            Player1.client = Clients[0];
            Player1.local = true;
            Player1.setName(PlayerPrefs.GetString("Player1Name"));

            Player2.client = Clients[1];
            Player2.local = false;
            Player2.setName(Player2.client.clientName);

            if (server != null)
            {
                server.board = GameObject.FindObjectOfType<NetworkBoard>();
            }
            Player1.client.Send("GetTilesStart|");
            StartCoroutine(Player1.client.RequestUntil(() => Player1.Tiles[2] != null, "GetTilesStart|"));
            StartCoroutine(Player1.client.RequestWhile(() => String.IsNullOrEmpty(Player1.CurrentPlayerName), "Current?"));

        }

        public static void debug(string s)
        {
            Instance.DebugText.text += s + "\n";
        }



    }
}