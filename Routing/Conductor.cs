using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routing
{
    public class Conductor
    {
        private int firstNode;
        private int secondNode;
        public  Conductor(int fn, int sn)
        {
            this.firstNode = fn;
            this.secondNode = sn;
        }

        public int FirstNode
        {
            get
            {
                return firstNode;
            }
        }

        public int SecondNode
        {
            get
            {
                return secondNode;
            }
        }

        public bool Equals(Conductor c)
        {
            if ((this.firstNode == c.firstNode) && (this.secondNode == c.secondNode))
                return true;
            if ((this.secondNode == c.firstNode) && (this.firstNode == c.secondNode))
                return true;
            return false;
        }

        //public override bool Equals(object obj)
        //{
        //    if (obj == null)
        //        return false;
        //    Conductor c = obj as Conductor;
        //    if (c == null)
        //        return false;
        //    if ((this.firstNode == c.firstNode) && (this.secondNode == c.secondNode))
        //        return true;
        //    if ((this.secondNode == c.firstNode) && (this.firstNode == c.secondNode))
        //        return true;
        //    return false;
        //}
        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}       
    }
}
