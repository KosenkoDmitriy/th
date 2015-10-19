using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TexasHoldEmFoldUp

{
    public partial class BetForm : Form
    {
        Form1 parentForm;

        public BetForm(Form1 ParentForm)
        {
            parentForm = ParentForm;
            InitializeComponent();
            this.OKbutton.DialogResult = DialogResult.OK;
        }
        
        
        
        public double betAmount;
        public double BetAmount
        {
            set
            {
                betAmount += value;
                if (parentForm.GameState == Form1.GameStates.Ante)
                {
                    if (betAmount > parentForm.betLimit)
                    {
                        betAmount = parentForm.betLimit;
                    }
                }
                else
                {
                    if (betAmount > parentForm.raiseLimit)
                    {
                        betAmount = parentForm.raiseLimit;
                    }
                }
                
                //if (betAmount > parentForm.gameDenomination * parentForm.gameDenomMultiplier)
                //{
                //    betAmount = parentForm.gameDenomination * parentForm.gameDenomMultiplier;
                //}
                string DollarAmount = String.Format("{0:C}", betAmount);
                BetAmountLabel.Text = DollarAmount;
                parentForm.buttonSound.Play();
                
            }
            get
            {
                return betAmount;
            }
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            
            BetAmount = 1.00;
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            betAmount = 0;
            BetAmount = 0;
        }

        private void OKbutton_Click(object sender, EventArgs e)
        {
            
            
            this.Hide();
            if(betAmount > 0)

            //if (sender == button8)
            {
                parentForm.autoStart = true;
            }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BetAmount = 5.00;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BetAmount = 10.00;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            BetAmount = 20.00;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            BetAmount = 25.00;
        }

        private void getWindowTitle()
        {
            titleLabel.Text = parentForm.betWindowTitles[parentForm.betStringPtr];
        }

        private void BetForm_VisibleChanged(object sender, EventArgs e)
        {

            if (this.Visible)
            {
                getWindowTitle();
                betAmount = 0;
                BetAmount = 0;
                button8.Visible = false;
                button6.Text = String.Format("{0:C}", parentForm.gameDenomination);
                if (parentForm.isAnteBet() == true)
                {
                    if (parentForm.lastBet > 0)
                    {
                        button8.Visible = true;
                    }
                }
               
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            BetAmount = parentForm.gameDenomination;// .25;// 50.00;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (parentForm.lastBet > 0)
            {
                
                //parentForm.anteBet = parentForm.lastBet;
                BetAmount = parentForm.lastBet;
                OKbutton_Click(sender, e);
                
            }
        }

    }
}
