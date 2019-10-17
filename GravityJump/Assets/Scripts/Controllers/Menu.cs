﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

namespace Controllers
{
    public class Menu : MonoBehaviour
    {
        UI.Stack Screens;

        UI.TitleScreen TitleScreen;
        UI.GameModeSelectionScreen GameModeSelectionScreen;
        UI.HostScreen HostScreen;
        UI.JoinScreen JoinScreen;
        UI.ChatScreen ChatScreen;

        Network.Connection Connection;
        bool Ready = false;
        bool OtherPlayerReady = false;

        String messageToSend = null;

        void Awake()
        {
            this.TitleScreen = GameObject.Find("Canvas/TitleScreen").GetComponent<UI.TitleScreen>();
            this.GameModeSelectionScreen = GameObject.Find("Canvas/GameModeSelectionScreen").GetComponent<UI.GameModeSelectionScreen>();
            this.HostScreen = GameObject.Find("Canvas/HostScreen").GetComponent<UI.HostScreen>();
            this.JoinScreen = GameObject.Find("Canvas/JoinScreen").GetComponent<UI.JoinScreen>();
            this.ChatScreen = GameObject.Find("Canvas/ChatScreen").GetComponent<UI.ChatScreen>();

            this.Screens = new UI.Stack();
            this.Connection = null;
        }

        void Start()
        {
            this.SetButtonsCallbacks();

            this.TitleScreen.Clear();
            this.GameModeSelectionScreen.Clear();
            this.HostScreen.Clear();
            this.JoinScreen.Clear();
            this.ChatScreen.Clear();

            this.Screens.Push(this.TitleScreen);
        }

        void SetButtonsCallbacks()
        {
            this.GameModeSelectionScreen.HostButton.onClick.AddListener(() =>
            {
                try
                {
                    this.Screens.Push(this.HostScreen);
                }
                catch
                {
                    Debug.Log("Could not start the listener");
                    this.Screens.Pop();
                }
            });
            this.GameModeSelectionScreen.JoinButton.onClick.AddListener(() =>
            {
                this.Screens.Push(this.JoinScreen);
            });
            this.HostScreen.Back.onClick.AddListener(() =>
            {
                this.Screens.Pop();
            });
            this.JoinScreen.Back.onClick.AddListener(() =>
            {
                this.Screens.Pop();
            });
            this.JoinScreen.Join.onClick.AddListener(() =>
            {
                try
                {
                    this.Connection = new Network.Connection(this.JoinScreen.Ip);
                    this.Screens.Push(this.ChatScreen);
                }
                catch
                {
                    Debug.Log($"Could not establish a connection with {this.JoinScreen.Ip}");
                }
            });
            this.ChatScreen.Send.onClick.AddListener(() =>
            {
                try
                {
                    this.Connection.Write(new Network.Message(this.ChatScreen.Input.text));
                    this.messageToSend = this.ChatScreen.Input.text;
                }
                catch
                {
                    Debug.Log("Connection lost");
                    this.Screens.Pop();
                }
            });
            this.ChatScreen.Quit.onClick.AddListener(() =>
            {
                this.Connection.Close();
                this.Connection = null;
                this.Screens.Pop();
            });
            this.ChatScreen.Start.onClick.AddListener(() =>
            {
                this.Ready = true;
            });
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (this.Screens.Top() == this.TitleScreen)
                {
                    this.Screens.Push(this.GameModeSelectionScreen);
                }
            }

            if (this.Screens.Top() == this.HostScreen)
            {
                if (this.Connection == null && this.HostScreen.GetConnection() != null)
                {
                    this.Connection = this.HostScreen.GetConnection();
                    this.Screens.Push(this.ChatScreen);
                }
            }

            if (this.Screens.Top() == this.ChatScreen)
            {
                if (this.messageToSend != null)
                {
                    this.ChatScreen.Input.text = "";
                    this.Connection.Write(new Network.Message(this.messageToSend));
                    this.ChatScreen.Conversation.text += $"[Me] {this.messageToSend}\n";
                    this.messageToSend = null;
                }

                if (this.Ready && this.OtherPlayerReady)
                {
                    SceneManager.LoadScene("GameScene");
                }

                this.Connection.Read(this.ChatScreen.Conversation);
            }
        }
    }
}
