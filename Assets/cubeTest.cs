using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        if (Application.isMobilePlatform)
        {
            QualitySettings.vSyncCount = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 1, 1) * Time.deltaTime * 30);
    }
}
