using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;

public class PBS_BtnCon : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private string writeDeviceAddress = "M0";
    [SerializeField] private string readDeviceAddress = "M200";
    public Renderer lampRenderer;

    private string writePrefix = "";
    private int writeIndex = -1;

    private string readPrefix = "";
    private int readIndex = -1;

    private void OnEnable()
    {
        if (this.gameObject.activeSelf == false) return;

        if (!ParseDeviceAddress(writeDeviceAddress, out writePrefix, out writeIndex)) return;
        if (!ParseDeviceAddress(readDeviceAddress, out readPrefix, out readIndex)) return;

        StartCoroutine("WaitForConnectionAndStartReading");
    }

    private IEnumerator WaitForConnectionAndStartReading()
    {
        while (McProtocolManager.Instance == null || !McProtocolManager.Instance.IsConnected)
        {
            yield return new WaitForSeconds(0.5f);
        }

        InvokeRepeating(nameof(CheckPLCState), 0f, 0.05f);
    }

    public async void OnPointerDown(PointerEventData eventData)
    {
        if (!McProtocolManager.Instance.IsConnected) return;
        await McProtocolManager.Instance.WriteAsync(writePrefix, writeIndex, 1, DeviceType.Bit);
    }

    public async void OnPointerUp(PointerEventData eventData)
    {
        if (!McProtocolManager.Instance.IsConnected) return;
        await McProtocolManager.Instance.WriteAsync(writePrefix, writeIndex, 0, DeviceType.Bit);
    }

    private async void CheckPLCState()
    {
        if (string.IsNullOrEmpty(readPrefix) || readIndex < 0) return;

        int value = await McProtocolManager.Instance.ReadAsync(readPrefix, readIndex, DeviceType.Bit);
        lampRenderer.material.color = (value == 1) ? Color.red : Color.gray;
        //Debug.Log(readPrefix + readIndex);
    }

    private bool ParseDeviceAddress(string address, out string prefix, out int index)
    {
        prefix = "";
        index = -1;

        if (string.IsNullOrWhiteSpace(address) || address.Length < 2)
        {
            Debug.LogWarning($"Invalid device address: \"{address}\"");
            return false;
        }

        if (string.IsNullOrWhiteSpace(address) || address.Length < 2) return false;
        prefix = address.Substring(0, 1);
        return int.TryParse(address.Substring(1), out index);
    }
}