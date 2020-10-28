using System;
using System.Drawing;
using System.Windows.Forms;

namespace Учет_достижений___Миронов
{
	public partial class FormAddEvens : Form
	{
		private WorkWithBD dataDB = new WorkWithBD();
		public FormAddEvens()
		{
			InitializeComponent();
		}

		private void textBox1_Enter(object sender, EventArgs e)
		{
			if (textBox1.Text == "Название")
			{
				textBox1.Clear();
				textBox1.ForeColor = Color.Black;
			}
		}

		private void textBox1_Leave(object sender, EventArgs e)
		{
			if (textBox1.Text == String.Empty)
			{
				textBox1.Text = "Название";
				textBox1.ForeColor = Color.Silver;
			}
		}

		private void FormAddEvens_Load(object sender, EventArgs e)
		{
			foreach (string item in dataDB.ListStudens())
				((DataGridViewComboBoxColumn)dataGridView1.Columns["Student"]).Items.Add(item);
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (textBox1.Text != "Название")
			{
				int id = dataDB.AddEvents(textBox1.Text), done = 0, fail = 0;
				if (id != -1)
				{
					for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
					{
						if (dataGridView1.Rows[i].Cells[0].Value.ToString() != String.Empty && dataGridView1.Rows[i].Cells[1].Value.ToString() != String.Empty)
						{
							if (dataDB.AddAchievements(dataGridView1.Rows[i].Cells[0].Value.ToString(), id, dataGridView1.Rows[i].Cells[1].Value.ToString()) != -1) done++;
							else fail++;
						}						
					}
					MessageBox.Show($"Записалось {done} достижений\nОшибка {fail} записей", "Результат");
					Close();
				}
				else
					MessageBox.Show("Ошибка", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
