
namespace Assets.Scripts
{
    class PayTable
    {
        #region init_vars


        public int[] adjustedRanks = new int[] {ROYAL_FLUSH,
                                                STRAIGHT_FLUSH,
                                                FOUR_OF_A_KIND,
                                                FULL_HOUSE,
                                                FLUSH,
                                                STRAIGHT,
                                                THREE_OF_A_KIND,
                                                TWO_PAIR,
                                                PAIR
                                                };


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

        public int[] PayTableAmounts = new int[] { 250, 50, 25, 9, 6, 4, 3, 2, 1 };
        public int paytableEntries = 9;

        #endregion
        public PayTable() {

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
    }
}
