using System;
using System.Configuration;
using System.Data;
using System.Windows;
using Npgsql;

namespace EnergoKomplekt
{
    /// <summary>
    ///SELECT count(Код)=1 AS res FROM СпрПользователи WHERE Код = 2 AND Пароль = '2222'
    ///"SELECT CASE WHEN Пользователь = '" + ComboBoxLogin.Text + "' AND Пароль = '" + txtPassword.Password + "' THEN 'True' ELSE 'False' END FROM СпрПользователи WHERE Пользователь = '" + ComboBoxLogin.Text + "'"
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        private DataTable usersDT = new DataTable();
        public Authorization()
        {
            InitializeComponent();

            UsersToDataTable();

        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            bool log = false;

            string sql1 = "SELECT count(Код)=1 AS res FROM СпрПользователи WHERE Пользователь = '" + ComboBoxLogin.Text + "' AND Пароль = '" + txtPassword.Password + "'";

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
                MessageBox.Show(dr[0].ToString());
                log = dr[0].ToString().Equals("True");
            }

            //MessageBox.Show(log.ToString());

            npgSqlConnection.Close();

            if (log)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }

        private void UsersToDataTable()
        {
            string sql1 = "SELECT Код, Пользователь FROM СпрПользователи";

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

            NpgsqlDataAdapter dataAdapter =new NpgsqlDataAdapter(sql1, npgSqlConnection);

            dataAdapter.Fill(usersDT);

            DataView dataView = new DataView(usersDT);

            ComboBoxLogin.DataContext = dataView;

            ComboBoxLogin.ItemsSource = usersDT.DefaultView;
            ComboBoxLogin.DisplayMemberPath = usersDT.Columns["Пользователь"].ToString();
            ComboBoxLogin.SelectedValuePath = usersDT.Columns["Пользователь"].ToString();

            npgSqlConnection.Close();
            Console.WriteLine("Подключение закрыто...");
        }
    }
}
