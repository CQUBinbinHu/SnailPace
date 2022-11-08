using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MoveSocketStruct
{
    public Transform Socket;
    public MoveSocketStruct Next;
    // public MoveSocketStruct Prev;
}

public class LoopMoveGrid : MonoBehaviour
{
    public float MoveWidth = 6;
    public float MoveSpeed = 0;
    public List<Transform> MoveSockets;
    private List<MoveSocketStruct> MoveSocketStructs;
    private float _endPos;

    void Start()
    {
        MoveSocketStructs = new List<MoveSocketStruct>();
        foreach (var socket in MoveSockets)
        {
            MoveSocketStructs.Add(new MoveSocketStruct()
            {
                Socket = socket
            });
        }

        int num = MoveSocketStructs.Count;
        for (int i = 0; i < num; i++)
        {
            MoveSocketStructs[i].Next = MoveSocketStructs[(i + 1) % num];
            // MoveSocketStructs[i].Prev = MoveSocketStructs[(i + num - 1) % num];
        }

        _endPos = MoveSockets[^1].localPosition.x;
    }

    void Update()
    {
        bool doMove = false;
        MoveSocketStruct lastSocket = new MoveSocketStruct();
        float distance = MoveSpeed * Time.deltaTime;
        foreach (var socket in MoveSocketStructs)
        {
            var pos = socket.Socket.localPosition;
            pos.x -= distance;
            if (pos.x < _endPos)
            {
                doMove = true;
                lastSocket = socket;
            }
            else
            {
                socket.Socket.localPosition = pos;
            }
        }

        if (doMove)
        {
            var movePos = lastSocket.Next.Socket.localPosition;
            movePos.x += MoveWidth;
            lastSocket.Socket.localPosition = movePos;
        }
    }
}