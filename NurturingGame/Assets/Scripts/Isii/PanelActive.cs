using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextManager))]
[RequireComponent(typeof(GameDataManager))]
[RequireComponent(typeof(Animation))]
public class PanelSettings : MonoBehaviour
{
    #region Panel Settings
    [Header("パネル設定")]
    [SerializeField] float alphaSet;
    [SerializeField] float alphaSpeed;

    [Header("モブ兵士パネル")]
    [SerializeField] Button jobPanelButton;
    [SerializeField] GameObject jobPanels;
    [SerializeField] GameObject jobPanel;
    GameObject jobBG;
    [SerializeField] Button jobPanelCloseButton;

    [Header("プレイヤースキルパネル")]
    [SerializeField] Button skillPanelButton;
    [SerializeField] GameObject skillPanels;
    [SerializeField] GameObject skillPanel;
    GameObject skillBG;
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
        skillPanels.SetActive(true);
        jobPanels.SetActive(true);

        ObjectInit();
        ButtonInit();
        AnimationInit();
        skillPanels.SetActive(false);
        jobPanels.SetActive(false);
    }

    /// <summary>
    /// Object初期設定
    /// </summary>
    void ObjectInit()
    {
        // BG
        jobBG = jobPanels.transform.GetChild(0).gameObject;
        skillBG = skillPanels.transform.GetChild(0).gameObject;

        // Panel
        // jobPanel = jobPanels.transform.GetChild(1).gameObject;
        // skillPanel = skillPanels.transform.GetChild(1).gameObject;
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

        // Close
        jobBG.GetComponent<Button>().onClick.AddListener(() => JobPanelSet());
        skillBG.GetComponent<Button>().onClick.AddListener(() => SkillPanelSet());
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
        if (!jobPanels.activeSelf && !skillPanelOpen)
        {
            // これから開くとき
            // OnOff切り替え
            jobPanelOpen = true;
            jobPanels.SetActive(true);
            StartCoroutine(EnableBGShadowUpdate(jobBG));
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
        StartCoroutine(DisableBGShadowUpdate(jobBG));
        jobAnimator.Play(jobPanelAnimationClose.name);
        // アニメーションが終了したらパネルを非表示にする
        StartCoroutine(DisablePanelAfterAnimation(jobPanels, jobPanelAnimationClose.length));
    }

    /// <summary>
    /// skillPanel状態変化
    /// </summary>
    void SkillPanelSet()
    {
        if(transformation)
            return;

        if (!skillPanels.activeSelf && !jobPanelOpen)
        {
            // これから開くとき
            skillPanelOpen = true;
            skillPanels.SetActive(true);
            StartCoroutine(EnableBGShadowUpdate(skillBG));
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
        StartCoroutine(DisableBGShadowUpdate(skillBG));
        skillAnimator.Play(skillPanelAnimationClose.name);
        // アニメーションが終了したらパネルを非表示にする
        StartCoroutine(DisablePanelAfterAnimation(skillPanels, skillPanelAnimationClose.length));
    }

    IEnumerator EnableBGShadowUpdate(GameObject bg)
    {
        Image image = bg.GetComponent<Image>();
        image.color = new Vector4(0, 0, 0, 0);
        float nowAlpha = 0;

        for(;;)
        {
            nowAlpha += alphaSpeed / 255f;
            image.color = new Vector4(0, 0, 0, nowAlpha);

            if(nowAlpha >= 140f / 255f)
                break;

            yield return new WaitForSeconds(1f/60f);
        }

        image.color = new Vector4(0, 0, 0, 150f / 255f);
    }

    IEnumerator DisableBGShadowUpdate(GameObject bg)
    {
        Image image = bg.GetComponent<Image>();
        image.color = new Vector4(0, 0, 0, 150f / 255f);
        float nowAlpha = 150f / 255f;

        for(;;)
        {
            nowAlpha -= alphaSpeed / 255f;
            image.color = new Vector4(0, 0, 0, nowAlpha);

            if(nowAlpha <= 20f / 255f)
                break;

            yield return new WaitForSeconds(1f/60f);
        }

        image.color = new Vector4(0, 0, 0, 0);
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

        if(panel == jobPanels)
            jobPanelOpen = false;
        else if(panel == skillPanels)
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
