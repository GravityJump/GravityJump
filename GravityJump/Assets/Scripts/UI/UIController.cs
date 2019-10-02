using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using System;
using System.Collections;

using Network;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        public readonly string Version = "0.0.1";

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

        Client Client;

        void Start()
        {
            this.GetGameObjects();
            this.SetButtonsCallbacks();
            this.VersionText.text = $"Version {this.Version}";
            this.HideAllGameObjects();

            this.Screens = new Stack();

            this.ConfigureNetwork();
            NetworkTransport.Init();

            this.Screens.Push(this.TitleScreen);
            StartCoroutine(this.BlinkText(this.TitleScreenCaption, 0.7f, this.TitleScreen));
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

        void ConfigureNetwork()
        {
            if (Network.Utils.IsInternetAvailable())
            {
                try
                {
                    this.Client = new Client(Network.Utils.GetHostIpAddress(), 3000);
                    this.IpText.text = $"IP {this.Client.Self.Ip}";
                }
                catch
                {
                    this.DisableMultiPlayer("No IP address");
                }
            }
            else
            {
                this.DisableMultiPlayer("No Internet connection");
            }
        }

        void DisableMultiPlayer(string reason)
        {
            this.GameModeSelectionScreenHostButton.interactable = false;
            this.GameModeSelectionScreenJoinButton.interactable = false;
            this.IpText.text = reason;

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
