using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class RainingItems : MonoBehaviour
{
    public GameObject[] objects;
    public GameObject parent;
    public float duration;
    public float fallRate;
    public float spinRate;
    float fallTimer;

    void Update()
    {
        if(fallTimer > 0)
        {
            fallTimer -= Time.deltaTime;
        }
        else
        {
            int rng = Random.Range(0, objects.Length);
            GameObject item = Instantiate(objects[rng], parent.transform);
            item.transform.position = new Vector2(Random.Range(0f, Screen.width), Screen.height + 100);

            int rng2 = Random.Range(0, 2);
            if(rng2 == 0)
                item.transform.DORotate(new Vector3(0, 0, Random.Range(spinRate, spinRate+200)), duration);
            else
                item.transform.DORotate(new Vector3(0, 0, Random.Range(-spinRate, -spinRate - 200)), duration);
            item.transform.DOMoveY(-200, duration).SetEase(Ease.Linear);
            item.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);

            Destroy(item, duration);

            fallTimer = fallRate;
        }
    }
}
