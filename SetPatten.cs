using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HDevelop
{
    public partial class SetPatten : Form
    {
        public SetPatten()
        {
            InitializeComponent();           
        }

        private void btnDown_Click(object sender, EventArgs e)
        {

        }

        private void btnPoi_Click(object sender, EventArgs e)
        {
            if (btnPoi.Text == "点.移动")
            {
                btnPoi.Text = "框.移动";
            }
            else if (btnPoi.Text == "框.移动")
            {
                btnPoi.Text = "比例";
            }
            else
            {
                btnPoi.Text = "点.移动";
            }
        }

        private void btnSpeed_Click(object sender, EventArgs e)
        {
            if (btnSpeed.Text == "快")
            {
                btnSpeed.Text = "中";
            }
            else if (btnSpeed.Text == "中")
            {
                btnSpeed.Text = "慢";
            }
            else
            {
                btnSpeed.Text = "快";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }
    }
}
