using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class Headtrack : MonoBehaviour
{

    [Tooltip("Object to receive msgs from realsense camera")]
    public UDPReceiver udpReceiver = null;

    [Tooltip("Default eye position when there's no one in front of the screen")]
    public Transform eyePosition = null;
	
	public Vector3 offset = new Vector3(0f,0f,0f);
	public Vector3 multiply = new Vector3(0f,0f,0f);

	/*
    public float OffsetX = 0f;
    public float OffsetY = 0f;
    public float OffsetZ = 0f;
    public float MultiplyX = 1f;
    public float MultiplyY = 1f;
    public float MultiplyZ = 1f;*/

    // Update is called once per frame
    void Update()
    {
        eyePosition.position = GetPosition();
    }

	/*
    Vector3 GetPosition()
    {
        string data = udpReceiver.GetLastestData();
        if (data.Length == 0) return eyePosition.position;

        string[] info = data.Split(',');

        // expect exactly 3 pieces of information
        if (info.Length != 3) return eyePosition.position;

        return new Vector3(
            (int.Parse(info[0])-320) * MultiplyX + OffsetX,
            (int.Parse(info[1])-180) * MultiplyY + OffsetY,
            (int.Parse(info[2])) * MultiplyZ + OffsetZ
            );
    }*/
	
	    Vector3 GetPosition()
    {
        string data = udpReceiver.GetLastestData();
        if (data.Length == 0) return eyePosition.position;

        string[] info = data.Split(',');

        // expect exactly 3 pieces of information
        if (info.Length != 3) return eyePosition.position;

        return new Vector3(
            (int.Parse(info[0])-320) * multiply.x + offset.x,
            (int.Parse(info[1])-180) * multiply.y + offset.y,
            (int.Parse(info[2])) * multiply.z + offset.z
            );
    }
}
