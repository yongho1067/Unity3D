using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Craft
{
    public string craftName;
    public GameObject go_Prefab; // 실제 설치될 프리팹
    public GameObject go_PreviewPrefab; // 미리보기 프리팹

}

public class CraftManual : MonoBehaviour
{
    private bool isActivated = false;
    private bool isPreviewActivated = false;

    [SerializeField] private GameObject go_BaseUI; // 기본 베이스 UI
    [SerializeField] private Craft[] craft_fire; // 모닥불용 탭
    private GameObject go_Preview; // 미리보기 프리팹을 담을 변수
    private GameObject go_Prefab; // 실제 생성될 프리팹을 담을 변수

    [SerializeField] private Transform tf_Player; // 플레이어 위치

    // RayCast 필요 변수 선언
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

                // y값은 0.1단위로 움직여서 조금 더 미세하게 움직임
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
