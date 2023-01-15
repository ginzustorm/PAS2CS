using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator
{
    public class Configuration
    {
        //Состояние алгоритма
        public Char s;
        //Номер позиции входной головки
        public Int32 i;
        //Элемент магазина
        public struct elem
        {
            public String value;
            public bool term;
            public Int32 altIndex;
            public elem(String v, bool t, Int32 i=0)
            {
                if (t) altIndex = 0;
                value = v;
                term = t;
                altIndex = i;
            }
            public elem(elem source)
            {   
                value = source.value;
                term = source.term;
                altIndex = source.altIndex;
            }

        }


        //Содержимое магазина L1
        public Stack<elem> L1;
        //Содержимое магазина L2
        public Stack<elem> L2;
        public Configuration()
        {
            s = 'q';
            i = 0;
            L1 = new Stack<elem>();
            L2 = new Stack<elem>();
            L2.Push(new elem("main", false, 0));  
        }

        public String Seq()
        {
            Stack<elem> l1 = new Stack<elem>();
            l1=L1;
            int count=L1.Count;
            String seq="";
            for (int i=0; i<count;i++)
            {
                var item = l1.Pop();
                if (!item.term)
                {
                    Pravilo p = Gramma.p.Find (x => x.leftSide==item.value);
                    seq=(Gramma.p.IndexOf(p)+1).ToString()+"("+item.altIndex+") "+seq;
                }
            }
            
            return seq;
        }




    }
}
