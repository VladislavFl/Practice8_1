using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Ribbon;
using EnergoKomplekt.trash;
using Npgsql;

namespace EnergoKomplekt
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        private DataTable usersDT = new DataTable();
        private ObservableCollection<Node> nodes;
        public MainWindow()
        {
            InitializeComponent();

            string sql1 = "SELECT * FROM ГлавноеМеню";

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

            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(sql1, npgSqlConnection);

            dataAdapter.Fill(usersDT);

            DataView dataView = new DataView(usersDT);

            nodes = new ObservableCollection<Node>
            {
                new Node
                {
                    Name ="Европа",
                    Nodes = new ObservableCollection<Node>
                    {
                        new Node {Name="Германия" },
                        new Node {Name="Франция" },
                        new Node
                        {
                            Name ="Великобритания",
                            Nodes = new ObservableCollection<Node>
                            {
                                new Node {Name="Англия" },
                                new Node {Name="Шотландия" },
                                new Node {Name="Уэльс" },
                                new Node {Name="Сев. Ирландия" },
                            }
                        }
                    }
                },
                new Node
                {
                    Name ="Азия",
                    Nodes = new ObservableCollection<Node>
                    {
                        new Node {Name="Китай(Каровавирус)" },
                        new Node {Name="Япония" },
                        new Node { Name ="Индия" }
                    }
                },
                new Node { Name="Африка" },
                new Node { Name="Америка" },
                new Node { Name="Австралия" }
            };

            TreeView.ItemsSource = nodes;

            //TreeView.ItemsSource = dataView;

            //TreeView.ItemsSource = usersDT.DefaultView;
            //TreeView.DisplayMemberPath = usersDT.Columns["Код"].ToString();
            //TreeView.SelectedValuePath = usersDT.Columns["Код"].ToString();

            npgSqlConnection.Close();
            Console.WriteLine("Подключение закрыто...");
        }
    }
}
