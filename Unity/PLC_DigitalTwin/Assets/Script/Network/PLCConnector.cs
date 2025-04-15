using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class PLCConnector : MonoBehaviour
{
    public TMP_InputField ipField;
    public TMP_InputField portField;
    public TMP_Text statusText;
    public GameObject OK_Btn;
    public GameObject OFF_Btn;
    public GameObject Con_Btn;

    void Start()
    {
        OK_Btn.SetActive(false);
        OFF_Btn.SetActive(false);
        Con_Btn.SetActive(true);
    }

    public async void OnClickConnectButton()
    {
        string ip = ipField.text;
        if (!int.TryParse(portField.text, out int port))
        {
            statusText.text = "Invalid port number.";
            return;
        }

        statusText.text = "Connecting to PLC...";

        bool result = await McProtocolManager.Instance.ConnectToPLC(ip, port);
        statusText.text = result ? "PLC connected successfully." : "Failed to connect to PLC.";

        if(result)
        {
            OK_Btn.SetActive(true);
            OFF_Btn.SetActive(true);
            Con_Btn.SetActive(false);
        }
    }
}