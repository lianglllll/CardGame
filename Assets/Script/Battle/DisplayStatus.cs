using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayStatus : MonoBehaviour
{
    public Text status;

    private void Awake()
    {
        BattleManager.Instance.StatusChangeEvent.AddListener(UpdataText);
    }


    public void UpdataText()
    {

        status.text = BattleManager.Instance.gameStatus.ToString();

    }


}
