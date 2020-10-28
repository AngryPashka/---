using System;
using System.Drawing;
using System.Windows.Forms;

namespace Учет_достижений___Миронов
{
	public partial class FormAuthorization : Form
	{
		#region Конструктор

		int Fail = 0;
		public FormAuthorization()
		{
			InitializeComponent();
		}
		#endregion
		#region Ok
		private void buttonOk_Click(object sender, EventArgs e)
		{
            string Login = textBoxLogin.Text, Password = textBoxPassword.Text;
            textBoxLogin.BackColor = Login == String.Empty || Login == "Логин" ? Color.LightPink : Color.White;
            textBoxPassword.BackColor = Password == String.Empty || Password == "Пароль" ? Color.LightPink : Color.White;
            if (Login != String.Empty && Login != "Логин"
             && Password != String.Empty && Password != "Пароль")
            {
                int rank = -1;
                try
                {
                    rank = new WorkWithBD().Authentication(Login, Convert.ToInt32(Password));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                }
                if (rank != -1)
                {
                    Hide();
                    FormMain form = new FormMain(rank);
                    form.ShowDialog();
                    Close();
                }
                else
                {
                    Fail++;
                    MessageBox.Show("Неверный логин и(или) пароль!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (Fail > 2) Close();
                }
            }
        }
        #endregion
        #region Обработка событий нажатий клавиш
        private void textBoxLogin_Enter(object sender, EventArgs e)
        {
            if (textBoxLogin.Text == "Логин")
            {
                textBoxLogin.Clear();
                textBoxLogin.ForeColor = Color.Black;
            }
            textBoxLogin.BackColor = Color.White;
        }

        private void textBoxLogin_Leave(object sender, EventArgs e)
        {
            if (textBoxLogin.Text == String.Empty)
            {
                textBoxLogin.Text = "Логин";
                textBoxLogin.ForeColor = Color.Gray;
            }
        }

        private void textBoxPassword_Enter(object sender, EventArgs e)
        {
            if (textBoxPassword.Text == "Пароль")
            {
                textBoxPassword.Clear();
                textBoxPassword.ForeColor = Color.Black;
            }
            textBoxPassword.BackColor = Color.White;
            textBoxPassword.PasswordChar = '*';
        }

        private void textBoxPassword_Leave(object sender, EventArgs e)
        {
            if (textBoxPassword.Text == String.Empty)
            {
                textBoxPassword.Text = "Пароль";
                textBoxPassword.ForeColor = Color.Gray;
                textBoxPassword.PasswordChar = '\0';
            }
        }

        private void textBoxLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                textBoxPassword.Focus();
        }

        private void textBoxPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) buttonOk_Click(null, null);
            if ((e.KeyChar >= '0') && (e.KeyChar <= '9')) return;
            if (e.KeyChar != 8) e.Handled = true;
        }
		#endregion
	}
}
