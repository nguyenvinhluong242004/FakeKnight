using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using System.Collections;

public class NetworkSpeedTester : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMPro.TextMeshProUGUI speedText;
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] sprites;

    private const int NumChecks = 3; // Số lần kiểm tra tốc độ mạng
    private const float Interval = 7f; // Thời gian giữa các lần kiểm tra (giây)

    private void Start()
    {
        StartCoroutine(MeasureNetworkSpeed());
    }

    private IEnumerator MeasureNetworkSpeed()
    {
        while (true)
        {
            float totalLatency = 0f;

            for (int i = 0; i < NumChecks; i++)
            {
                float latency = PhotonNetwork.GetPing();
                totalLatency += latency;
                yield return new WaitForSeconds(0.5f); // Đợi 0.5 giây giữa các lần kiểm tra
            }

            float averageLatency = totalLatency / NumChecks;
            UpdateSpeedUI(averageLatency);

            yield return new WaitForSeconds(Interval);
        }
    }

    private void UpdateSpeedUI(float latency)
    {
        speedText.text = latency.ToString("0") + " ms";
        if (latency <= 50)
        {
            image.sprite = sprites[0];
        }    
        else if (latency <=100)
        {
            image.sprite = sprites[1];
        }
        else if (latency <=150)
        {
            image.sprite = sprites[2];
        }    
        else
        {
            image.sprite = sprites[3];
        }
        Debug.Log("Network Speed: " + latency.ToString("0.00") + " ms");
    }
}