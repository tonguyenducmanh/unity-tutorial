﻿using System.Collections;
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

    #region declare

    /// <summary>
    /// tốc độ di chuyển của người chơi
    /// </summary>
    public float moveSpeed;

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
    private void Update()
    {
        if(!isMoving)
        {
            // lấy ra tọa độ di chuyển dựa vào phím mà người dùng nhập
            input.x = Input.GetAxisRaw(_horizontal);
            input.y = Input.GetAxisRaw(_vertical);

            // chặn việc di chuyển chéo, tại 1 thời điểm chỉ có thể sang ngang hoặc dọc
            if(input.x != 0 )
            {
                input.y = 0;
            }

            if(input != Vector2.zero)
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

                StartCoroutine(Move(targetPosition));
            }
        }

        // cập nhật trạng thái có đang di chuyển không cho người chơi
        animator.SetBool(_characterIsMoving, isMoving);
    }

    /// <summary>
    /// hàm thực hiện di chuyển tới vị trí tiếp theo 
    /// </summary>
    /// <param name="targetPosition">vị trí tiếp theo muốn di chuyển</param>
    /// <returns></returns>
    IEnumerator Move(Vector3 targetPosition)
    {
        isMoving = true;

        while((targetPosition - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        
        transform.position = targetPosition;

        isMoving = false;
    }

    #endregion
}