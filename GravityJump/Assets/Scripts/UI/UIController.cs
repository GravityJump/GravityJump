﻿using UnityEngine;
using UnityEngine.UI;
using Network;
using System;
using System.Collections;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        public readonly string Version = "0.0.1";
        public readonly int Port = 3000;

        public string Ip { get; private set; }

        GameObject TitleScreen;
        GameObject GameModeSelectionScreen;
        GameObject HostScreen;
        GameObject JoinScreen;
        Text TitleScreenCaption;
        Text VersionText;
        Text IpText;
        Text JoinScreenHostIpInputText;
        Button GameModeSelectionScreenSoloButton;
        Button GameModeSelectionScreenHostButton;
        Button GameModeSelectionScreenJoinButton;
        Button GameModeSelectionScreenExitButton;
        Button HostScreenBackButton;
        Button JoinScreenBackButton;
        Button JoinScreenJoinButton;

        Stack Screens;

        IEnumerator titleScreenCoroutine;

        void Start()
        {
            this.GetGameObjects();

            this.SetButtonsCallbacks();

            this.VersionText.text = $"Version {this.Version}";
            this.Ip = Network.Utils.GetHostIpAddress();
            this.IpText.text = $"IP {this.Ip}";

            this.HideAllGameObjects();
            this.Screens = new Stack();
            this.Screens.Push(this.TitleScreen);

            this.titleScreenCoroutine = this.BlinkText(this.TitleScreenCaption, 0.7f, this.TitleScreen);
            StartCoroutine(this.titleScreenCoroutine);
        }

        void GetGameObjects()
        {
            this.TitleScreen = GameObject.Find("UICanvas/TitleScreen");
            this.GameModeSelectionScreen = GameObject.Find("UICanvas/GameModeSelectionScreen");
            this.HostScreen = GameObject.Find("UICanvas/HostScreen");
            this.JoinScreen = GameObject.Find("UICanvas/JoinScreen");
            this.TitleScreenCaption = GameObject.Find("UICanvas/TitleScreen/PressStart").GetComponent<Text>();
            this.VersionText = GameObject.Find("UICanvas/Version").GetComponent<Text>();
            this.IpText = GameObject.Find("UICanvas/Ip").GetComponent<Text>();
            this.JoinScreenHostIpInputText = GameObject.Find("UICanvas/JoinScreen/HostIpInput/HostIpInputText").GetComponent<Text>();
            this.GameModeSelectionScreenSoloButton = GameObject.Find("UICanvas/GameModeSelectionScreen/SoloButton").GetComponent<Button>();
            this.GameModeSelectionScreenHostButton = GameObject.Find("UICanvas/GameModeSelectionScreen/HostButton").GetComponent<Button>();
            this.GameModeSelectionScreenJoinButton = GameObject.Find("UICanvas/GameModeSelectionScreen/JoinButton").GetComponent<Button>();
            this.GameModeSelectionScreenExitButton = GameObject.Find("UICanvas/GameModeSelectionScreen/ExitButton").GetComponent<Button>();
            this.HostScreenBackButton = GameObject.Find("UICanvas/HostScreen/BackButton").GetComponent<Button>();
            this.JoinScreenBackButton = GameObject.Find("UICanvas/JoinScreen/BackButton").GetComponent<Button>();
            this.JoinScreenJoinButton = GameObject.Find("UICanvas/JoinScreen/JoinButton").GetComponent<Button>();
        }

        void SetButtonsCallbacks()
        {
            this.GameModeSelectionScreenSoloButton.onClick.AddListener(() => { Debug.Log("Start a solo game"); });
            this.GameModeSelectionScreenHostButton.onClick.AddListener(() => { this.Screens.Push(this.HostScreen); });
            this.GameModeSelectionScreenJoinButton.onClick.AddListener(() => { this.Screens.Push(this.JoinScreen); });
            this.GameModeSelectionScreenExitButton.onClick.AddListener(() => { Application.Quit(); });
            this.HostScreenBackButton.onClick.AddListener(() => { this.Screens.Pop(); });
            this.JoinScreenBackButton.onClick.AddListener(() => { this.Screens.Pop(); });
            this.JoinScreenJoinButton.onClick.AddListener(() => { Debug.Log($"{this.JoinScreenHostIpInputText.text}"); });
        }

        void HideAllGameObjects()
        {
            this.TitleScreen.gameObject.SetActive(false);
            this.GameModeSelectionScreen.gameObject.SetActive(false);
            this.HostScreen.gameObject.SetActive(false);
            this.JoinScreen.gameObject.SetActive(false);
            this.VersionText.gameObject.SetActive(false);
            this.IpText.gameObject.SetActive(false);
        }

        void Update()
        {
            if (this.Screens.Top() == this.TitleScreen)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    this.Screens.Push(this.GameModeSelectionScreen);
                    this.VersionText.gameObject.SetActive(true);
                    this.IpText.gameObject.SetActive(true);
                    StopCoroutine(this.titleScreenCoroutine);
                }
            }
        }

        IEnumerator BlinkText(Text text, float frequency, GameObject linkedScreen)
        {
            while (this.Screens.Top() == linkedScreen)
            {
                text.gameObject.SetActive(true);
                yield return new WaitForSeconds(frequency);
                text.gameObject.SetActive(false);
                yield return new WaitForSeconds(frequency);
            }
        }
    }
}
