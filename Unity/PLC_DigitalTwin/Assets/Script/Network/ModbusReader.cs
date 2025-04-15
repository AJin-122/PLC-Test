using UnityEngine;
using EasyModbus;

public class ModbusReader : MonoBehaviour
{
    private ModbusClient modbus;
    public GameObject motorObject;

    void Start()
    {
        modbus = new ModbusClient("127.0.0.1", 5511); //GX sim3�� ������ ��Ʈ
        try
        {
            modbus.Connect();
        }
        catch
        {
            Debug.LogError("Modbus ���� ���� ����");
        }

    }

    void Update()
    {
        if (modbus.Connected)
        {
            try
            {
                // Y0 = ��� �ּ� 0��
                bool[] outputs = modbus.ReadCoils(0, 1);
                bool y0 = outputs[0];

                if(y0)
                {
                    motorObject.transform.Rotate(Vector3.up * 100 * Time.deltaTime);
                }
            }
            catch
            {
                Debug.LogWarning("Modbus ������ �б� ����");
            }
        }
    }

    private void OnApplicationQuit()
    {
        if(modbus != null && modbus.Connected)
            modbus.Disconnect();
    }
}
