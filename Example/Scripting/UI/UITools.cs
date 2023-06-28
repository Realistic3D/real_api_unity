using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace REAL.Example
{
    public class UITools : MonoBehaviour
    {
        #region Panels
        
        public LBPanel lbPanel;
        public UIPanel uiPanel;
        public InfoPanel infoPanel;
        public JobPanel jobPanel;
        public LoginPanel loginPanel;

        #endregion

        private void Awake()
        {
            ShowRenderUI(false);
        }
        public void ShowRenderUI(bool show)
        {
            uiPanel.gameObject.SetActive(show);
            jobPanel.gameObject.SetActive(show);
        }
    }
}
