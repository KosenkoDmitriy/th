using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    class PayTable : MonoBehaviour
    {
        #region init_vars
        List<variableContainer> values;
        public int paytableEntries = 9;

        public readonly int ColsInRowOfTheBonusTable = 5;
        public readonly int RowsCount = 8; // Entries - the number of entries you wish in the paytable

        public List<int> PayTableAmounts;
        public List<string> PayTableStrings;

        public int selGridInt = 0;

        Rect pos;
        float screenX, screenY;
        float width = Settings.bonusTableWidth;
        float height = Settings.bonusTableHeight;
        float offsetY = 50;
        float offsetX = 0;
        
        //TODO: remove this duplication vars, because they already exsits in the LoadOnClick class.
        const int ROYAL_FLUSH = 21;
        const int STRAIGHT_FLUSH = 20;
        const int HIGH_FOUR_OF_A_KIND = 19;
        const int MID_FOUR_OF_A_KIND = 18;
        const int FOUR_OF_A_KIND = 17;
        const int FULL_HOUSE = 16;
        const int FLUSH = 15;
        const int STRAIGHT = 14;
        const int HIGH_THREE_OF_A_KIND = 13;
        const int MID_THREE_OF_A_KIND = 12;
        const int THREE_OF_A_KIND = 11;
        const int TWO_PAIR = 10;
        const int HIGH_PAIR = 9;
        const int MID_PAIR = 8;
        const int PAIR = 7;
        const int FOUR_TO_A_FLUSH = 6;
        const int THREE_TO_A_FLUSH = 5;
        const int FOUR_TO_A_STRAIGHT_INSIDE = 4;
        const int THREE_TO_A_STRAIGHT_INSIDE = 3;
        const int FOUR_TO_A_STRAIGHT_OUTSIDE = 2;
        const int THREE_TO_A_STRAIGHT_OUTSIDE = 1;

        GUIStyle smallFont;
        GUIStyle largeFont;

        #endregion

        public PayTable()
        {
            PayTableStrings = new List<string>() {
                "ROYAL FLUSH",
                "STRAIGHT FLUSH",
                "FOUR OF A KIND",
                "FULL HOUSE",
                "FLUSH",
                "STRAIGHT",
                "THREE OF A KIND",
                "TWO PAIR",
                //"PAIR",
                //"HIGH CARD"
            };

            PayTableAmounts = new List<int> {
                250,    // ROYAL_FLUSH,
                50,     // STRAIGHT_FLUSH,
                25,     // FOUR_OF_A_KIND,
                9,      // FULL_HOUSE,
                6,      // FLUSH,
                4,      // STRAIGHT,
                3,      // THREE_OF_A_KIND,
                2,      // TWO_PAIR,
                //1,      // PAIR
            };
            //screenX = screenY = 0;
        }

        private void Start() {

        }

        public void OnGUI()
        {
            smallFont = new GUIStyle();
            largeFont = new GUIStyle();

            smallFont.fontSize = Settings.bonusTableFontSize;
            largeFont.fontSize = 20;
            smallFont.normal.textColor = Color.yellow;
            largeFont.normal.textColor = Color.white;

            screenX = ((Screen.width - offsetX) - (width));
            screenY = offsetY;// ((Screen.height) - height);
            //pos = new Rect(screenX, screenY, width - offsetX, height - offsetX);
            pos = new Rect(screenX, screenY, width, height);

            //GUIStyle style = new GUIStyle();
            //style.fontSize = 10;

            GUI.backgroundColor = Color.blue;
            GUI.contentColor = Color.yellow;
            
            //GUI.color = Color.white;
            
            // var skin = new GUISkin();
            GUILayout.BeginArea(pos);
            GUILayout.BeginVertical("Box");
            //var btn = new GUIContent();

            values = new List<variableContainer>();

            int i = 0;
            foreach (int item in PayTableAmounts)
            {
                values.Add(new variableContainer(PayTableStrings[i], item, 5));
                i++;
            }

            foreach (var item in values)
            {
                GUILayout.BeginHorizontal();
                //TODO: exceed width GUILayout.SelectionGrid(-1, item.listStr.ToArray(), 6);
                //GUILayout.Label(PayTableStrings[pos]);
                foreach (string s in item.listStr)
                {
                    GUILayout.Label(s, smallFont);
                }
                GUILayout.EndHorizontal();
            }

            /*
            selGridInt = GUILayout.SelectionGrid(selGridInt, selStrings, 1);
            
            pos = GUILayoutUtility.GetLastRect();
            if (GUILayout.Button("Start"))
                Debug.Log("You chose " + selStrings[selGridInt]);
            */

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        public void SetPaytableSelectedWin(int rank)
        {
            //TODO: grid/table
            /*
            SetPaytableSelectedColumn(9);//clear the grid
            int tempRank = AdjustWinRank(rank);
            tempRank = ROYAL_FLUSH - tempRank;
            if (selectedColumn > paytableGrid.ColumnCount - 1)
            {
                selectedColumn = paytableGrid.ColumnCount - 1;
            }
            if (rank >= videoPokerLowRank)
            {
                paytableGrid[selectedColumn, tempRank].Selected = true;
                paytableGrid[0, tempRank].Selected = true;
            }
            */
        }

        public void SetPaytableSelectedColumn(int column)
        {
            //TODO: grid/table
            /*
                    for (int row = 0; row < paytableGrid.RowCount; row++)
                    {
                        for (int col = 0; col < paytableGrid.ColumnCount; col++)
                        {
                            if (col == column)
                            {
                                paytableGrid[col, row].Selected = true;
                            }
                            else
                            {
                                paytableGrid[col, row].Selected = false;
                            }
                        }
                    }
                    if (column > paytableGrid.ColumnCount)
                    {

                    }
                    */
        }

        public void BuildVideoBonusPaytable()
        {
            //TODO:
            /*
            paytableGrid.Width = 3;
            paytableGrid.Height = 3;
            for (int w = 0; w < paytableGrid.ColumnCount; w++)
            {
                paytableGrid.Width += paytableGrid.Columns[w].Width;
                if (w == 0)
                {
                    paytableGrid.Columns[w].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }

            }

            for (int x = 0; x < paytableEntries; x++)
            {
                paytableGrid.Rows.Add();
                paytableGrid[0, x].Value = PayTableStrings[x];
                paytableGrid[0, x].Selected = false;
                paytableGrid.Height += paytableGrid.Rows[x].Height;
                for (int w = 1; w < paytableGrid.ColumnCount; w++)
                {
                    paytableGrid[w, x].Value = (PayTableAmounts[x] * w).ToString();

                }
            }
            UpdateVideoBonusMaxMultiplier(5);
            SetPaytableSelectedColumn(1);
            */
        }

        public void UpdateVideoBonusMaxMultiplier(int multiplier)
        {
            multiplier = 5;
            for (int x = 0; x < paytableEntries; x++)
            {
                if (x == 0)
                {
                    PayTableAmounts[x] = 800;
                }
                //paytableGrid[paytableGrid.ColumnCount - 1, x].Value = (PayTableAmounts[x] * multiplier).ToString();
            }
        }

        public double GetVideoPokerBonus(int rank)
        {
            rank = AdjustWinRank(rank);

            int newRank = ROYAL_FLUSH - rank;
            if (newRank < paytableEntries)
            {
                return (double)PayTableAmounts[newRank] * Settings.gameDenomination;
            }
            else
            {
                return 0;
            }
        }

        public int AdjustWinRank(int rank)
        {
            int retRank = 0;
            switch (rank)
            {
                case ROYAL_FLUSH: retRank = ROYAL_FLUSH; break;
                case STRAIGHT_FLUSH: retRank = ROYAL_FLUSH - 1; break;
                case HIGH_FOUR_OF_A_KIND:
                case MID_FOUR_OF_A_KIND:
                case FOUR_OF_A_KIND: retRank = ROYAL_FLUSH - 2; break;
                case FULL_HOUSE: retRank = ROYAL_FLUSH - 3; break;
                case FLUSH: retRank = ROYAL_FLUSH - 4; break;
                case STRAIGHT: retRank = ROYAL_FLUSH - 5; break;
                case HIGH_THREE_OF_A_KIND:
                case MID_THREE_OF_A_KIND:
                case THREE_OF_A_KIND: retRank = ROYAL_FLUSH - 6; break;
                case TWO_PAIR: retRank = ROYAL_FLUSH - 7; break;
                case HIGH_PAIR:
                case MID_PAIR:
                case PAIR: retRank = ROYAL_FLUSH - 8; break;
                default: retRank = ROYAL_FLUSH - 9; break;
            }
            return retRank;
        }

        public int GetEntriesCount() {
            return paytableEntries;
        }

        private class variableContainer : Object
        {
            public List<string> listStr = new List<string>();
            public List<int> listInt = new List<int>();

            public variableContainer(string title, int value, int size)
            {
                listStr = new List<string>();
                listInt = new List<int>();

                if (title.Length <= 7) title += "\t";
                listStr.Add(title +"\t");

                //listStr.Add(Title(title));
                listInt.Add(-1); // Title is not used for any calculation

                for (int i = 1; i <= size; i++)
                {
                    int result = i * value;
                    listInt.Add(result);
                    listStr.Add(result + "");
                }
            }

            private string Title(string title) {
                int titleMaxSize = Settings.bonusTableMaxTitleSize;
                //return title.PadRight(titleMaxSize);
                if (title.Length <= titleMaxSize)
                {
                    title = title.Insert(title.Length, GetEmptyString(titleMaxSize - title.Length));
                }
                return title;
            }

            private string GetEmptyString(int size)
            {
                string emptyString = "";
                for (int i = 0; i <= size; i++)
                    emptyString += " ";
                return emptyString;
            }

            public int Count()
            {
                return listStr.Count;
            }
        }

    }
}
