﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;




/*******************************************
*                                          * 
*             18.07.22 수정                *
*           Class GameManager              * 
* 이 클래스를 통해 게임의 루틴이 정해진다. *
*                                          *
*******************************************/


public class GameManager : MonoBehaviour
{
    // public variable



    public static GameManager instance = null;                   // 싱글톤 구현을 위한 변수
    [System.Serializable]
    public class SkillEntry { public string PlayerClass; public SkillBlock[] skill; }
    public enum GameState { GameReady, GamePlay, GameOver, GameClear }    // 게임 상태 enum

    [Header("UI")]
    public GameObject GameImage;                                 // 게임 이미지 
    public Text GameText;                                        // 게임 텍스트 
    public Text SkillLeft;                                       // 남은 스킬 개수 텍스트
    public Text Score;                                           // 스코어 텍스트

    [Space]

    [Header("Game State")]
    public GameState gameState = GameState.GameOver;             // 게임 상태
    public bool GameWait = true;                                 // 게임 중 기다려야 할 때 쓸 flag

    [Space]

    [Header("Game Ready")]
    private int InitSkillCount = 50;                              // 게임 시작 시 사용 가능한 스킬 개수 정하기, 20개로 설정
    public float GameStartDelay = 2.0f;                          // 게임 시작 대기 시간
    public SkillEntry[] _skillEntry;

    [Space]

    [Header("Game Play")]
    public int StageLevel;                                       // 스테이지 레벨
    [HideInInspector] public int score;                                            // 현재 내 점수
    public int StageClearEnemyCount;                                  // 이번 스테이지 필요 점수
    public int SkillLeftCount;                                   // 남은 스킬 사용 개수
    public int KillCount;


    [Space]

    [Header("Game Clear")]
    public int AddCount;                                           // 스테이지 클리어 시 추가 블록 개수, Rand(1, 10)
    public int NextPoint = 20;                                    // 스테이지마다 NextPoint만큼 NextLevelScore가 올라감
    public GameObject SkillSellect;

    [Space]

    [Header("Player")]
    public Player[] playerList;
    public int playerTargetIndex = 0;

    [Space]

    [Header("Effects")]
    public GameObject EffectHolder;
    public GameObject Angel;
    public GameObject Devil;



    //private variable
    private SkillBlockController skillBlockController;           // 스킬 블록 생성 클래스
    private EnemySpawner enemySpawner;
    private PlayerController playerController;





    private void Awake()
    {
        // 싱글톤 구현, 인스턴스가 이미 있는지 확인, 없으면 인스턴스를 this로 할당
        if (instance == null) instance = this;

        // 인스턴스가 this로 할당되있다면  게임오브젝트 삭제
        else if (instance != this) Destroy(gameObject);

        //    DontDestroyOnLoad(gameObject);

        // SkillBlockController를 GameManager에서 관리하기 위해 컴포넌트 가져옴
        skillBlockController = GetComponent<SkillBlockController>();

        // EnemyController를 GameManager에서 관리하기 위해 컴포넌트를 가져옴
        enemySpawner = GetComponent<EnemySpawner>();

        playerController = GetComponent<PlayerController>();

        // 게임 초기화
        InitGame();
    }

    private void Update()
    {
        // 게임 대기 중이면 Update 함수 빠져나옴
        if (GameWait) return;

        // 게임 상태에 따른 switch 구문
        switch (gameState)
        {
            // 게임 준비
            case GameState.GameReady:

                // 게임 초기화
                InitGame();

                break;

            /* 사용 x
            // 게임 플레이
            case GameState.GamePlay:    

                break;
            */

            // 게임 오버
            case GameState.GameOver:

                // 게임 오버 함수 호출
                GameOver();

                break;

            // 스테이지 클리어
            case GameState.GameClear:

                // 마지막 스테이지 클리어 시
                if (StageLevel == MapGenerator.instance.height) GameClear();

                // 다음 레벨 호출
                else NextLevel();

                break;
        }
    }

    // 게임 초기화 함수
    public void InitGame()
    {
        // 기존에 실행되던 코루틴 모두 정지
        StopAllCoroutines();

        // 게임 대기
        GameWait = true;

        // 게임 상태를 게임 준비로 바꿈
        gameState = GameState.GameReady;

        // 스테이지 레벨을 현재 맵 Depth로 설정(StageLevel = 0, 1, 2, ...  Depth = 1, 2, 3, ...)
        StageLevel = SoundManager.instance.Depth;

        // 블록 개수 제한을 두기 위한 개수 초기화
        if (StageLevel.Equals(1)) SkillLeftCount = InitSkillCount;

        else SkillLeftCount = SoundManager.instance.SkillLeft;

        // 플레이어 타겟 인덱스 초기화
        playerTargetIndex = 0;

        // SkillBlockController에서 블록 배열 초기화
        skillBlockController.InitBlockArray();

        // EnemySpawner에서 에너미 배열 초기화
        enemySpawner.InitEnemyArray();

        //
        playerController.InitPlayerPosition();

        // 스코어 초기화
        score = SoundManager.instance.Score;

        // 스테이지 클리어 점수 설정
        StageClearEnemyCount = ScoreFomuler(NextPoint);

        // 게임 텍스트 업데이트, Stage X
        GameText.text = "STAGE " + StageLevel;

        // 남은 블록 개수 업데이트
        SkillLeftUpdate();

        // 스코어 업데이트
        ScoreUpdate();

        // 게임 이미지 활성화
        GameImage.SetActive(true);

        // GameReady 상태에서 플레이어들은 움직이게 하기 위해서
        StartCoroutine(playerController.SpawnPlayer());

        // GameStartDelay 이후 GamePlay 호출
        Invoke("GamePlay", GameStartDelay);
    }

    // GameImage를 비활성화 후 게임 시작 상태로 변경하는 함수
    private void GamePlay()
    {
        // GameImage 비활성화
        GameImage.SetActive(false);

        // 게임 상태를 게임 플레이로 변경
        gameState = GameState.GamePlay;

        // 게임 진행을 위해 false로 바꿈
        GameWait = false;

        // 블록 생성을 위한 코루틴 시작
        StartCoroutine(skillBlockController.GeneratingBlock());

        // 에너미 생성을 위한 코루틴 시작
        StartCoroutine(enemySpawner.SpawnEnemy());
    }

    // 게임 오버 함수
    private void GameOver()
    {
        // 게임 텍스트 업데이트
        GameText.text = "GAME OVER.";

        // 남은 블록 개수 업데이트
        SkillLeftUpdate();

        // 스코어 업데이트
        ScoreUpdate();

        // 게임 이미지 활성화
        GameImage.SetActive(true);

        // 스테이지 레벨 1로 초기화
        //  StageLevel = 1;

        // 스킬리스트 초기화
        SoundManager.instance.SkillNum = new int[6];

        //
        SoundManager.instance.Depth = 0;

        SoundManager.instance.SkillLeft = 0;

        SoundManager.instance.Score = 0;

        //
        // MapGenerator.instance.IsGenerate = false;

        //
        Destroy(MapGenerator.instance.gameObject);

        // GameStartDelay 이후 InitGame 호출
        Invoke("ReturnToTitle", GameStartDelay);

        // 게임이 초기화 될 때까지 true 상태로 유지
        GameWait = true;

        StopAllCoroutines();
    }

    // 다음 레벨로 넘어가는 함수
    private void NextLevel()
    {
        // 남은 블록 개수에 더해줄 블록 개수
        AddCount = Random.Range(1, 11) + 5 * StageLevel;

        // 게임 텍스트 업데이트
        GameText.text = "STAGE " + StageLevel + " CLEAR!";

        // 남은 블록 개수 업데이트
        SkillLeftUpdate(AddCount);

        // 스코어 업데이트
        ScoreUpdate();

        // 게임 이미지 활성화
        GameImage.SetActive(true);

        // 스테이지 레벨 증가
        //  StageLevel++;

        // 남은 블록 개수에 AddCount 추가
        SkillLeftCount += AddCount;

        // 사운드 매니저에 스킬 남은 개수 저장
        SoundManager.instance.SkillLeft = SkillLeftCount;

        // 사운드 매니저에 스코어 저장
        SoundManager.instance.Score = score;

        // GameStartDelay 이후 InitGame 호출
        //Invoke("ReturnToMap", GameStartDelay);

        // 게임이 초기화 될 때까지 true 상태로 유지
        GameWait = true;

        StopAllCoroutines();

        // 스킬 선택 패널 활성화
        Invoke("SkillSellectActive", GameStartDelay);
    }

    private void GameClear()
    {
        // 게임 텍스트 업데이트
        GameText.text = "ALL STAGE CLEAR!";

        // 남은 블록 개수 업데이트
        SkillLeftUpdate();

        // 스코어 업데이트
        ScoreUpdate();

        // 게임 이미지 활성화
        GameImage.SetActive(true);

        // 스킬리스트 초기화
        SoundManager.instance.SkillNum = new int[6];

        SoundManager.instance.Depth = 0;

        SoundManager.instance.SkillLeft = 0;

        SoundManager.instance.Score = 0;

        // 기존에 있던 스테이지 제거
        Destroy(MapGenerator.instance.gameObject);

        // GameStartDelay 이후 InitGame 호출
        Invoke("ReturnToTitle", GameStartDelay);

        // 게임이 초기화 될 때까지 true 상태로 유지
        GameWait = true;

        StopAllCoroutines();
    }

    // 점수 공식 함수
    private int ScoreFomuler(int nextPoint)
    {
        return 30 + StageLevel * nextPoint;
        //  return nextPoint * (StageLevel + 3);
    }

    // 다음 스테이지로 넘어갈 수 있는지 확인하는 함수
    public void IsNextLevel()
    {
        if (KillCount >= StageClearEnemyCount) gameState = GameState.GameClear;
    }

    // 게임 오버인지 확인하는 함수
    public void IsGameOver()
    {
        if (SkillLeftCount <= 0) gameState = GameState.GameOver;
    }

    // 블록 체인 확인하는 함수
    public void IsChain()
    {
        // 체인 구현 시 이전 스킬블록을 저장하기 위한 임시 변수
        SkillBlock temp = null;

        for (int i = 0; i < SkillBlockController.skillBlockArray.Count; i++)
        {
            // 현재 인덱스의 스킬 블록
            SkillBlock sb = SkillBlockController.skillBlockArray[i];

            // temp는 sb의 이전 스킬 블록
            if (temp != null)
            {
                // temp와 sb의 스킬 종류가 같고, temp 체인 배열에 sb가 포함이 안되있고, 체인 배열 크기가 3보다 작으면
                if (temp.tag.Equals(sb.tag)
                    && sb.blockActive
                    && !temp.ChainBlock.Contains(sb)
                    && temp.ChainBlock.Count < skillBlockController.MaxChainNum)
                {
                    // sb 체인 블록의 크기가 2 이상일 경우 중복이 되므로 제거해주어야 함
                    if (sb.ChainBlock.Count != 1) sb.ChainBlock.Remove(sb);

                    // 크기가 1이면 sb의 체인블록을 제거해서 메모리 확보
                    else sb.ChainBlock.Clear();

                    // temp 체인 블록에 sb 추가
                    temp.ChainBlock.Add(sb);

                    // 이후 sb 체인 블록이 temp 체인 블록과 같게 함
                    sb.ChainBlock = temp.ChainBlock;
                }
                // 다음 스킬 블록을 불러오기 전 temp에 sb를 할당
                temp = sb;
            }
            // 첫 스킬 블록일 때 이므로 temp에 sb를 할당
            else temp = sb;
        }
    }

    //
    public bool IsActive()
    {
        if (gameState.Equals(GameState.GamePlay)
          && !GameWait) return true;

        return false;
    }

    //
    public void ScoreUpdate()
    {
        // 스코어 업데이트
        Score.text = "SCORE : " + score;
    }

    //
    public void SkillLeftUpdate()
    {
        // 남은 블록 개수 업데이트
        SkillLeft.text = "SKILL LEFT : " + SkillLeftCount;
    }

    //
    public void SkillLeftUpdate(int Num)
    {
        // 남은 블록 개수 업데이트
        SkillLeft.text = "+" + Num + " SKILL LEFT : " + SkillLeftCount;
    }

    public void SkillSellectActive()
    {
        // 스킬 선택 패널 활성화
        SkillSellect.SetActive(true);
    }

    public void ReturnToTitle()
    {
        SoundManager.instance.BGM[SoundManager.instance.NowPlaying].Stop();

        //        SoundManager.instance.NowPlaying = 0;

        SoundManager.instance.main();

        SceneManager.LoadScene("Title");
    }

    //
    public void ReturnToMap()
    {
        MapGenerator.instance.gameObject.SetActive(true);
        SceneManager.LoadScene("Map");
    }
}