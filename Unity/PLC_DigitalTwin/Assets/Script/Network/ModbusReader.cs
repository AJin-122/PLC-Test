using UnityEngine;
using EasyModbus;

public class ModbusReader : MonoBehaviour
{
    private ModbusClient modbus;
    public GameObject motorObject;

    void Start()
    {
        modbus = new ModbusClient("127.0.0.1", 5511); //GX sim3과 동일한 포트
        try
        {
            modbus.Connect();
        }
        catch
        {
            Debug.LogError("Modbus 서버 연결 실패");
        }

    }

    void Update()
    {
        if (modbus.Connected)
        {
            try
            {
                // Y0 = 출력 주소 0번
                bool[] outputs = modbus.ReadCoils(0, 1);
                bool y0 = outputs[0];

                if(y0)
                {
                    motorObject.transform.Rotate(Vector3.up * 100 * Time.deltaTime);
                }
            }
            catch
            {
                Debug.LogWarning("Modbus 데이터 읽기 실패");
            }
        }
    }

    private void OnApplicationQuit()
    {
        if(modbus != null && modbus.Connected)
            modbus.Disconnect();
    }
}
