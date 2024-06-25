using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageController : MonoBehaviour
{
    public static StageController instance = null; // �̱��� ����

    public float dayCount = 1;

    public int stageNum = 0;
    public int cookCount = 0; // �丮 Ƚ��

    public bool isPreparedStage; // Ȱ��ȭ �� ���� ���������� �̵�
    public bool startItems;

    public Transform itemSpawnPoint;

    public GameObject fieldItemPrefab;

    public List<TypeofItem> stage1Items = new List<TypeofItem>(); // ���������� ������ ����Ʈ
    public List<TypeofItem> stage2Items = new List<TypeofItem>();
    public List<TypeofItem> stage3Items = new List<TypeofItem>();
    public List<TypeofItem> stage4Items = new List<TypeofItem>();
    public List<TypeofItem> stage5Items = new List<TypeofItem>();
    public List<TypeofItem> legendItems = new List<TypeofItem>();

    public NPC elfArcher;
    public NPCA humanPriest;
    public NPCS dwarfWarrior;
    public NPCD humanWarrior;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        ResetStageStatus();
        startItems = false;
    }

    void Start()
    {
        GenerateStageItemList();
    }

    void Update()
    {
        GoToNextStage();
        ResetStageStatus();
    }

    void SpawnItems()
    {
        // ���� ����
        int itemCount = 4;
        float avrMorale = 0;
        float avrCourage = 0;

        avrMorale = (elfArcher.morale + humanPriest.morale + dwarfWarrior.morale + humanWarrior.morale) / 4;
        avrCourage = (elfArcher.courage + humanPriest.courage + dwarfWarrior.courage + humanWarrior.courage) / 4;



        if (startItems) // itemCount���
        {
            itemCount = 9;
            Debug.Log("Spawn start items");
            startItems = false;
        }
        else
        {
            if (elfArcher.fullness < 20) itemCount -= 3;
            else if (elfArcher.fullness < 50) itemCount -= 2;
            else if (elfArcher.fullness < 80) itemCount -= 1;
            if (humanPriest.fullness < 20) itemCount -= 3;
            else if (humanPriest.fullness < 50) itemCount -= 2;
            else if (humanPriest.fullness < 80) itemCount -= 1;
            if (dwarfWarrior.fullness < 20) itemCount -= 3;
            else if (dwarfWarrior.fullness < 50) itemCount -= 2;
            else if (dwarfWarrior.fullness < 80) itemCount -= 1;
            if (humanWarrior.fullness < 20) itemCount -= 3;
            else if (humanWarrior.fullness < 50) itemCount -= 2;
            else if (humanWarrior.fullness < 80) itemCount -= 1;

            itemCount += (int)(avrMorale / 20);
            itemCount += (int)(avrCourage / 20);
        }
        if (itemCount < 0) itemCount = 0; // 0���� ������ 0���� ��ȯ
        Debug.Log($"ItemCount: {itemCount}");


        for (int i = 0; i < itemCount; i++) // ������ ����
        {
            int randomIndex;
            float randomX = Random.Range(-0.5f, 0.5f);
            float randomZ = Random.Range(-0.5f, 0.5f);
            Vector3 spawnPosition = itemSpawnPoint.position + new Vector3(randomX, 0.3f, randomZ); // ������ ��������Ʈ ����

            // ������ ����

            GameObject spawnedItem = Instantiate(fieldItemPrefab, spawnPosition, Quaternion.identity);
            switch (stageNum)
            {
                case 1: // �������� 1
                    randomIndex = Random.Range(0, stage1Items.Count);
                    spawnedItem.GetComponent<FieldItems>().SetItem(stage1Items[randomIndex]);
                    break;
                case 2: // �������� 2
                    randomIndex = Random.Range(0, stage2Items.Count);
                    spawnedItem.GetComponent<FieldItems>().SetItem(stage2Items[randomIndex]);
                    break;
                case 3: // �������� 3
                    randomIndex = Random.Range(0, stage3Items.Count);
                    spawnedItem.GetComponent<FieldItems>().SetItem(stage3Items[randomIndex]);
                    break;
                case 4: // �������� 4
                    randomIndex = Random.Range(0, stage4Items.Count);
                    spawnedItem.GetComponent<FieldItems>().SetItem(stage4Items[randomIndex]);
                    break;
                case 5: // �������� 5
                    randomIndex = Random.Range(0, stage5Items.Count);
                    spawnedItem.GetComponent<FieldItems>().SetItem(stage5Items[randomIndex]);
                    break;
            }
        }
    }
    void GenerateStageItemList()
    {
        if (stage1Items.Count == 0 || stage2Items.Count == 0 || stage3Items.Count == 0 || stage4Items.Count == 0 || stage5Items.Count == 0)
        {
            for (int i = 1; i < ItemDB.instance.typeofitem.Count; i++)
            {
                if (((ItemDB.instance.typeofitem[i].origin == Origins.Stage0 || ItemDB.instance.typeofitem[i].origin == Origins.Stage1) && ItemDB.instance.typeofitem[i].itemtag < 100 && ItemDB.instance.typeofitem[i].uitemimage != null) || ItemDB.instance.typeofitem[i].itemtag == 9995) stage1Items.Add(ItemDB.instance.typeofitem[i]);
                if (((ItemDB.instance.typeofitem[i].origin == Origins.Stage0 || ItemDB.instance.typeofitem[i].origin == Origins.Stage2) && ItemDB.instance.typeofitem[i].itemtag < 100 && ItemDB.instance.typeofitem[i].uitemimage != null) || ItemDB.instance.typeofitem[i].itemtag == 9996) stage2Items.Add(ItemDB.instance.typeofitem[i]);
                if (((ItemDB.instance.typeofitem[i].origin == Origins.Stage0 || ItemDB.instance.typeofitem[i].origin == Origins.Stage3) && ItemDB.instance.typeofitem[i].itemtag < 100 && ItemDB.instance.typeofitem[i].uitemimage != null) || ItemDB.instance.typeofitem[i].itemtag == 9997) stage3Items.Add(ItemDB.instance.typeofitem[i]);
                if (((ItemDB.instance.typeofitem[i].origin == Origins.Stage0 || ItemDB.instance.typeofitem[i].origin == Origins.Stage4) && ItemDB.instance.typeofitem[i].itemtag < 100 && ItemDB.instance.typeofitem[i].uitemimage != null) || ItemDB.instance.typeofitem[i].itemtag == 9998) stage4Items.Add(ItemDB.instance.typeofitem[i]);
                if (((ItemDB.instance.typeofitem[i].origin == Origins.Stage0 || ItemDB.instance.typeofitem[i].origin == Origins.Stage5) && ItemDB.instance.typeofitem[i].itemtag < 100 && ItemDB.instance.typeofitem[i].uitemimage != null) || ItemDB.instance.typeofitem[i].itemtag == 9999) stage5Items.Add(ItemDB.instance.typeofitem[i]);
                if (ItemDB.instance.typeofitem[i].itemtag == 9995 || ItemDB.instance.typeofitem[i].itemtag == 9996 || ItemDB.instance.typeofitem[i].itemtag == 9997 || ItemDB.instance.typeofitem[i].itemtag == 9998 || ItemDB.instance.typeofitem[i].itemtag == 9999) legendItems.Add(ItemDB.instance.typeofitem[i]);
            }
            Debug.Log("Stage item list Generated");
        }
    }
    void GoToNextStage() // �������� �̵��Լ�
    {
        if (isPreparedStage)
        {
            switch (stageNum)
            {
                case 0:
                    SceneManager.LoadScene("Stage1_Forest");
                    startItems = true;
                    stageNum++;
                    break;
                case 1: // �������� 1���� 2�� �̵�
                    SceneManager.LoadScene("Stage2_Ocean");
                    startItems = true;
                    stageNum++;
                    break;
                case 2: // �������� 2���� 3�� �̵�
                    SceneManager.LoadScene("Stage3_MushroomForest");
                    startItems = true;
                    stageNum++;
                    break;
                case 3: // �������� 3���� 4�� �̵�
                    SceneManager.LoadScene("Stage4_Snowy land");
                    startItems = true;
                    stageNum++;
                    break;
                case 4: // �������� 4���� 5�� �̵�
                    SceneManager.LoadScene("Stage5_Volcano");
                    startItems = true;
                    stageNum++;
                    break;
                case 5: // �������� 5���� �������� �̵�
                    SceneManager.LoadScene("ending");
                    break;
            }
        }
    }
    public void GoToNextTime() // �ð� ��ȯ �Լ�
    {
        dayCount += 0.5f;
        if (dayCount % 1 != 0)  // �� ��ȯ
        {
            SpawnItems();
            elfArcher.ResetStatus();
            humanPriest.ResetStatus();
            dwarfWarrior.ResetStatus();
            humanWarrior.ResetStatus();
        }
        else // ��ħ
        {
            elfArcher.fullness = 0;
            humanPriest.fullness = 0;
            dwarfWarrior.fullness = 0;
            humanWarrior.fullness = 0;
            Inventory.instance.ReducedFreshness();
        }
    }
    void ResetStageStatus() // �������� ���� �Լ�
    {
        isPreparedStage = false;

        elfArcher = GameObject.Find("Elf Archer").GetComponent<NPC>();
        humanPriest = GameObject.Find("Human Priest").GetComponent<NPCA>();
        dwarfWarrior = GameObject.Find("Dwarf Warrior").GetComponent<NPCS>();
        humanWarrior = GameObject.Find("Human Warrior").GetComponent<NPCD>();
        itemSpawnPoint = GameObject.Find("ItemBucket").GetComponent<Transform>(); // item������ġ �ʱ�ȭ
    }

    public void OnPrepareStage()
    {
        Debug.Log("1");
        if (Inventory.instance.items.Find(element => element.itemtag == 9995) != null || Inventory.instance.items.Find(element => element.itemtag == 9996) != null || Inventory.instance.items.Find(element => element.itemtag == 9997) != null || Inventory.instance.items.Find(element => element.itemtag == 9998) != null)
        {
            Debug.Log("2");
            isPreparedStage = true;
        }
    }

    public void SpawnLegends()
    {
        for (int i = 0; i < 4; i++) // ������ ����
        {
            float randomX = Random.Range(-0.5f, 0.5f);
            float randomZ = Random.Range(-0.5f, 0.5f);
            Vector3 spawnPosition = itemSpawnPoint.position + new Vector3(randomX, 0.3f, randomZ); // ������ ��������Ʈ ����

            // ������ ����

            GameObject spawnedItem = Instantiate(fieldItemPrefab, spawnPosition, Quaternion.identity);
            spawnedItem.GetComponent<FieldItems>().SetItem(legendItems[i]);
        }
        Debug.Log("Spawn legends");
    }
}
