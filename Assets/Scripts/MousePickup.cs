using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MousePickup : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseOverPrefab;

    private Dictionary<int, GameObject> mouseOverDictionary = new Dictionary<int, GameObject>();
    private GameObject mouseOverObject;
    private GameObject mouseOverPopup;
    private GameObject canvas;

    [SerializeField]
    private Vector2 popupOffset;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float rotateSpeed;

    private Rigidbody2D heldItem;
    private GameObject heldItemPopup;

    private int foodMask;

    void Start()
    {
        canvas = GameObject.Find("UI");
        foodMask = LayerMask.GetMask("Food");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonUp("Fire1"))
        {
            DropItem();
        }

        bool holdDown = Input.GetButtonDown("Fire1");

        Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D collider = Physics2D.OverlapPoint(point, foodMask);
        if (holdDown && collider)
        {
            // User clicked down this frame on a collider, pick it up.
            HoldItem(collider.GetComponent<Rigidbody2D>());
        }

        // Move the held item toward the cursor
        if(heldItem)
        {
            heldItem.velocity = (point - heldItem.position) * moveSpeed * Time.deltaTime;
            float diff = Mathf.DeltaAngle(heldItem.rotation, 0);
            heldItem.angularVelocity = diff * rotateSpeed * Time.deltaTime;
        }

        UpdateMouseOver(collider ? collider.gameObject : null, point);
        UpdatePopup(mouseOverPopup, point);
        UpdatePopup(heldItemPopup, point);
    }

    GameObject GetOrCreatePopup(GameObject gameObject)
    {
        GameObject popup;
        if(!mouseOverDictionary.TryGetValue(gameObject.GetInstanceID(), out popup))
        {
            popup = Instantiate(mouseOverPrefab, canvas.transform);
            Food food = gameObject.GetComponent<Food>();
            Text text = popup.GetComponentInChildren<Text>();
            text.text = food.Name;
            mouseOverDictionary.Add(gameObject.GetInstanceID(), popup);
            return popup;
        }
        return popup;
    }

    void UpdateMouseOver(GameObject overObject, Vector2 mousePosition)
    {
        if(overObject != mouseOverObject)
        {
            // Moused over something different to last time
            // Clear the current mouse over (if any)
            if (mouseOverObject)
            {
                mouseOverObject = null;
                // Animate the fade out for this object
                //mouseOverPopup.GetComponent<Animator>().FadeOut();
                mouseOverPopup.SetActive(false);
                mouseOverPopup = null;
            }

            // Mouse over the new one (if any)
            if(overObject)
            {
                mouseOverObject = overObject;
                mouseOverPopup = GetOrCreatePopup(overObject);
                mouseOverPopup.SetActive(true);
                // Animate the fade in for this object
                // mouseOverPopup.GetComponent<Animator>().FadeIn();
            }
        }
    }

    void UpdatePopup(GameObject popup, Vector2 mousePosition)
    {
        if (popup)
        {
            // Update the position of the popup
            popup.transform.position = (Vector2)Camera.main.WorldToScreenPoint(mousePosition) + popupOffset;
        }
    }

    public void DropItem()
    {
        if(!heldItem)
        {
            return;
        }

        heldItem = null;
        heldItemPopup.SetActive(false);
        heldItemPopup = null;
    }

    public void HoldItem(Rigidbody2D item)
    {
        if(heldItem)
        {
            return;
        }

        heldItem = item;
        heldItemPopup = GetOrCreatePopup(item.gameObject);
        heldItemPopup.SetActive(true);
    }

}
