using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage : MonoBehaviour
{
    public GameObject Clear;
    public int Depth;
    public Stage[] target;
    public int targetNum; // 분할 시 얻게 되는 숫자
    public int StageNum; // 0 ~ 8
    public int SoundNum; // 1 ~ 5


    public LineRenderer lr;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.startColor = Color.white;
        lr.endColor = Color.white;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.positionCount = 2 * targetNum;


        if (target[0] != null) for (int i = 0; i < targetNum; i++) DrawLine(target[i], i);
    }
    public void DrawLine(Stage target, int index)
    {
        lr.SetPosition(2 * index, gameObject.transform.position);

        lr.SetPosition(2 * index + 1, target.transform.position);
    }

    private void OnMouseDown()
    {

        //   Debug.Log("MouseOn");
        // if (MapController.isOpen) return;

        if (Depth == SoundManager.instance.Depth)
        {
            SoundManager.instance.CamPos = MoveCamera.instance.transform.position;

            if (MapGenerator.instance.previousStage == null)
            {
                MapGenerator.instance.previousStage = this;

                GotoStage();
            }
            else
            {
                for (int i = 0; i < MapGenerator.instance.previousStage.targetNum; i++)
                {
                    if (MapGenerator.instance.previousStage.target[i] == this)
                    {
                        if (MapGenerator.instance.previousStage.targetNum == 2)
                        {
                            if (MapGenerator.instance.previousStage.target[0] == this)
                            {
                                MapGenerator.instance.previousStage.lr.startColor = Color.red;

                                MapGenerator.instance.previousStage.lr.endColor = Color.white;
                            }
                            else
                            {
                                MapGenerator.instance.previousStage.lr.startColor = Color.white;

                                MapGenerator.instance.previousStage.lr.endColor = Color.red;
                            }
                        }
                        else
                        {
                            MapGenerator.instance.previousStage.lr.startColor = Color.red;

                            MapGenerator.instance.previousStage.lr.endColor = Color.red;
                        }

                        MapGenerator.instance.previousStage = this;

                        GotoStage();
                    }
                }
            }
        }
    }

    private void GotoStage()
    {
        GameObject clear = Instantiate(Clear, transform.position, Quaternion.identity);

        clear.transform.parent = gameObject.transform;

        gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 100);

        if (SoundManager.instance.NowPlaying != SoundNum)
        {
            SoundManager.instance.BGM[SoundManager.instance.NowPlaying].Stop();

            SoundManager.instance.NowPlaying = SoundNum;

            SoundManager.instance.BGM[SoundNum].PlayDelayed(0.5f);
        }

      //  Debug.Log(SoundManager.instance.NowPlaying);

        switch (StageNum)
        {
            case 0:
                SceneManager.LoadScene("1. Copy");

                // SceneManager.LoadScene("1. Copy");

                MapGenerator.instance.gameObject.SetActive(false);

                SoundManager.instance.Depth++;

                break;

            case 1:
                SceneManager.LoadScene("1. Copy_Boss");

                MapGenerator.instance.gameObject.SetActive(false);

                SoundManager.instance.Depth++;

                break;

            case 2:
                SceneManager.LoadScene("2. AbandonedArea");

                MapGenerator.instance.gameObject.SetActive(false);

                SoundManager.instance.Depth++;

                break;

            case 3:
                SceneManager.LoadScene("2. AbandonedArea_Boss");

                MapGenerator.instance.gameObject.SetActive(false);

                SoundManager.instance.Depth++;

                break;

            case 4:
                SceneManager.LoadScene("3. Biology");

                MapGenerator.instance.gameObject.SetActive(false);

                SoundManager.instance.Depth++;

                break;

            case 5:
                SceneManager.LoadScene("3. Biology_Boss");

                MapGenerator.instance.gameObject.SetActive(false);

                SoundManager.instance.Depth++;

                break;

            case 6:
                SceneManager.LoadScene("4. Spritism");

                MapGenerator.instance.gameObject.SetActive(false);

                SoundManager.instance.Depth++;

                break;

            case 7:
                SceneManager.LoadScene("4. Spritism_Boss");

                MapGenerator.instance.gameObject.SetActive(false);

                SoundManager.instance.Depth++;

                break;

            case 8:
                SceneManager.LoadScene("5. Final");

                MapGenerator.instance.gameObject.SetActive(false);

                SoundManager.instance.Depth++;

                break;
        }

    }
}
