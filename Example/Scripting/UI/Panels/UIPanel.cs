using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace REAL.Example
{
    public class UIPanel : MonoBehaviour
    {
        public Button renderBtn;
        public Button showHideBtn;
        public Sprite showSprt, hideSprt;

        private bool _showing;

        private void Awake()
        {
            showHideBtn.GetComponent<Image>().sprite = showSprt;
            ActivateView(false);
            renderBtn.onClick.AddListener(RenderClick);
            showHideBtn.onClick.AddListener(ShowHideClick);
        }
    
        public void ActivateView(bool show)
        {
            renderBtn.gameObject.SetActive(!show);
            showHideBtn.gameObject.SetActive(show);
        }
    
        private void RenderClick()
        {
            _showing = !_showing;
            SwapSprite();
            Commons.Renderer.canvas.infoPanel.SetStatus("Exporting....");
            JobTools.NewJob();
        }

        private void ShowHideClick()
        {
            _showing = !_showing;
            SwapSprite();
        }
    
        private void SwapSprite()
        {
            showHideBtn.GetComponent<Image>().sprite = _showing ? hideSprt : showSprt;
        }
    }

}