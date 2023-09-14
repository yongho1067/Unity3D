using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodOnFire : MonoBehaviour
{
    [SerializeField] private float cookTime; // 익히거나 타는데 걸리는 시간
    private float currentTime = 0;

    private bool cookedDone; // 끝났으면, 더이상 불에 있어도 계산 무시할 수 있게

    [SerializeField] private GameObject go_CookedItem; // 익혀진 혹은 탄 아이템 교체

    private void OnTriggerStay(Collider other)
    {
        if(other.transform.tag == "Fire" && !cookedDone)
        {
            currentTime += Time.deltaTime;

            if(currentTime >= cookTime)
            {
                cookedDone = true;
                Instantiate(go_CookedItem, transform.position, Quaternion.Euler(transform.eulerAngles));
                Destroy(gameObject);
            }
        }
    }
}
