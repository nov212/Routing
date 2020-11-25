﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routing
{
    public class MultilayerGragh : IGraph
    {
        private IGraph[] layers;    //слои графа
        private int layersCount;        //количество слоёв в графе
        private int factor;               //множитель, использующийся для нумерации узлов
        private int current;            //номер текущего слоя
        private byte[] via;

        private const byte VS_NONE = 0;
        private const byte VS_UP = 1;
        private const byte VS_DOWN = 2;
        private const byte VS_UPDOWN = 3;

        public MultilayerGragh(IEnumerable<IGraph> layers)
        {
            this.layers = layers.ToArray();
            layersCount = layers.Count();
            factor = this.layers[0].GetN();
            via = new byte[factor * layersCount];
            for (int i = 0; i < via.Length; i++)
                via[i] = VS_NONE;
            current = 0;
        }
        public IEnumerable<int> GetAdj(int node)
        {
            int actualLayer = ActualLayerNum(node);
            int actualNode = ActualNumeration(node);
            //соседи на одном слое с node
            foreach (int n in layers[actualLayer].GetAdj(actualNode))
                yield return factor*actualLayer+n;
            //сосед сверху node
            if (via[node]==VS_UP || via[node]==VS_UPDOWN)
                yield return factor * (actualLayer + 1) + actualNode;
            //сосед снизу node
            if (via[node]==VS_DOWN || via[node]==VS_UPDOWN)
                yield return factor * (actualLayer - 1) + actualNode;
        }

        public int GetN()
        {
            return layersCount * layers[0].GetN();
        }

        public int GetCol(int node)
        {
            if (node>=0 && node<GetN())
                return layers[current].GetCol(ActualNumeration(node));
            else return -1;
        }

        public int GetRow(int node)
        {
            if (node >= 0 && node < GetN())
                return layers[current].GetRow(ActualNumeration(node));
            else return -1;
        }

        public int ToNum(int row, int col)
        {
            return layers[current].ToNum(row, col)+factor*current;
        }

        public void SetVia(int row, int col, int lower, int upper)
        {
            if (lower>upper)
            {
                int t = lower;
                lower = upper;
                upper = t;
            }
            if (lower == upper)
                return;
            int copyCurrent = current;
            MoveTo(lower);
            if (via[ToNum(row, col)] == VS_NONE)
                via[ToNum(row, col)] = VS_UP;
            MoveTo(upper);
            if (via[ToNum(row, col)] == VS_NONE)
                via[ToNum(row, col)] = VS_DOWN;
            for (int i=lower+1;i<upper;i++)
            {
                MoveTo(i);
                via[ToNum(row, col)] = VS_UPDOWN;
            }
            current = copyCurrent;

        }


        private int ActualNumeration(int n)
        {
            return n % factor;
        }

        private int ActualLayerNum(int n)
        {
            int res = n / factor;
            if (res >= 0 && res < layersCount)
                return res;
            else return -1;
        }

        public IEnumerable<int> GetAdj(bool direction, int node)
        {
            throw new NotImplementedException();
        }

        public IGraph MoveUp()
        {
            if (current<current-1)
                current++;
            return layers[current];
        }

        public IGraph MoveDown()
        {
            if (current >0)
                current--;
            return layers[current];
        }

        public IGraph MoveTo(int layer)
        {
            if (layer >= 0 && layer < layersCount)
            {
                current = layer;
                return layers[layer];
            }
            return null;
        }
    }
}