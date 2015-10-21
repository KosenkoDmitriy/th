using System;
using System.Drawing;
using System.Windows.Forms;

namespace TexasHoldEmFoldUp
{
    public class AmountWindow : Panel
    {
        private Panel amountPanel;
        private Label titleLabel;
        private Label amountLabel;
        string displayAmount;
        public Font amountFont = new Font("Microsoft Sans Serif", 20, FontStyle.Bold);
        double Amount;
        public AmountWindow(string title, int left, int top)
        {

            this.Width = 172;
            this.Height = 60;
            this.Visible = true;
            this.Left = left;
            this.Top = top;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = TexasHoldem.CreditWindow;


            titleLabel = new Label();
            titleLabel.Text = title;
            titleLabel.Top = 5;
            titleLabel.Left = 24;
            titleLabel.AutoSize = false;
            titleLabel.Height = 15;
            titleLabel.Width = 130;
            titleLabel.BackColor = System.Drawing.Color.Transparent;
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(titleLabel);

            amountLabel = new Label();
            amountLabel.Left = 0;
            amountLabel.Top = 20;
            amountLabel.AutoSize = false;
            amountLabel.Width = 172;
            amountLabel.Height = 34;
            amountLabel.ForeColor = System.Drawing.Color.Yellow;
            amountLabel.Font = amountFont;
            amountLabel.Text = "$00.00";
            amountLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(amountLabel);
        }
        public double DollarAmount
        {
            set
            {
                Amount = value;
                amountLabel.Text = String.Format("{0:C}", Amount);
                amountLabel.Invalidate();
                amountLabel.Update();
            }
            get
            {
                return Amount;
            }
        }
    }

}
