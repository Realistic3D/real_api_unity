using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using REAL.Networks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


namespace REAL.Example
{
    public class LoginPanel : MonoBehaviour
    {
        #region Texts

        public Text status;
        public Text balance;
        public Text userName;
        public Text prodName;

        #endregion
        
        #region Buttons
        
        public Button login;
        public Button logout;

        #endregion
        
        private void Awake()
        {
            SetStatus("Offline");
            login.onClick.AddListener(LoginClick);
            logout.onClick.AddListener(LogoutClick);
        }

        public void SetAccountInfo(AccountResponse info)
        {
            var data = info.data;
            if(data == null) return;
            userName.text = data.userName;
            prodName.text = data.prodName;
            balance.text = data.balance.ToString(CultureInfo.InvariantCulture);
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
            RealSocket.Logout();
        }

        private void EnableButtons(bool loggedIn = true)
        {
            login.enabled = !loggedIn;
            logout.enabled = loggedIn;
        }
    }
}
