using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextManager))]
[RequireComponent(typeof(GameDataManager))]
[RequireComponent(typeof(Animation))]
public class PanelSettings : MonoBehaviour
{
    #region Panel Settings
    [Header("モブ兵士パネル")]
    [SerializeField] Button jobPanelButton;
    [SerializeField] GameObject jobPanel;
    [SerializeField] Button jobPanelCloseButton;

    [Header("プレイヤースキルパネル")]
    [SerializeField] Button skillPanelButton;
    [SerializeField] GameObject skillPanel;
    [SerializeField] Button skillPanelCloseButton;

    [Header("アニメーション")]
    [SerializeField] AnimationClip jobPanelAnimationOpen;
    [SerializeField] AnimationClip jobPanelAnimationClose;
    [SerializeField] AnimationClip skillPanelAnimationOpen;
    [SerializeField] AnimationClip skillPanelAnimationClose;
    [SerializeField] bool transformation;
    [SerializeField] bool jobPanelOpen;
    [SerializeField] bool skillPanelOpen;

    Animator jobAnimator;
    Animator skillAnimator;


    #endregion

    [System.Obsolete]

    void Start()
    {
        ButtonInit();
        AnimationInit();
        skillPanel.SetActive(false);
        jobPanel.SetActive(false);
    }

    /// <summary>
    /// ボタンの初期化
    /// </summary>
    void ButtonInit()
    {
        jobPanelButton.onClick.AddListener(() => JobPanelSet());
        skillPanelButton.onClick.AddListener(() => SkillPanelSet());

        jobPanelCloseButton.onClick.AddListener(() => JobPanelSet());
        skillPanelCloseButton.onClick.AddListener(() => SkillPanelSet());
    }

    /// <summary>
    /// アニメーション関係の初期化
    /// </summary>
    void AnimationInit()
    {
        // jobPanel
        jobAnimator = jobPanel.GetComponent<Animator>();

        // skillPanel
        skillAnimator = skillPanel.GetComponent<Animator>();
    }

    /// <summary>
    /// jobPanel状態変化
    /// </summary>
    void JobPanelSet()
    {
        if(transformation)
            return;

        if (!jobPanel.activeSelf && !skillPanelOpen)
        {
            // これから開くとき
            // OnOff切り替え
            jobPanelOpen = true;
            jobPanel.SetActive(true);
            GetComponent<TextManager>().ResourcesTextUpdate(GetComponent<GameDataManager>().playerData.resources);
            GetComponent<TextManager>().SkillLevelTextUpdate(0, GetComponent<GameDataManager>().playerData);

            jobAnimator.Play(jobPanelAnimationOpen.name);
            StartCoroutine(EnablePanelAfter(jobPanelAnimationOpen.length));
        }
        else
        {
            JobPanelClose();
        }
    }

    void JobPanelClose()
    {
        if(skillPanelOpen)
            return;
        // これから閉じるとき
        jobAnimator.Play(jobPanelAnimationClose.name);
        // アニメーションが終了したらパネルを非表示にする
        StartCoroutine(DisablePanelAfterAnimation(jobPanel, jobPanelAnimationClose.length));
    }

    /// <summary>
    /// skillPanel状態変化
    /// </summary>
    void SkillPanelSet()
    {
        if(transformation)
            return;

        if (!skillPanel.activeSelf && !jobPanelOpen)
        {
            // これから開くとき
            skillPanelOpen = true;
            skillPanel.SetActive(true);
            GetComponent<TextManager>().ResourcesTextUpdate(GetComponent<GameDataManager>().playerData.resources);
            GetComponent<TextManager>().JobLevelTextUpdate(1, GetComponent<GameDataManager>().playerData);

            skillAnimator.Play(skillPanelAnimationOpen.name);
            StartCoroutine(EnablePanelAfter(skillPanelAnimationOpen.length));
        }
        else
        {
            SkillPanelClose();
        }
    }

    void SkillPanelClose()
    {
        if(jobPanelOpen)
            return;

        // これから閉じるとき
        skillAnimator.Play(skillPanelAnimationClose.name);
        // アニメーションが終了したらパネルを非表示にする
        StartCoroutine(DisablePanelAfterAnimation(skillPanel, skillPanelAnimationClose.length));
    }



    /// <summary>
    /// パネルをアニメーションの後に非表示にするコルーチン
    /// </summary>
    /// <param name="panel">非表示にするパネルのゲームオブジェクト</param>
    /// <param name="delay">アニメーションの待機時間</param>
    private IEnumerator DisablePanelAfterAnimation(GameObject panel, float delay)
    {
        transformation = true;
        yield return new WaitForSeconds(delay);
        panel.SetActive(false);
        transformation = false;

        if(panel == jobPanel)
            jobPanelOpen = false;
        else if(panel == skillPanel)
            skillPanelOpen = false;
    }

    /// <summary>
    /// パネルをアニメーションの後に表示可能にするコルーチン
    /// </summary>
    /// <param name="delay">アニメーションの待機時間</param>
    private IEnumerator EnablePanelAfter(float delay)
    {
        transformation = true;
        yield return new WaitForSeconds(delay);
        transformation = false;
    }
}
