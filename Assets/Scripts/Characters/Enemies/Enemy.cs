using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    protected override IEnumerator Die(float time)
    {
        sprRend.color = Color.gray;
        sprRend.sortingLayerName = "Background";
        sprRend.sortingOrder = 2;
        yield return StartCoroutine(base.Die(time));
        FindObjectOfType<GameController>().EnemyDied(this);
    }
}
