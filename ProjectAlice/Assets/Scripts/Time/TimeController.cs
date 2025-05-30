using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public static float gravity = -9.81f; // 使用更真实的重力值
    public struct RecordedData
    {
        public Vector3 pos;  // 修改为Vector3
        public Vector3 vel;  // 修改为Vector3
        public Quaternion rot; // 添加旋转记录
        public Vector3 scale;  // 添加缩放记录

        public float animationTime;

    }

    RecordedData[,] recordedData;
    int recordMax = 100000;
    int recordCount;
    int recordIndex;

    bool wasSteppingBack = false;

    TimeControlled[] timeObjects;

    private void Awake()
    {
        timeObjects = GameObject.FindObjectsOfType<TimeControlled>();
        recordedData = new RecordedData[timeObjects.Length, recordMax];
    }

    void Start()
    {

    }

    void Update()
    {

        bool pause = Input.GetKeyDown(KeyCode.X);// 时间暂停
        bool stepBack = Input.GetKeyDown(KeyCode.Z);// 时间倒流
        bool stepForward = Input.GetKeyDown(KeyCode.C);// 时间前进

       if (stepBack)
        {
            wasSteppingBack = true;
            if (recordIndex > 0)
            {
                recordIndex--;
                for (int objectIndex = 0; objectIndex < timeObjects.Length; objectIndex++)
                {
                    TimeControlled timeObject = timeObjects[objectIndex];
                    RecordedData data = recordedData[objectIndex, recordIndex];
                    timeObject.transform.position = data.pos;
                    timeObject.transform.rotation = data.rot;
                    timeObject.transform.localScale = data.scale;
                    timeObject.velocity = data.vel;
                    timeObject.animationTime = data.animationTime;
                    timeObject.UpdateAnimation();
                }
            }
        }
        else if (pause && stepForward)
        {
            wasSteppingBack = true;
            if (recordIndex < recordCount - 1)
            {
                recordIndex++;
                for (int objectIndex = 0; objectIndex < timeObjects.Length; objectIndex++)
                {
                    TimeControlled timeObject = timeObjects[objectIndex];
                    RecordedData data = recordedData[objectIndex, recordIndex];
                    timeObject.transform.position = data.pos;
                    timeObject.transform.rotation = data.rot;
                    timeObject.transform.localScale = data.scale;
                    timeObject.velocity = data.vel;
                    timeObject.animationTime = data.animationTime;
                    timeObject.UpdateAnimation();
                }
            }
        }
        else if (!pause && !stepBack)
        {
            if (wasSteppingBack)
            {
                recordCount = recordIndex;
                wasSteppingBack = false;
            }

            for (int objectIndex = 0; objectIndex < timeObjects.Length; objectIndex++)
            {
                TimeControlled timeObject = timeObjects[objectIndex];
                RecordedData data = new RecordedData();
                data.pos = timeObject.transform.position;
                data.rot = timeObject.transform.rotation;
                data.scale = timeObject.transform.localScale;
                data.vel = timeObject.velocity;
                data.animationTime = timeObject.animationTime;
                recordedData[objectIndex, recordCount] = data;
            }
            recordCount++;
            recordIndex = recordCount;

            foreach (TimeControlled timeObject in timeObjects)
            {
                timeObject.TimeUpdate();
                timeObject.UpdateAnimation();
            }
        }
    }
}
