using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelect : MonoBehaviour
{
    [System.Serializable]
    public class ClassEntry { public string PlayerClass; public Sprite[] Skills; }

    [System.Serializable]
    public class SkillText { public string PlayerClass; public Text[] Info; }

    public ClassEntry[] _ClassSkill;

    public SkillText[] _SkillText;

    // 0: Class Image, 1: Current Skill, 2: Change Skill, 3: SkillInfo Image
    public Image[] CurrentSkillImage;

    public Text SkillInfo;

    

    private int RandomClass = -1;

    private int RandomSkill = -1;








    // Use this for initialization
    void Start()
    {
        // 0: Agent, 1: Scientist, 2: Priest
        RandomClass = Random.Range(0, 3);

        do RandomSkill = Random.Range(0, 6);

        while (RandomSkill == SoundManager.instance.SkillNum[RandomClass]);

        CurrentSkillImage[0].sprite = _ClassSkill[RandomClass].Skills[6];

        CurrentSkillImage[1].sprite = _ClassSkill[RandomClass].Skills[SoundManager.instance.SkillNum[RandomClass]];

        CurrentSkillImage[2].sprite = _ClassSkill[RandomClass].Skills[RandomSkill];

        CurrentSkillImage[3].sprite = _ClassSkill[RandomClass].Skills[RandomSkill];

        SkillInfo.text = _SkillText[RandomClass].Info[RandomSkill].text;

    }

    public void OnButtonClicked(bool Active)
    {
        if (Active) SoundManager.instance.SkillNum[RandomClass] = RandomSkill;

        GameManager.instance.ReturnToMap();
    }

    public void OnSkillButtonClicked(bool Active)
    {
        if (Active)
        {
            CurrentSkillImage[3].sprite = _ClassSkill[RandomClass].Skills[RandomSkill];

            SkillInfo.text = _SkillText[RandomClass].Info[RandomSkill].text;
        }
        else
        {
            CurrentSkillImage[3].sprite = _ClassSkill[RandomClass].Skills[SoundManager.instance.SkillNum[RandomClass]];

            SkillInfo.text = _SkillText[RandomClass].Info[SoundManager.instance.SkillNum[RandomClass]].text;
        }
    }
}