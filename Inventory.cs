using ModAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using TheForest.Items;
using TheForest.Items.Craft;
using TheForest.Utils;
using UnityEngine;

namespace InventoryMod2
{
    internal class Inventory : MonoBehaviour
    {
        private Vector2[] scrollPositions = new Vector2[6];
        private Dictionary<int, List<Item>> categoryItems = new Dictionary<int, List<Item>>();

        private readonly string[] tabNames = { "Weapons", "Items", "Res.", "Head", "Other", "All" };

        private readonly HashSet<string> stackableItems = new HashSet<string>
        {
            "Arrows", "ArrowFire", "PoisonnedArrow", "BombTimed",
            "dynamite", "FlareGunAmmo", "flintlockAmmo", "Molotov", "CrossbowAmmo"
        };

        private readonly string[] catWeapons = {
            "arrow", "axe", "ArrowFire", "bow", "bomb", "club",
            "PoisonnedArrow", "katana", "flaregun", "molotov", "dynamite",
            "spear", "spearRaise", "upgraded", "flintlock", "bowFire",
            "drawBow", "smallAxe", "repairHammer", "chainSaw", "RecurveBow", "camCorder"
        };

        private readonly string[] catItems = {
            "lighter", "lighterIgnite", "walkman", "torch", "firestick",
            "compass", "flare", "snowshoes", "boots", "quiver", "pouch",
            "waterskin", "pedometer", "rebreather", "canister", "armor",
            "rockbag", "stickbag", "metaltintray", "draw", "pot"
        };

        private readonly string[] catResources = {
            "cboard", "cloth", "glass", "hairspray", "cod", "coneflower",
            "bone", "stick", "rock", "leaf", "tree", "sap", "battery",
            "booze", "rope", "food", "soda", "energy", "chocolate",
            "circuit", "cash", "tooth", "berry", "arm", "aloe", "leg",
            "lizard", "shell", "meds", "watch", "feather", "skin",
            "chicory", "mari", "coin", "paint", "skull", "seed", "meat",
            "rabbit dead", "rabbit alive", "log", "map", "generic",
            "genericWide", "genericHoldPouch"
        };

        private readonly string[] catAnimals = { "head" };

        private bool visible;
        private bool sorted;
        private GUIStyle labelStyle;
        private int currentTab;

        private const float WINDOW_X = 5f;
        private const float WINDOW_Y = 5f;
        private const float WINDOW_WIDTH = 400f;
        private const float WINDOW_HEIGHT = 430f;
        private const float SCROLL_HEIGHT = 370f;
        private const float ITEM_HEIGHT = 30f;
        private const float LABEL_WIDTH = 150f;
        private const float BUTTON_WIDTH = 70f;

        [ExecuteOnGameStart]
        private static void AddMeToScene()
        {
            new GameObject("__InventoryMenu__").AddComponent<Inventory>();
        }

        private void Start()
        {
            for (int i = 0; i < 6; i++)
            {
                scrollPositions[i] = Vector2.zero;
            }
        }

        private void Sort()
        {
            if (sorted) return;
            sorted = true;

            // Start() 전에 호출될 수 있으므로 여기서 초기화
            for (int i = 0; i < 6; i++)
            {
                if (!categoryItems.ContainsKey(i))
                {
                    categoryItems[i] = new List<Item>();
                }
            }

            foreach (var item in ItemDatabase.Items)
            {
                int category = CategorizeItem(item);
                categoryItems[category].Add(item);
            }

            for (int i = 0; i < 5; i++)
            {
                categoryItems[i].Sort((x, y) => string.Compare(x._name, y._name));
            }
        }

        private int CategorizeItem(Item item)
        {
            string nameLower = item._name.ToLower();

            if (ContainsAny(nameLower, catWeapons)) return 0;
            if (ContainsAny(nameLower, catAnimals)) return 3;
            if (ContainsAny(nameLower, catItems)) return 1;
            if (ContainsAny(nameLower, catResources)) return 2;
            return 4;
        }

        private bool ContainsAny(string text, string[] keywords)
        {
            foreach (string keyword in keywords)
            {
                if (text.Contains(keyword.ToLower()))
                    return true;
            }
            return false;
        }

        private void OnGUI()
        {
            if (!visible) return;

            GUI.skin = ModAPI.Interface.Skin;
            InitializeLabelStyle();

            GUI.Box(new Rect(WINDOW_X, WINDOW_Y, WINDOW_WIDTH, WINDOW_HEIGHT),
                    "Inventory menu", GUI.skin.window);

            currentTab = GUI.Toolbar(
                new Rect(WINDOW_X, WINDOW_Y, WINDOW_WIDTH, 30f),
                currentTab,
                tabNames,
                GUI.skin.GetStyle("Tabs")
            );

            RenderTabContent(currentTab);
        }

        private void InitializeLabelStyle()
        {
            if (labelStyle == null)
            {
                labelStyle = new GUIStyle(GUI.skin.label) { fontSize = 12 };
            }
        }

        private void RenderTabContent(int tab)
        {
            if (tab == 5)
            {
                RenderAllItemsTab();
            }
            else
            {
                bool showMaxButton = (tab == 0 || tab == 2);
                RenderItemList(tab, categoryItems[tab], showMaxButton);
            }
        }

        private void RenderItemList(int tabIndex, List<Item> items, bool showMaxButton)
        {
            float contentHeight = 25f + (items.Count * ITEM_HEIGHT);

            scrollPositions[tabIndex] = GUI.BeginScrollView(
                new Rect(WINDOW_X, 45f, 390f, SCROLL_HEIGHT),
                scrollPositions[tabIndex],
                new Rect(0f, 0f, 330f, contentHeight)
            );

            float yPos = 25f;
            foreach (var item in items)
            {
                RenderItemRow(item, ref yPos, showMaxButton);
            }

            GUI.EndScrollView();
        }

        private void RenderItemRow(Item item, ref float yPos, bool showMaxButton)
        {
            GUI.Label(new Rect(20f, yPos, LABEL_WIDTH, 20f), item._name, labelStyle);

            bool isStackable = showMaxButton && stackableItems.Contains(item._name);

            if (isStackable)
            {
                if (GUI.Button(new Rect(210f, yPos, BUTTON_WIDTH, 20f), "Add"))
                {
                    AddItemToInventory(item._id, 1);
                }
                if (GUI.Button(new Rect(290f, yPos, BUTTON_WIDTH, 20f), "MAX"))
                {
                    AddItemToInventory(item._id, item._maxAmount);
                }
            }
            else
            {
                if (GUI.Button(new Rect(210f, yPos, 150f, 20f), "Add"))
                {
                    AddItemToInventory(item._id, 1);
                }
            }

            yPos += ITEM_HEIGHT;
        }

        private void RenderAllItemsTab()
        {
            var allItems = ItemDatabase.Items.OrderBy(i => i._name).ToList();
            float contentHeight = 60f + (allItems.Count * ITEM_HEIGHT);

            scrollPositions[5] = GUI.BeginScrollView(
                new Rect(WINDOW_X, 45f, 390f, SCROLL_HEIGHT),
                scrollPositions[5],
                new Rect(0f, 0f, 330f, contentHeight)
            );

            float yPos = 25f;

            GUI.Label(new Rect(20f, yPos, 150f, 20f), "[ All Items ]", labelStyle);
            if (GUI.Button(new Rect(100f, yPos, 70f, 20f), "Give all"))
            {
                GiveAllItems();
            }
            yPos += 35f;

            GUI.Label(new Rect(20f, yPos, 150f, 20f), "[ List Items - item number ]", labelStyle);
            yPos = 55f;

            foreach (var item in allItems)
            {
                yPos += ITEM_HEIGHT;
                string text = item._name + " [" + item._id + "]";
                GUI.Label(new Rect(20f, yPos, 150f, 20f), text, labelStyle);
            }

            GUI.EndScrollView();
        }

        private void AddItemToInventory(int itemId, int amount)
        {
            try
            {
                LocalPlayer.Inventory.AddItem(itemId, amount, false, false, null);
            }
            catch (Exception ex)
            {
                ModAPI.Console.Write($"Failed to add item {itemId}: {ex.Message}", "InventoryMod2");
            }
        }

        private void GiveAllItems()
        {
            foreach (var item in ItemDatabase.Items)
            {
                try
                {
                    if (item._maxAmount >= 0)
                    {
                        int currentAmount = LocalPlayer.Inventory.AmountOf(item._id, true);
                        int toAdd = Math.Min(1000, item._maxAmount) - currentAmount;
                        if (toAdd > 0)
                        {
                            LocalPlayer.Inventory.AddItem(item._id, toAdd, true, false, null);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void Update()
        {
            if (!sorted) Sort();

            if (ModAPI.Input.GetButtonDown("StartInventoryMenu2"))
            {
                ToggleMenu();
            }
        }

        private void ToggleMenu()
        {
            visible = !visible;

            if (visible)
                LocalPlayer.FpCharacter.LockView(true);
            else
                LocalPlayer.FpCharacter.UnLockView();
        }
    }
}