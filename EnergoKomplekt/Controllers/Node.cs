using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergoKomplekt.Controllers
{
    public class Node
    {
        public string Name { get; set; }

        public int Type { get; set; }

        public int KodeIcon { get; set; }

        public Node() { }
        public ObservableCollection<Node> Nodes { get; set; }

        public void SetName(String name)
        {
            this.Name = name;
        }

        public void SetType(int type)
        {
            this.Type = type;
        }

        public void SetKodeIcon(int kodeIcon)
        {
            this.KodeIcon = kodeIcon;
        }

        public Node(String name)
        {
            this.Name = name;
        }

        public Node(ObservableCollection<Node> nodes)
        {
            this.Nodes = nodes;
        }

        public Node(String name, ObservableCollection<Node> nodes)
        {
            this.Name = name;
            this.Nodes = nodes;
        }

    }
}
