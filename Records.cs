using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeLab
{
	public partial class SaveRecord : Form
	{
		private readonly int score; 
		public static bool recordIsOpened = false;
		public SaveRecord(int score)
		{
			InitializeComponent();
			this.score = score;
		}

		private void Records_Load(object sender, EventArgs e)
		{
			recordIsOpened = true;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(name.Text))
			{
				Properties.Settings.Default.Records.Add(DateTime.Now.ToString() + " " + score + " — " + name.Text);
				recordIsOpened = false;
				this.Close();
				this.Dispose();
				return;
			}
			else
			{
				MessageBox.Show("The name must not be empty", "", MessageBoxButtons.OK);
			}
		}

		private void Records_FormClosed(object sender, FormClosedEventArgs e)
		{
			Properties.Settings.Default.Save();
		}

		private void Records_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				this.Close();
			}
			if (e.KeyCode == Keys.Enter)
			{
				button1_Click(sender, e);
			}
		}
	}
}
