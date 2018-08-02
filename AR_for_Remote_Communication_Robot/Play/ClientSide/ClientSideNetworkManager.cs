using UnityEngine;
using System.Collections;
using Windows.Kinect;
using System.Linq;
/// <summary>
/// クライアント側ネットワーク管理
/// </summary>
public class ClientSideNetworkManager : MonoBehaviour
{

    #region Parameters
    public Quaternion[] qq;
    public BodySourceManager _BodyManager;
    public FaceToCam _FaceToCam;
    public bool MTH_open = false;
    public bool EYE_CLOSE=false;
    NetworkView _thisNetwork = null;
    public double HandLeftBe_X = 0;
    public double HandRightBe_X = 0;
    public double HandLeftAf_X = 0;
    public double HandRightAf_X = 0;
    public double HandLeftBe_Y = 0;
    public double HandRightBe_Y = 0;
    public double HandLeftAf_Y = 0;
    public double HandRightAf_Y = 0;
    public bool RLstate = false;
    public bool RLstateTrue = false;
    

    NetworkView thisNetwork
    {
        get { return _thisNetwork ?? (_thisNetwork = this.GetComponent<NetworkView>()); }
    }

    [Header("<接続先IPアドレス>")]
    [SerializeField]
    string ipAddress = "192.168.1.1";
    [Header("<ポート番号>")]
    [SerializeField]
    int port = 8888;

    #endregion

    void Start()
    {
        if (ErrorCheckOfParameters()) return;

        // サーバへ接続
        Network.Connect(ipAddress, port);
    }
    public bool IFState(bool AA, bool BB)
    {
        bool RLC = false;
        if (AA && BB)
        {
            RLC = true;
        }
        return RLC;
    }
    private void Update()
    {
        if (_BodyManager == null)
        {
            Debug.Log("_BodyManager == null");
            return;
        }

        // Bodyデータを取得する
        var data = _BodyManager.GetData();
        if (data == null)
        {
            return;
        }
        else
        {
            var body = data.FirstOrDefault(b => b.IsTracked);
            RLstateTrue = false;
            RLstate = IFState(body.HandRightState == HandState.Closed, body.HandLeftState == HandState.Closed);
            EYE_CLOSE = false;
            MTH_open = false;

            if (RLstate)
            {
                //手のX座標の位置を比較
                HandRightBe_X = HandRightAf_X;
                HandLeftBe_X = HandLeftAf_X;
                HandLeftAf_X = body.Joints[JointType.HandLeft].Position.X;
                HandRightAf_X = body.Joints[JointType.HandRight].Position.X;
                //手のY座標の位置を比較
                HandRightBe_Y = HandRightAf_Y;
                HandLeftBe_Y = HandLeftAf_Y;
                HandLeftAf_Y = body.Joints[JointType.HandLeft].Position.Y;
                HandRightAf_Y = body.Joints[JointType.HandRight].Position.Y;
                double aaa = 0;
                aaa = HandLeftAf_Y - HandLeftBe_Y;
                if (aaa < 0) { aaa = 0 - aaa; }
                if ((aaa > 0.1))
                {

                    RLstateTrue = true;
                }

            }
            var floorPlane = _BodyManager.FloorClipPlane;
            var comp = Quaternion.FromToRotation(
            new Vector3(floorPlane.X, floorPlane.Y, floorPlane.Z), Vector3.up);
            var joints = body.JointOrientations;
            var comp2 = Quaternion.AngleAxis(90, new Vector3(0, 1, 0)) *
                   Quaternion.AngleAxis(-90, new Vector3(0, 0, 1));
            var q = transform.rotation;
            transform.rotation = Quaternion.identity;
            qq = new Quaternion[27];
            qq[0] = joints[JointType.SpineBase].Orientation.ToMirror().ToQuaternion(comp);        
            qq[1] = joints[JointType.SpineMid].Orientation.ToMirror().ToQuaternion(comp);
            qq[2] = joints[JointType.SpineShoulder].Orientation.ToMirror().ToQuaternion(comp);
            qq[3] = joints[JointType.ShoulderRight].Orientation.ToMirror().ToQuaternion(comp);
            qq[4] = joints[JointType.ShoulderLeft].Orientation.ToMirror().ToQuaternion(comp);
            qq[5] = joints[JointType.ElbowRight].Orientation.ToMirror().ToQuaternion(comp);
            qq[6] = joints[JointType.WristRight].Orientation.ToMirror().ToQuaternion(comp);
            qq[7] = joints[JointType.HandRight].Orientation.ToMirror().ToQuaternion(comp);
            qq[8] = joints[JointType.ElbowLeft].Orientation.ToMirror().ToQuaternion(comp);
            qq[9] = joints[JointType.WristLeft].Orientation.ToMirror().ToQuaternion(comp);
            qq[10] = joints[JointType.HandLeft].Orientation.ToMirror().ToQuaternion(comp);
            qq[11] = joints[JointType.KneeRight].Orientation.ToMirror().ToQuaternion(comp);
            qq[12] = joints[JointType.AnkleRight].Orientation.ToMirror().ToQuaternion(comp);
            qq[13] = joints[JointType.KneeLeft].Orientation.ToMirror().ToQuaternion(comp);
            qq[14] = joints[JointType.AnkleLeft].Orientation.ToMirror().ToQuaternion(comp);      
            qq[15] = qq[1] * comp2;
            qq[16] = qq[8] * comp2;
            qq[17] = qq[9] * comp2;
            qq[18] = qq[10] * comp2;
            qq[19] = qq[5] * comp2;
            qq[20] = qq[6] * comp2;
            qq[21] = qq[7] * comp2;
            qq[22] = qq[13] * comp2;
            qq[23] = qq[14] * comp2;
            qq[24] = qq[11] * Quaternion.AngleAxis(-90, new Vector3(0, 0, 1));
            qq[25] = qq[12] * Quaternion.AngleAxis(-90, new Vector3(0, 0, 1));
            qq[26] = q;

            MTH_open = _FaceToCam.Mth_Open;
            EYE_CLOSE = _FaceToCam.Eye_CLOSE;
            thisNetwork.RPC("CallTestRPC", RPCMode.OthersBuffered, qq , RLstateTrue,MTH_open,EYE_CLOSE);
            Debug.Log(qq[23]);
        }
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
    /// サーバ接続時
    /// </summary>
    void OnConnectedToServer()
    {
        int i = 3;

        Debug.Log("Connected to Server.", this);
       // thisNetwork.RPC("CallTestRPC", RPCMode.OthersBuffered, "aa");
    }

    /// <summary>
    /// サーバ切断時
    /// </summary>
    void OnDisconnectedFromServer(NetworkDisconnection disconnection)
    {
        Debug.Log("Disconnected from Server.", this);
    }

    /// <summary>
    /// 接続失敗時
    /// </summary>
    void OnFailedToConnect(NetworkConnectionError error)
    {
        Debug.Log("Connection Error.\nError: [" + error + "]");
    }

    [RPC]
    void CallTestRPC(Quaternion[] qqq,bool state ,bool state2,bool state3)
    {
        // 同名・同引数のRPCメソッドを空でも書いておかないとRPC呼び出しでエラーになる

    }

    #endregion
}