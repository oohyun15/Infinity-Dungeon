using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*******************************************
*                                          * 
*             18.07.22 수정                *
*           Class SkillBlock               * 
*    스킬 블록의 타입을 구현한 클래스      *
*                                          *
*******************************************/


public class SkillBlock : MonoBehaviour
{
    // public variable
    [Header("Input")]
    public float BlockEdge;                                      // 블록 모서리 크기
    public float BlockGap = 0.2f;                                // 블록 사이 크기
    public float SmoothTime = 0.025f;                           // 블록 속도

    [Space]

    [Header("Block Information")]
    public bool blockActive;                                     // 블록이 활성화 상태인지 알려주는 flag
    public int index;                                            // 블록 배열의 인덱스
    public float GenTime;                                        // 블록 생성 시간
    public float DelayTime = 0.1f;                               // 블록이 생성되자마자 체인이 되는걸 방지하기 위한 딜레이
    public int ChainNum;                                         // 체인 개수
    public List<SkillBlock> ChainBlock;                                 // 체인 블록 배열


    // private variable
    private float localScale;                                    // 블록 크기 조정을 위한 스케일 값
    private float BlockVelocity = 0.0f;                          // SmoothDamp Parameter
    private const int MaxChainNum = 3;
    private SpriteRenderer spr;
    private Player TargetPlayer;




    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();

        WhoseSkill();

        // 스킬 블록 배열에서 인덱스 찾기
        index = SkillBlockController.skillBlockArray.IndexOf(this);

        // 체인 블록 배열에 자기 자신 넣기
        ChainBlock = new List<SkillBlock>(MaxChainNum) { this };

        // 생성된 시간 기록
        GenTime = Time.fixedTime;

        // 블록 모서리 길이 만큼 스케일 값 키운 후 
        localScale = BlockEdge / GetComponent<Renderer>().bounds.size.x;

        // 스케일 적용
        transform.localScale = localScale * Vector3.one;
    }

    private void Update()
    {
        // 블록이 활성화 상태인지 확인
        blockActive = IsBlockActive();

        // 후에 에너미가 플레이어를 처치했을 때 알파값을 변경하도록 하자. 지금 메모리 너무 먹음
        if (TargetPlayer.Hp <=0)
        {
            spr.color = new Color(1f, 1f, 1f, 0.3f);
        }

        // 현재 이 블록이 스킬 블록 리스트에서 몇번째인지 알아내는 변수
        index = SkillBlockController.skillBlockArray.IndexOf(this);

        // 스킬 블록을 지정된 자리로 보내는 코드
        if (gameObject.transform.localPosition.x - index * (BlockGap + BlockEdge) > Mathf.Epsilon)
            gameObject.transform.localPosition
            = new Vector3(Mathf.SmoothDamp(gameObject.transform.localPosition.x, index * (BlockGap + BlockEdge), ref BlockVelocity, SmoothTime),
            0,
            0);
    }

    private void OnMouseDown()
    {
        // 클릭 시 블록이 활성화 상태이면 스킬 활성화
        if (IsBlockActive()) SkillActive();
    }

    public bool IsBlockActive()
    {
        if (GameManager.instance.IsActive()
            && GenTime + DelayTime < Time.fixedTime) return true;

        return false;
    }

    private void WhoseSkill()
    {
        if (gameObject.CompareTag("Agent"))
           TargetPlayer = GameManager.instance.playerList[0];
        else if (gameObject.CompareTag("Priest"))
            TargetPlayer = GameManager.instance.playerList[2];
        else if (gameObject.CompareTag("Scientist"))
            TargetPlayer = GameManager.instance.playerList[1];
        return;
    }


    private void SkillActive()
    {
        // 블록 체인 확인
        GameManager.instance.IsChain();

        // 남은 스킬 개수 차감
        GameManager.instance.SkillLeftCount--;

        // 블록 제거
        if (ChainBlock != null)
        {
            // 체인 개수 확인
            ChainNum = ChainBlock.Count;

            // 블록 제거
            for (int i = 0; i < ChainBlock.Count; i++)
            {
                SkillBlockController.skillBlockArray.Remove(ChainBlock[i]);

                Destroy(ChainBlock[i].gameObject);
            }

            if (TargetPlayer.Hp >0) SendMessage("Activation", ChainNum, SendMessageOptions.DontRequireReceiver);

            // 찌꺼기 리스트 제거 메모리 차지하기 때문
            ChainBlock.Clear();

            // 스킬 사운드
            SoundManager.instance.Touch.Play();
        }

        GameManager.instance.SkillLeftUpdate();

        // 게임 오버인가 확인
        GameManager.instance.IsGameOver();
    }

    public static implicit operator SkillBlock(GameManager.SkillEntry v)
    {
        throw new NotImplementedException();
    }
}
