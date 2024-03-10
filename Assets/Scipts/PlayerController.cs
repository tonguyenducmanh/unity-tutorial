using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region const

    /// <summary>
    /// chiều ngang
    /// </summary>
    private const string _horizontal = "Horizontal";

    /// <summary>
    /// chiều dọc
    /// </summary>
    private const string _vertical = "Vertical";

    /// <summary>
    /// di chuyển x
    /// </summary>
    /// tên param animator trong blend tree của player
    private const string _characterMoveX = "moveX";

    /// <summary>
    /// di chuyển y
    /// </summary>
    /// tên param animator trong blend tree của player
    private const string _characterMoveY = "moveY";

    /// <summary>
    /// người chơi có đang di chuyển
    /// </summary>
    /// tên param animator trong blend tree của player
    private const string _characterIsMoving = "isMoving";

    #endregion

    #region declear unity using

    /// <summary>
    /// tốc độ di chuyển của người chơi
    /// </summary>
    public float moveSpeed;

    /// <summary>
    /// các object mà người chơi không được phép đi qua
    /// ( vật cản trong game , để public cho object trong project unity trỏ tơis)
    /// </summary>
    public LayerMask solidObjectsLayer;

    /// <summary>
    /// các object mà người chơi có thể tương tác được
    /// </summary>
    public LayerMask interactableLayer;
    #endregion

    #region declare private using
    /// <summary>
    /// người chơi có đang di chuyển không
    /// </summary>
    private bool isMoving;

    /// <summary>
    /// cấu hình tọa độ theo x value và y value ( tọa độ 2D )
    /// theo đầu vào của người dùng
    /// </summary>
    private Vector2 input;

    /// <summary>
    /// chuyển động của người chơi
    /// </summary>
    private Animator animator;

    #endregion

    #region function

    /// <summary>
    /// hàm này sẽ gọi khi file này được tạo 1 insance
    /// </summary>
    private void Awake()
    {
        // lấy ra component chuyển động
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// luôn update theo từng frame
    /// </summary>
    public void HandleUpdate()
    {
        if (!isMoving)
        {
            // lấy ra tọa độ di chuyển dựa vào phím mà người dùng nhập
            input.x = Input.GetAxisRaw(_horizontal);
            input.y = Input.GetAxisRaw(_vertical);

            // chặn việc di chuyển chéo, tại 1 thời điểm chỉ có thể sang ngang hoặc dọc
            if (input.x != 0)
            {
                input.y = 0;
            }

            if (input != Vector2.zero)
            {
                // dựa vào tham số người dùng nhập mà set animation hợp lý
                // cấu hình blend tree idle của player trong unity tab animator sẽ dựa vào 2 tham số moveX và moveY này để quyết định di chuyển
                animator.SetFloat(_characterMoveX, input.x);
                animator.SetFloat(_characterMoveY, input.y);

                // transform là object mặc định gắn với charactor
                // xem ở tab Inspector trong unity sẽ rõ
                Vector3 targetPosition = transform.position;
                targetPosition.x += input.x;
                targetPosition.y += input.y;

                // kiểm tra xem có chuẩn bị đi vào vị trí có vật cản không
                // nếu có thì dừng lại <> không có thì đi vào
                if (IsWalkable(targetPosition))
                {
                    StartCoroutine(Move(targetPosition));
                }
            }
        }

        // cập nhật trạng thái có đang di chuyển không cho người chơi
        animator.SetBool(_characterIsMoving, isMoving);

        // lắng nghe phím z để tương tác với người chơi
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Interact();
        }
    }

    /// <summary>
    /// hàm xử lý việc tương tác của người chơi
    /// </summary>
    private void Interact()
    {
        // lấy ra tọa độ di chuyển tiếp theo của người chơi
        Vector3 facingDir = new Vector3(animator.GetFloat(_characterMoveX), animator.GetFloat(_characterMoveY));

        // lấy ra tọa độ của vật thể muốn tương tác
        Vector3 interactPos = transform.position + facingDir;

        // thêm dòng debug lúc chạy để xem 2 điểm này nối với nhau nó sẽ như thế nào
        //Debug.DrawLine(transform.position, interactPos, Color.red, 1f);

        Collider2D collider = Physics2D.OverlapCircle(interactPos, 0.2f, interactableLayer);

        // nếu tìm thấy đối tượng tương tác
        if (collider != null)
        {
            // kiểm tra xem đối tượng tương tác này có đang chứa script
            // trong script đó kế thừa Ineractable interface không
            // nếu có thì gọi hàm Interact()
            collider.GetComponent<Interactable>()?.Interact();
        }
    }

    /// <summary>
    /// kiểm tra xem có được phép đi vào vị trí hiện tại không
    /// </summary>
    /// <param name="targetPosition">vị trí cần kiểm tra</param>
    /// <returns></returns>
    private bool IsWalkable(Vector3 targetPosition)
    {
        bool result = true;
        // kiểm tra xem vị trí hiện tại có chạm vật thể có tên là solidObjectsLayer không
        if (Physics2D.OverlapCircle(targetPosition, 0.2f, solidObjectsLayer | interactableLayer) != null)
        {
            result = false;
        }
        return result;
    }

    /// <summary>
    /// hàm thực hiện di chuyển tới vị trí tiếp theo 
    /// </summary>
    /// <param name="targetPosition">vị trí tiếp theo muốn di chuyển</param>
    /// <returns></returns>
    IEnumerator Move(Vector3 targetPosition)
    {
        isMoving = true;

        while ((targetPosition - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;

        isMoving = false;
    }

    #endregion
}
