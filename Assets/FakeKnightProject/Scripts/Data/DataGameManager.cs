using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class DataGameManager : MonoBehaviourPunCallbacks
{
    // Dictionary để lưu trữ trạng thái sống/chết của quái
    private Dictionary<string, bool> enemyStatus = new Dictionary<string, bool>();

    // Gọi khi một quái bị giết
    public void EnemyKilled(string enemyID)
    {
        // Cập nhật trạng thái của quái trong dictionary
        enemyStatus[enemyID] = false;
        // Gửi thông điệp đến tất cả người chơi để cập nhật trạng thái
        photonView.RPC("UpdateEnemyStatus", RpcTarget.All, enemyID, false);
    }

    // Gọi khi một quái được tái tạo
    public void RespawnEnemy(string enemyID)
    {
        // Cập nhật trạng thái của quái trong dictionary
        enemyStatus[enemyID] = true;
        // Gửi thông điệp đến tất cả người chơi để cập nhật trạng thái
        photonView.RPC("UpdateEnemyStatus", RpcTarget.All, enemyID, true);
    }

    [PunRPC]
    void UpdateEnemyStatus(string enemyID, bool status)
    {
        // Cập nhật trạng thái của quái trong dictionary
        enemyStatus[enemyID] = status;
    }

    // Kiểm tra trạng thái của quái trước khi tái tạo
    void CheckEnemyStatusBeforeRespawn(string enemyID)
    {
        if (enemyStatus.ContainsKey(enemyID) && enemyStatus[enemyID])
        {
            // Tái tạo quái
            RespawnEnemy(enemyID);
        }
        else
        {
            // Quái đã bị giết, không tái tạo
            Debug.Log("Quái đã bị giết.");
        }
    }
}