using System;
using System.Configuration;
using System.Data;
using System.Windows;
using Npgsql;

namespace EnergoKomplekt
{
    /// <summary>
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
            if (CheckPassAndLogin())
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }

        private bool CheckPassAndLogin()
        {
            bool log = false;

            DataRowView vr = (DataRowView)ComboBoxLogin.SelectedItem;

            string sql1 = "SELECT count(Код)=1 AS res FROM СпрПользователи WHERE Код = " + vr[0] + " AND Пароль = '" + txtPassword.Password + "'";

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

            NpgsqlCommand command = new NpgsqlCommand(sql1, npgSqlConnection);

            NpgsqlDataReader dr = command.ExecuteReader();

            while (dr.Read())
            {
                Console.Write("{0}\n", dr[0]);
                log = dr[0].ToString().Equals("True");
            }

            npgSqlConnection.Close();

            return log;
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

            //ComboBoxLogin.ItemsSource = usersDT.DefaultView;
            //ComboBoxLogin.DisplayMemberPath = usersDT.Columns["Пользователь"].ToString();
            //ComboBoxLogin.SelectedValuePath = usersDT.Columns["Пользователь"].ToString();

            npgSqlConnection.Close();
            Console.WriteLine("Подключение закрыто...");
        }
    }
}
