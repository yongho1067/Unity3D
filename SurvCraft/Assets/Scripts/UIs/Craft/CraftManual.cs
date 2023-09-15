using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Craft
{
    public string craftName;
    public Sprite craftImage; // 이미지
    public string craftContent; // 설명
    public string[] craftNeedItems; // 필요한 아이템
    public int[] craftNeedItemsCount; // 필요한 아이템의 갯수 
    public GameObject go_Prefab; // 실제 설치될 프리팹
    public GameObject go_PreviewPrefab; // 미리보기 프리팹

}

public class CraftManual : MonoBehaviour
{
    private bool isActivated = false;
    private bool isPreviewActivated = false;

    [SerializeField] private GameObject go_BaseUI; // 기본 베이스 UI
    private int tabNum = 0;
    private int page = 1;
    private int selecteSlotNum;
    private Craft[] craft_selectedTab;

    [SerializeField] private Craft[] craft_Fire; // 모닥불용 탭
    [SerializeField] private Craft[] craft_Build; // 건설용 탭

    private GameObject go_Preview; // 미리보기 프리팹을 담을 변수
    private GameObject go_Prefab; // 실제 생성될 프리팹을 담을 변수

    [SerializeField] private Transform tf_Player; // 플레이어 위치

    // RayCast 필요 변수 선언
    private RaycastHit hitinfo;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float range;

    // 필요한 UI 슬롯
    [SerializeField] private GameObject[] go_Slots;
    [SerializeField] private Image[] image_Slot;
    [SerializeField] private TextMeshProUGUI[] text_SlotsName;
    [SerializeField] private TextMeshProUGUI[] text_SlotsContent;
    [SerializeField] private TextMeshProUGUI[] text_SlotNeedItem;

    // 필요한 컴포넌트
    [SerializeField] private Inventory inventory;

    private void Start()
    {
        tabNum = 0;
        page = 1;

        if(craft_Fire != null)
        {
            TabSlotSetting(craft_Fire);
        }

    }

    public void TabSetting(int _tabNum)
    {
        tabNum = _tabNum;
        page = 1;

        switch (tabNum)
        {
            case 0:
                // 불 셋팅
                TabSlotSetting(craft_Fire);
                break;
            case 1:
                // 건축 셋팅
                TabSlotSetting(craft_Build);
                break;

        }
    }

    private void ClearSlot()
    {
        for (int i = 0; i < go_Slots.Length; i++)
        {
            image_Slot[i].sprite = null;
            text_SlotsContent[i].text = "";
            text_SlotsName[i].text = "";
            text_SlotNeedItem[i].text = "";
            go_Slots[i].SetActive(false);
        }
    }
    
    private void TabSlotSetting(Craft[] _craft_tab)
    {
        ClearSlot();
        craft_selectedTab = _craft_tab;

        int startSlotNum;
        startSlotNum = (page - 1) * go_Slots.Length; // 0 4 8 -> 4의 배수

        for(int i = startSlotNum; i <craft_selectedTab.Length; i++)
        {
            if (i == page * go_Slots.Length)
                break;

            go_Slots[i - startSlotNum].SetActive(true);

            image_Slot[i - startSlotNum].sprite = craft_selectedTab[i].craftImage;
            text_SlotsName[i - startSlotNum].text = craft_selectedTab[i].craftName;
            text_SlotsContent[i - startSlotNum].text = craft_selectedTab[i].craftContent;

            for (int  j = 0; j < craft_selectedTab[i].craftNeedItems.Length;  j++)
            {
                text_SlotNeedItem[i - startSlotNum].text += craft_selectedTab[i].craftNeedItems[j];
                text_SlotNeedItem[i - startSlotNum].text += " x " + craft_selectedTab[i].craftNeedItemsCount[j] + "\n";
            }
        }
    }

    public void SlockClick(int _slotNum)
    {
        selecteSlotNum = _slotNum + ((page - 1) * go_Slots.Length);

        if(!CheckedIngredient())
        {
            return;
        }

        go_Preview = Instantiate(craft_selectedTab[selecteSlotNum].go_PreviewPrefab, tf_Player.position + tf_Player.forward, Quaternion.identity);
        go_Prefab = craft_selectedTab[selecteSlotNum].go_Prefab;

        isPreviewActivated = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        go_BaseUI.SetActive(false);
    }

    private bool CheckedIngredient()
    {
        for (int i = 0; i < craft_selectedTab[selecteSlotNum].craftNeedItems.Length; i++)
        {
            if (inventory.GetItemCount(craft_selectedTab[selecteSlotNum].craftNeedItems[i]) < craft_selectedTab[selecteSlotNum].craftNeedItemsCount[i])
            {
                return false;
            }
        }
        return true;
    }

    private void UseIngredient()
    {
        for (int i = 0; i < craft_selectedTab[selecteSlotNum].craftNeedItems.Length; i++)
        {
            inventory.SetItemCount(craft_selectedTab[selecteSlotNum].craftNeedItems[i],
                craft_selectedTab[selecteSlotNum].craftNeedItemsCount[i]);
                
        }
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

        if(Input.GetKeyDown(KeyCode.Escape) || (Input.GetKeyDown(KeyCode.B) && isPreviewActivated))
        {
            CraftCancle();
        }
    }

    
    private void Build()
    {
        if(isPreviewActivated && go_Preview.GetComponent<PreviewObject>().IsBuildable())
        {
            GameManager.isOpenCraftManual = false;

            UseIngredient();
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
        GameManager.isOpenCraftManual = false;

        if (Physics.Raycast(tf_Player.position, tf_Player.forward, out hitinfo, range, layerMask))
        {
            if(hitinfo.transform != null)
            {
                Vector3 _location = hitinfo.point;

                if (Input.GetKeyDown(KeyCode.Q))
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
        GameManager.isOpenCraftManual = false;

        Destroy(go_Preview);

        isActivated = false;
        isPreviewActivated = false;
        go_Preview = null;
        go_Prefab = null;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

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
        GameManager.isOpenCraftManual = true;

        isActivated = true;
        go_BaseUI.SetActive(true);
    }

    private void CloseCraftTab()
    {
        GameManager.isOpenCraftManual = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        isActivated = false;
        go_BaseUI.SetActive(false);
    }

    #region 페이지 셋팅
    public void RightPageSetting()
    {
        if(page < (craft_selectedTab.Length / go_Slots.Length) + 1) // 선택된 탭의 갯수(불, 건축) / 슬롯 갯수(4) +1 = 최대 페이디 
        {
            page++;
        }
        else // 최대 페이지 도달시
        {
            page = 1;   
        }

        TabSlotSetting(craft_selectedTab);
    }

    public void LeftPageSetting()
    {
        if (page != 1) // 최소페이지가 아니라면
        {
            page--;
        }
        else // 최소 페이지 도달시 최대 페이지로 이동
        {
            page = (craft_selectedTab.Length / go_Slots.Length) + 1;
        }

        TabSlotSetting(craft_selectedTab);
    }

    #endregion
}
