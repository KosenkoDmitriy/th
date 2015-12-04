using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    class PayTable
    {
        #region init_vars

        public int paytableRowSize = 9; //rows
        public int paytableColumnSize = 6; //cols

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

        Text[,] paytableGrid;

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
                "PAIR",
                "HIGH CARD"
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
                1,      // PAIR
            };
            //screenX = screenY = 0;
        }

        public void SetPaytableSelectedWin(int rank)
        {
            //TODO: grid/table
            /*
            SetPaytableSelectedColumn(9);//clear the grid
            int tempRank = AdjustWinRank(rank);
            tempRank = ROYAL_FLUSH - tempRank;
            if (selectedColumn > paytableColumnSize - 1)
            {
                selectedColumn = paytableColumnSize - 1;
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
            for (int row = 0; row < paytableRowSize; row++)
            {
                for (int col = 0; col < paytableColumnSize; col++)
                {
                    if (col == column)
                    {
                        paytableGrid[row, col].color = Color.red; // selected = true
                    }
                    else
                    {
                        paytableGrid[row, col].color = Color.yellow; // selected = false
                    }
                }
            }
            if (column > paytableColumnSize)
            {

            }
        }

        public void BuildVideoBonusPaytable()
        {
            if (Settings.isDebug) Debug.Log("BuildVideoBonusPaytable()");
            paytableGrid = new Text[paytableRowSize, paytableColumnSize];

            for (int j = 0; j < paytableColumnSize; j++)
            {
                for (int i = 0; i < paytableRowSize; i++)
                {
                    paytableGrid[i, j] = GameObject.Find("lblBonus" + i + "Col" + j).GetComponent<Text>();
                }
            }

            for (int i = 0; i < paytableRowSize; i++)
            {
                paytableGrid[i, 0].text = PayTableStrings[i];
                paytableGrid[i, 0].color = Color.yellow;
                //paytableGrid[i, 0].enabled = false; //hide
                for (int j = 1; j < paytableColumnSize; j++)
                {
                    paytableGrid[i, j].text = (PayTableAmounts[i] * j).ToString();
                }
            }
            //TODO: UpdateVideoBonusMaxMultiplier(5);
            SetPaytableSelectedColumn(0);
        }

        public void UpdateVideoBonusMaxMultiplier(int multiplier)
        {
            multiplier = 5;
            for (int x = 0; x < paytableRowSize; x++)
            {
                if (x == 0)
                {
                    PayTableAmounts[x] = 800;
                }
                //paytableGrid[paytableColumnSize - 1, x].Value = (PayTableAmounts[x] * multiplier).ToString();
            }
        }

        public double GetVideoPokerBonus(int rank)
        {
            rank = AdjustWinRank(rank);

            int newRank = ROYAL_FLUSH - rank;
            if (newRank < paytableRowSize)
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
            return paytableRowSize;
        }

    }
}
