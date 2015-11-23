using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToPolska
{
    public partial class ToPolska : Form
    {
        public ToPolska()
        {
            InitializeComponent();
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            toPolska(tbExpression.Text);
        }

        private void tbExpression_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)//и эта клавиша Enter
            {
                toPolska(tbExpression.Text);
            }
        }

        public void toPolska(string str)
        {
            PW pw = new PW();
            lbPolska.Text = pw.toPolska(str);
        }
    }
}
