using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public void InitPlayerPosition()
    {
        for (int i = 0; i < GameManager.instance.playerList.Length; i++)
        {
            Player player = GameManager.instance.playerList[i];

            player.gameObject.transform.localPosition = player.InitPlayerPosition;

            player.spr.color = Color.white;

            player.Hp = GameManager.instance.playerList[i].InitHp;

            player.AttackDelay = GameManager.instance.playerList[i].InitAttackDelay;

            player.Damage = GameManager.instance.playerList[i].InitDamage;

            player.HpHUD.value = 1.0f;

            player.IsHit = false;

            player.target = null;
        }
    }

    public IEnumerator SpawnPlayer()
    {
        for (int i = 0; i < GameManager.instance.playerList.Length; i++)
        {
            Player player = GameManager.instance.playerList[i];

            player.PlayerMove();

            yield return null;
        }
        yield return null;
    }
}
