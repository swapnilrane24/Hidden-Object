using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResize : MonoBehaviour
{
    private float defaultSize = 5; //size for 480 * 800 (0.6) screen resolution

    // Start is called before the first frame update
    void Start()
    {
        float ratio = (float)Screen.width / Screen.height;

        GetComponent<Camera>().orthographicSize = (0.6f / ratio) * 5;
    }
}
