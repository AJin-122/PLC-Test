using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using MCProtocol;
using UnityEngine;

public class MitsubishiMCClient : MonoBehaviour
{
    public string plcIP = "192.168.0.205";
    public int plcPort = 5000;

    public async void Read()
    {
        PLCData.PLC = new Mitsubishi.McProtocolTcp("192.168.0.205", 5000, Mitsubishi.McFrame.MC3E);
        await PLCData.PLC.Open();

        // D300부터 1워드 읽기 (int 타입 기준)
        var d300 = new PLCData<short>(Mitsubishi.PlcDeviceType.D, 300, 1);
        await d300.ReadData();

        short value = d300[0];
        Debug.Log($"[PLC] D300 = {value}");

        // 연결 종료
        PLCData.PLC.Close();
    }
}
