using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;

    [Header("# Game Control")]
    public bool isGameOver = false;//gamecontrol
    [SerializeField] private CountDown countDown;

    [Header("# Player Info")]
    public float curHealth;
    public float maxHealth;
    public float curMana;
    public float maxMana;
    public float MaxArmor;
    public float curArmor;
    public float MaxDamage;
    public float curDamage;
    public int skillpoint;
    float LevelUpDamage = 1.5f;
    float LevelUpArmor = 1.2f;
    float LevelUpHealth = 1.25f;
    public int level;
    public float maxExp = 2f;
    public float curExp;
    public int Kill;
    public int coin;
    public int maxHeart =3;
    public int curHeart;
    public int addHeart;


    [Header(" Game Object")]
    public GameObject characterPrefab;
    public GameObject menuSet;
    public GameObject settingSet;
    public GameObject buttonPanel;
    public GameObject BossStagePortal;
    public GameObject RevivePanel;
    public GameObject heartRevivePanel;
    public GameObject addHeartRevivePanel;
    public LevelUp UiLevelUp;
    private CharacterScripts characterScripts;
    private CharacterSpawnPoint CharacterSpawn;
    // GameManager�� �ν��Ͻ��� ������ �� �ִ� �Ӽ�
    public static GameManager Instance
    {
        get { return _instance; }
    }

    // isGameOver�� ���� �ٸ� ������


    private void Awake()
    {
        // �ν��Ͻ��� �̹� �����ϴ��� Ȯ���ϰ�, ������ �ڽ��� �Ҵ�
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            // �̹� �ν��Ͻ��� �����ϸ� �ڽ��� �ı�
            Destroy(gameObject);
        }
        countDown = FindObjectOfType<CountDown>();
    }

    private void Start()
    {
        GameLoad();
        CharacterSpawn = FindObjectOfType<CharacterSpawnPoint>();
        

    }

    private void Update()
    {
        GameMenu();
    }

    public List<BaseBuff> onBuff = new List<BaseBuff>();

    public float BuffChange(string type, float origin)
    {
        if(onBuff.Count> 0)
        {
            float temp = 0;
            for (int i=0; i<onBuff.Count; i++)
            {
                if (onBuff[i].type.Equals(type))
                    temp += origin * onBuff[i].percentage;
            }
            return origin + temp;
        }
        else
        {
            return origin;
        }
    }
    public void ChooseBuff(string type)
    {
        switch (type)
        {
            case "Atk":
                curDamage = BuffChange(type, MaxDamage);
                break;
            case "Def":
                curArmor = BuffChange(type, MaxArmor);
                break;

        }
    }
    private void PlayerStats(int Level)
    {
        Level = 1;

    }
    private void GameMenu()
    {


        if (Input.GetButtonDown("Cancel"))
        {
            if(menuSet.activeSelf)
                menuSet.SetActive(false);
            else
                menuSet.SetActive(true);
        }

    }

    public void GameSave()
    {
        PlayerPrefs.SetFloat("PlayerX",characterPrefab.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY",characterPrefab.transform.position.y);
        PlayerPrefs.SetFloat("PlayerZ",characterPrefab.transform.position.z);
        PlayerPrefs.Save();

        menuSet.SetActive(false);
    }
    public void GameLoad()
    {

        if (!PlayerPrefs.HasKey("PlayerX"))
            return;
        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        float z = PlayerPrefs.GetFloat("PlayerZ");

        characterPrefab.transform.position = new Vector3(x,y,z);
    }

    public void GameExit()
    {
        Application.Quit();
    }

    

    public void GameSetting()
    {
        if (buttonPanel.activeSelf)
        {
            buttonPanel.SetActive(false);
            settingSet.SetActive(true);
        }
        else if (settingSet.activeSelf)
        {
            buttonPanel.SetActive(true);
            settingSet.SetActive(false);
        }
    }
    public void GetExp(float exp)
    {
        if (isGameOver)
            return;
        curExp += exp;
        if (curExp >= maxExp)
        {
            maxHealth += LevelUpHealth;
            curHealth = maxHealth;
            curDamage += LevelUpDamage;
            curArmor += LevelUpArmor;
            level++;
            curExp -= maxExp;
            maxExp += 1.1f;
            skillpoint++;

        }
    }

    public void GetCoin(int coin)
    {
        if (isGameOver)
            return;
        this.coin += coin;
    }
    public void SetCharacterScripts(CharacterScripts character)
    {
        characterScripts = character;
    }

    public void ReviveYes()
    {
        curHeart--;
        heartRevivePanel.SetActive(false);
        RevivePanel.SetActive(false);
        if (characterScripts != null && curHeart>=0)
        {
            characterScripts.ActivateGameObject(); // 이 메서드는 CharacterScripts에 추가해야 함
        }
        else if (curHeart < 0)
        {
            Debug.Log("안됨");
        }
    }

    public void ReviveNo()
    {
        heartRevivePanel.SetActive(false);
        RevivePanel.SetActive(false);
    }

    public void AddReviveYes()
    {

        addHeart--;
        addHeartRevivePanel.SetActive(false);
        RevivePanel.SetActive(false);

        if (characterScripts != null && addHeart >= 0)
        {
            characterScripts.ActivateGameObject(); // 이 메서드는 CharacterScripts에 추가해야 함
        }
        else if (addHeart < 0)
        {
            Debug.Log("안됨");
        }

    }
    
    public void Revive() //만약 기본 목숨이 없다면 추가 목숨을 사용하거나 안하거나, 기본 목숨 또는 추가 목숨이 없다면 창을 띄우지 말도록 할것.
    {
        

        if (isGameOver)
            return;

            RevivePanel.SetActive(true);
            if (heartRevivePanel.activeSelf)
            {
                Debug.Log("아직 죽지 않았습니다.");
                heartRevivePanel.SetActive(false);
            }
            else if(curHeart >= 1 || addHeart >= 1)
            {
            // 기본 목숨이나 추가 목숨이 있는 경우에만 RevivePanel을 활성화하도록 수정
                if (curHeart >= 1)
                {
                    ReviveYes();
                    heartRevivePanel.SetActive(true);
                // 살리겠다는 버튼을 눌렀다면 함수 추가
            }
                else if ( curHeart <=0 && addHeart >= 1)
                {
                    AddReviveYes();
                    addHeartRevivePanel.SetActive(true);
                }
                ReviveNo();
             
        }
            else if(curHeart <=0 && addHeart<=0)
            {
                heartRevivePanel.SetActive(false);
                addHeartRevivePanel.SetActive(false);
            }
        
    }
    public void PortalOpen(int kill) //킬과 레벨이 되면 포탈이 열린다.
    {
        if (isGameOver)
            return;
        Kill += kill;
        if (Kill >= 10 && level >= 10)
        {
            if (BossStagePortal.activeSelf == false)
                BossStagePortal.SetActive(true);
        }
    }
}