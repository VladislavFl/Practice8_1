using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        private DataSet dataSet = new DataSet();
        private ObservableCollection<Node> nodes;
        private ObservableCollection<Node> nodes2;
        private List<ObservableCollection<Node>> listNodes;
        public MainWindow()
        {
            InitializeComponent();

            string sql1 = "SELECT * FROM ГлавноеМеню order by Код desc";

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

            listNodes = new List<ObservableCollection<Node>>();
            nodes2 = new ObservableCollection<Node>(){
            new Node
            {
                Name ="001",
                Nodes = new ObservableCollection<Node>
                {
                    new Node {Name="001001" },
                    new Node {Name="001002" },
                }
            }
            };

            int maxIdRod = 0;
            for (int i = 0; i < usersDT.Rows.Count; i++)
            {
                if (usersDT.Rows[i][1].ToString() != "")
                {
                    if (maxIdRod < Convert.ToInt32(usersDT.Rows[i][1]))
                        maxIdRod = Convert.ToInt32(usersDT.Rows[i][1]);
                }
                else
                {
                    usersDT.Rows[i][1] = 0;
                }
            }

            for (int j = 0; j <= maxIdRod; j++)
            {
                listNodes.Add(new ObservableCollection<Node>());
            }
            
            for (int j = maxIdRod; j >= 0; j--)
            {
                for (int i = 0; i < usersDT.Rows.Count; i++)
                {
                    if (Convert.ToInt32(usersDT.Rows[i][1]) == j)
                    {
                        Console.WriteLine(usersDT.Rows[i][4].ToString() + j);
                        if (Convert.ToInt32(usersDT.Rows[i][0]) <= maxIdRod)
                        {
                            listNodes[j].Add(new Node(usersDT.Rows[i][4].ToString(), listNodes[Convert.ToInt32(usersDT.Rows[i][0])]));
                        }
                        else
                        {
                            listNodes[j].Add(new Node(usersDT.Rows[i][4].ToString()));
                        }
                    }
                }
            }
            
            TreeView.ItemsSource = listNodes[0];

            npgSqlConnection.Close();
            Console.WriteLine("Подключение закрыто...");
        }
    }
}
