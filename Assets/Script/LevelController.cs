using UnityEngine;

public class LevelController : MonoBehaviour
{
    private static LevelController _inst;
    public static LevelController Inst
    {
        get
        {
            _inst = FindAnyObjectByType<LevelController>();
            if (_inst == null)
            {
                GameObject obj = new GameObject("LevelController");
                _inst = obj.AddComponent<LevelController>();
                DontDestroyOnLoad(obj);
            }
            return _inst;
        }
    }

    private int playerCurLevel = 1;
    private int playerMaxLevel = 10;
    private int playerMaxExp = 20;
    private int playerCurExp = 0;
    void Start()
    {
    }

    public void Init()
    {
        playerCurLevel = 1;
        playerMaxExp = 20;
        playerCurExp = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Inst.IsGameBegin) return;
        LevelUp();
        Cheat();
    }

    void LevelUp()
    {
        if (playerCurExp >= playerMaxExp && playerCurLevel < playerMaxLevel)
        {
            int tempExp = 0;
            if (playerCurExp > playerMaxExp)
            {
                tempExp = playerCurExp - playerMaxExp;
            }
            playerCurExp = 0 + tempExp;
            playerCurLevel++;
            playerMaxExp = (int)(playerCurLevel * 20 + (20 * 1.5f / playerCurLevel));
            if (playerCurLevel >= playerMaxLevel) playerCurExp = playerMaxExp;
        }
    }
    void Cheat()
    {
        if (Input.GetMouseButton(2))
        {
            playerCurLevel = 10;
            GameManager.Inst.player.missileCount = 5;
        }
    }

    public void AddExp(int exp)
    {
        if (playerCurLevel <= playerMaxLevel) playerCurExp += exp;
    }

    public int GetCurExp()
    {
        return playerCurExp;
    }

    public int GetMaxExp()
    {
        return playerMaxExp;
    }
    public int GetLevel()
    {
        return playerCurLevel;
    }
    public int GetMaxLevel()
    {
        return playerMaxLevel;
    }
}
