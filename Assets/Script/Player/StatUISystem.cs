using Mirror;
using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatUISystem : MonoBehaviour
{
    [Tooltip("플레이어 스텟을 넣어주세요")]
    public PlayerStat source;
    [Space(5)]
    [Header("HP")]
    public Image HPGauge;
    public Image HPGaugeHelp;
    public TMP_Text HPText;
    public MMF_Player HPChangeFeedback;
    private List<MMF_ImageFill> hpImageFill;
    private MMF_TMPColor hpTextColor;
    private float hp;
    [Space(5)]
    [Header("Score")]
    public TMP_Text scoreText;
    public MMF_Player ScoreChangeFeedback;
    private MMF_TMPDilate scoreDilate;
    private MMF_TMPFontSize scoreFontSize;
    private int score;
    [Space(5)]
    [Header("SpeedLevel")]
    public TMP_Text speedLevelText;
    public MMF_Player SpeedChangeFeedback;
    private MMF_TMPColor speedLevelTextColor;
    private int speedLevel;
    [Space(5)]
    [Header("Attack")]
    public TMP_Text attackText;
    public MMF_Player attackChangeFeedback;
    public MMF_Player attackUIFeedback;
    private MMF_ImageFill attackUIImageFill;
    public TMP_Text BulletRemain;
    public MMF_Player BulletRemainFeedback;
    private void Start()
    {
        hp = source.Hp;
        score = source.Score;
        speedLevel = source.Speed_level;
        updateAttack();
        updateRemainBullet();
        updateScore();
        updateHP();
    }

    private void Update()
    {
        updateHP();
        updateScore();
        updateSpeedLevel();
    }

    #region HP
    public void updateHP()
    {
        if (hp == source.Hp)
            return;
        if (hpImageFill == null)
            hpImageFill = HPChangeFeedback.GetFeedbacksOfType<MMF_ImageFill>();
        if (hpTextColor == null)
            hpTextColor = HPChangeFeedback.GetFeedbackOfType<MMF_TMPColor>(MMF_Player.AccessMethods.First, 0); ;

        HPText.text = String.Format("{0}/{1}", source.Hp, source.Max_hp);

        if (hp > source.Hp) // 기존 hp보다 줄어듦 -> 데미지
            DamageHP();
        else // 기존보다 hp가 많음 -> 힐
            HealHP();

        HPChangeFeedback?.PlayFeedbacks();
        hp = source.Hp;
    }
    public void HealHP()
    {
        if (hpTextColor != null)
        {
            hpTextColor.DestinationColor = new Color(0, 200, 0);
        }
        if (hpImageFill != null)
        {
            hpImageFill[0].BoundImage = HPGaugeHelp;
            hpImageFill[0].CurveRemapOne = source.Hp / source.Max_hp;
            hpImageFill[1].BoundImage = HPGauge;
            hpImageFill[1].CurveRemapZero = hp / source.Max_hp;
            hpImageFill[1].CurveRemapOne = source.Hp / source.Max_hp;
        }
    }
    public void DamageHP()
    {
        if (hpTextColor != null)
        {
            hpTextColor.DestinationColor = new Color(200, 0, 0);
        }
        if (hpImageFill != null)
        {
            hpImageFill[0].BoundImage = HPGauge;
            hpImageFill[0].CurveRemapOne = source.Hp / source.Max_hp;
            hpImageFill[1].BoundImage = HPGaugeHelp;
            hpImageFill[1].CurveRemapZero = hp / source.Max_hp;
            hpImageFill[1].CurveRemapOne = source.Hp / source.Max_hp;
        }
    }
    #endregion
    #region Score
    public void updateScore()
    {
        scoreText.text = source.Score.ToString();
        if (source.Score - score > 1)
        {
            int gap = source.Score - score;
            if (scoreFontSize == null)
                scoreFontSize = ScoreChangeFeedback.GetFeedbackOfType<MMF_TMPFontSize>();
            if (scoreDilate == null)
                scoreDilate = ScoreChangeFeedback.GetFeedbackOfType<MMF_TMPDilate>();

            if (scoreFontSize != null)
                scoreFontSize.RemapOne = gap * 2;
            if (scoreDilate != null)
                scoreDilate.RemapOne = gap / 2;
            ScoreChangeFeedback.Initialization(true);
            ScoreChangeFeedback?.PlayFeedbacks();
        }

        score = source.Score;
    }
    #endregion
    #region SpeedLevel
    public void updateSpeedLevel()
    {
        int nowSpeedLevel = source.playerSpeed.getSpeedLevel();
        if (nowSpeedLevel == speedLevel)
            return;

        speedLevelText.text = String.Format("속도레벨 : {0} Lv", nowSpeedLevel);

        if (speedLevelTextColor == null)
            speedLevelTextColor = SpeedChangeFeedback.GetFeedbackOfType<MMF_TMPColor>();
        if (speedLevelTextColor != null)
        {
            if (speedLevel > nowSpeedLevel)
                speedLevelTextColor.DestinationColor = new Color(255, 0, 0);
            else
                speedLevelTextColor.DestinationColor = new Color(0, 255, 0);
        }
        SpeedChangeFeedback.PlayFeedbacks();
        speedLevel = nowSpeedLevel;
    }
    #endregion
    #region Attack
    public void updateAttack()
    {
        attackText.text = source.Attack.ToString();
        attackChangeFeedback?.PlayFeedbacks();
    }

    public void updateRemainBullet()
    {
        BulletRemain.text = String.Format("X {0}", source.RemainBullet);
        BulletRemainFeedback?.PlayFeedbacks();
    }
    public void playAttackUIFeedback()
    {
        updateRemainBullet();
        if (attackUIImageFill == null)
            attackUIImageFill = attackUIFeedback.GetFeedbackOfType<MMF_ImageFill>();
        if (attackUIImageFill != null)
        {
            attackUIImageFill.Duration = source.attackInterval - 0.2f;
            attackUIImageFill.ComputeTotalDuration();
        }
        attackUIFeedback?.PlayFeedbacks();
    }
    #endregion
}
