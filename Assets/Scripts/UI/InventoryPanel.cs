using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RPG.Inventory;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RPG.UI
{
	/// <summary>
	/// InventoryPanel
	/// </summary>
	public class InventoryPanel : MonoBehaviour
	{
        public GameObject ItemPrefab;
        public Transform ItemParent;

        public GameObject DragDestinationDoll;
        public GameObject DragDestinationBag;

        private Item selectedItem = null;
        private Item draggingItem = null;

        public void Show(List<Item> items)
        {
            foreach (GameObject child in ItemParent)
                Destroy(child);

            foreach (Item item in items)
            {
                Item itemClosure = item;

                // Instantiate
                GameObject go = Instantiate(ItemPrefab);
                go.transform.SetParent(ItemParent);

                // Set icon
                go.transform.Find("Image").GetComponent<Image>().sprite = item.Icon;

                // Clicking on item
                go.GetComponent<Button>()?.onClick.AddListener(() => {
                    OnItemClicked(itemClosure);
                });

                // Handle hovering over items
                EventTrigger et = go.GetComponent<EventTrigger>();
                et.triggers.Clear();

                EventTrigger.Entry onItemHover = new EventTrigger.Entry();
                onItemHover.eventID = EventTriggerType.PointerEnter;
                onItemHover.callback.AddListener((e) => { OnItemPointerEnter(itemClosure); });

                EventTrigger.Entry onItemExit = new EventTrigger.Entry();
                onItemExit.eventID = EventTriggerType.PointerExit;
                onItemExit.callback.AddListener((e) => { OnItemPointerExit(itemClosure); });

                et.triggers.Add(onItemHover);
                et.triggers.Add(onItemExit);

                // TODO: Drag the whole bullshit over the paper doll.
                // No need to select the proper slot.
                go.GetComponent<Draggable>().OnDragStarted.AddListener(() => { DragStartFromBag(itemClosure); });
            }
        }

        public void ShowItemDetails(Item item)
        {

        }
        public void HideItemDetails()
        {

        }

        public void OnDropOnDoll()
        {
            // Equip this item
        }
        public void OnDropOnInventory()
        {
            // Unequip this item
        }

        private void OnItemClicked(Item item)
        {
            selectedItem = item;
            ShowItemDetails(item);
        }
        private void OnItemPointerEnter(Item item)
        {
            if (selectedItem != null)
                ShowItemDetails(item);
        }
        private void OnItemPointerExit(Item item)
        {
            if (selectedItem != null)
                HideItemDetails();
        }

        private void DragStartFromBag(Item item)
        {
            DragDestinationDoll.SetActive(true);
        }
        private void DragStartFromDoll(Item item)
        {
            DragDestinationBag.SetActive(true);
        }
        private void DragEnd()
        {
            DragDestinationBag.SetActive(false);
            DragDestinationDoll.SetActive(false);
        }
    }
}