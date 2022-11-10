using System.Collections;
using System.Collections.Generic;
using Core;
using DefaultNamespace;
using MoreMountains.Tools;
using Unity.Mathematics;
using UnityEngine;

class MoveSocketStruct
{
    public LoopBlock Block;

    public MoveSocketStruct Next;
    // public MoveSocketStruct Prev;
}

public class LoopMoveGrid : MonoBehaviour, MMEventListener<RunGameEvent>
{
    public float MoveWidth = 6;
    public float MoveSpeed = 0;
    public List<LoopBlock> MoveSockets;
    private List<MoveSocketStruct> MoveSocketStructs;
    private float _endPos;
    private MoveStatus _status;

    void Start()
    {
        _status = MoveStatus.Idle;
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
        if (_status != MoveStatus.Run)
        {
            return;
        }

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

    public void OnMMEvent(RunGameEvent eventType)
    {
        switch (eventType.EventType)
        {
            case RunEventTypes.RunStart:
                _status = MoveStatus.Run;
                break;
            case RunEventTypes.Encounter:
                _status = MoveStatus.Encounter;
                break;
            case RunEventTypes.Continue:
                _status = MoveStatus.Run;
                break;
        }
    }
    /// <summary>
    /// OnDisable, we start listening to events.
    /// </summary>
    protected virtual void OnEnable()
    {
        // this.MMEventStartListening<CoreGameEvent>();
        this.MMEventStartListening<RunGameEvent>();
    }

    /// <summary>
    /// OnDisable, we stop listening to events.
    /// </summary>
    protected virtual void OnDisable()
    {
        // this.MMEventStopListening<CoreGameEvent>();
        this.MMEventStopListening<RunGameEvent>();
    }
}