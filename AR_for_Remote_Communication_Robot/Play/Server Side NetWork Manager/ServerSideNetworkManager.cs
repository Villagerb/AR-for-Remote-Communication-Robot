using UnityEngine;
using System.Collections;
using Windows.Kinect;
using System.Linq;
/// <summary>
/// サーバ側ネットワーク管理
/// </summary>
public class ServerSideNetworkManager : MonoBehaviour
{

    #region Parameters
    //public BodySourceManager _BodyManager;
    public Quaternion[] jointQ;
    public bool setuzoku = false;
    public bool RL = false;
    public bool E_CLOTH = false;
    public bool M_OPEN = false;
    NetworkView _thisNetwork = null;
    NetworkView thisNetwork
    {
        get { return _thisNetwork ?? (_thisNetwork = this.GetComponent<NetworkView>()); }
    }

    [Header("<同時接続許容数>")]
    [SerializeField]
    int concurrentConnections = 10;
    [Header("<ポート番号>")]
    [SerializeField]
    int port = 8888;

    #endregion

    void Start()
    {
        if (ErrorCheckOfParameters()) return;
        jointQ = new Quaternion[26];
        // サーバの初期化
        Network.InitializeServer(concurrentConnections, port, !Network.HavePublicAddress());
    }

    bool ErrorCheckOfParameters()
    {
        var error = false;
        if (thisNetwork == null)
        {
            Debug.LogError("NetworkViewが存在しません", this);
            error = true;
        }
        return error;
    }

    #region NetworkEvents

    /// <summary>
    /// サーバ初期化時
    /// </summary>
    /// 
    public bool ST() {

        return setuzoku;

    }
    public bool RLS()
    {

        return RL;

    }
    public Quaternion[] JQ()
    {

        return jointQ;

    }
    void OnServerInitialized()
    {
        Debug.Log("Server Initialized.", this);
    }

    /// <summary>
    /// クライアント接続時
    /// </summary>
    void OnPlayerConnected(NetworkPlayer player)
    {
        Debug.Log("Player Connected.\nIP[" + player.ipAddress + "], GUID[" + player.guid + "]", this);
    }

    /// <summary>
    /// クライアント切断時
    /// </summary>
    void OnPlayerDisconnected(NetworkPlayer player)
    {
        Debug.Log("Player Disconnected.\nIP[" + player.ipAddress + "], GUID[" + player.guid + "]", this);
        setuzoku = false;
    }

    /// <summary>
    /// RPC呼び出しテスト
    /// </summary>
    [RPC]
    void CallTestRPC(Quaternion[] qqq,bool RLstate,bool MTH_OPEN,bool EYE_CLOSE)
    {
        setuzoku = true;
        RL = RLstate;
        E_CLOTH = EYE_CLOSE;
        M_OPEN = MTH_OPEN;
        
        Debug.Log("Recieve RPC.");
       
        
        for(int i = 0; i < 26; i++)
        {
            jointQ[i]= qqq[i];

        }

    }
    #endregion
}