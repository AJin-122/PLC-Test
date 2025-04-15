using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using MCProtocol;
using static MCProtocol.Mitsubishi;

public class MotorController : MonoBehaviour
{
    public Button forwardButton;
    public Button reverseButton;
    public Transform motor;           // 회전할 오브젝트 (예: Cylinder)
    public float rotationSpeed = 100f;

    private PLCData<bool> m0;
    private PLCData<bool> m1;
    private PLCData<bool> m100;
    private PLCData<bool> m101;

    private void Start()
    {
        // 버튼 이벤트 등록
        forwardButton.onClick.AddListener(() => SetMBit(m0));
        reverseButton.onClick.AddListener(() => SetMBit(m1));

        // PLC 연결
        PLCData.PLC = new Mitsubishi.McProtocolTcp("192.168.0.205", 5000, Mitsubishi.McFrame.MC3E);
        PLCData.PLC.Open().Wait();

        // M접점 설정
        m0 = new PLCData<bool>(PlcDeviceType.M, 0, 1);     // 정회전 요청
        m1 = new PLCData<bool>(PlcDeviceType.M, 1, 1);     // 역회전 요청
        m100 = new PLCData<bool>(PlcDeviceType.M, 100, 1); // 정회전 상태
        m101 = new PLCData<bool>(PlcDeviceType.M, 101, 1); // 역회전 상태

        // 상태 폴링 시작
        InvokeRepeating(nameof(CheckStatus), 0.5f, 0.1f);
    }

    // M접점 순간 ON 후 OFF
    private async void SetMBit(PLCData<bool> bit)
    {
        bit[0] = true;
        await bit.WriteData();
        await Task.Delay(200);
        bit[0] = false;
        await bit.WriteData();
    }

    // M100/M101 읽고 회전 방향 제어
    private async void CheckStatus()
    {
        await m100.ReadData();
        await m101.ReadData();

        if (m100[0])
            motor.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);         // 정회전
        else if (m101[0])
            motor.Rotate(-Vector3.up * rotationSpeed * Time.deltaTime);        // 역회전
    }
}
