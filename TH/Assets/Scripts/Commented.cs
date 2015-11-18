namespace TexasHoldEmFoldUp
{
    class Commented
    {
        /*
 * 
 * */

        /* GROUP 1
         * High Four Kind
         * Nut Full House
         * Nut Flush
         * Nut Straight
         * **** GROUP 2
         * Low Full House
         * Flush
         * Straight
         * **** GROUP 3
         * Three of a kind using pocket pair
         * Three connected cards
         * **** GROUP 4
         * Top 2 pair
         * Two pair using both hole cards
         * ** GROUP 5
         * Bottom 2 pair (9s and below)
         * ***GROUP 6 
         * Overpair (pocket pair above highest card in flop)
         * *** GROUP 7
         * Top Pair Ace kicker 
         * *** GROUP 8
         * Top Pair Weak kicker
         * ***GROUP 9
         * Second pair (pocket pair between flop high and middle card)
         * ***GROUP 10
         * Middle pair weak kicker
         * *** GROUP 11
         * Third Pair (pocket pair below flop 2nd card)
         * ****GROUP 12
         * Low Pair Ace kicker
         * ****GROUP 13
         * Low Pair low kicker
         ***** GROUP 14
         * Nut Draw with 9 plus outs
         * ****GROUP 15
         * Draw with 9 or more outs
         * *** GROUP 16
         * Draws wiht 8 or less outs
         * *** GROUP 17
         * Overcards (AK,AQ,KQ,AJ)
         * 
         * 
         * 
         * 

         */
        //gameTimer.Interval = 1000;
        //gameTimer.Tick += new EventHandler(gameTimer_Tick);

        //gameStartTimer.Interval = 1000;
        //gameStartTimer.Enabled = false;
        //gameStartTimer.Tick += new EventHandler(gameStartTimer_Tick);
        //for (int player = 0; player < 6; player++)
        //{
        //    if (virtualPlayers[player].Folded == false)
        //    {
        //        temp = GetFiveCardRanking(player);
        //        if (temp > rank)
        //        {
        //            rank = temp;
        //            winner = player;
        //        }
        //        virtualPlayers[player].FinalHandRank = temp;
        //        GamePlayers[player].hand.HandRank = temp;
        //    }
        //}
        //int highestHandTotal = 0;
        //for (int i = 0; i < 6; i++)//get how many ties
        //{
        //    if (GamePlayers[i].hand.HandRank == rank && virtualPlayers[i].Folded == false)
        //    {
        //        if (GamePlayers[i].hand.CardValueTotal > highestHandTotal)
        //        {
        //            highestHandTotal = GamePlayers[i].hand.CardValueTotal;//we have the highest hand total
        //        }
        //        WinnerCount++;
        //        ties[i] = true;
        //    }
        //}

        //if (WinnerCount > 1)//we need to break the tie
        //{
        //    WinnerCount = 0;//reset this to check for more ties
        //    int highHolder = 0;
        //    int highCard = 0;
        //    int highCount = 0;
        //    int cardTotal = 0;
        //    int highKicker = 0;
        //    for (int x = 0; x < 6; x++)
        //    {
        //        if (ties[x] == true)
        //        {
        //            if (GamePlayers[x].hand.CardValueTotal == highestHandTotal)
        //            {
        //                if (GamePlayers[x].hand.CardValueTotal > cardTotal)
        //                {
        //                    cardTotal = GamePlayers[x].hand.CardValueTotal;
        //                    highHolder = x;
        //                }
        //                WinnerCount++;
        //            }
        //            else
        //            {
        //                if (GamePlayers[x].hand.CardValueTotal != highestHandTotal)
        //                {
        //                    ties[x] = false; //get rid of the lower ties
        //                }
        //            }
        //        }
        //    }
        //    if (WinnerCount > 1)// more ties 
        //    {
        //        int kicker = 0;
        //        int WinnerCount2 = 0;
        //        for (int x = 0; x < 6; x++)
        //        {
        //            if (ties[x] == true)
        //            {
        //                if (rank == PAIR || rank == THREE_OF_A_KIND || rank == FOUR_OF_A_KIND)
        //                {
        //                    kicker = GamePlayers[x].hand.XofaKindKicker;
        //                }
        //                else
        //                {
        //                    kicker = GetKicker(GamePlayers[x].hand.cardHand, GamePlayers[x].winCards);
        //                }
        //                if (kicker > highKicker)
        //                {
        //                    highKicker = kicker;
        //                    highHolder = x;
        //                }
        //                ties[x] = false;
        //            }
        //        }
        //        for (int x = 0; x < 6; x++)//one last time 
        //        {
        //            if (rank == PAIR || rank == THREE_OF_A_KIND || rank == FOUR_OF_A_KIND)
        //            {
        //                if (highKicker == GamePlayers[x].hand.XofaKindKicker)
        //                {
        //                    WinnerCount2++;
        //                    ties[x] = true;
        //                }
        //            }
        //            else
        //            {
        //                if (highKicker == GetKicker(GamePlayers[x].hand.cardHand, GamePlayers[x].winCards))
        //                {
        //                    WinnerCount2++;
        //                    ties[x] = true;
        //                }
        //            }

        //        }
        //        if (WinnerCount2 > 1)//finally we pick a winner
        //        {
        //            highHolder = 10;//which means we are going go have to split the pot
        //        }


        //    }
        //    winner = highHolder;
        //}
        //return winner;

        //private void ContinueBetting()
        //{
        //    bool finished = false;
        //    do
        //    {
        //        if (CheckForBetFinish() == true)
        //        {
        //            finished = true;
        //            //return;
        //        }
        //        else
        //        if (CurrentBetPosition != 0)
        //        {
        //            int Amount = BetPlayer(CurrentBetPosition);
        //            CurrentBetPosition++;
        //        }

        //    } 
        //    while (CurrentBetPosition != 0 && finished == false);
        //    if (finished == true)
        //    {
        //        finishThisRoundBetting();
        //    }
        //    else
        //    {
        //        //CurrentBetPosition++;
        //    }
        //    gameTimer.Start();
        //}

        //if (temp >= STRAIGHT && temp != FOUR_OF_A_KIND)
        //{
        //    tempTotal = GetFiveCardTotal(playerHand);
        //    setPlayerWinCards(player, playerHand);
        //    int tempKicker = GetKicker(GamePlayers[player].hand.cardHand, GamePlayers[player].winCards);


        //    if (temp > highHand || (temp == highHand && tempTotal > highTotal) || (temp == highHand && tempTotal == highTotal && tempKicker > highKicker))
        //    {
        //        highTotal = tempTotal;
        //        highHand = temp;
        //        highKicker = tempKicker;
        //        highCombo = x;
        //        GamePlayers[player].hand.CardValueTotal = highTotal;//now we have the total to break a tie
        //        GamePlayers[player].hand.Kicker = highKicker;
        //        GamePlayers[player].hand.HandRank = temp;
        //    }

        //}
        //if (temp == PAIR || temp == THREE_OF_A_KIND || temp == FOUR_OF_A_KIND)
        //{
        //    tempTotal = GetXofaKindTotal(GamePlayers[player].hand.XofaKindValue, playerHand);
        //    setPlayerWinCards(player, playerHand);
        //    int tempKicker = GetXofaKindKicker(GamePlayers[player].hand.XofaKindValue, GamePlayers[player].hand.cardHand);

        //    if (temp > highHand || (temp == highHand && tempTotal > highTotal) || (temp == highHand && tempTotal == highTotal && tempKicker > highKicker))
        //    {
        //        highTotal = tempTotal;
        //        highHand = temp;
        //        highKicker = tempKicker;
        //        highCombo = x;
        //        GamePlayers[player].hand.CardValueTotal = highTotal;//now we have the total to break a tie
        //        GamePlayers[player].hand.Kicker = highKicker;
        //        GamePlayers[player].hand.HandRank = temp;
        //    }
        //}


        //if (temp >= STRAIGHT && temp != FOUR_OF_A_KIND)//only 5 card hands
        //{
        //    if (GetFiveCardTotal(playerHand) > highTotal)
        //    {
        //        highTotal = GetFiveCardTotal(playerHand);
        //        GamePlayers[player].hand.CardValueTotal = highTotal;//now we have the total to break a tie
        //        setPlayerWinCards(player, playerHand);//and record which cards we used for non five card hands 
        //        highKicker = GetKicker(GamePlayers[player].hand.cardHand, GamePlayers[player].winCards);
        //    }

        //}
        //else
        //{
        //    if (temp == PAIR || temp == THREE_OF_A_KIND || temp == FOUR_OF_A_KIND)
        //    {
        //        if (GetXofaKindTotal(GamePlayers[player].hand.XofaKindValue, playerHand) > highTotal)
        //        {
        //            highTotal = GetXofaKindTotal(GamePlayers[player].hand.XofaKindValue, playerHand);
        //            GamePlayers[player].hand.CardValueTotal = highTotal;
        //            GamePlayers[player].hand.XofaKindKicker = GetXofaKindKicker(GamePlayers[player].hand.XofaKindValue, GamePlayers[player].hand.cardHand);
        //        }
        //    }
        //    else
        //    {
        //        if (temp == TWO_PAIR)
        //        {

        //        }
        //    }
        //}

        //void secondRoundTimer_Tick(object sender, EventArgs e)
        //{
        //    GameState = GameStates.SecondRoundBet;
        //    clearBetLabels();
        //    CallAmount = 0;
        //    secondRoundTimer.Stop();
        //    dealFlop();
        //    BetPlayers(buttonPosition + 1, 5);
        //    playerRaiseButton.Visible = true;
        //    playerCallButton.Visible = true;
        //    if (CallAmount == 0)
        //    {
        //        playerCheckButton.Visible = true;
        //    }

        //}

        //void gameTimer_Tick(object sender, EventArgs e)
        //{

        //    if (checkForPlayerWin() == true)
        //    {
        //        GameState = GameStates.PlayerWin;
        //        updateBettingButtonTitle();
        //        bettingGroupBox.Visible = true;//show the betting buttons
        //        playerRaiseButton.Visible = false;//we can always raise
        //        playerAllInButton.Visible = false;//we can alway go all in
        //        playerFoldButton.Visible = false;//we can always fold
        //        PlayerCredits += PotAmount;
        //        PotAmount = 0;
        //        return;
        //    }
        //    gameTimer.Stop();
        //    if(GetCurrentBet() == virtualPlayers[0].CurrentBetAmount)
        //    {
        //        if (GetCurrentBet() > 0)
        //        {
        //            RoundFinished = true;
        //            finishThisRoundBetting();
        //            return;
        //        }

        //    }
        //    if (RoundFinished == true)
        //    {
        //        DealNextRound();
        //        RoundFinished = false;
        //        StartBetting(); 
        //    }

        //    if (GameState != GameStates.EndGame)
        //    {

        //        //BetPlayers(buttonPosition + 1, 5);

        //        //StartBetting();
        //        CallAmount = GetCurrentBet() - virtualPlayers[0].CurrentBetAmount;
        //        //int temp = CallAmount - virtualPlayers[0].RoundRaiseAmount;
        //        //callLabel.Text = temp.ToString();
        //        updateBettingButtonTitle();
        //        bettingGroupBox.Visible = true;//show the betting buttons
        //        playerRaiseButton.Enabled = true;//we can always raise
        //        playerAllInButton.Enabled = true;//we can alway go all in
        //        playerFoldButton.Enabled = true;//we can always fold

        //        if (CallAmount == 0)
        //        {
        //            playerCheckButton.Enabled = true;
        //            playerCallButton.Enabled = false;
        //        }
        //        else
        //        {
        //            playerCheckButton.Enabled = false;
        //            playerCallButton.Enabled = true;
        //        }
        //    }
        //    else
        //    {
        //        for (int x = 1; x < 6; x++ )
        //        {
        //            if (virtualPlayers[x].Folded)
        //            {
        //                ClearPlayerCards(x);
        //            }
        //            else
        //            {
        //                if (showdown)
        //                {
        //                    ShowPlayerCards(x);
        //                }
        //            }
        //        }
        //    }
        //}

        //void gameStartTimer_Tick(object sender, EventArgs e)
        //{
        //    GameState = GameStates.FirstRoundBet;
        //    gameStartTimer.Stop();
        //    CallAmount = 0;
        //    StartNewGame();
        //    gameStartTimer.Enabled = false;
        //}
        //System.Windows.Forms.Timer thirdRoundTimer = new System.Windows.Forms.Timer();
        //System.Windows.Forms.Timer fourthRoundTimer = new System.Windows.Forms.Timer();
        //System.Windows.Forms.Timer gameTimer = new System.Windows.Forms.Timer();


        //if (virtualPlayers[player].TwoCardBet != 0 || virtualPlayers[player].RoundChecked != false)//is this the first time through
        //{//this is not the initial round
        //    if (virtualPlayers[player].RoundRaiseAmount > 0)//we raised previously
        //    {
        //        if (realPlayerPotRaisePercentage >= 25)
        //        {
        //            BetType = BetTypes.raising;
        //            raise = RoundUp(PotAmount * (0.5));
        //        }
        //    }

        //}
        //else 

        ////////if (GameState == GameStates.HoldCardBet)//set up the betting rules
        ////////{

        ////////    if (virtualPlayers[player].TwoCardBet == 0 && virtualPlayers[player].RoundChecked == false)// have we bet here before?
        ////////    {//no
        ////////        //BetType = BetTypes.calling;//for testing 

        ////////        if (rank <= virtualPlayers[player].MinimumHoleRaiseThreshold)//our best hand"
        ////////        {
        ////////            if (limp == true)//put in the minimum we can?
        ////////            {
        ////////                if (ThisPlayersCall == 0)
        ////////                {
        ////////                    BetType = BetTypes.checking;
        ////////                }
        ////////                else
        ////////                {
        ////////                    BetType = BetTypes.calling;
        ////////                }
        ////////            }
        ////////            else//we are going for it
        ////////            {
        ////////                BetType = BetTypes.raising;
        ////////                bluff = false;//we are serious
        ////////            }
        ////////        }
        ////////        else
        ////////        {
        ////////            if (rank <= virtualPlayers[player].HoleMinThreshold)
        ////////            {
        ////////                {
        ////////                    BetType = BetTypes.calling;
        ////////                }
        ////////            }
        ////////            else//we are below our minimum
        ////////            {
        ////////                if (ThisPlayersCall == 0)
        ////////                {
        ////////                    BetType = BetTypes.checking;//always take a freebe
        ////////                }
        ////////                else
        ////////                {
        ////////                    BetType = BetTypes.folding;//just bail
        ////////                }
        ////////                if (bluff)//we only bluff on a lousy hand
        ////////                {
        ////////                    if (getWeightedResult(virtualPlayers[player].BluffCallRaisePercentage) == false)
        ////////                    {
        ////////                        BetType = BetTypes.raising;//lets just give it a shot
        ////////                    }
        ////////                    else
        ////////                    {
        ////////                        BetType = BetTypes.calling;//
        ////////                    }
        ////////                }
        ////////            }
        ////////        }
        ////////    }
        ////////    else
        ////////    {//yes we have been here before

        ////////        BetType = BetTypes.calling;//just call for now to get the game working EXPAND THIS
        ////////    }
        ////////    if (virtualPlayers[0].AllIn == true)
        ////////    {
        ////////        if (BetType != BetTypes.folding)//we are folding for a reason
        ////////        {
        ////////            if (ThisPlayersCall == 0)
        ////////            {
        ////////                BetType = BetTypes.checking;
        ////////            }
        ////////            else
        ////////            {
        ////////                // fix this to check for credit limits and or go all in 
        ////////                BetType = BetTypes.calling;
        ////////            }
        ////////        }
        ////////    }

        ////////}

        //cardValues[,] Group1 = new cardValues[,] {
        //                                            {cardValues.A, cardValues.A, cardValues.US, cardValues.G1},
        //                                            {cardValues.K, cardValues.K, cardValues.US, cardValues.G1},
        //                                            {cardValues.Q, cardValues.Q, cardValues.US, cardValues.G1},
        //                                            {cardValues.J, cardValues.J, cardValues.US, cardValues.G1},
        //                                            {cardValues.A, cardValues.K, cardValues.S, cardValues.G1}};//must be suited

        //cardValues[,] Group2 = new cardValues[,] {
        //                                            {cardValues.T, cardValues.T, cardValues.US, cardValues.G2},
        //                                            {cardValues.A, cardValues.Q, cardValues.S, cardValues.G2},
        //                                            {cardValues.A, cardValues.J, cardValues.S, cardValues.G2},
        //                                            {cardValues.K, cardValues.Q, cardValues.S, cardValues.G2},
        //                                            {cardValues.A, cardValues.K, cardValues.US, cardValues.G2}};//unsuited

        //cardValues[,] Group3 = new cardValues[,] {
        //                                            {cardValues._9, cardValues._9, cardValues.US, cardValues.G3},
        //                                            {cardValues.J, cardValues.T, cardValues.S, cardValues.G3},
        //                                            {cardValues.Q, cardValues.J, cardValues.S, cardValues.G3},
        //                                            {cardValues.K, cardValues.J, cardValues.S, cardValues.G3},
        //                                            {cardValues.A, cardValues.T, cardValues.S, cardValues.G3},
        //                                            {cardValues.A, cardValues.Q, cardValues.US, cardValues.G3},
        //                                            };

        //cardValues[,] Group4 = new cardValues[,] { 
        //                                            {cardValues.T, cardValues._9, cardValues.S, cardValues.G4},
        //                                            {cardValues.K, cardValues.Q, cardValues.US, cardValues.G4},
        //                                            {cardValues._8, cardValues._8, cardValues.US, cardValues.G4},
        //                                            {cardValues.Q, cardValues.T, cardValues.S, cardValues.G4},
        //                                            {cardValues._9, cardValues._8, cardValues.S, cardValues.G4},
        //                                            {cardValues.J, cardValues._9, cardValues.S, cardValues.G4},
        //                                            {cardValues.A, cardValues.J, cardValues.US, cardValues.G4},
        //                                            {cardValues.K, cardValues.T, cardValues.S, cardValues.G4},
        //                                            };

        //cardValues[,] Group5 = new cardValues[,] { 
        //                                            {cardValues._7, cardValues._7, cardValues.US, cardValues.G5},
        //                                            {cardValues._8, cardValues._7, cardValues.S, cardValues.G5},
        //                                            {cardValues.Q, cardValues._9, cardValues.S, cardValues.G5},
        //                                            {cardValues.T, cardValues._8, cardValues.S, cardValues.G5},
        //                                            {cardValues.K, cardValues.J, cardValues.US, cardValues.G5},
        //                                            {cardValues.Q, cardValues.J, cardValues.US, cardValues.G5},
        //                                            {cardValues.J, cardValues.T, cardValues.US, cardValues.G5},
        //                                            {cardValues._7, cardValues._6, cardValues.S, cardValues.G5},
        //                                            {cardValues._9, cardValues._7, cardValues.S, cardValues.G5},
        //                                            {cardValues.A, cardValues.ANY, cardValues.S, cardValues.G5},
        //                                            {cardValues._6, cardValues._5, cardValues.S, cardValues.G5},

        //                                            };

        //cardValues[,] Group6 = new cardValues[,] { 
        //                                            {cardValues._6, cardValues._6, cardValues.US, cardValues.G6},
        //                                            {cardValues.A, cardValues.T, cardValues.US, cardValues.G6},
        //                                            {cardValues._5, cardValues._5, cardValues.US, cardValues.G6},
        //                                            {cardValues._8, cardValues._6, cardValues.S, cardValues.G6},
        //                                            {cardValues.K, cardValues.T, cardValues.US, cardValues.G6},
        //                                            {cardValues.Q, cardValues.T, cardValues.US, cardValues.G6},
        //                                            {cardValues._5, cardValues._4, cardValues.S, cardValues.G6},
        //                                            {cardValues.K, cardValues._9, cardValues.S, cardValues.G6},
        //                                            {cardValues.J, cardValues._8, cardValues.S, cardValues.G6},
        //                                            {cardValues._7, cardValues._5, cardValues.S, cardValues.G6}};

        //cardValues[,] Group7 = new cardValues[,] { 
        //                                            {cardValues._4, cardValues._4, cardValues.US },
        //                                            {cardValues.J, cardValues._9, cardValues.US },
        //                                            {cardValues._6, cardValues._4, cardValues.S },
        //                                            {cardValues.T, cardValues._9, cardValues.US },
        //                                            {cardValues._5, cardValues._3, cardValues.S },
        //                                            {cardValues._3, cardValues._3, cardValues.US },
        //                                            {cardValues._9, cardValues._8, cardValues.US },
        //                                            {cardValues._4, cardValues._3, cardValues.S },
        //                                            {cardValues._2, cardValues._2, cardValues.US },
        //                                            {cardValues.K, cardValues.ANY, cardValues.S },
        //                                            {cardValues.T, cardValues._7, cardValues.S },
        //                                            {cardValues.Q, cardValues._8, cardValues.S }};

        //cardValues[,] Group8 = new cardValues[,] { 
        //                                            {cardValues._8, cardValues._7, cardValues.US },
        //                                            {cardValues.A, cardValues._9, cardValues.US },
        //                                            {cardValues.Q, cardValues._9, cardValues.US },
        //                                            {cardValues._7, cardValues._6, cardValues.US },
        //                                            {cardValues._4, cardValues._2, cardValues.S },
        //                                            {cardValues._3, cardValues._2, cardValues.S },
        //                                            {cardValues._9, cardValues._6, cardValues.S },
        //                                            {cardValues._8, cardValues._5, cardValues.S },
        //                                            {cardValues.J, cardValues._8, cardValues.US },
        //                                            {cardValues.J, cardValues._7, cardValues.S },
        //                                            {cardValues._6, cardValues._5, cardValues.US },
        //                                            {cardValues._5, cardValues._4, cardValues.US },
        //                                            {cardValues._7, cardValues._4, cardValues.S },
        //                                            {cardValues.K, cardValues._9, cardValues.US },
        //                                            {cardValues.T, cardValues._8, cardValues.US }};

        //public void BuildVirtualPlayerProfiles()
        //        {
        //            int i = 1;
        //            bool done = false;
        //            string temp;
        //            int test;
        //            int test1;

        //            string holeRaiseHands = "                                               ";
        //            string Player = "                                                        ";
        //            //string name = new string(' ',20);
        //            string[] stringArray = new string[20];
        //            virtualPlayers[0] = new VirtualPlayer();//create a virtual player for the actual player
        //            string currentDirectory = Directory.GetCurrentDirectory();
        //            string fileName = Directory.GetCurrentDirectory() + "\\TexasHoldem.ini";
        //            do
        //            {
        //                int index = 0;
        //                Player = "Player" + i.ToString();
        //                //test to see if there is anything in the player if not we are done. 
        //                int charsTransferred;// = Win32Support.GetPrivateProfileString(Player, "Hole Min Threshold", null, temp, 5, currentDirectory + "\\TexasHoldem.ini");
        //                string iniTest = GetIniString(Player, "Hole Min Threshold", null, out charsTransferred, currentDirectory + "\\TexasHoldem.ini");
        //                if (charsTransferred == 0)
        //                {
        //                    done = true;
        //                }
        //                else
        //                {
        //                    virtualPlayerCount++;
        //                    try
        //                    {
        //                        virtualTempPlayers[i] = new VirtualPlayer();
        //                        for (int x = 0; x < 6; x++)//lets get the raise parameters
        //                        {
        //                            virtualTempPlayers[i].RaiseLevels[x] = new RaiseLevel();
        //                        }
        //                        for (int x = 0; x < 8; x++)//get the fold stuff
        //                        {
        //                            virtualTempPlayers[i].FoldLevels[x] = new FoldLevel();
        //                        }
        //                        int testchars;
        //                        virtualTempPlayers[i].Name = GetIniString(Player, "Player Name", "Player " + i.ToString(), out testchars, fileName);
        //                        virtualTempPlayers[i].FoldOnAnyRaise = GetIniBool(Player, "Fold On Any Raise", false, currentDirectory + "\\TexasHoldem.ini");
        //                        //string value;
        //                        virtualTempPlayers[i].HoleMinThreshold = GetIniInt(Player, "Hole Min Threshold", 72, fileName);
        //                        for (int x = 0; x < 6; x++)//lets get the raise parameters
        //                        {
        //                            test1 = x;
        //                            string raiseHand = "Hole Raise " + (x + 1).ToString() + " Hand Array";
        //                            virtualTempPlayers[i].RaiseLevels[x].RaiseHands = GetINIIntArray(Player, raiseHand,1, fileName);
        //                            //value = GetIniString(Player, raiseHand, "1,2,3,4,5", out testchars, fileName);
        //                            //stringArray = value.Split(',');//now go get the comma delimited strings
        //                            //index = 0;
        //                            //virtualTempPlayers[i].RaiseLevels[x].RaiseHands = new int[stringArray.GetLength(0)];
        //                            //foreach (string tstring in stringArray)
        //                            //{
        //                            //    virtualTempPlayers[i].RaiseLevels[x].RaiseHands[index] = int.Parse(tstring);
        //                            //    index++;
        //                            //}
        //                            string holeRaiseRange = "Hole Raise " + (x + 1).ToString() + " Range";
        //                            virtualTempPlayers[i].RaiseLevels[x].Range = GetINIDoubleArray(Player, holeRaiseRange,2, fileName);
        //                            //value = GetIniString(Player, holeRaiseRange, "0,30", out testchars, fileName);
        //                            //stringArray = value.Split(',');//now go get the comma delimited strings
        //                            //index = 0;
        //                            //foreach (string tstring in stringArray)
        //                            //{
        //                            //    virtualTempPlayers[i].RaiseLevels[x].Range[index] = double.Parse(tstring);
        //                            //    index++;
        //                            //}
        //                            virtualTempPlayers[i].RaiseLevels[x].RaisePercentage = GetIniInt(Player, "Hole Raise " + (x + 1).ToString() + " Percentage", 50, fileName);

        //                            string holeReRaiseRange = "Hole Raise " + (x + 1).ToString() + " ReRaise Range";
        //                            virtualTempPlayers[i].RaiseLevels[x].ReRaiseRange = GetINIDoubleArray(Player, holeReRaiseRange, 2, fileName);
        //                            //value = GetIniString(Player, holeReRaiseRange, "0,30", out testchars, fileName);
        //                            //stringArray = value.Split(',');//now go get the comma delimited strings
        //                            //index = 0;
        //                            //foreach (string tstring in stringArray)
        //                            //{
        //                            //    virtualTempPlayers[i].RaiseLevels[x].ReRaiseRange[index] = double.Parse(tstring);
        //                            //    index++;
        //                            //}

        //                            virtualTempPlayers[i].RaiseLevels[x].ReRaisePercentage = GetIniInt(Player, "Hole Raise " + (x + 1).ToString() + " ReRaise Percentage", 50, fileName);

        //                        }

        //                        for (int x = 0; x < 8; x++)//get the fold stuff
        //                        {
        //                            test = x;
        //                            string holeFoldHands = "Hole Fold " + (x + 1).ToString() + " Hand Array";
        //                            virtualTempPlayers[i].FoldLevels[x].FoldHands = GetINIIntArray(Player, holeFoldHands, 1, fileName);
        //                            //value = GetIniString(Player, holeFoldHands, "1,2,3", out testchars, fileName);
        //                            //stringArray = value.Split(',');//now go get the comma delimited strings
        //                            //index = 0;
        //                            //virtualTempPlayers[i].FoldLevels[x].FoldHands = new int[stringArray.GetLength(0)];
        //                            //foreach (string tstring in stringArray)
        //                            //{
        //                            //    virtualTempPlayers[i].FoldLevels[x].FoldHands[index] = int.Parse(tstring);
        //                            //    index++;
        //                            //}
        //                            virtualTempPlayers[i].FoldLevels[x].Range = GetINIDoubleArray(Player, "Hole Fold " + (x + 1).ToString() + " Range", 2, fileName);
        //                            //value = GetIniString(Player, "Hole Fold " + (x + 1).ToString() + " Range", "1,50", out testchars, fileName);
        //                            //stringArray = value.Split(',');//now go get the comma delimited strings
        //                            //index = 0;
        //                            //foreach (string tstring in stringArray)
        //                            //{
        //                            //    virtualTempPlayers[i].FoldLevels[x].Range[index] = double.Parse(tstring);
        //                            //    index++;
        //                            //}
        //                        }

        //                        virtualTempPlayers[i].BluffHands = GetINIIntArray(Player, "Bluff Hands", 1, fileName);
        //                        //value = GetIniString(Player, "Bluff Hands", "15,16,25,34", out testchars, fileName);
        //                        //stringArray = value.Split(',');//now go get the comma delimited strings
        //                        //index = 0;
        //                        //foreach (string tstring in stringArray)
        //                        //{
        //                        //    virtualTempPlayers[i].BluffHands[index] = int.Parse(tstring);
        //                        //    index++;
        //                        //}
        //                        virtualTempPlayers[i].SlowPlayHands = GetINIIntArray(Player, "Slow Play Hands", 1, fileName);
        //                        //value = GetIniString(Player, "Slow Play Hands", "15,16,25,34", out testchars, fileName);
        //                        //stringArray = value.Split(',');//now go get the comma delimited strings
        //                        //index = 0;
        //                        //foreach (string tstring in stringArray)
        //                        //{
        //                        //    virtualTempPlayers[i].SlowPlayHands[index] = int.Parse(tstring);
        //                        //    index++;
        //                        //}
        //                        virtualTempPlayers[i].AllInHands = GetINIIntArray(Player, "Hole All In Hands", 1, fileName);
        //                        //value = GetIniString(Player, "Hole All In Hands", "15,16,25,34", out testchars, fileName);
        //                        //stringArray = value.Split(',');//now go get the comma delimited strings
        //                        //index = 0;
        //                        //foreach (string tstring in stringArray)
        //                        //{
        //                        //    virtualTempPlayers[i].AllInHands[index] = int.Parse(tstring);
        //                        //    index++;
        //                        //}
        //                        virtualTempPlayers[i].BluffPercentage = GetIniInt(Player, "Bluff Percentage", 0, fileName);
        //                        virtualTempPlayers[i].BluffCallRaisePercentage = GetIniInt(Player, "Bluff Call Raise Percentage", 50, fileName);
        //                        virtualTempPlayers[i].Folded = false;
        //                    }
        //                    catch (FormatException e)
        //                    {
        //                        MessageBox.Show(e.Message, "INI FILE Error");
        //                        string ex = e.Message;
        //                    }
        //                    i++;
        //                }
        //            } while (done == false);
        //        }

        //if (fcRank >= STRAIGHT_FLUSH)
        //{
        //    BetType = BetTypes.allIn;
        //}
        //else
        //    if(fcRank >= THREE_OF_A_KIND)
        //    {
        //        BetType = BetTypes.raising;
        //        if (GamePlayers[player].hand.XofaKindValue > 9)
        //        {
        //            raise = RoundUp(PotAmount / 5.0);
        //        }
        //        else
        //        {
        //            raise = RoundUp(PotAmount / 10.0);
        //        }
        //        if (GamePlayers[player].RoundRaiseCount > 2)
        //        {
        //            raise = 0;
        //            BetType = BetTypes.calling;
        //        }
        //    }
        //else
        //    if(fcRank >= PAIR)
        //    {
        //        BetType = BetTypes.calling;
        //    }   
        //else
        //    if (fcRank < PAIR)
        //    {
        //        BetType = BetTypes.calling;//
        //    }   
        //if (fcRank == 0)
        //{
        //    if (ThisPlayersCall == 0)
        //    {
        //        BetType = BetTypes.checking;
        //    }
        //    else
        //    {
        //        BetType = BetTypes.folding;
        //        if (virtualPlayers[player].TwoCardBet > 0)
        //        {
        //            if (virtualPlayers[player].Bluffing == true)
        //            {
        //                BetType = BetTypes.raising;
        //            }
        //            else
        //            {
        //                BetType = BetTypes.calling;
        //            }
        //        }

        //    }
        //}
        //if (virtualPlayers[0].AllIn == true)
        //{
        //    if (ThisPlayersCall == 0)
        //    {
        //        BetType = BetTypes.checking;
        //    }
        //    else
        //    {
        //        // fix this to check for credit limits and or go all in 
        //        BetType = BetTypes.calling;
        //    }
        //}

        //////////if (GameState == GameStates.TurnBet)
        //////////{
        //////////    int fcRank = virtualPlayers[player].FiveCardHandRank;
        //////////    if (fcRank >= PAIR)
        //////////    {
        //////////        if (GetCurrentBet() < 3.00)
        //////////            BetType = BetTypes.raising;
        //////////        else
        //////////            BetType = BetTypes.calling;
        //////////    }
        //////////    if (fcRank < PAIR)
        //////////    {
        //////////        BetType = BetTypes.calling;//
        //////////    }
        //////////    if (fcRank == 0)
        //////////    {
        //////////        if (ThisPlayersCall == 0)
        //////////        {
        //////////            BetType = BetTypes.checking;
        //////////        }
        //////////        else
        //////////        {
        //////////            BetType = BetTypes.folding;
        //////////            if (virtualPlayers[player].TwoCardBet > 0)
        //////////            {
        //////////                if (virtualPlayers[player].Bluffing == true)
        //////////                {
        //////////                    BetType = BetTypes.raising;
        //////////                }
        //////////                else
        //////////                {
        //////////                    BetType = BetTypes.calling;
        //////////                }
        //////////            }

        //////////        }
        //////////    }
        //////////    if (virtualPlayers[0].AllIn == true)
        //////////    {
        //////////        if (ThisPlayersCall == 0)
        //////////        {
        //////////            BetType = BetTypes.checking;
        //////////        }
        //////////        else
        //////////        {
        //////////            //fix this to check for credit limits and or go all in 
        //////////            BetType = BetTypes.calling;
        //////////        }
        //////////    }
        //////////    textBox1.AppendText("Player " + player.ToString() + " Third Round Rank = " + fcRank.ToString() + Environment.NewLine);

        //////////}

        //////////if (GameState == GameStates.RiverBet)
        //////////{
        //////////    int fcRank = virtualPlayers[player].FiveCardHandRank;
        //////////    if (fcRank >= PAIR)
        //////////    {
        //////////        if (GetCurrentBet() < 3.00)
        //////////            BetType = BetTypes.raising;
        //////////        else
        //////////            BetType = BetTypes.calling;
        //////////    }
        //////////    if (fcRank < PAIR)
        //////////    {
        //////////        if (playerBet + playerRaise > PotAmount / GetNotFoldedPlayerCount())
        //////////        {
        //////////            BetType = BetTypes.folding;
        //////////        }
        //////////        else
        //////////        {
        //////////            BetType = BetTypes.calling;//
        //////////        }
        //////////    }
        //////////    if (fcRank == 0)
        //////////    {
        //////////        if (ThisPlayersCall == 0)
        //////////        {
        //////////            BetType = BetTypes.checking;
        //////////        }
        //////////        else
        //////////        {
        //////////            BetType = BetTypes.folding;
        //////////            if (virtualPlayers[player].TwoCardBet > 0)
        //////////            {
        //////////                if (virtualPlayers[player].Bluffing == true)
        //////////                {
        //////////                    BetType = BetTypes.raising;
        //////////                }
        //////////                else
        //////////                {
        //////////                    BetType = BetTypes.calling;
        //////////                }
        //////////            }

        //////////        }
        //////////    }
        //////////    if (virtualPlayers[0].AllIn == true)
        //////////    {
        //////////        if (ThisPlayersCall == 0)
        //////////        {
        //////////            BetType = BetTypes.checking;
        //////////        }
        //////////        else
        //////////        {
        //////////            //TED fix this to check for credit limits and or go all in 
        //////////            BetType = BetTypes.calling;
        //////////        }
        //////////    }
        //////////    textBox1.AppendText("Player " + player.ToString() + " Forth Round Rank = " + fcRank.ToString() + '\n');
        //////////}

        ////////TURN
        //////if (GameState == GameStates.TurnBet)//the turn
        //////{
        //////    bool folding = false;
        //////    int fcRank = virtualPlayers[player].FiveCardHandRank;
        //////    int potRaisePercentage = GetPercentPotRaised(player);
        //////    potRaisePercentage = ThisRoundRaisePercentage;
        //////    int[] hand = GamePlayers[player].hand.cardHand;

        //////    // turn possibilities #1
        //////    if (potRaisePercentage == 0)//the pot has not been raised
        //////    {
        //////        double tempRaise = 0;
        //////        int tempRank = fcRank;
        //////        //double tempRaise = virtualPlayers[player].FlopNoRaiseBetPercentages[20 - fcRank] * .01;
        //////        if (fcRank == HIGH_PAIR || fcRank == MID_PAIR || fcRank == PAIR)//adjust the pairs
        //////        {

        //////            switch (GetPairType(hand))
        //////            {
        //////                case PairTypes.Bottom: tempRank = 6; break;
        //////                case PairTypes.Middle: tempRank = 7; break;
        //////                case PairTypes.Top: tempRank = 8; break;
        //////                case PairTypes.Pocket: tempRank = 9; break;
        //////            }
        //////        }
        //////        if (fcRank < PAIR && fcRank > 0)
        //////        {
        //////            tempRank--; ;
        //////        }
        //////        tempRaise = virtualPlayers[player].TurnNoRaiseBetPercentages[21 - tempRank] * .01;


        //////        if (tempRaise > 0)
        //////        {
        //////            BetType = BetTypes.raising;
        //////            raise = RoundUp(PotAmount * tempRaise);
        //////        }
        //////        if (tempRaise < 0)
        //////        {
        //////            folding = true;
        //////        }
        //////    }

        //////    //turn possibilities #2
        //////    if (/*virtualPlayers[0].RoundRaiseAmount*/ potRaisePercentage > 0 && flopTurnRiverRaised == true)
        //////    {
        //////        if (fcRank == PAIR || fcRank == MID_PAIR || fcRank == HIGH_PAIR)
        //////        {
        //////            //if (virtualPlayers[player].PocketPair == 0)// #1 no pocket pair - any raise
        //////            if(rank > 12)
        //////            {
        //////                folding = true;
        //////            }
        //////            if (/*virtualPlayers[0].RoundRaiseAmount*/ potRaisePercentage > PotAmount * .50)//#2
        //////            {
        //////                if (virtualPlayers[player].PocketPair < 12)
        //////                {
        //////                    folding = true;
        //////                }
        //////            }
        //////            if (/*virtualPlayers[0].RoundRaiseAmount*/ potRaisePercentage > PotAmount * .25)//#3 #4
        //////            {
        //////                if (GetPairType(hand) == PairTypes.Bottom || GetPairType(hand) == PairTypes.Middle)
        //////                {
        //////                    if (fcRank < HIGH_PAIR)
        //////                    {
        //////                        folding = true;
        //////                    }
        //////                }
        //////            }
        //////            if (/*virtualPlayers[0].RoundRaiseAmount*/ potRaisePercentage > PotAmount * .30)//#5
        //////            {
        //////                if (GetPairType(hand) == PairTypes.Top)
        //////                {
        //////                    if (fcRank < HIGH_PAIR)
        //////                    {
        //////                        folding = true;
        //////                    }
        //////                }
        //////            }
        //////            if (fcRank == TWO_PAIR)//#6
        //////            {
        //////                int cardTotal = GamePlayers[player].hand.XofaKindValue + GamePlayers[player].hand.TwoPairSecondValue;
        //////                if (cardTotal < 20)
        //////                {
        //////                    folding = true;
        //////                }
        //////            }
        //////            if (fcRank == THREE_TO_A_FLUSH || fcRank == THREE_TO_A_STRAIGHT_INSIDE)//#7
        //////            {
        //////                if (virtualPlayers[player].RoundRaiseAmount > 40)
        //////                {
        //////                    folding = true;
        //////                }
        //////            }
        //////            if (fcRank == THREE_TO_A_STRAIGHT_OUTSIDE || fcRank == FOUR_TO_A_STRAIGHT_OUTSIDE)//#8
        //////            {
        //////                if (virtualPlayers[player].RoundRaiseAmount > 25)
        //////                {
        //////                    folding = true;
        //////                }
        //////            }
        //////            if (fcRank == FOUR_TO_A_STRAIGHT_INSIDE || fcRank == FOUR_TO_A_FLUSH)//#9
        //////            {
        //////                if (virtualPlayers[player].RoundRaiseAmount > 75)
        //////                {
        //////                    folding = true;
        //////                }
        //////            }

        //////        }


        //////    }
        //////    //turn possibilities #3
        //////    if (virtualPlayers[0].RoundRaiseAmount > 0 && flopTurnRiverRaised == false)//the real player raised
        //////    {
        //////        if (fcRank < PAIR && rank > 12)//#1
        //////        {
        //////            folding = true;
        //////        }
        //////        if (fcRank == PAIR)// #2 #3 #4
        //////        {
        //////            if (GetPairType(hand) != PairTypes.Pocket)
        //////            {
        //////                if (GamePlayers[player].hand.XofaKindValue <= 10)
        //////                {
        //////                    folding = true;
        //////                }
        //////            }
        //////        }

        //////        if (fcRank == TWO_PAIR)//#5
        //////        {
        //////            int cardTotal = GamePlayers[player].hand.XofaKindValue + GamePlayers[player].hand.TwoPairSecondValue;
        //////            if (cardTotal < 20)
        //////            {
        //////                folding = true;
        //////            }
        //////        }
        //////        if (fcRank == THREE_TO_A_FLUSH || fcRank == THREE_TO_A_STRAIGHT_INSIDE)//#6
        //////        {
        //////            if (virtualPlayers[player].RoundRaiseAmount > 50)
        //////            {
        //////                folding = true;
        //////            }
        //////        }
        //////        if (fcRank == THREE_TO_A_STRAIGHT_OUTSIDE || fcRank == FOUR_TO_A_STRAIGHT_OUTSIDE)//#7
        //////        {
        //////            if (virtualPlayers[player].RoundRaiseAmount > 30)
        //////            {
        //////                folding = true;
        //////            }
        //////        }
        //////        if (fcRank == FOUR_TO_A_STRAIGHT_INSIDE || fcRank == FOUR_TO_A_FLUSH)//#8
        //////        {
        //////            if (virtualPlayers[player].RoundRaiseAmount > 75)
        //////            {
        //////                folding = true;
        //////            }
        //////        }

        //////    }

        //////    textBox1.AppendText(virtualPlayers[player].Name + " #" + player.ToString() + " Turn Bet Rank = " + fcRank.ToString() + Environment.NewLine);
        //////    if (folding == true)
        //////    {
        //////        BetType = BetTypes.folding;
        //////    }
        //////}

        ////////RIVER
        //////if (GameState == GameStates.RiverBet)//the river
        //////{
        //////    bool folding = false;
        //////    int fcRank = virtualPlayers[player].FiveCardHandRank;
        //////    int potRaisePercentage = GetPercentPotRaised(player);
        //////    potRaisePercentage = ThisRoundRaisePercentage;
        //////    int[] hand = GamePlayers[player].hand.cardHand;

        //////    //river possibilities #1
        //////    if (potRaisePercentage == 0)//the pot has not been raised
        //////    {
        //////        double tempRaise = 0;
        //////        int tempRank = fcRank;
        //////        //double tempRaise = virtualPlayers[player].FlopNoRaiseBetPercentages[20 - fcRank] * .01;
        //////        if (fcRank == HIGH_PAIR || fcRank == MID_PAIR || fcRank == PAIR)//adjust the pairs
        //////        {

        //////            switch (GetPairType(hand))
        //////            {
        //////                case PairTypes.Bottom: tempRank = 6; break;
        //////                case PairTypes.Middle: tempRank = 7; break;
        //////                case PairTypes.Top: tempRank = 8; break;
        //////                case PairTypes.Pocket: tempRank = 9; break;
        //////            }
        //////        }
        //////        if (fcRank < PAIR && fcRank > 0)
        //////        {
        //////            tempRank--; ;
        //////        }
        //////        tempRaise = virtualPlayers[player].RiverNoRaiseBetPercentages[21 - tempRank] * .01;

        //////        if (tempRaise > 0)
        //////        {
        //////            BetType = BetTypes.raising;
        //////            raise = RoundUp(PotAmount * tempRaise);
        //////        }
        //////        if (tempRaise < 0)
        //////        {
        //////            folding = true;
        //////        }
        //////    }

        //////    // river possibilities #2
        //////    if (/*virtualPlayers[0].RoundRaiseAmount*/ potRaisePercentage > 0 && flopTurnRiverRaised == true)
        //////    {
        //////        if (fcRank == PAIR || fcRank == MID_PAIR || fcRank == HIGH_PAIR)
        //////        {
        //////            //if (virtualPlayers[player].PocketPair == 0)// #1 no pocket pair - any raise
        //////            if(rank > 12)
        //////            {
        //////                folding = true;
        //////            }
        //////            if (/*virtualPlayers[0].RoundRaiseAmount*/ potRaisePercentage > PotAmount * .50)//#2
        //////            {
        //////                if (virtualPlayers[player].PocketPair < 12)
        //////                {
        //////                    folding = true;
        //////                }
        //////            }
        //////            if (/*virtualPlayers[0].RoundRaiseAmount*/ potRaisePercentage > PotAmount * .25)//#3 #4
        //////            {
        //////                if (GetPairType(hand) == PairTypes.Bottom || GetPairType(hand) == PairTypes.Middle)
        //////                {
        //////                    if (fcRank < HIGH_PAIR)
        //////                    {
        //////                        folding = true;
        //////                    }
        //////                }
        //////            }
        //////            if (/*virtualPlayers[0].RoundRaiseAmount*/ potRaisePercentage > PotAmount * .30)//#5
        //////            {
        //////                if (GetPairType(hand) == PairTypes.Top)
        //////                {
        //////                    if (fcRank < HIGH_PAIR)
        //////                    {
        //////                        folding = true;
        //////                    }
        //////                }
        //////            }
        //////            if (fcRank == TWO_PAIR)//#6
        //////            {
        //////                int cardTotal = GamePlayers[player].hand.XofaKindValue + GamePlayers[player].hand.TwoPairSecondValue;
        //////                if (cardTotal < 20)
        //////                {
        //////                    folding = true;
        //////                }
        //////            }
        //////            if (fcRank == THREE_TO_A_FLUSH || fcRank == THREE_TO_A_STRAIGHT_INSIDE)//#7
        //////            {
        //////                if (virtualPlayers[player].RoundRaiseAmount > 40)
        //////                {
        //////                    folding = true;
        //////                }
        //////            }
        //////            if (fcRank == THREE_TO_A_STRAIGHT_OUTSIDE || fcRank == FOUR_TO_A_STRAIGHT_OUTSIDE)//#8
        //////            {
        //////                if (virtualPlayers[player].RoundRaiseAmount > 25)
        //////                {
        //////                    folding = true;
        //////                }
        //////            }
        //////            if (fcRank == FOUR_TO_A_STRAIGHT_INSIDE || fcRank == FOUR_TO_A_FLUSH)//#9
        //////            {
        //////                if (virtualPlayers[player].RoundRaiseAmount > 75)
        //////                {
        //////                    folding = true;
        //////                }
        //////            }

        //////        }


        //////    }
        //////    //river possibilities #3
        //////    if (virtualPlayers[0].RoundRaiseAmount > 0 && flopTurnRiverRaised == false)//the real player raised
        //////    {
        //////        if (fcRank < PAIR && rank > 12)//#1
        //////        {
        //////            folding = true;
        //////        }
        //////        if (fcRank == PAIR)// #2 #3 #4
        //////        {
        //////            if (GetPairType(hand) != PairTypes.Pocket)
        //////            {
        //////                if (GamePlayers[player].hand.XofaKindValue <= 10)
        //////                {
        //////                    folding = true;
        //////                }
        //////            }
        //////        }

        //////        if (fcRank == TWO_PAIR)//#5
        //////        {
        //////            int cardTotal = GamePlayers[player].hand.XofaKindValue + GamePlayers[player].hand.TwoPairSecondValue;
        //////            if (cardTotal < 20)
        //////            {
        //////                folding = true;
        //////            }
        //////        }
        //////        if (fcRank == THREE_TO_A_FLUSH || fcRank == THREE_TO_A_STRAIGHT_INSIDE)//#6
        //////        {
        //////            if (virtualPlayers[player].RoundRaiseAmount > 50)
        //////            {
        //////                folding = true;
        //////            }
        //////        }
        //////        if (fcRank == THREE_TO_A_STRAIGHT_OUTSIDE || fcRank == FOUR_TO_A_STRAIGHT_OUTSIDE)//#7
        //////        {
        //////            if (virtualPlayers[player].RoundRaiseAmount > 30)
        //////            {
        //////                folding = true;
        //////            }
        //////        }
        //////        if (fcRank == FOUR_TO_A_STRAIGHT_INSIDE || fcRank == FOUR_TO_A_FLUSH)//#8
        //////        {
        //////            if (virtualPlayers[player].RoundRaiseAmount > 75)
        //////            {
        //////                folding = true;
        //////            }
        //////        }

        //////    }


        //////    textBox1.AppendText(virtualPlayers[player].Name + " #" + player.ToString() + " River Bet Rank = " + fcRank.ToString() + Environment.NewLine);
        //////    if (folding == true)
        //////    {
        //////        BetType = BetTypes.folding;
        //////    }
        //////}

        ////////END RIVER 
    }
}
