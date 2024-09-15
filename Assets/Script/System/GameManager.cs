using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("PlayerStatus")]
    public PlayerStat playerStat = new PlayerStat(0, 50, 100, 70, 60, 2);
    public int currentHP = 50;
    public int currentMP = 100;

    public int playerLevel;
    public int currentEXP;
    public int EXPNeed = 25;
    public int skillPoint;

    BarUI expBar;
    private Stat weaponStat = new Stat();
    private Stat passiveSkillStat = new Stat();

    [Header("Spell Card")]
    public BarUI spellCardBar;
    public int maxSpellCard = 2;
    public int currentSoul;
    public int maxSoul = 10;

    List<SpellCard> spellCardHolder = new List<SpellCard>();

    [Header("Inventory")]
    public InventoryUI inventoryUI;
    public Inventory inventory;
    [Space]
    public int coin;

    private WeaponItem selectWeapon;
    private float[] timeLeft = new float[3];

    [Header("Equipment Slot")]
    public SlotData[] equippedWeapons = new SlotData[3];
    public SlotData[] equippedSpellCards = new SlotData[3];
    public SlotData consummingItem;

    [Header("Skill")]
    public PassiveSkillController passiveSkillController;

    [Header("Camera Setting")]
    public NoiseSettings noiseSettings;

    [Header("Event UI")]
    public BarUI bossHPUI;
    public ClearAreaTextManager clearAreaText;
    public GameObject pausePanel;

    [Header("Game Time")]
    public int hours;
    public int minutes;
    public int seconds;
    public float totalSecond;

    [Header("Cursor")]
    public Texture2D cursorTexture;
    private Vector2 cursorHotspot;

    private CinemachineVirtualCamera currentCameraController;
    private CinemachineConfiner cinemachineConfiner;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        playerStat.AddNewStat(weaponStat);
        playerStat.AddNewStat(passiveSkillStat);
    }
    private void Start()
    {
        cursorHotspot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.ForceSoftware);

        expBar = GameObject.Find("Player EXP Bar").GetComponent<BarUI>();
        expBar.SetMaxValue(EXPNeed);
        expBar.SetValue(currentEXP);
        expBar.TextDisplay("LV." + playerLevel.ToString());

    }

    private void Update()
    {
        TimeCounter();

        PauseTheGame();
    }
    #region Player Status
    public void GainEXP(int amount)
    {
        
        currentEXP += amount;

        if(currentEXP >= EXPNeed)
        {
            playerLevel++;
            currentEXP -= EXPNeed;
            EXPNeed = 25 * (1 + playerLevel);
            skillPoint++;

            expBar.SetMaxValue(EXPNeed);
            passiveSkillController.UpdateData();
        }

        expBar.SetValue(currentEXP);
        expBar.TextDisplay("LV." + playerLevel.ToString());
    }
    public Stat GetPassiveStatData()
    {
        return passiveSkillStat;
    }

    public void GainEnemySoul()
    {
        if(spellCardHolder.Count < maxSpellCard)
        {
            currentSoul++;
            if (currentSoul >= maxSoul)
            {
                SpellCardManager.instance.AddSpellCard();
                currentSoul -= maxSoul;
            }

            spellCardBar.SetMaxValue(maxSoul);
            spellCardBar.SetValue(currentSoul);
        }
    }
    public void AddSpellCard(SpellCard _spellCard)
    {
        spellCardHolder.Add(_spellCard);
    }
    public void RemoveSpellCard()
    {
        spellCardHolder.RemoveAt(0);
    }
    #endregion

    #region Game Time
    void TimeCounter()
    {
        totalSecond += Time.deltaTime;

        hours = (int)(totalSecond / 3600f);
        minutes = (int)((totalSecond % 3600f) / 60f);
        seconds = (int)(totalSecond % 60f);

        SkillCD();
    }
    public void SkillCD()
    {
        for (int i = 0; i < timeLeft.Length; i++)
        {
            if (timeLeft[i] > 0)
            {
                timeLeft[i] -= Time.deltaTime;

                if (timeLeft[i] <= 0)
                    timeLeft[i] = 0;
            }
        }
    }
    #endregion

    #region Time System
    public void PauseTheGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(pausePanel.activeSelf == false)
            {
                pausePanel.SetActive(true);
                PauseGame();
            }
            else
            {
                pausePanel.SetActive(false);
                ResumeGame();
            }
        }
    }


    public void PauseGame()
    {
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
    #endregion

    #region Camera Setting
    public void ChangeCurrentCam(GameObject m_cam)
    {
        currentCameraController = m_cam.GetComponent<CinemachineVirtualCamera>();
        cinemachineConfiner = m_cam.GetComponent<CinemachineConfiner>();
    }
    public void ShakeTheCamera()
    {
        StartCoroutine(CameraShakeCO());
    }
    private IEnumerator CameraShakeCO()
    {
        CinemachineBasicMultiChannelPerlin shakeSetting = currentCameraController.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        shakeSetting.m_AmplitudeGain = 2;
        shakeSetting.m_NoiseProfile = noiseSettings;
        yield return new WaitForSeconds(0.25f);
        shakeSetting.m_AmplitudeGain = 0;
    }
    #endregion

    #region Boss HP
    public void SetMaxValue(int value)
    {
        bossHPUI.SetMaxValue(value);
    }
    public void SetValue(int value)
    {
        bossHPUI.SetValue(value);
    }
    public void TurnOffBossBar()
    {
        bossHPUI.gameObject.SetActive(false);
    }
    #endregion

    #region UI Feedback
    public void StageClear()
    {
        clearAreaText.ShowAreaClearText();
    }
    #endregion

    #region Inventory System
    public void AddItem(ItemBase item)
    {
        inventory.AddItem(item);
        inventoryUI.UpdateInventoryData();
    }
    public void GainCoin(int amount)
    {
        coin += amount;
        inventoryUI.UpdateStatusData();
    }

    public bool IsFull(ItemBase item)
    {
        return inventory.IsFull(item);
    }

    public void SetSelectedWeapon(WeaponItem _selectedWeapon)
    {
        selectWeapon = _selectedWeapon;

        UpdateWeaponStat(selectWeapon);
    }

    void UpdateWeaponStat(WeaponItem weapon)
    {
        if (weapon == null)
            return;

        weaponStat.ATK = weapon.ATK;
        weaponStat.AMPR = weapon.AMPR;
        weaponStat.SPD = weapon.SPD;
        weaponStat.ASPD = weapon.ASPD;
    }
    public WeaponItem GetSelectedWeapon()
    {
        return selectWeapon;
    }
    #endregion
}
