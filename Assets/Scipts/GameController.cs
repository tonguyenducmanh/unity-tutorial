using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState
{
    FreeRoam,
    Dialog,
    Battle
}

public class GameController : MonoBehaviour
{
    // tạo ra tham số để gán Player vào ở trong unitySence
    [SerializeField] PlayerController playerController;

    GameState state;

    private void Start()
    {
        // nếu show dialog thì đổi trạng thái game để người dùng không di chuyển được
        DialogManager.Instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
        };

        // nếu ngừng show dialog thì đổi trạng thái game để người dùng di chuyển

        DialogManager.Instance.OnHideDialog += () =>
        {
            if (state == GameState.Dialog)
            {
                state = GameState.FreeRoam;
            }
        };
    }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }
        else if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
        else if (state == GameState.Battle)
        {

        }
    }
}
