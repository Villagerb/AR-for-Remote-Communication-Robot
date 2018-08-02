using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Windows.Kinect;
using Microsoft.Kinect.Face;
using System.IO;

public class FaceToCam : MonoBehaviour
{
    private KinectSensor kinectSensor;
    private int bodyCount;
    private Body[] bodies;
    private FaceFrameSource[] faceFrameSources;
    private FaceFrameReader[] faceFrameReaders;
    public GameObject bodyManager;
    public SkinnedMeshRenderer MTH_DEF;
    public SkinnedMeshRenderer EYE_DEF;
    public SkinnedMeshRenderer EL_DEF;
    //this is our game camera
    public GameObject face;
    public bool Eye_Rclose = false;
    public bool Eye_Lclose = false;
    public bool Mth_Open = false;
    public bool Eye_CLOSE = false;
    StreamWriter sx;
    StreamWriter sy;
    StreamWriter kx;

    //my value 0.005
    public float qq;

    //my value 0.01
    public float rr;

    //my value 0.0001
    public float init_state;

    //bigger values faster rotations and more noise. my value 10
    public float headSmooth;

    private float last_x = 0;
    private float[] last;
    private float last_x3 = 0;

    private float last_y = 0;

    private float last_mod = 0;


    private int updateFrame;

    /*KalmanFilterSimple1D kalman_X;
    KalmanFilterSimple1D kalman_Y;
    KalmanFilterSimple1D kalman_mod;*/

    void Start()
    {
        updateFrame = 0;
        /*kalman_X = new KalmanFilterSimple1D(f: 1, h: 1, q: qq, r: rr);
        kalman_Y = new KalmanFilterSimple1D(f: 1, h: 1, q: qq, r: rr);
        kalman_mod = new KalmanFilterSimple1D(f: 1, h: 1, q: qq, r: rr);*/

        sx = new StreamWriter("coords_X.txt");
        kx = new StreamWriter("coords_KX.txt");


        // one sensor is currently supported
        kinectSensor = KinectSensor.GetDefault();


        // set the maximum number of bodies that would be tracked by Kinect
        bodyCount = kinectSensor.BodyFrameSource.BodyCount;

        // allocate storage to store body objects
        bodies = new Body[bodyCount];

        // specify the required face frame results
        FaceFrameFeatures faceFrameFeatures =
            FaceFrameFeatures.BoundingBoxInColorSpace
                | FaceFrameFeatures.PointsInColorSpace
                | FaceFrameFeatures.BoundingBoxInInfraredSpace
                | FaceFrameFeatures.PointsInInfraredSpace
                | FaceFrameFeatures.RotationOrientation
                | FaceFrameFeatures.FaceEngagement
                | FaceFrameFeatures.Glasses
                | FaceFrameFeatures.Happy
                | FaceFrameFeatures.LeftEyeClosed
                | FaceFrameFeatures.RightEyeClosed
                | FaceFrameFeatures.LookingAway
                | FaceFrameFeatures.MouthMoved
                | FaceFrameFeatures.MouthOpen;

        // create a face frame source + reader to track each face in the FOV
        faceFrameSources = new FaceFrameSource[bodyCount];
        faceFrameReaders = new FaceFrameReader[bodyCount];
        for (int i = 0; i < bodyCount; i++)
        {
            // create the face frame source with the required face frame features and an initial tracking Id of 0
            faceFrameSources[i] = FaceFrameSource.Create(kinectSensor, 0, faceFrameFeatures);

            // open the corresponding reader
            faceFrameReaders[i] = faceFrameSources[i].OpenReader();
        }
    }
    //2fに一回更新させる

    public bool MReturn() {



        return Mth_Open;
    }
    public bool EReturn()
    {



        return Eye_CLOSE;
    }
    
    void LateUpdate()
    {
        if (updateFrame < 2)
        {
            updateFrame++;
            return;
        }
        updateFrame = 0;
        // get bodies either from BodySourceManager object get them from a BodyReader
        var bodySourceManager = bodyManager.GetComponent<BodySourceManager>();
        bodies = bodySourceManager.GetData();
        if (bodies == null)
        {
            return;
        }

        // iterate through each body and update face source
        for (int i = 0; i < bodyCount; i++)
        {
            //このfaceソースで有効な顔があるのかを確認
            // check if a valid face is tracked in this face source				
            if (faceFrameSources[i].IsTrackingIdValid)
            {
                using (FaceFrame frame = faceFrameReaders[i].AcquireLatestFrame())
                {
                    Eye_Rclose = false;
                    Eye_Lclose = false;
                    Mth_Open = false;
                    Eye_CLOSE = false;
                    if (frame != null)
                    {
                        if (frame.TrackingId == 0)
                        {
                            continue;
                        }

                        // do something with result
                        var result = frame.FaceFrameResult.FaceRotationQuaternion;
                        var debug = frame.FaceFrameResult.FacePointsInColorSpace;
                        var prope = frame.FaceFrameResult.FaceProperties;
               
                        if (frame.FaceFrameResult.FaceProperties != null)
                        {
                     
                            foreach (var item in frame.FaceFrameResult.FaceProperties) {
                                if (item.Key.ToString()=="MouthOpen") { 
                                if (MTH_DEF != null)
                                {

                                    switch (item.Value) {
                                        case DetectionResult.Yes:
                                                Mth_Open = true;
                                            MTH_DEF.SetBlendShapeWeight(5, 100);
                                            Debug.Log("全会");

                                            break;

                                        /*case DetectionResult.Maybe:
                                            //MTH_DEF.SetBlendShapeWeight(5, 50);
                                            Debug.Log("半分");
                                            break;*/
                                        case DetectionResult.No:
                                            MTH_DEF.SetBlendShapeWeight(5, 0);
                                            Debug.Log("占める");
                                            break;


                                    }
                                }

                                }

                                // 目と眉を動かす
                                if (item.Key.ToString()=="LeftEyeClosed")
                                {
                                    if ((EYE_DEF != null) && (EL_DEF != null))
                                    {
                                        switch (item.Value)
                                        {
                                            case DetectionResult.Yes:
                                                Eye_Lclose = true;
                                                /*EYE_DEF.SetBlendShapeWeight(6, 100);
                                                EL_DEF.SetBlendShapeWeight(6, 100);
                                                Debug.Log("全会");*/
                                                break;

                                            /*case DetectionResult.Maybe:
                                                EYE_DEF.SetBlendShapeWeight(6, 50);
                                                EL_DEF.SetBlendShapeWeight(6, 50);
                                                Debug.Log("半分");
                                                break;*/
                                            case DetectionResult.No:
                                                /*EYE_DEF.SetBlendShapeWeight(6, 0);
                                                EL_DEF.SetBlendShapeWeight(6, 0);
                                                Debug.Log("占める");*/
                                                break;
                                                // var value = (int)((left.intensity + right.intensity) / 2);

                                        }

                                    }
                                }

                                if (item.Key.ToString() == "RightEyeClosed")
                                {
                                    if ((EYE_DEF != null) && (EL_DEF != null))
                                    {
                                        switch (item.Value)
                                        {
                                            case DetectionResult.Yes:
                                                Eye_Rclose = true;
                                                /*EYE_DEF.SetBlendShapeWeight(6, 100);
                                                EL_DEF.SetBlendShapeWeight(6, 100);
                                                Debug.Log("全会");*/
                                                break;

                                            /*case DetectionResult.Maybe:
                                                EYE_DEF.SetBlendShapeWeight(6, 50);
                                                EL_DEF.SetBlendShapeWeight(6, 50);
                                                Debug.Log("半分");
                                                break;*/
                                            case DetectionResult.No:
                                               /* EYE_DEF.SetBlendShapeWeight(6, 0);
                                                EL_DEF.SetBlendShapeWeight(6, 0);
                                                Debug.Log("占める");*/
                                                break;
                                                // var value = (int)((left.intensity + right.intensity) / 2);

                                        }

                                    }
                                }
                                
                            }
                            if (Eye_Rclose && Eye_Lclose)
                            {
                                Eye_CLOSE = true;
                                EYE_DEF.SetBlendShapeWeight(6, 100);
                                EL_DEF.SetBlendShapeWeight(6, 100);
                                Debug.Log("全会");
                            } else { 

                                            EYE_DEF.SetBlendShapeWeight(6, 0);
                                            EL_DEF.SetBlendShapeWeight(6, 0);
                                            Debug.Log("占める");
                                // var value = (int)((left.intensity + right.intensity) / 2);

                            }

                                
                            

                        }
                    

                    //updateFrame = !updateFrame;
                }
                }
            }
            else
            {
                // check if the corresponding body is tracked 
                if (bodies[i].IsTracked)
                {
                    // update the face frame source to track this body
                    faceFrameSources[i].TrackingId = bodies[i].TrackingId;
                }
            }
        }

    }
}