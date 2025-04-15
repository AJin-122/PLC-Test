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
    public Transform motor;           // ȸ���� ������Ʈ (��: Cylinder)
    public float rotationSpeed = 100f;

    private PLCData<bool> m0;
    private PLCData<bool> m1;
    private PLCData<bool> m100;
    private PLCData<bool> m101;

    private void Start()
    {
        // ��ư �̺�Ʈ ���
        forwardButton.onClick.AddListener(() => SetMBit(m0));
        reverseButton.onClick.AddListener(() => SetMBit(m1));

        // PLC ����
        PLCData.PLC = new Mitsubishi.McProtocolTcp("192.168.0.205", 5000, Mitsubishi.McFrame.MC3E);
        PLCData.PLC.Open().Wait();

        // M���� ����
        m0 = new PLCData<bool>(PlcDeviceType.M, 0, 1);     // ��ȸ�� ��û
        m1 = new PLCData<bool>(PlcDeviceType.M, 1, 1);     // ��ȸ�� ��û
        m100 = new PLCData<bool>(PlcDeviceType.M, 100, 1); // ��ȸ�� ����
        m101 = new PLCData<bool>(PlcDeviceType.M, 101, 1); // ��ȸ�� ����

        // ���� ���� ����
        InvokeRepeating(nameof(CheckStatus), 0.5f, 0.1f);
    }

    // M���� ���� ON �� OFF
    private async void SetMBit(PLCData<bool> bit)
    {
        bit[0] = true;
        await bit.WriteData();
        await Task.Delay(200);
        bit[0] = false;
        await bit.WriteData();
    }

    // M100/M101 �а� ȸ�� ���� ����
    private async void CheckStatus()
    {
        await m100.ReadData();
        await m101.ReadData();

        if (m100[0])
            motor.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);         // ��ȸ��
        else if (m101[0])
            motor.Rotate(-Vector3.up * rotationSpeed * Time.deltaTime);        // ��ȸ��
    }
}
