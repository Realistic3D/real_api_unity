using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


namespace REAL.Example
{
    public class LoginPanel : MonoBehaviour
    {
        public Text status;
        public Text userName;
        public Button login;
        public Button logout;

        private void Awake()
        {
            SetStatus("Offline");
            login.onClick.AddListener(LoginClick);
            logout.onClick.AddListener(LogoutClick);
        }
        public void SetStatus(string stInfo)
        {
            status.text = stInfo;
            switch (stInfo.ToLower())
            {
                case "online":
                    EnableButtons();
                    break;
                case "offline":
                    EnableButtons(false);
                    break;
            }
        }
        private static void LoginClick()
        {
            Commons.Renderer.canvas.infoPanel.SetStatus("Connecting....");
            RealSocket.Connect();
        }
        private static void LogoutClick()
        {
            Commons.Renderer.canvas.infoPanel.SetStatus("Closing....");
            RealSocket.Logout();
        }

        private void EnableButtons(bool loggedIn = true)
        {
            login.enabled = !loggedIn;
            logout.enabled = loggedIn;
        }
    }
}
