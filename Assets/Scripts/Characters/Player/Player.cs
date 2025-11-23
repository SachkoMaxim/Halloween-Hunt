using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    [Header("References")]
    [SerializeField] public Transform shootPoint;
    [SerializeField] public GameObject playerMarker;

    [HideInInspector] public Vector2 attackDirection;
    public static event Action OnPlayerDied;

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(ShowPlayerMarker());
    }

    public IEnumerator ShowPlayerMarker()
    {
        playerMarker.SetActive(true);
        Image img = playerMarker.GetComponent<Image>();

        float duration = 3f;
        float endTime = Time.time + duration;

        while (Time.time < endTime)
        {
            float t = Mathf.PingPong(Time.time * 2f, 1f);
            Color c = img.color;
            c.a = Mathf.Lerp(0.4f, 1f, t);
            img.color = c;

            yield return null;
        }

        playerMarker.SetActive(false);
    }

    protected override IEnumerator Die(float time)
    {
        yield return StartCoroutine(base.Die(time));
        OnPlayerDied.Invoke();
    }
}
