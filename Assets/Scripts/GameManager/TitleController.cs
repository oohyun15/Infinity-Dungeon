using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;


public class TitleController : MonoBehaviour {

    public GameObject[] Button; // 0: SkillList
    public Sprite[] AgentSkill;
    public Sprite[] ScientistSkill;
    public Sprite[] PriestSkill;
    public Image[] CurrentSkillImage;


    public Image FadeImage;
    private Image fadeOut;

    // SkillList variable
    public bool isOpen = false; 
    public bool ButtonActive = true;
    private float velocity;

    // GameStart variable
    public float time = 0f;
    public float PlayTime = 2.0f;
    float inversePlayTime;


    private void Awake()
    {
        fadeOut = FadeImage.GetComponent<Image>();
        inversePlayTime = 1.0f / PlayTime;
    }

    public void OnStartButtonClicked()
    {
        //  GameManager.instance.InitGame();

        //  SceneManager.LoadScene("Main");

        if (ButtonActive)
        {
            if(isOpen) StartCoroutine(Close());

            StartCoroutine(FadeOut());
        }
    }

    public void OnSkilltreeButtonClicked()
    {
        if (!isOpen
            && ButtonActive) StartCoroutine(Open());
        else if (isOpen
            && ButtonActive) StartCoroutine(Close());
    }

    public void OnAgentSkillButtonClicked(int num)
    {
        CurrentSkillImage[0].sprite = AgentSkill[num];
        SoundManager.instance.SkillNum[0] = num;
    }
    public void OnScientistSkillButtonClicked(int num)
    {
        CurrentSkillImage[1].sprite = ScientistSkill[num];
        SoundManager.instance.SkillNum[1] = num;
    }
    public void OnPriestSkillButtonClicked(int num)
    {
        CurrentSkillImage[2].sprite = PriestSkill[num];
        SoundManager.instance.SkillNum[2] = num;
    }

    IEnumerator Open()
    {
        ButtonActive = false;
        isOpen = true;
        float TargetPositionX = Button[0].transform.localPosition.x - 900f;
        while (Button[0].transform.localPosition.x - TargetPositionX > Mathf.Epsilon)
        {
            Vector3 vec = new Vector3(
                Mathf.SmoothDamp(Button[0].transform.localPosition.x, TargetPositionX-0.5f, ref velocity, 0.25f),
                0,
                0);

            Button[0].transform.localPosition = vec;
            yield return null;
        }
        ButtonActive = true;
        yield break;
    }
    IEnumerator Close()
    {
        ButtonActive = false;
        isOpen = false;
        float TargetPositionX = Button[0].transform.localPosition.x + 900f;
        while (Button[0].transform.localPosition.x - TargetPositionX < Mathf.Epsilon)
        {
            Vector3 vec = new Vector3(
                Mathf.SmoothDamp(Button[0].transform.localPosition.x, TargetPositionX+0.5f, ref velocity, 0.25f),
                0,
                0);

            Button[0].transform.localPosition = vec;
            yield return null;
        }
        ButtonActive = true;
        yield break;
    }
    IEnumerator FadeOut()
    {
        ButtonActive = false;

        Color color = fadeOut.color;
        time = 0f;
        while (color.a < 1f)
        {
            color.a = Mathf.Lerp(0.0f, 1.0f, time);
            time += Time.deltaTime * inversePlayTime;
            fadeOut.color = color;

            yield return null;
        }
        ButtonActive = true;
        SceneManager.LoadScene("Map");
    }
}
