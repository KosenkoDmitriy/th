using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class PayTable
    {
        #region init_vars
		public int selectedCol;
		private double bonusVideoPoker;

        public int paytableRowSize = Settings.paytableRowSize; //rows
        public int paytableColumnSize = Settings.paytableColumnSize; //cols

        public int ColsCount = Settings.ColsCount;
        public int RowsCount = Settings.RowsCount; // Entries - the number of entries you wish in the paytable

		public List<int> payTableValuesOfFirstColumn, payTableDt;
		public List<string> payTableStrings;

        Text[,] payTableGrid;
		double[,] payTableValues;

        #endregion

        public PayTable()
        {
			payTableStrings = HandCombination.names;

            payTableValuesOfFirstColumn = new List<int> {
                250,	//minBonusBetInCredits * 10		// ROYAL_FLUSH,
                50,     // STRAIGHT_FLUSH,
                30,     // FOUR_OF_A_KIND,
                25,      // FULL_HOUSE,
                20,      // FLUSH,
                15,      // STRAIGHT,
                10,      // THREE_OF_A_KIND,
                4,      // TWO_PAIR,
                2,      // PAIR
            };
			payTableDt = new List<int> {
				250,
				50,
				30,
				25,
				20,
				15,
				10,
				4,
				2
			};

			payTableGrid = new Text[paytableRowSize, paytableColumnSize];
			payTableValues = new double[paytableRowSize, paytableColumnSize];
        }

        public double GetAndSelectBonusWin(Player pWin)
        {
			this.bonusVideoPoker = 0;
			string handString = pWin.GetHandStringFromHandObj ();
			int col = selectedCol;

			// clear selected column
			for (int row = 0; row < paytableRowSize; row++) {
				payTableGrid[row, col].color = Color.black; //.Selected = false;
			}
			// select item
			for (int row = 0; row < paytableRowSize; row++) {
				if (payTableStrings[row].ToLower() == handString.ToLower()) {
					payTableGrid [row, col].color = Color.red; //.Selected = true;
					this.bonusVideoPoker = payTableValues [row, col];
				}
			}
			return this.bonusVideoPoker;
        }

        public void SelectColumnByIndex(int column)
        {
            for (int row = 0; row < paytableRowSize; row++)
            {
                for (int col = 0; col < paytableColumnSize; col++)
                {
                    if (col == column)
                    {
                        payTableGrid[row, col].color = Color.red; // .Selected = true
						selectedCol = column;
                    }
                    else
                    {
                        payTableGrid[row, col].color = Color.black; // .Selected = false
                    }
                }
            }
        }

        public void Init()
        {
            if (Settings.isDebug) Debug.Log("BuildVideoBonusPaytable()");

            for (int j = 0; j < paytableColumnSize; j++)
            {
                for (int i = 0; i < paytableRowSize; i++)
                {
                    payTableGrid[i, j] = GameObject.Find("lblBonus" + i + "Col" + j).GetComponent<Text>();
                }
            }

            for (int i = 0; i < paytableRowSize; i++)
            {
                payTableGrid[i, 0].text = payTableStrings[i];
                payTableGrid[i, 0].color = Color.black;
                //paytableGrid[i, 0].enabled = false; //hide
				double val = 0;
                for (int j = 1; j < paytableColumnSize; j++)
                {
					val = payTableValuesOfFirstColumn[i] + (payTableDt[i] * j);
                    payTableGrid[i, j].text = val.ToString();
					payTableValues[i, j] = val;
                }
            }
//            SetPaytableSelectedColumn(0);
        }

		public void SetBet (double betBonusAmount)
		{
			double betMax = betBonusAmount * Settings.betBonusMaxMultiplier * Settings.betCreditsMultiplier;
			int selectedColumn = -1;

			if (betBonusAmount > 0) {
				for (int j = 1; j <= ColsCount; j++) {
					if (payTableValues [0, j] == betMax) {
						selectedColumn = j;
						break;
					}
				}
			}

			SelectColumnByIndex (selectedColumn);
		}

    }
}
