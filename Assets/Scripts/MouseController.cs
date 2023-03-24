using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class MouseController : MonoBehaviour
{
    public GameObject cursor;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        RaycastHit2D? hit = GetFocusedOnTile();

        if (hit.HasValue)
        {
            GameObject overlayTile = hit.Value.collider.gameObject;
            cursor.transform.position = overlayTile.transform.position;
            cursor.GetComponent<SpriteRenderer>().sortingOrder = overlayTile.GetComponent<SpriteRenderer>().sortingOrder;

            if (Input.GetMouseButtonDown(0))
            {
                overlayTile.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
        }
    }

    public RaycastHit2D? GetFocusedOnTile()
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), 100, layerMask);

        if (hit.collider != null && hit.collider.gameObject.layer == 0)
        {
            return hit;
        }

        return null;
    }
}
