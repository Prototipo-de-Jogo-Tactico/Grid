using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    float speed = 5f;
    float horizontal = 0;
    float vertical = 0;
    bool isMoving = false;
    Rigidbody2D rigidbody2d;
    Animator animator;
    Vector2 targetPosition;
    Vector2 lookDirection = new Vector2(0, -1);
    // [SerializeField]
    // Tilemap obstacles;
    private void Awake()
    {
        targetPosition = new Vector2(transform.position.x, transform.position.y);
        transform.position = (Vector2)targetPosition;
    }

    private void Start()
    {
        // test with 10 fps
        // Application.targetFrameRate = 10;
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        isMoving = (Vector2)transform.position != targetPosition;
        if (!isMoving)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
            if (horizontal != 0)
            {
                vertical = 0;
            }

            targetPosition += new Vector2(horizontal, vertical);
            Vector2 move = new Vector2(horizontal, vertical);

            if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
            {
                // Vector2Int obstacleMapTile = (Vector2Int)obstacles.WorldToCell(targetPosition);
                // Debug.Log(obstacleMapTile);
                lookDirection.Set(move.x, move.y);
                lookDirection.Normalize();
            }

            animator.SetFloat("Look X", lookDirection.x);
            animator.SetFloat("Look Y", lookDirection.y);
            animator.SetFloat("Speed", move.magnitude);
        }
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attack");
            StartCoroutine("Attacking");
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        int layerMask = 1 << gameObject.layer;
        layerMask = ~layerMask;
        RaycastHit2D hit = Physics2D.Linecast(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(targetPosition.x, targetPosition.y + 0.5f), layerMask);

        // Debug.DrawLine(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(targetPosition.x, targetPosition.y + 0.5f), Color.black);
        if (hit)
        {
            // position = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
            targetPosition = position;
        }

        position = Vector2.MoveTowards(position, targetPosition, speed * Time.deltaTime);
        rigidbody2d.MovePosition(position);
    }

    private void Attacking()
    {
        int layerMask = 1 << gameObject.layer;
        layerMask = ~layerMask;
        RaycastHit2D hit = Physics2D.Linecast(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(targetPosition.x + 1f * lookDirection.x, targetPosition.y + 0.5f + 1f * lookDirection.y), layerMask);
        Debug.DrawLine(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(targetPosition.x + 1f * lookDirection.x, targetPosition.y + 0.5f + 1f * lookDirection.y), Color.black);

        if (hit && hit.collider.tag == "Enemy")
        {
            Debug.Log("hit!");
        }
    }
}
