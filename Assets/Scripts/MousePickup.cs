using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MousePickup : MonoBehaviour
{
    private class PopupItem
    {
        private GameObject gameObject;
        private Text text;
        private int count = 0;

        public int Count
        {
            set
            {
                count = value;
                gameObject.SetActive(count > 0);
            }

            get
            {
                return count;
            }
        }

        public Text Text
        {
            get { return text; }
        }

        public PopupItem(GameObject gameObject)
        {
            this.gameObject = gameObject;
            text = gameObject.GetComponentInChildren<Text>();
        }

        public void Update(Camera camera, Vector2 position, Vector2 screenOffset)
        {
            if (count > 0)
            {
                gameObject.transform.position = (Vector2)camera.WorldToScreenPoint(position) + screenOffset;
            }
        }
    }

    [SerializeField]
    private GameObject mouseOverPrefab;

    private Dictionary<GameObject, PopupItem> popups = new Dictionary<GameObject, PopupItem>();
    private GameObject mouseOverObject;
    private GameObject canvas;

    [SerializeField]
    private Vector2 popupOffset;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float rotateSpeed;

    private Rigidbody2D heldItem;
    private Vector2 mousePoint = new Vector2();

    private int foodMask;
    private int dropTargetMask;

    void Start()
    {
        canvas = GameObject.Find("UI");
        foodMask = LayerMask.GetMask("PickupTarget");
        dropTargetMask = LayerMask.GetMask("DropTarget");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetButtonUp("Fire1"))
        {
            DropItem(mousePoint);
        }
     
        Collider2D collider = Physics2D.OverlapPoint(mousePoint, foodMask);
        if (Input.GetButtonDown("Fire1") && collider)
        {
            // User clicked down this frame on a collider, pick it up.
            HoldItem(collider.GetComponent<Rigidbody2D>());
        }

        UpdateMouseOver(collider ? collider.gameObject : null, mousePoint);
        UpdatePopups(mousePoint);
    }

    void FixedUpdate()
    {
        // Move the held item toward the cursor
        if (heldItem)
        {
            heldItem.velocity = (mousePoint - heldItem.position) * moveSpeed * Time.fixedDeltaTime;
            float diff = Mathf.DeltaAngle(heldItem.rotation, 0);
            heldItem.angularVelocity = diff * rotateSpeed * Time.fixedDeltaTime;
        }
    }

    PopupItem GetOrCreatePopup(GameObject foodObject)
    {
        PopupItem popup;
        if(!popups.TryGetValue(foodObject, out popup))
        {
            popup = new PopupItem(Instantiate(mouseOverPrefab, canvas.transform));
            Food food = foodObject.GetComponent<Food>();
            popup.Text.text = food.Name;
            popups.Add(foodObject, popup);
            return popup;
        }
        return popup;
    }

    void IncrementPopup(GameObject foodObject)
    {
        if (mouseOverPrefab != null)
        {
            PopupItem popup = GetOrCreatePopup(foodObject);
            popup.Count++;
        }
    }

    void DecrementPopup(GameObject foodObject)
    {
        if (mouseOverPrefab != null)
        {
            PopupItem popup;
            if (popups.TryGetValue(foodObject, out popup))
            {
                popup.Count--;
            }
        }
    }

    void UpdatePopups(Vector2 worldPoint)
    {
        foreach(PopupItem popup in popups.Values)
        {
            popup.Update(Camera.main, worldPoint, popupOffset);
        }
    }

    void UpdateMouseOver(GameObject overObject, Vector2 mousePosition)
    {
        if(overObject != mouseOverObject)
        {
            // Moused over something different to last time
            // Clear the current mouse over (if any)
            if (mouseOverObject)
            {
                DecrementPopup(mouseOverObject);
                mouseOverObject = null;
            }

            // Mouse over the new one (if any)
            if(overObject)
            {
                mouseOverObject = overObject;
                IncrementPopup(mouseOverObject);
            }
        }
    }

    public void DropItem(Vector2 dropPoint)
    {
        if(!heldItem)
        {
            return;
        }

        DecrementPopup(heldItem.gameObject);
        // Check if it's being dropped onto a drop target
        Collider2D collider = Physics2D.OverlapPoint(dropPoint, dropTargetMask);
        if(collider)
        {
            collider.gameObject.SendMessage("OnDrop", heldItem.gameObject);
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
        IncrementPopup(heldItem.gameObject);
    }

}
