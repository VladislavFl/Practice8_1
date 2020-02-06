using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data;

namespace EnergoKomplekt.Controllers
{
    class NodesHierarchy
    {
        private DataTable usersDT = new DataTable();

        public NodesHierarchy(DataTable usersDT)
        {
            this.usersDT = usersDT;
        }

        public ObservableCollection<Node> GetHodesHierarchy()
        {
            List<ObservableCollection<Node>> listNodes = new List<ObservableCollection<Node>>();

            for (int j = 0; j <= MaxIdRod(); j++)
                listNodes.Add(new ObservableCollection<Node>());

            for (int j = MaxIdRod(); j >= 0; j--)
            {
                for (int i = 0; i < usersDT.Rows.Count; i++)
                {
                    Node node;
                    if (Convert.ToInt32(usersDT.Rows[i][1]) == j)
                    {
                        if (Convert.ToInt32(usersDT.Rows[i][0]) <= MaxIdRod() && listNodes[Convert.ToInt32(usersDT.Rows[i][0])] != null)
                        {
                            listNodes[j].Add(node = new Node(usersDT.Rows[i][4].ToString(), listNodes[Convert.ToInt32(usersDT.Rows[i][0])]));
                        }
                        else
                        {
                            listNodes[j].Add(node = new Node(usersDT.Rows[i][4].ToString()));
                        }
                        node.SetType(Convert.ToInt32(usersDT.Rows[i][3]));
                        node.SetKodeIcon(Convert.ToInt32(usersDT.Rows[i][5]));
                        node.SetIconPatch(Convert.ToInt32(usersDT.Rows[i][5]));
                    }
                }
            }

            return listNodes[0];
        }

        public ObservableCollection<Node> GetHodesHierarchyFolders()
        {
            List<ObservableCollection<Node>> listNodes = new List<ObservableCollection<Node>>();

            for (int j = 0; j <= MaxIdRod(); j++)
                listNodes.Add(new ObservableCollection<Node>());

            for (int j = MaxIdRod(); j >= 0; j--)
            {
                for (int i = 0; i < usersDT.Rows.Count; i++)
                {
                    Node node;
                    if (Convert.ToInt32(usersDT.Rows[i][1]) == j && Convert.ToInt32(usersDT.Rows[i][5]) == 1)
                    {
                        if (Convert.ToInt32(usersDT.Rows[i][0]) <= MaxIdRod() && listNodes[Convert.ToInt32(usersDT.Rows[i][0])] != null)
                        {
                            listNodes[j].Add(node = new Node(usersDT.Rows[i][4].ToString(), listNodes[Convert.ToInt32(usersDT.Rows[i][0])]));
                        }
                        else
                        {
                            listNodes[j].Add(node = new Node(usersDT.Rows[i][4].ToString()));
                        }
                        node.SetType(Convert.ToInt32(usersDT.Rows[i][3]));
                        node.SetKodeIcon(Convert.ToInt32(usersDT.Rows[i][5]));
                        node.SetIconPatch(Convert.ToInt32(usersDT.Rows[i][5]));
                    }
                }
            }

            return listNodes[0];
        }

        public int MaxIdRod()
        {
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

            return maxIdRod;
        }

    }
}
