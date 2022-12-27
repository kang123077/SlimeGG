using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    [SerializeField]
    private GameObject PenalForPause;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (LocalStorage.BATTLE_SCENE_LOADING_DONE && !LocalStorage.IS_GAME_PAUSE)
        {
            bool isOneSideAllDead;
            foreach (List<MonsterBattleController> sideList in LocalStorage.monsterBattleControllerList)
            {
                isOneSideAllDead = true;
                foreach (MonsterBattleController battleController in sideList)
                {
                    isOneSideAllDead = isOneSideAllDead && battleController.isDead;
                }
                if (isOneSideAllDead) pauseOrResumeBattle();
            }
        }
    }

    public void pauseOrResumeBattle()
    {
        LocalStorage.IS_GAME_PAUSE = !LocalStorage.IS_GAME_PAUSE;
        PenalForPause.SetActive(LocalStorage.IS_GAME_PAUSE);
    }
}
