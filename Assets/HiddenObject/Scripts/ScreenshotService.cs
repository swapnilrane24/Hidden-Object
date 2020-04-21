using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class ScreenshotService : MonoBehaviour
    {
        public static ScreenshotService instance;

        private Camera myCamera;

        private bool takeScreenshot = false;

        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);

            myCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                TakeScreenshotScreenSize();
            }
        }

        private void OnPostRender()
        {
            if (takeScreenshot)
            {
                takeScreenshot = false;

                RenderTexture renderTexture = myCamera.targetTexture;

                Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
                Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
                texture2D.ReadPixels(rect, 0, 0);

                byte[] byteArray = texture2D.EncodeToPNG();
                System.IO.File.WriteAllBytes(Application.dataPath + "/CameraScreenshot.png", byteArray);
                Debug.Log("Screenshot Taken");
            }
        }

        public void TakeScreenshotScreenSize()
        {
            TakeScreenshotSize(Screen.width, Screen.height);
        }

        public void TakeScreenshotSize(int width, int height)
        {
            myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
            takeScreenshot = true;
        }

    }
}