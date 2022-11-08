using System.Collections;
using System.Collections.Generic;
using Core;
using DefaultNamespace;
using Unity.Mathematics;
using UnityEngine;

class MoveSocketStruct
{
    public LoopBlock Block;

    public MoveSocketStruct Next;
    // public MoveSocketStruct Prev;
}

public class LoopMoveGrid : MonoBehaviour
{
    public float MoveWidth = 6;
    public float MoveSpeed = 0;
    public List<LoopBlock> MoveSockets;
    private List<MoveSocketStruct> MoveSocketStructs;
    private float _endPos;

    void Start()
    {
        MoveSocketStructs = new List<MoveSocketStruct>();
        foreach (var socket in MoveSockets)
        {
            MoveSocketStructs.Add(new MoveSocketStruct()
            {
                Block = socket
            });
        }

        int num = MoveSocketStructs.Count;
        for (int i = 0; i < num; i++)
        {
            MoveSocketStructs[i].Next = MoveSocketStructs[(i + 1) % num];
            // MoveSocketStructs[i].Prev = MoveSocketStructs[(i + num - 1) % num];
        }

        _endPos = MoveSockets[^1].transform.localPosition.x;
    }

    void FixedUpdate()
    {
        bool doMove = false;
        MoveSocketStruct lastSocket = new MoveSocketStruct();
        float distance = MoveSpeed * Time.fixedDeltaTime;
        foreach (var socket in MoveSocketStructs)
        {
            var pos = socket.Block.transform.localPosition;
            pos.x -= distance;
            if (pos.x < _endPos)
            {
                doMove = true;
                lastSocket = socket;
            }
            else
            {
                socket.Block.transform.localPosition = pos;
            }
        }

        if (doMove)
        {
            var movePos = lastSocket.Next.Block.transform.localPosition;
            movePos.x += MoveWidth;
            lastSocket.Block.transform.localPosition = movePos;
            var encounter = Instantiate(GameManager.Instance.EncounterGo, lastSocket.Block.IncidentSocket);
            encounter.transform.localPosition = Vector3.zero;
        }
    }
}