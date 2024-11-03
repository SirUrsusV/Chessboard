using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Шахматная_доска
{
    public partial class HelloForm : Form
    {
        public HelloForm()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            radioButton1.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainForm mainForm;
            if(radioButton1.Checked)
                mainForm = new MainForm(GameMod.Classic, this);
            else
                mainForm = new MainForm(GameMod.Testing, this);
            mainForm.Show();
            this.Hide();
        }
    }
}
