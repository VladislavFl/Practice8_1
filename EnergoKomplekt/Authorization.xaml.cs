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
            
            //SELECT * FROM СпрПользователи WHERE Пользователь = 'User 1' AND Пароль = '1111'

            /*
            SELECT Код,
                CASE WHEN Пользователь = 'User 2' AND Пароль = '222' THEN 'True'

            ELSE 'False' END
                FROM СпрПользователи WHERE Пользователь = 'User 2'
            */

        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            string sql1 = "SELECT Код, CASE WHEN Пользователь = '" + txtLogin.Text + "' AND Пароль = '" + txtPassword.Password + "' THEN 'True' ELSE 'False' END FROM СпрПользователи WHERE Пользователь = '" + txtLogin.Text + "'";
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
                Console.Write("{0}\t{1} \n", dr[0], dr[1]);
                log = dr[1].Equals("True");
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
