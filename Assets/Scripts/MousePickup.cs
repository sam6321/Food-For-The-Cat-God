using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePickup : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float rotateSpeed;

    private Rigidbody2D heldItem;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonUp("Fire1"))
        {
            DropItem();
        }

        bool holdDown = Input.GetButtonDown("Fire1");

        if(holdDown || heldItem)
        {
            Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(holdDown)
            {
                Collider2D collider = Physics2D.OverlapPoint(point);
                if(collider)
                {
                    HoldItem(collider.GetComponent<Rigidbody2D>());
                }
            }

            if(heldItem)
            {
                heldItem.velocity = (point - heldItem.position) * moveSpeed * Time.deltaTime;
                float diff = Mathf.DeltaAngle(heldItem.rotation, 0);
                heldItem.angularVelocity = diff * rotateSpeed * Time.deltaTime;
            }
        }
    }

    public void DropItem()
    {
        if(!heldItem)
        {
            return;
        }

        heldItem = null;
    }

    public void HoldItem(Rigidbody2D item)
    {
        if(heldItem)
        {
            return;
        }

        heldItem = item;
    }
}
