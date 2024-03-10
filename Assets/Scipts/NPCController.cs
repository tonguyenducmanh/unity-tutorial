using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    /// <summary>
    /// hàm xử lý việc tương tác của người chơi với NPC
    /// </summary>
    public void Interact()
    {
        Debug.Log("You will talk to this NPC");
    }
}
