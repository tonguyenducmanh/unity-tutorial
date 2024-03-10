using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    /// <summary>
    /// dòng hội thoại hiện tại muốn hiển thị
    /// </summary>
    private int currentLine = 0;

    /// <summary>
    /// dialog có đang in ra từng chữ không
    /// </summary>
    private bool isTyping;

    /// <summary>
    /// hội thoại được truyền vào
    /// </summary>
    private Dialog dialog;
    // tham số để gán đối tượng muốn hiển thị dialog vào
    [SerializeField] GameObject dialogBox;

    // tham số để truyền nội dung muốn gán vào
    [SerializeField] Text dialogText;
    // số ký tự muốn hiện ra trong 1 thời điểm
    [SerializeField] int lettersPerSecond;

    public event Action OnShowDialog;
    public event Action OnHideDialog;

    public static DialogManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Hàm thực hiện hiển thị dialog dựa vào nội dung truyền vào
    /// </summary>
    /// <param name="dialog">nội dung truyền vào</param>
    public IEnumerator ShowDialog(Dialog dialog)
    {
        yield return new WaitForEndOfFrame();
        OnShowDialog?.Invoke();
        this.dialog = dialog;
        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(dialog.Lines[0]));
    }

    public IEnumerator TypeDialog(string line)
    {
        isTyping = true;
        dialogText.text = "";
        foreach (var letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

        isTyping = false;
    }

    /// <summary>
    /// cập nhật hiển thị dialog
    /// </summary>
    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isTyping)
        {
            // tăng dòng hiển thị lên 1 đơn vị
            ++currentLine;
            // nếu còn dòng dialog thì hiển thị ra không thì ẩn đi
            if (currentLine < dialog.Lines.Count)
            {
                StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
            }
            else
            {
                // reset toàn bộ
                dialogBox.SetActive(false);
                OnHideDialog?.Invoke();
                currentLine = 0;
            }
        }
    }
}
