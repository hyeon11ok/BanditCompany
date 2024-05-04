using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 오큘러스 컨트롤러의 입력을 받아오는 스크립트
public class OculusController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            GetComponent<OVRPlayerController>().Jump();
        }
    }
}
