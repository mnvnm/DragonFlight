using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIDlg : MonoBehaviour
{

    [SerializeField] Image HpBarImg; // 체력바 이미지
    [SerializeField] Image ExpBarImg; // 경험치바 이미지
    [SerializeField] Image BombCooltimeImg; // 폭탄 쿨타임 이미지
    [SerializeField] TextMeshProUGUI BombCountTxt; // 플레이어가 소지한 폭탄 개수 텍스트
    [SerializeField] TextMeshProUGUI LevelTxt; // 플레이어 레벨 텍스트

    private void Update()
    {
        ShowUI();
    }

    void ShowUI()
    {
        float hpBarFillAmount = (float)GameManager.Inst.player.GetPlayerCurHP() / (float)GameManager.Inst.player.GetPlayerMaxHP();
        float expBarFillAmount = (float)LevelController.Inst.GetCurExp() / (float)LevelController.Inst.GetMaxExp();
        float bombImageFillAmount = GameManager.Inst.player.GetBombCurCooltime() / GameManager.Inst.player.GetBombMaxCooltime();
        HpBarImg.fillAmount = Mathf.Lerp(HpBarImg.fillAmount, hpBarFillAmount, 4 * Time.deltaTime);
        ExpBarImg.fillAmount = Mathf.Lerp(ExpBarImg.fillAmount, expBarFillAmount, 4 * Time.deltaTime);
        BombCooltimeImg.fillAmount = bombImageFillAmount;
        BombCountTxt.text = string.Format("Bomb : {0}", GameManager.Inst.player.GetPlayerBombCount());
        LevelTxt.text = string.Format("Lv : {0}", LevelController.Inst.GetLevel() < LevelController.Inst.GetMaxLevel() ? LevelController.Inst.GetLevel() : "MAX");
    }
}
