using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*******************************************
*                                          * 
*             18.07.21 수정                *
*        Class SkillBlockController        * 
*     스킬 블록에 관한 작업들을 구현       *
*                                          *
*******************************************/


public class SkillBlockController : MonoBehaviour
{
    // public variable
    public int MaxBlockNum = 8;                                  // 최대 블록 개수
    public int MaxChainNum = 3;                                  // 최대 블록 체인 개수

    public float Timer = 1.0f;                                   // 스킬 블록 생성 타이머, 1초로 설정
    public SkillBlock[] SkillList;                               // 스킬 01,02,03 배열                 
    public GameObject skillBlocks;                               // 생성된 스킬블록들을 담을 오브젝트
    public static List<SkillBlock> skillBlockArray;                     // 스킬블록들이 담길 리스트


    // private variable
    private readonly float InitSkillBlockPosition = 15.0f;                // 블록 생성 위치
    





    public void InitBlockArray()
    {
        // 임시로 Title에서 스킬 선택한 값을 SoundManager로 받아서 왔음
        for (int i =0; i< SkillList.Length; i++) SkillList[i] = GameManager.instance._skillEntry[i].skill[SoundManager.instance.SkillNum[i]];

        
        // 스킬 블록 배열 초기화
      //  if (GameManager.instance.StageLevel != 1
       //     && skillBlockArray != null) for (int i = 0; i < skillBlockArray.Count; i++) Destroy((skillBlockArray[i]).gameObject);
       

        // 배열 초기화
        skillBlockArray = new List <SkillBlock>(MaxBlockNum);
    }

    // 블록 생성을 위한 열거자
    public IEnumerator GeneratingBlock()
    {
        // 게임 플레이 상태일 때 계속 반복됨
        while (GameManager.instance.gameState==GameManager.GameState.GamePlay)
        {
            // 만약 대기 중이 아니고 블록 배열 크기가 최대가 아닐 경우
            if (!GameManager.instance.GameWait
                && skillBlockArray.Count < MaxBlockNum)
            {
                // 블록 생성
                SkillBlockGen();

                // 블록 체인 확인
                GameManager.instance.IsChain();

                // Timer 초 동안 쉼
                yield return new WaitForSeconds(Timer);
            }
            // 게임 대기 상태거나 블록 배열 크기가 최대일 경우 계속 기다린다.
            else yield return null;
        }
        // 게임 플레이 상태가 아닐 경우 코루틴에서 빠져나옴
        yield break;
    }

    // 스킬블록을 만드는 함수
    private void SkillBlockGen()
    {
        // skillBlock 인스턴스화
        SkillBlock skillBlock = Instantiate(
            SkillBlockKind(),
            SkillBlockPosition(),
            Quaternion.identity
            );

        // skillBlock을 skillBlocks의 부모로 설정
        skillBlock.transform.parent = skillBlocks.transform;

        // skillBlock을 skillBlockArray에 추가
        skillBlockArray.Add(skillBlock);
    }

    // 어떤 스킬 블록을 사용할 지 무작위로 정하는 함수
    SkillBlock SkillBlockKind()
    {
        // skillBlock 변수 초기화
        SkillBlock skillBlock = null;

        // SkillList 내의 스킬 블록 종류 무작위로 고르기
        int index = Random.Range(0, SkillList.Length);

        // index값을 토대로 스킬블록에 할당
        skillBlock = SkillList[index];

        return skillBlock;
    }

    // 스킬 블록 초기 위치 벡터
    Vector3 SkillBlockPosition()
    {
        return new Vector3(InitSkillBlockPosition, 0, 0);
    }
}