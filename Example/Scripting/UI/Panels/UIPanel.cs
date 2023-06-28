using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace REAL.Example
{
    public class UIPanel : MonoBehaviour
    {
        public Image jobPreview;
        public Button closeBtn;
        public Button renderBtn;
        public Button showHideBtn;
        public Sprite showSprt, hideSprt;

        public bool showing;

        private void Awake()
        {
            showHideBtn.GetComponent<Image>().sprite = showSprt;
            ActivateView(false);
            closeBtn.onClick.AddListener(CloseClick);
            renderBtn.onClick.AddListener(RenderClick);
            showHideBtn.onClick.AddListener(ShowHideClick);
        }
    
        public void ActivateView(bool show)
        {
            showing = show;
            closeBtn.gameObject.SetActive(show);
            renderBtn.gameObject.SetActive(!show);
            showHideBtn.gameObject.SetActive(show);
        }
        private static void CloseClick()
        {
            Commons.Renderer.canvas.jobPanel.DisplayResult();
        }
        private static void RenderClick()
        {
            Commons.Renderer.canvas.infoPanel.SetStatus("Exporting....");
            JobTools.NewJob();
        }

        private void ShowHideClick()
        {
            showing = !showing;
            SwapSprite();
            jobPreview.gameObject.SetActive(showing);;
        }
    
        private void SwapSprite()
        {
            showHideBtn.GetComponent<Image>().sprite = showing ? hideSprt : showSprt;
        }
    }

}