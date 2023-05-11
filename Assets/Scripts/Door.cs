using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private static List<Door> _all = new List<Door>();
    public static List<Door> all => _all;

    public DoorDirection direction;
    public ArenaManager owner;

    private void OnEnable()
    {
        _all.Add(this);
    }

    private void OnDisable()
    {
        _all.Remove(this);
    }

    public void SetupDirection(ArenaManager _owner)
    {
        owner = _owner;
        Vector3 doorDirectionAxis = _owner.transform.DirectionTo(transform).AxisAlign();
        if (doorDirectionAxis.z > 0.5f) direction = DoorDirection.North;
        else if (doorDirectionAxis.z < -0.5f) direction = DoorDirection.South;
        else if (doorDirectionAxis.x < -0.5f) direction = DoorDirection.West;
        else if (doorDirectionAxis.x > 0.5f) direction = DoorDirection.East;

        _owner.doors.Add(this);
    }

    public static void SetDoorState(bool istrue)
    {
        if (istrue)
        {
            for (int i = 0; i < _all.Count; i++)
            {
                //Debug.Log($"{_all[i]} istrue? {istrue}");
                if (_all[i] != null)
                {
                    _all[i].gameObject.layer = 9;
                    _all[i].gameObject.GetComponent<Renderer>().material = GameManager.gameManager.openableMat;
                }
            }
        }
        else
        {
            for (int i = 0; i < _all.Count; i++)
            {
                if (_all[i] != null)
                {
                    _all[i].gameObject.layer = 0;
                    _all[i].gameObject.GetComponent<Renderer>().material = GameManager.gameManager.nonOpenableMat;
                }
            }
        }
    }
}

public enum DoorDirection
{
    None = 0,
    South = 1,
    North = 2,
    West = 3,
    East = 4
}