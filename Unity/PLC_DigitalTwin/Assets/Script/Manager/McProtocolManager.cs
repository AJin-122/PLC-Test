using System;
using System.Threading.Tasks;
using UnityEngine;
using MCProtocol;

public enum DeviceType
{
    Bit,
    Word
}

public class McProtocolManager : MonoBehaviour
{
    public static McProtocolManager Instance { get; private set; }

    private Mitsubishi.Plc plc;

    public bool IsConnected => plc?.Connected ?? false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public async Task<bool> ConnectToPLC(string ip, int port)
    {
        try
        {
            plc = new Mitsubishi.McProtocolTcp(ip, port, Mitsubishi.McFrame.MC3E);
            PLCData.PLC = plc;
            await plc.Open();
            Debug.Log($"PLC 연결 성공: {ip}:{port}");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("PLC 연결 실패: " + e.Message);
            return false;
        }
    }

    public async Task<bool> WriteAsync(string device, int address, int value, DeviceType type)
    {
        string deviceName = device + address;
        int[] data = new int[] { value };

        if (type == DeviceType.Bit)
        {
            int result = await plc.SetBitDevice(deviceName, 1, data);
            return result == 0;
        }
        else
        {
            int result = await plc.SetDevice(deviceName, value);
            return result == 0;
        }
    }

    public async Task<int> ReadAsync(string device, int address, DeviceType type)
    {
        string deviceName = device + address;

        if (type == DeviceType.Bit)
        {
            int[] result = new int[1];
            await plc.GetBitDevice(deviceName, 1, result);
            return result[0];
        }
        else
        {
            return await plc.GetDevice(deviceName);
        }
    }

    private void OnApplicationQuit()
    {
        plc?.Close();
    }
}
