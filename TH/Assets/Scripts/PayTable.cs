using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class PayTable
    {
        #region init_vars

        public int paytableRowSize = 9; //rows
        public int paytableColumnSize = 6; //cols

        public readonly int ColsInRowOfTheBonusTable = 5;
        public readonly int RowsCount = 8; // Entries - the number of entries you wish in the paytable

        public List<int> payTableAmounts;
        public List<string> payTableStrings;

        Text[,] paytableGrid;

        #endregion

        public PayTable()
        {
			payTableStrings = HandCombination.names;

            payTableAmounts = new List<int> {
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
        }

        public void SetPaytableSelectedWin(Player pWin)
        {
			string handString = pWin.GetHandStringFromHandObj ();
			for (int row = 0; row < paytableRowSize; row++) {
				for (int col = 0; col < paytableColumnSize; col++) {
					if (payTableStrings[row] == handString) {
						if (payTableAmounts[col] * Settings.betBonusMultiplier == Settings.betBonus) {
			                paytableGrid[row, col].color = Color.red;                        //.Selected = true;
						}
					}
				}
			}
//			var handString = player.GetHandStringFromHandObj();
//			if (HandCombination.isFlush (pWin.hand)) {
//
//			}

//            SetPaytableSelectedColumn(9);//clear the grid
//            int tempRank = AdjustWinRank(rank);
//            tempRank = ROYAL_FLUSH - tempRank;
//            if (Settings.selectedColumn > paytableColumnSize - 1)
//            {
//                Settings.selectedColumn = paytableColumnSize - 1;
//            }
//            if (rank >= Settings.videoPokerLowRank)
//            {
//                paytableGrid[tempRank, Settings.selectedColumn].color = Color.red;  //.Selected = true;
//                paytableGrid[tempRank, 0].color = Color.red;                        //.Selected = true;
//            }
        }

        public void SetPaytableSelectedColumn(int column)
        {
            for (int row = 0; row < paytableRowSize; row++)
            {
                for (int col = 0; col < paytableColumnSize; col++)
                {
                    if (col == column)
                    {
                        paytableGrid[row, col].color = Color.red; // .Selected = true
                    }
                    else
                    {
                        paytableGrid[row, col].color = Color.yellow; // .Selected = false
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
                paytableGrid[i, 0].text = payTableStrings[i];
                paytableGrid[i, 0].color = Color.yellow;
                //paytableGrid[i, 0].enabled = false; //hide
                for (int j = 1; j < paytableColumnSize; j++)
                {
                    paytableGrid[i, j].text = (payTableAmounts[i] * j).ToString();
                }
            }
            //TODO: UpdateVideoBonusMaxMultiplier(5);
            SetPaytableSelectedColumn(0);
        }

//        public void UpdateVideoBonusMaxMultiplier(int multiplier)
//        {
//            multiplier = 5;
//            for (int x = 0; x < paytableRowSize; x++)
//            {
//                if (x == 0)
//                {
//                    payTableAmounts[x] = 800;
//                }
//                //paytableGrid[paytableColumnSize - 1, x].Value = (PayTableAmounts[x] * multiplier).ToString();
//            }
//        }

        public double GetVideoPokerBonus(int rank)
        {
			double res = 0;
			res = payTableAmounts [rank];
			return res;
//            rank = AdjustWinRank(rank);
//
//            int newRank = ROYAL_FLUSH - rank;
//            if (newRank < paytableRowSize)
//            {
//                return (double)payTableAmounts[newRank] * Settings.gameDenominationDx;
//            }
//            else
//            {
//                return 0;
//            }
        }


        public int GetEntriesCount() {
            return paytableRowSize;
        }

		public void SetBet (double betBonusAmount)
		{
			int selectedColumn = 1;
			SetPaytableSelectedColumn(selectedColumn);
		}
    }
}
