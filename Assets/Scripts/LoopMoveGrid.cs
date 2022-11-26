using System.Collections;
using System.Collections.Generic;
using Core;
using DefaultNamespace;
using Lean.Pool;
using MoreMountains.Tools;
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
            MoveSocketStructs[i].Next = MoveSocketStructs[(i + num - 1) % num];
        }

        _endPos = MoveSockets[0].transform.localPosition.x;

        InitStartEncounters();
    }

    private void InitStartEncounters()
    {
        for (int i = 3; i < MoveSocketStructs.Count; i++)
        {
            var encounter = LeanPool.Spawn(BattleManager.Instance.EncounterEnemyPrefab, MoveSocketStructs[i].Block.IncidentSocket);
            encounter.transform.localPosition = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        if (BattleManager.Instance.Status != BattleManager.BattleStatus.Run)
        {
            return;
        }

        bool doUpdate = false;
        MoveSocketStruct lastSocket = new MoveSocketStruct();
        float distance = MoveSpeed * Time.fixedDeltaTime;
        foreach (var socket in MoveSocketStructs)
        {
            var pos = socket.Block.transform.localPosition;
            pos.x -= distance;
            if (pos.x < _endPos)
            {
                doUpdate = true;
                lastSocket = socket;
            }
            else
            {
                socket.Block.transform.localPosition = pos;
            }
        }

        if (doUpdate)
        {
            var movePos = lastSocket.Next.Block.transform.localPosition;
            movePos.x += MoveWidth;
            lastSocket.Block.transform.localPosition = movePos;
            var encounter = LeanPool.Spawn(BattleManager.Instance.EncounterEnemyPrefab, lastSocket.Block.IncidentSocket);
            encounter.transform.localPosition = Vector3.zero;
        }
    }

    /// <summary>
    /// OnDisable, we start listening to events.
    /// </summary>
    protected virtual void OnEnable()
    {
    }

    /// <summary>
    /// OnDisable, we stop listening to events.
    /// </summary>
    protected virtual void OnDisable()
    {
    }
}