using UnityEngine;

public class PLCExampleController : MonoBehaviour
{
    public MitsubishiMCClient mcClient;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            mcClient.Read(); // �����̽� Ű�� �� �б�
        }
    }
}