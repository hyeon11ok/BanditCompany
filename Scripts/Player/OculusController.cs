using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ŧ���� ��Ʈ�ѷ��� �Է��� �޾ƿ��� ��ũ��Ʈ
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
