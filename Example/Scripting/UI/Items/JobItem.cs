
using REAL.Items;
using UnityEngine;
using UnityEngine.UI;

namespace REAL.Example
{
    public class JobItem : MonoBehaviour
    {
        #region Inspector

        public Image thumb;
        public Text status;

        public Sprite successIcon, failedIcon;
        
        public Job job;

        #endregion
        
        #region Private
        
        private Button _thumbButton;
        
        #endregion
        
        private void Awake()
        {
            _thumbButton = thumb.GetComponent<Button>();
            _thumbButton.onClick.AddListener(ResultClicked);
            _thumbButton.interactable = false;
        }
        
        private void ResultClicked()
        {
            JobTools.DownloadResult(job, DownloadProgress);
        }
        
        public void SaveStatus(Job jobInfo)
        {
            job = jobInfo;
            var jobStatus = job.status;
            status.text = jobStatus.ToUpper();
            if (!jobInfo.finished) return;
            if (jobInfo.result.StartsWith("http"))
            {
                thumb.sprite = successIcon;
                _thumbButton.interactable = true;
            }
            else
            {
                thumb.sprite = failedIcon;
            }
        }

        private static void DownloadProgress(float progress)
        {
            Commons.Renderer.canvas.lbPanel.SetStatus(progress);
        }
    } 
}
