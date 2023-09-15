using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Craft
{
    public string craftName;
    public GameObject go_Prefab; // ���� ��ġ�� ������
    public GameObject go_PreviewPrefab; // �̸����� ������

}

public class CraftManual : MonoBehaviour
{
    private bool isActivated = false;
    private bool isPreviewActivated = false;

    [SerializeField] private GameObject go_BaseUI; // �⺻ ���̽� UI
    [SerializeField] private Craft[] craft_fire; // ��ںҿ� ��
    private GameObject go_Preview; // �̸����� �������� ���� ����
    private GameObject go_Prefab; // ���� ������ �������� ���� ����

    [SerializeField] private Transform tf_Player; // �÷��̾� ��ġ

    // RayCast �ʿ� ���� ����
    private RaycastHit hitinfo;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float range;


    public void SlockClick(int _slotNum)
    {
        go_Preview = Instantiate(craft_fire[_slotNum].go_PreviewPrefab, tf_Player.position + tf_Player.forward, Quaternion.identity);
        go_Prefab = craft_fire[_slotNum].go_Prefab;

        isPreviewActivated = true;
        go_BaseUI.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B) && !isPreviewActivated)
        {
            CraftTab();
        }

        if(isPreviewActivated)
        {
            PreviewPositionUpdate();
        }

        if(Input.GetButtonDown("Fire1"))
        {
            Build();
        }    

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            CraftCancle();
        }
    }
    
    private void Build()
    {
        if(isPreviewActivated && go_Preview.GetComponent<PreviewObject>().IsBuildable())
        {
            Instantiate(go_Prefab, go_Preview.transform.position, go_Preview.transform.rotation);
            Destroy(go_Preview);
            isActivated = false;
            isPreviewActivated = false;
            go_Preview = null;
            go_Prefab = null;
        }
    }

    private void PreviewPositionUpdate()
    {
        if(Physics.Raycast(tf_Player.position, tf_Player.forward, out hitinfo, range, layerMask))
        {
            if(hitinfo.transform != null)
            {
                Vector3 _location = hitinfo.point;
                
                if(Input.GetKeyDown(KeyCode.Q))
                {
                    go_Preview.transform.Rotate(0f, -90f, 0f);
                }
                else if(Input.GetKeyDown(KeyCode.E))
                {
                    go_Preview.transform.Rotate(0f, 90f, 0f);
                }

                // y���� 0.1������ �������� ���� �� �̼��ϰ� ������
                _location.Set(Mathf.Round(_location.x), Mathf.Round(_location.y / 0.1f) * 0.1f, Mathf.Round(_location.z));
                go_Preview.transform.position = _location;

            }
        }
    }

    private void CraftCancle()
    {
        if(!isPreviewActivated)
        {
            Destroy(go_Preview);
        }

        isActivated = false;
        isPreviewActivated = false;
        go_Preview = null;
        go_Prefab = null;

        go_BaseUI.SetActive(false);
    }

    private void CraftTab()
    {
        if(!isActivated)
        {
            OpenCraftTab();
        }
        else
        {
            CloseCraftTab();
        }
    }

    private void OpenCraftTab()
    {
        isActivated = true;
        go_BaseUI.SetActive(true);
    }

    private void CloseCraftTab()
    {
        isActivated = false;
        go_BaseUI.SetActive(false);
    }
}
