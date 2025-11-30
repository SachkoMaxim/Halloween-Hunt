using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float alertDuration = 1.5f;

    [Header("References")]
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private GameObject exclamationMark;

    private bool previousTargetVisible = false;
    private bool isAlerting = false;

    void Update()
    {
        if (enemyAI.targetVisible && enemyAI.isPlayerInRange && !previousTargetVisible && !isAlerting)
        {
            StartCoroutine(ShowAlert());
        }

        previousTargetVisible = enemyAI.targetVisible;
    }

    IEnumerator ShowAlert()
    {
        isAlerting = true;

        exclamationMark.SetActive(true);

        yield return new WaitForSeconds(alertDuration);

        exclamationMark.SetActive(false);
        enemyAI.isPlayerInRange = false;
        isAlerting = false;
    }
}
