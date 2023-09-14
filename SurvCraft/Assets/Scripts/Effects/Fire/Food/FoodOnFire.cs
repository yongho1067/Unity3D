using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodOnFire : MonoBehaviour
{
    [SerializeField] private float cookTime; // �����ų� Ÿ�µ� �ɸ��� �ð�
    private float currentTime = 0;

    private bool cookedDone; // ��������, ���̻� �ҿ� �־ ��� ������ �� �ְ�

    [SerializeField] private GameObject go_CookedItem; // ������ Ȥ�� ź ������ ��ü

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
