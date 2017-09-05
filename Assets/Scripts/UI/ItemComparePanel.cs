using UnityEngine;
using UnityEngine.UI;
using RPG.Inventory;

namespace RPG.UI
{
	/// <summary>
	/// Generic panel that compares item stats.
	/// </summary>
	public class ItemComparePanel : MonoBehaviour
	{
        [Header("Static controls")]
        public Text ItemName1;
        public Text ItemName2;
        public Text Description1, Description2;
        public Image Icon1, Icon2;

        [Header("Dynamic stats")]
        public Transform StatParent;
        public Transform StatItem;

        void Awake()
        {
            StatItem.gameObject.SetActive(false);
        }
        
        public void SetItems(Item newItem, Item oldItem, params StatBlock[] stats)
        {
            // Static stuff
            ItemName1.text = newItem.Name;
            ItemName2.text = oldItem.Name;
            Description1.text = newItem.Description;
            Description2.text = oldItem.Description;
            Icon1.sprite = newItem.Icon;
            Icon2.sprite = oldItem.Icon;

            // Dynamic stuff
            foreach (StatBlock stat in stats)
            {
                Transform tr = Instantiate(StatItem);
                tr.gameObject.SetActive(true);
                tr.SetParent(StatParent, false);

                tr.Find("Title").GetComponent<Text>().text = stat.Title;
                tr.Find("Stat1").GetComponent<Text>().text = stat.Stat1.ToString();
                tr.Find("Stat2").GetComponent<Text>().text = stat.Stat2.ToString();
            }
        }

        void OnDisable()
        {
            foreach (Transform tr in StatParent)
                Destroy(tr.gameObject);
        }

        public class StatBlock
        {
            public string Title;
            public object Stat1, Stat2;

            public StatBlock(string title, object stat1, object stat2)
            {
                Title = title;
                Stat1 = stat1;
                Stat2 = stat2;
            }
        }
    }
}