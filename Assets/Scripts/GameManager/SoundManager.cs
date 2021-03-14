using UnityEngine;




/*******************************************
*                                          * 
*             18.07.20 수정                *
*           Class SounManager              * 
*    게임 내 사운드를 관리하는 클래스      *
*                                          *
*******************************************/


public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;
    public AudioSource[] BGM;
    public int NowPlaying;        // 0: Title, 1: Copy, 2: Abandoned 3: Biology, 4: Spritism, 5: Final

    [Space]

    [Header("Default SFX")]
    public AudioSource Touch;
    public AudioSource Hit;

    [Space]

    [Header("Skill Sound")]
    public AudioSource GunFire;
    public AudioSource Fireball;
    public AudioSource GlassBroken;

    [Space]

    [Header("Public Variable")]
    
    public int[] SkillNum;
    public int Depth;
    public int SkillLeft;
    [HideInInspector] public int Score;
    [HideInInspector] public Vector3 CamPos;   // Map 씬에서 카메라 위치 저장


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            main();
        }

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void main()
    {

        // 0.5초 딜레이
        BGM[0].PlayDelayed(0.5f);

        NowPlaying = 0;

        SkillNum = new int[6];

        Depth = 0;

      //  Debug.Log(NowPlaying);
    }
}