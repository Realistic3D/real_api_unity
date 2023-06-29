using UnityEngine;

namespace REAL.Example
{
    public class UICanvas : MonoBehaviour
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
            lbPanel.gameObject.SetActive(true);
            infoPanel.gameObject.SetActive(true);
            loginPanel.gameObject.SetActive(true);
            ShowRenderUI(false);
        }
        public void ShowRenderUI(bool show)
        {
            uiPanel.gameObject.SetActive(show);
            jobPanel.gameObject.SetActive(show);
        }
    }
}
