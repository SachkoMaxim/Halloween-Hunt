using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [HideInInspector] public List<Transform> targets = null;
    [HideInInspector] public Collider2D[] obstacles = null;

    [HideInInspector] public Transform currentTarget;
    [HideInInspector] public bool targetVisible = false;
    [HideInInspector] public bool isPlayerInRange = false;

    public int GetTargetsCount() => targets == null ? 0 : targets.Count;
}
