using UnityEngine;

public class PLCExampleController : MonoBehaviour
{
    public MitsubishiMCClient mcClient;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            mcClient.Read(); // 스페이스 키로 값 읽기
        }
    }
}