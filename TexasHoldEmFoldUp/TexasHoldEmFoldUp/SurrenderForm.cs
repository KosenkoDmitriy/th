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
    public partial class SurrenderForm : Form
    {
        Form1 parent;
        Timer flashTimer;
        bool show;
        public SurrenderForm(Form1 parentForm)
        {
            InitializeComponent();
            parent = parentForm;
            textBox1.Text = parent.surrenderString;
            flashTimer = new Timer();
            flashTimer.Interval = 500;
            flashTimer.Tick += new EventHandler(flashTimer_Tick);
            flashTimer.Start();
        }

        void flashTimer_Tick(object sender, EventArgs e)
        {
            show = !show;
            if (show)
            {
                label1.Text = " SURRENDER?";
            }
            else
            {
                label1.Text = "";
            } //label1.Visible = show;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            SurrenderForm_Click(sender, e);
        }

        private void SurrenderForm_Click(object sender, EventArgs e)
        {
            Hide();
            parent.playerFoldButton_Click(parent.playerSurrenderButton, e);
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            SurrenderForm_Click(sender, e);
        }

        
        

        
    }
}
