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
        private List<ObservableCollection<Node>> listNodes = new List<ObservableCollection<Node>>();

        public NodesHierarchy(DataTable usersDT)
        {
            this.usersDT = usersDT;

            for (int j = 0; j <= MaxIdRod(); j++)
            {
                listNodes.Add(new ObservableCollection<Node>());
            }
        }

        public ObservableCollection<Node> GetHodesHierarchy()
        {
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
