using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    // 현재 코인 개수
    public int Coin = 0;
    private static GameManager _Inst;
    // 싱글톤 인스턴스
    public static GameManager Inst
    {
        get
        {
            if (_Inst == null)
            {
                _Inst = FindAnyObjectByType<GameManager>();
                if (_Inst == null)
                {
                    GameObject obj = new GameObject("GameManager");
                    _Inst = obj.AddComponent<GameManager>();
                    DontDestroyOnLoad(obj);
                }
            }
            return _Inst;
        }
    }

    public bool IsGameBegin = true;
    public Player player;
    public EnemyRespown enemys;

    public GameOverDlg gameOverDlg;
    public GameSuccessDlg gameSuccessDlg;

    private void Start()
    {
        RestartGame();
    }

    void Update()
    {
        if (!IsGameBegin) return;
        if (Coin >= 100)
        {
            gameSuccessDlg.Show(true);
            IsGameBegin = false;
        }
    }

    public void EndGame()
    {
        IsGameBegin = false;
        if (player.GetPlayerCurHP() <= 0) gameOverDlg.Show(true);
    }

    public void RestartGame()
    {
        enemys.ClearEnemy();
        IsGameBegin = true;
        Coin = 0;
        player.Init();
        enemys.Init();
        gameOverDlg.Init();
        gameSuccessDlg.Init();
        LevelController.Inst.Init();
    }
}
