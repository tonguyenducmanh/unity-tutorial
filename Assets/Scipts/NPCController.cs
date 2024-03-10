using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    /// <summary>
    /// nội dung sẽ được truyền từ ngoài vào để hiển thị
    /// </summary>
    [SerializeField] Dialog dialog;
    /// <summary>
    /// hàm xử lý việc tương tác của người chơi với NPC
    /// </summary>
    public void Interact()
    {
       StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
    }
}
