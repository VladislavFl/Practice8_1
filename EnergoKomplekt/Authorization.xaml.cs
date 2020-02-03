using System;
using System.Configuration;
using System.Windows;
using Npgsql;

namespace EnergoKomplekt
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();

            //SELECT count(Код)=1 AS res FROM СпрПользователи WHERE Код = 2 AND Пароль = '2222'
            string sql1 = "SELECT Пользователь FROM СпрПользователи";

            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
            try
            {
                npgSqlConnection.Open();
                Console.WriteLine("Подключение открыто");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                // закрываем подключение
                //npgSqlConnection.Close();
                //Console.WriteLine("Подключение закрыто...");
            }

            NpgsqlCommand command = new NpgsqlCommand(sql1, npgSqlConnection);

            NpgsqlDataReader dr = command.ExecuteReader();

            while (dr.Read())
            {
                Console.Write("{0}\n", dr[0]);
                ComboBoxLogin.Items.Add(dr[0].ToString());
            }

            npgSqlConnection.Close();

        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            string sql1 = "SELECT CASE WHEN Пользователь = '" + ComboBoxLogin.Text + "' AND Пароль = '" + txtPassword.Password + "' THEN 'True' ELSE 'False' END FROM СпрПользователи WHERE Пользователь = '" + ComboBoxLogin.Text + "'";
            bool log = false;

            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
            try
            {
                npgSqlConnection.Open();
                Console.WriteLine("Подключение открыто");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                // закрываем подключение
                //npgSqlConnection.Close();
                //Console.WriteLine("Подключение закрыто...");
            }

            NpgsqlCommand command = new NpgsqlCommand(sql1, npgSqlConnection);

            NpgsqlDataReader dr = command.ExecuteReader();

            while (dr.Read())
            {
                Console.Write("{0}\n", dr[0]);
                log = dr[0].Equals("True");
            }

            MessageBox.Show(log.ToString());

            npgSqlConnection.Close();

            if (log)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }
    }
}
