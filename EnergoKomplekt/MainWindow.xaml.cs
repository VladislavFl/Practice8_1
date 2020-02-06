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
using EnergoKomplekt.Controllers;
using Npgsql;

namespace EnergoKomplekt
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public ObservableCollection<Phone> Phones { get; set; }
        private DataTable usersDT = new DataTable();
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

            npgSqlConnection.Close();
            Console.WriteLine("Подключение закрыто...");

            NodesHierarchy nodesHierarchy = new NodesHierarchy(usersDT);

            ObservableCollection<Node> nodes = nodesHierarchy.GetHodesHierarchyFolders();

            ObservableCollection<Node> nodes2 = nodesHierarchy.GetHodesHierarchy();

            TreeView.ItemsSource = nodes;

            Phones = new ObservableCollection<Phone>
            {
                new Phone {Id=1, ImagePath="/Images/folder.png", Title="iPhone 6S", Company="Apple" },
                new Phone {Id=2, ImagePath="/Images/folder.png", Title="Lumia 950", Company="Microsoft" },
                new Phone {Id=3, ImagePath="/Images/folder.png", Title="Nexus 5X", Company="Google" },
                new Phone {Id=4, ImagePath="/Images/folder.png", Title="Galaxy S6", Company="Samsung"}
            };
            phonesList.ItemsSource = nodes2;

        }

        private void phonesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Node p = new Node();
            p = (Node)phonesList.SelectedItem;
            if (p != null)
            {
                phonesList.ItemsSource = p.Nodes;
            }
        }
    }

}
