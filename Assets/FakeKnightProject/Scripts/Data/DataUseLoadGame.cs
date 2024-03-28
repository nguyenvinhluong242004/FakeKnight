using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DataInServer
{
    public Vector3[] enemySlimePositions;
    public Vector3[] enemyDevilPositions;
    public Vector3[] NPCPositions;
    public Vector3[] NPCKillPositionsL;
    public Vector3[] NPCKillPositionsR;
    public DataInServer(int eS, int eD, int nP, int nKL, int nKR) // cập nhật số nhân vật, quái ban đầu
    {
        enemySlimePositions = new Vector3[eS];
        enemyDevilPositions = new Vector3[eD];
        NPCPositions = new Vector3[nP];
        NPCKillPositionsL = new Vector3[nKL];
        NPCKillPositionsR = new Vector3[nKR];
    }
}
[System.Serializable]
public class ServerData
{
    public string _nameServer;
    public DataInServer dataInServers = new DataInServer(5, 5, 1, 1, 1); 
}
[CreateAssetMenu(fileName = "GameProgressData", menuName = "Custom/GameProgressData", order = 1)]
public class DataUseLoadGame : ScriptableObject
{
    public List<ServerData> ServersData = new List<ServerData>();
    public string _name;

    public void add(string _name) // thêm dữ liệu vào server
    {
        if (getIdServer(_name) >= 0)
            return;
        ServerData server = new ServerData();
        server._nameServer = _name;

        server.dataInServers.enemySlimePositions[0] = new Vector3(5.5f, 0, 0);
        server.dataInServers.enemySlimePositions[1] = new Vector3(11, 2, 0);
        server.dataInServers.enemySlimePositions[2] = new Vector3(15, -2, 0);
        server.dataInServers.enemySlimePositions[3] = new Vector3(3, -6, 0);
        server.dataInServers.enemySlimePositions[4] = new Vector3(30, 0, 0);

        server.dataInServers.enemyDevilPositions[0] = new Vector3(0, -16, 0);
        server.dataInServers.enemyDevilPositions[1] = new Vector3(3, -24, 0);
        server.dataInServers.enemyDevilPositions[2] = new Vector3(17, -17, 0);
        server.dataInServers.enemyDevilPositions[3] = new Vector3(25, -25, 0);
        server.dataInServers.enemyDevilPositions[4] = new Vector3(50, -2, 0);

        server.dataInServers.NPCPositions[0] = new Vector3(4.52f, 2.14f, 0);

        server.dataInServers.NPCKillPositionsR[0] = new Vector3(29.5f, -12.4f, 0);
        server.dataInServers.NPCKillPositionsL[0] = new Vector3(33.8f, -12.4f, 0);

        ServersData.Add(server);
    }
    public int getIdServer(string _nameServer)
    {
        for (int i = 0; i < ServersData.Count; i++)
            if (ServersData[i]._nameServer == _nameServer)
                return i;
        return -1;
    }    
}