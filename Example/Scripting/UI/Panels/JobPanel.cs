using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using REAL.Items;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace REAL.Example
{
    public class JobPanel : MonoBehaviour
    {
        #region Inspector

        public GameObject jobPrefab;
        public Transform resultArea;
        public List<JobItem> jobs;
        
        #endregion

        #region Private

        private const float Width = 250f;
        private const float Height = 250f;
        private const float Distance = 50;

        #endregion

        private void Start()
        {
            DisplayResult();
        }

        #region Result UI

        public void DisplayResult(Sprite result=null)
        {
            var show = (bool) result;
            var uiPanel = Commons.Renderer.canvas.uiPanel;
            var loginPanel = Commons.Renderer.canvas.loginPanel;
            
            uiPanel.jobPreview.sprite = result;

            uiPanel.ActivateView(show);
            uiPanel.jobPreview.gameObject.SetActive(show);
            loginPanel.gameObject.SetActive(!show);
            resultArea.transform.parent.gameObject.SetActive(!show);
        }

        #endregion

        #region Job List

        public void AddJobs(Job[] jobList)
        {
            if(jobList == null || jobList.Length == 0) return;
            foreach (var job in jobList)
            {
                AddJob(job);
            }
        }
        private void AddJob(Job job)
        {
            var updated = UpdateJob(job);
            if(updated) return;
            var count = jobs.Count;

            const float xPos = -Width / 2;
            var yPos = count > 0 ? jobs[count - 1].transform.localPosition.y - Height - Distance / 2 : -Distance;
            var item = Instantiate(jobPrefab, resultArea);
            // var xPos = item.transform.localPosition.x;
            item.transform.localPosition = new Vector3(xPos, yPos);
            var resultItem = item.GetComponent<JobItem>();
            resultItem.SaveStatus(job);
            jobs.Add(resultItem);
            UpdateRect();
        }
        private bool UpdateJob(Job job)
        {
            if (jobs.Count == 0) return false;
            foreach (var jobItem in jobs.Where(jobItem => jobItem.job.jobID == job.jobID))
            {
                jobItem.SaveStatus(job);
                return true;
            }
            return false;
        }

        #endregion
        
        #region Canvas Adjustment

        private void UpdateRect()
        {
            var count = jobs.Count;
            const float minHeight = 1000f;
            if (count < 4)
            {
                UpdateRectHeight(minHeight);
                return;
            }
            var yPos = jobs[count - 1].transform.localPosition.y;
            var exceed = count - 3;
            var endPos = yPos + Height + Distance / 2;
            var areaHeight = minHeight + exceed*(endPos - yPos);
            UpdateRectHeight(areaHeight);
        }
        private void UpdateRectHeight(float h)
        {
            var rect = resultArea.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.rect.width, h);
        }

        #endregion
    }
}
