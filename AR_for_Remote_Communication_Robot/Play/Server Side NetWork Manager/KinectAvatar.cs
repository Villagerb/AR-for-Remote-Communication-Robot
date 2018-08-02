using UnityEngine;
using System.Collections;
using Windows.Kinect;
using System.Collections.Generic;
using System.Linq;

public class KinectAvatar : MonoBehaviour
{

    public bool IsMirror = true;
    public bool RLstate = false;
    public bool ST = false;
    public Quaternion[] Joint;
    public BodySourceManager _BodyManager;
    public SerialR _SerialR;
    public GameObject _UnityChan;
    public ServerSideNetworkManager _ServerSideNetworkManager;
    public GameObject Ref;
    public GameObject Hips;
    public GameObject LeftUpLeg;
    public GameObject LeftLeg;
    public GameObject RightUpLeg;
    public GameObject RightLeg;
    public GameObject Spine1;
    public GameObject Spine2;
    public GameObject LeftShoulder;
    public GameObject LeftArm;
    public GameObject LeftForeArm;
    public GameObject LeftHand;
    public GameObject RightShoulder;
    public GameObject RightArm;
    public GameObject RightForeArm;
    public GameObject RightHand;
    public GameObject Neck;
    public GameObject Head;
    public bool Rs=false;
    public bool Ls = false;
    public double HandLeftBe_X = 0;
    public double HandRightBe_X = 0;
    public double HandLeftAf_X = 0;
    public double HandRightAf_X = 0;
    public double HandLeftBe_Y = 0;
    public double HandRightBe_Y = 0;
    public double HandLeftAf_Y = 0;
    public double HandRightAf_Y = 0;
    public SkinnedMeshRenderer MTH_DEF;
    public SkinnedMeshRenderer EYE_DEF;
    public SkinnedMeshRenderer EL_DEF;


    //ここに手の状況をがあっているか送る
    public bool IFState(bool AA,bool BB) {
        bool RLC = false;
        if (AA && BB) {
            RLC = true;
        }
        return RLC;
    }

    // Use this for initialization
    //ここでキャラクターの各所とこの変数を割り当てている。

    void Start()
    {
        Ref = _UnityChan.transform.Find("Character1_Reference").gameObject;

        Hips = Ref.gameObject.transform.Find("Character1_Hips").gameObject;
        LeftUpLeg = Hips.transform.Find("Character1_LeftUpLeg").gameObject;
        LeftLeg = LeftUpLeg.transform.Find("Character1_LeftLeg").gameObject;
        RightUpLeg = Hips.transform.Find("Character1_RightUpLeg").gameObject;
        RightLeg = RightUpLeg.transform.Find("Character1_RightLeg").gameObject;
        Spine1 = Hips.transform.Find("Character1_Spine").
                    gameObject.transform.Find("Character1_Spine1").gameObject;
        Spine2 = Spine1.transform.Find("Character1_Spine2").gameObject;
        LeftShoulder = Spine2.transform.Find("Character1_LeftShoulder").gameObject;
        LeftArm = LeftShoulder.transform.Find("Character1_LeftArm").gameObject;
        LeftForeArm = LeftArm.transform.Find("Character1_LeftForeArm").gameObject;
        LeftHand = LeftForeArm.transform.Find("Character1_LeftHand").gameObject;
        RightShoulder = Spine2.transform.Find("Character1_RightShoulder").gameObject;
        RightArm = RightShoulder.transform.Find("Character1_RightArm").gameObject;
        RightForeArm = RightArm.transform.Find("Character1_RightForeArm").gameObject;
        RightHand = RightForeArm.transform.Find("Character1_RightHand").gameObject;
        Neck = Spine2.transform.Find("Character1_Neck").gameObject;
        Head = Neck.transform.Find("Character1_Head").gameObject;

        Joint= new Quaternion[27];
    }

    // Update is called once per frame
    void Update()
    {

        ST = _ServerSideNetworkManager.ST();
        if (ST) { } else { return; }

        // Bodyデータを取得する


        // 最初に追跡している人を取得する


        
        //手の状態を判定していく、今回はグーで
        RLstate= _ServerSideNetworkManager.RLS();

        //RsとLs,つまり両手がグーならば
        if (RLstate) {

            Debug.Log("Succese!!!!!!!!!!!!!!!!!");
                    //_SerialR.Write("f");

                
             
         }
         
        // 床の傾きを取得する

        Quaternion SpineBase;
        Quaternion SpineMid;
        Quaternion SpineShoulder;
        Quaternion ShoulderLeft;
        Quaternion ShoulderRight;
        Quaternion ElbowLeft;
        Quaternion WristLeft;
        Quaternion HandLeft;
        Quaternion ElbowRight;
        Quaternion WristRight;
        Quaternion HandRight;
        Quaternion KneeLeft;
        Quaternion AnkleLeft;
        Quaternion KneeRight;
        Quaternion AnkleRight;
        for (int i = 0; i < 26; i++)
        {
            Joint[i] = _ServerSideNetworkManager.jointQ[i];

        }

        // 鏡
        if (IsMirror)
        {
            SpineBase = Joint[0];
            SpineMid = Joint[1];
            SpineShoulder = Joint[2];
            ShoulderLeft = Joint[3];
            ShoulderRight = Joint[4];
            ElbowLeft = Joint[5];
            WristLeft = Joint[6];
            HandLeft = Joint[7];
            ElbowRight = Joint[8];
            WristRight = Joint[9];
            HandRight = Joint[10];
            KneeLeft = Joint[11];
            AnkleLeft = Joint[12];
            KneeRight = Joint[13];
            AnkleRight = Joint[14];
        }
        else { }
        /* // そのまま
         else
         {
             SpineBase = joints[JointType.SpineBase].Orientation.ToQuaternion(comp);
             SpineMid = joints[JointType.SpineMid].Orientation.ToQuaternion(comp);
             SpineShoulder = joints[JointType.SpineShoulder].Orientation.ToQuaternion(comp);
             ShoulderLeft = joints[JointType.ShoulderLeft].Orientation.ToQuaternion(comp);
             ShoulderRight = joints[JointType.ShoulderRight].Orientation.ToQuaternion(comp);
             ElbowLeft = joints[JointType.ElbowLeft].Orientation.ToQuaternion(comp);
             WristLeft = joints[JointType.WristLeft].Orientation.ToQuaternion(comp);
             HandLeft = joints[JointType.HandLeft].Orientation.ToQuaternion(comp);
             ElbowRight = joints[JointType.ElbowRight].Orientation.ToQuaternion(comp);
             WristRight = joints[JointType.WristRight].Orientation.ToQuaternion(comp);
             HandRight = joints[JointType.HandRight].Orientation.ToQuaternion(comp);
             KneeLeft = joints[JointType.KneeLeft].Orientation.ToQuaternion(comp);
             AnkleLeft = joints[JointType.AnkleLeft].Orientation.ToQuaternion(comp);
             KneeRight = joints[JointType.KneeRight].Orientation.ToQuaternion(comp);
             AnkleRight = joints[JointType.AnkleRight].Orientation.ToQuaternion(comp);
         }*/

        // 関節の回転を計算する
        //調整するならここ
        var q = transform.rotation;
        transform.rotation = Quaternion.identity;


        Spine1.transform.rotation = Joint[15];
        RightArm.transform.rotation = Joint[16];
        RightForeArm.transform.rotation = Joint[17];
        RightHand.transform.rotation = Joint[18];
        LeftArm.transform.rotation = Joint[19];
        LeftForeArm.transform.rotation = Joint[20];
        LeftHand.transform.rotation = Joint[21];          
        RightUpLeg.transform.rotation = Joint[22];
        RightLeg.transform.rotation = Joint[23];
        LeftUpLeg.transform.rotation = Joint[24];
        LeftLeg.transform.rotation = Joint[25];

        if (_ServerSideNetworkManager.M_OPEN) {
            MTH_DEF.SetBlendShapeWeight(5, 100);

        }
        else
        {
            MTH_DEF.SetBlendShapeWeight(5, 0);



        }

        if (_ServerSideNetworkManager.E_CLOTH)
        {
            EYE_DEF.SetBlendShapeWeight(6, 100);
            EL_DEF.SetBlendShapeWeight(6, 100);


        }
        else
        {
            EYE_DEF.SetBlendShapeWeight(6, 0);
            EL_DEF.SetBlendShapeWeight(6, 0);


        }
        // モデルの回転を設定する
        transform.rotation = q;
    }
}