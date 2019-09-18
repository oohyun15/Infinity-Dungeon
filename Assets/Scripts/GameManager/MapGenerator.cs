using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator instance = null;

   // public bool IsGenerate = false;         // 첫 생성 시 카메라 자동 이동에 쓸 변수

    public int width;
    public int height;
    public Stage[] Stages;
    public Stage[,] Holder;
    public Stage previousStage;

    private int pre_width;
    private int Max_Path;
    private int Min_Path;
    private int Total_Path;
    private int _x, _y; // _x: Path 1의 개수, _y: Path 2의 개수
    private int index; // height에 따른 스테이지 종류를 정하기 위한 변수


    // Use this for initialization
    private void Awake()
    {
        if (instance == null) instance = this;

        else if (instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        Holder = new Stage[height, width + 1];

        GeneratingMap(width, height);
    }

    public void GeneratingMap(int width, int height)
    {
        for (int j = 0; j < height; j++)
        {
            pre_width = width;

            width = this.width;

            if (j != height - 1) width = Random.Range(width - 1, width + 2);

            else width = 1;

            for (int i = 0; i < width; i++)
            {
                // switch를 통해 각 층수마다 나올 수 있는 스테이지 종류 배치
                switch (j)
                {
                    case 0:

                        // 0이 나와야함
                        index = 0;

                        break;

                    case 1:

                        // 0, 2가 나와야함
                        index = Random.Range(0, 2) * 2;

                        break;

                    case 2:

                        // 0, 1, 2, 3이 나와야함
                        index = Random.Range(0, 4);

                        break;

                    case 3:

                        // 4가 나와야함
                        index = 4;

                        break;

                    case 4:

                        // 4, 6이 나와야함
                        index = Random.Range(2, 4) * 2;

                        break;

                    case 5:

                        // 4, 5, 6, 7이 나와야함
                        index = Random.Range(4, 8);

                        break;

                    case 6:

                        // 8이 나와야함
                        index = 8;

                        break;
                }

                // 스테이지 인스턴스화
                Stage stage = Instantiate(
                    Stages[index],
                    InitPosition(i, j, width),
                    Quaternion.identity
                    );

                stage.transform.parent = gameObject.transform;

                Holder[j, i] = stage;

                stage.target = new Stage[2];

                stage.Depth = j;
            }

            // 현재 층과 이전 층의 스테이지 개수를 고려하며 스테이지를 잇는 path 구현
            if (j != 0)
            {
                if (width > 2 * pre_width) Debug.Break(); // 성립 안됨.

                if (pre_width >= width)
                {
                    Min_Path = pre_width;
                    Max_Path = pre_width + width - 1;
                }
                else
                {
                    Min_Path = width;
                    Max_Path = 2 * pre_width;
                }
                Total_Path = Random.Range(Min_Path, Max_Path - 1);

                _y = Total_Path - pre_width;  // Path 크기 2 개수
                _x = pre_width - _y;  // Path 크기 1 개수

                int index = 0;
                int pivot = 0;
                int Left_Path = Total_Path;

                for (int i = 0; i < pre_width; i++)
                {
                    Stage stage = Holder[j - 1, i];

                    if (pivot + Left_Path <= width - 1) index = pivot + 1;

                    else if (i != 0
                        && pivot < width - _y - 1)
                    {
                        if (Random.Range(0, 2) == 0) index = pivot;

                        else index = pivot + 1;
                    }
                    else index = pivot;

                    if (Random.Range(0, 2) == 0)
                    {
                        if (_x > 0)
                        {
                            stage.targetNum = 1;
                            _x--;
                        }
                        else if (_y > 0)
                        {
                            stage.targetNum = 2;
                            _y--;
                        }
                    }
                    else
                    {
                        if (_y > 0)
                        {
                            stage.targetNum = 2;
                            _y--;
                        }
                        else if (_x > 0)
                        {
                            stage.targetNum = 1;
                            _x--;
                        }
                    }
                    // path로 연결된 스테이지들을 target으로 지정해 부모, 자식관계로 만듦
                    for (int h = 0; h < stage.targetNum; h++)
                    {
                        stage.target[h] = Holder[j, index + h];
                        pivot = index + h;
                        Left_Path--;
                    }
                }
            }
        }
    }

    // 스테이지 초기 위치 값 정함
    private Vector3 InitPosition(int i, int j, int width)
    {
        if (width != 1) return new Vector3(
                         (i + Random.Range(-0.25f, 0.25f)) * (14f / (width - 1)) - 7f,
                     (-j + Random.Range(-0.1f, 0.5f)) * 5f + 30f,
                     0);
        else return new Vector3(
                         0f,
                     (-j + Random.Range(-0.1f, 0.5f)) * 5f + 30f,
                     0);
    }
}