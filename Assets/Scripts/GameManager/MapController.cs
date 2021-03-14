using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MapController : MonoBehaviour
{

    public GameObject[] Button; // 0: SkillList, 1: Information (정확히는 버튼은 아님)
    public Sprite[] AgentSkill;
    public Sprite[] ScientistSkill;
    public Sprite[] PriestSkill;
    public Image[] CurrentSkillImage;

    // SkillList variable
    public static bool isOpen = false; // Stage 선택 시 스킬 창이 열려 있으면 Stage로 넘어가게 하지 않기 위해 static 변수로 설정
    public bool ButtonActive = true;
    public bool InfoButtonActive = false;
    private float velocity;

    private void Start()
    {
        CurrentSkillImage[0].sprite = AgentSkill[SoundManager.instance.SkillNum[0]];

        CurrentSkillImage[1].sprite = ScientistSkill[SoundManager.instance.SkillNum[1]];

        CurrentSkillImage[2].sprite = PriestSkill[SoundManager.instance.SkillNum[2]];
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
        SoundManager.instance.SkillNum[0] = num;

        CurrentSkillImage[0].sprite = AgentSkill[num];
    }
    public void OnScientistSkillButtonClicked(int num)
    {
        SoundManager.instance.SkillNum[1] = num;

        CurrentSkillImage[1].sprite = ScientistSkill[num];
    }
    public void OnPriestSkillButtonClicked(int num)
    {
        SoundManager.instance.SkillNum[2] = num;

        CurrentSkillImage[2].sprite = PriestSkill[num];
    }

    IEnumerator Open()
    {
        ButtonActive = false;
        isOpen = true;
        float TargetPositionX = Button[0].transform.localPosition.x - 1100f;
        while (Button[0].transform.localPosition.x - TargetPositionX > Mathf.Epsilon)
        {
            Vector3 vec = new Vector3(
                Mathf.SmoothDamp(Button[0].transform.localPosition.x, TargetPositionX - 0.5f, ref velocity, 0.25f),
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
        float TargetPositionX = Button[0].transform.localPosition.x + 1100f;
        while (Button[0].transform.localPosition.x - TargetPositionX < Mathf.Epsilon)
        {
            Vector3 vec = new Vector3(
                Mathf.SmoothDamp(Button[0].transform.localPosition.x, TargetPositionX + 0.5f, ref velocity, 0.25f),
                0,
                0);

            Button[0].transform.localPosition = vec;
            yield return null;
        }
        ButtonActive = true;
        yield break;
    }

    public void OnInfoButtonClicked()
    {
        if (!InfoButtonActive) Button[1].SetActive(true);

        else Button[1].SetActive(false);

        InfoButtonActive = !InfoButtonActive;

        SoundManager.instance.Touch.Play();
    }

}
