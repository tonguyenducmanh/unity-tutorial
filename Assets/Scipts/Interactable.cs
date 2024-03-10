using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// interface dùng chung cho tất cả các đối tượng trong game có thể tương tác
/// </summary>
public interface Interactable
{
    /// <summary>
    /// hàm xử lý việc tương tác của đối tượng
    /// </summary>
    void Interact();
}
