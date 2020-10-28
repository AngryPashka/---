using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Учет_достижений___Миронов
{
	public partial class FormMain : Form
	{
		private int Rank;
		WorkWithBD dataBase = new WorkWithBD();
		public FormMain(int rank)
		{
			Rank = rank;
			InitializeComponent();
		}

		private void UpdateElemebts()
		{
			listBox1.Items.Clear();
			comboBox1.Items.Clear();
			listBox3.Items.Clear();
			listBox4.Items.Clear();
			foreach (string item in dataBase.ListGroup())
			{
				listBox1.Items.Add(item);
				comboBox1.Items.Add(item);
			}
			foreach (string item in dataBase.ListEvents())
				listBox3.Items.Add(item);
			if (comboBox1.Items.Count > 0) comboBox1.SelectedIndex = 0;
			foreach (string item in dataBase.RatingAll())
				listBox4.Items.Add(item);
		}

		private void FormMain_Load(object sender, EventArgs e)
		{
			UpdateElemebts();
			button1.Visible = (Rank == 1 || Rank == 2);
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listBox1.SelectedItem != null)
			{
				string name = listBox1.SelectedItem.ToString();
				MessageBox.Show($"Рейтинг гр. {name} = {dataBase.RatingGroup(name)}", "Рейтинг", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			listBox2.Items.Clear();
			foreach (string item in dataBase.ListStudens(comboBox1.SelectedItem.ToString()))
			{
				listBox2.Items.Add(item);
			}
		}

		private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listBox2.SelectedItem != null)
			{
				string name = listBox2.SelectedItem.ToString();
				string txt = $"Рейтинг ст. {name} = {dataBase.RatingStudent(name)}" + Environment.NewLine;
				foreach (string item in dataBase.ListArStrudent(name))
					txt += item + Environment.NewLine;
				MessageBox.Show(txt, "Рейтинг", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listBox3.SelectedItem != null)
			{
				string txt = listBox3.SelectedItem.ToString() + Environment.NewLine;
				foreach (string item in dataBase.ListEventInfo(listBox3.SelectedItem.ToString()))
					txt += item + Environment.NewLine;
				MessageBox.Show(txt, "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Hide();
			FormAddEvens form = new FormAddEvens();
			form.ShowDialog();
			Show();
			UpdateElemebts();
		}
	}
}
