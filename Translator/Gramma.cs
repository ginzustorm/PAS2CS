using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator
{
    //Структура правила
    public class Pravilo
    {
        public String leftSide;
        public struct structRightSide
        {
            public String notTerminal, terminal;
            public structRightSide(String n, String term)
            {
                notTerminal = n;
                terminal = term;
            }
        };
        public List<List<structRightSide>> rightSide;

    }
    //Граматика
    class Gramma
    {

#if DEBUG
        static bool cheking(Pravilo pravilo, ref List<String> debugList)
        {
            foreach (List<Pravilo.structRightSide> alternative in pravilo.rightSide)
            {
                foreach (Pravilo.structRightSide item in alternative)
                {
                    if (item.notTerminal != "" && debugList.IndexOf(item.notTerminal) == -1)
                    {
                        Pravilo second = p.Find(x => x.leftSide == item.notTerminal);
                        if (second != null)
                        {
                            if (p.IndexOf(second) != p.IndexOf(pravilo))
                            {
                                debugList.Add(item.notTerminal);
                                if (!cheking(second, ref debugList)) return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

            }
            return true;
        }

        static List<String> debugList=new List<String>();

        //Проверка граматики
        public static bool Check()
        {
            if (!(cheking(p[0], ref debugList))) return false;
            return true;
        }

#endif


        public static List<Pravilo> p;
        public static void Init()
        {

            p = new List<Pravilo>();
            Pravilo f;
            List<Pravilo.structRightSide> alternative;

            //1
            f = new Pravilo();
            f.leftSide = "main";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("header",""));
            alternative.Add(new Pravilo.structRightSide("",";"));
            alternative.Add(new Pravilo.structRightSide("body",""));
            alternative.Add(new Pravilo.structRightSide("","."));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            p.Add(f);

            //2
            f = new Pravilo();
            f.leftSide ="header";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","program"));
            alternative.Add(new Pravilo.structRightSide("id",""));   
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            p.Add(f);

            //3
            f = new Pravilo();
            f.leftSide ="body";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("opis-part",""));
            alternative.Add(new Pravilo.structRightSide("work-part",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            p.Add(f);


            //4
            f = new Pravilo();
            f.leftSide ="opis-part";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("opis-const1",""));
            alternative.Add(new Pravilo.structRightSide("opis-perem1",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            p.Add(f);


            //5
            f = new Pravilo();
            f.leftSide ="opis-const1";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("opis-const",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","lambda"));
            f.rightSide.Add(alternative);

            p.Add(f);


            //6
            f = new Pravilo();
            f.leftSide ="opis-perem1";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("opis-perem",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","lambda"));
            f.rightSide.Add(alternative);

            p.Add(f);


            //7
            f = new Pravilo();
            f.leftSide ="opis-const";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","const"));
            alternative.Add(new Pravilo.structRightSide("opis-constant",""));
            alternative.Add(new Pravilo.structRightSide("",";"));
            alternative.Add(new Pravilo.structRightSide("opis-constant1",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            p.Add(f);



            //8
            f = new Pravilo();
            f.leftSide ="opis-constant1";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("opis-constant",""));
            alternative.Add(new Pravilo.structRightSide("",";"));
            alternative.Add(new Pravilo.structRightSide("opis-constant1",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","lambda"));
            f.rightSide.Add(alternative);

            p.Add(f);



            //9
            f = new Pravilo();
            f.leftSide ="opis-constant";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("id-const",""));
            alternative.Add(new Pravilo.structRightSide("","="));
            alternative.Add(new Pravilo.structRightSide("const",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            p.Add(f);


            //10
            f = new Pravilo();
            f.leftSide ="id-const";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("id",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            p.Add(f);


            //11
            f = new Pravilo();
            f.leftSide ="const";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("sign1",""));
            alternative.Add(new Pravilo.structRightSide("chislo",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("sign1",""));
            alternative.Add(new Pravilo.structRightSide("id",""));
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("string",""));
            f.rightSide.Add(alternative);

            p.Add(f);


            //12
            f = new Pravilo();
            f.leftSide ="sign1";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("sign",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);


            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","lambda"));
            f.rightSide.Add(alternative);

            p.Add(f);


            //13
            f = new Pravilo();
            f.leftSide ="opis-perem";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","var"));
            alternative.Add(new Pravilo.structRightSide("opis-peremenix",""));
            alternative.Add(new Pravilo.structRightSide("",";"));
            alternative.Add(new Pravilo.structRightSide("opis-peremenix1",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            p.Add(f);



            //14
            f = new Pravilo();
            f.leftSide ="opis-peremenix1";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("opis-peremenix",""));
            alternative.Add(new Pravilo.structRightSide("",";"));
            alternative.Add(new Pravilo.structRightSide("opis-peremenix1",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","lambda"));
            f.rightSide.Add(alternative);

            p.Add(f);


            //15
            f = new Pravilo();
            f.leftSide ="opis-peremenix";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("id",""));
            alternative.Add(new Pravilo.structRightSide("id1",""));
            alternative.Add(new Pravilo.structRightSide("",":"));
            alternative.Add(new Pravilo.structRightSide("type",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            p.Add(f);




            //16
            f = new Pravilo();
            f.leftSide ="id1";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("",","));
            alternative.Add(new Pravilo.structRightSide("id",""));
            alternative.Add(new Pravilo.structRightSide("id1",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","lambda"));
            f.rightSide.Add(alternative);

            p.Add(f);




            //17
            f = new Pravilo();
            f.leftSide ="type";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("array",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("elem-type",""));
            f.rightSide.Add(alternative);

            p.Add(f);



            //18
            f = new Pravilo();
            f.leftSide ="array";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","array"));    
            alternative.Add(new Pravilo.structRightSide("", "[")); 
            alternative.Add(new Pravilo.structRightSide("index-type",""));
            alternative.Add(new Pravilo.structRightSide("index-type1",""));
            alternative.Add(new Pravilo.structRightSide("","]"));
            alternative.Add(new Pravilo.structRightSide("", "of"));
            alternative.Add(new Pravilo.structRightSide("elem-type",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            p.Add(f);


            //19
            f = new Pravilo();
            f.leftSide ="index-type1";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("",","));
            alternative.Add(new Pravilo.structRightSide("index-type",""));
            alternative.Add(new Pravilo.structRightSide("index-type1",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","lambda"));
            f.rightSide.Add(alternative);

            p.Add(f);


            //20
            f = new Pravilo();
            f.leftSide ="index-type";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("integer",""));
            alternative.Add(new Pravilo.structRightSide("",".."));
            alternative.Add(new Pravilo.structRightSide("integer",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            p.Add(f);



            //21
            f = new Pravilo();
            f.leftSide ="elem-type";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","integer"));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","real"));
            f.rightSide.Add(alternative);


            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","boolean"));
            f.rightSide.Add(alternative);


            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","string"));
            f.rightSide.Add(alternative);

            p.Add(f);


            //22

            f = new Pravilo();
            f.leftSide ="work-part";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","begin"));
            alternative.Add(new Pravilo.structRightSide("work-seq",""));
            alternative.Add(new Pravilo.structRightSide("","end"));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            p.Add(f);


            //23
            f = new Pravilo();
            f.leftSide ="work-seq";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("utv",""));
            alternative.Add(new Pravilo.structRightSide("",";"));
            alternative.Add(new Pravilo.structRightSide("utv1",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("", "lambda"));
            f.rightSide.Add(alternative);


            p.Add(f);


            //24
            f = new Pravilo();
            f.leftSide ="utv1";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("utv",""));
            alternative.Add(new Pravilo.structRightSide("",";"));
            alternative.Add(new Pravilo.structRightSide("utv1",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","lambda"));
            f.rightSide.Add(alternative);

            p.Add(f);

            //25
            f = new Pravilo();
            f.leftSide ="utv";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("prost-utv",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("struct-utv",""));
            f.rightSide.Add(alternative);


            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("in-out",""));
            f.rightSide.Add(alternative);

            p.Add(f);



            //26
            f = new Pravilo();
            f.leftSide ="prost-utv";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("perem",""));
            alternative.Add(new Pravilo.structRightSide("",":="));
            alternative.Add(new Pravilo.structRightSide("virag",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            p.Add(f);

            //27
            f = new Pravilo();
            f.leftSide ="struct-utv";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("pod-utv",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("while",""));
            f.rightSide.Add(alternative);


            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("for",""));
            f.rightSide.Add(alternative);


            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("if",""));
            f.rightSide.Add(alternative);

            p.Add(f);

            //28
            f = new Pravilo();
            f.leftSide ="pod-utv";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","begin"));
            alternative.Add(new Pravilo.structRightSide("work-seq",""));
            alternative.Add(new Pravilo.structRightSide("","end"));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            p.Add(f);


            //29
            f = new Pravilo();
            f.leftSide ="while";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","while"));
            alternative.Add(new Pravilo.structRightSide("virag",""));
            alternative.Add(new Pravilo.structRightSide("","do"));
            alternative.Add(new Pravilo.structRightSide("utv",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            p.Add(f);

            //30
            f = new Pravilo();
            f.leftSide ="for";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","for"));
            alternative.Add(new Pravilo.structRightSide("id",""));
            alternative.Add(new Pravilo.structRightSide("",":="));
            alternative.Add(new Pravilo.structRightSide("integer",""));
            alternative.Add(new Pravilo.structRightSide("","to"));
            alternative.Add(new Pravilo.structRightSide("integer",""));
            alternative.Add(new Pravilo.structRightSide("","do"));
            alternative.Add(new Pravilo.structRightSide("utv",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);


            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","for"));
            alternative.Add(new Pravilo.structRightSide("id",""));
            alternative.Add(new Pravilo.structRightSide("",":="));
            alternative.Add(new Pravilo.structRightSide("integer",""));
            alternative.Add(new Pravilo.structRightSide("","downto"));
            alternative.Add(new Pravilo.structRightSide("integer",""));
            alternative.Add(new Pravilo.structRightSide("","do"));
            alternative.Add(new Pravilo.structRightSide("utv",""));
            f.rightSide.Add(alternative);

            p.Add(f);


            //31
            f = new Pravilo();
            f.leftSide ="if";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","if"));
            alternative.Add(new Pravilo.structRightSide("virag",""));
            alternative.Add(new Pravilo.structRightSide("","then"));
            alternative.Add(new Pravilo.structRightSide("utv",""));
            alternative.Add(new Pravilo.structRightSide("utv2",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            p.Add(f);



            //32
            f = new Pravilo();
            f.leftSide ="utv2";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","else"));
            alternative.Add(new Pravilo.structRightSide("utv",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","lambda"));
            f.rightSide.Add(alternative);

            p.Add(f);



            //33
            f = new Pravilo();
            f.leftSide ="virag";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("prost-virag",""));
            alternative.Add(new Pravilo.structRightSide("op-prost-virag",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            p.Add(f);


            //34
            f = new Pravilo();
            f.leftSide ="op-prost-virag";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("op-otn",""));
            alternative.Add(new Pravilo.structRightSide("prost-virag",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","lambda"));
            f.rightSide.Add(alternative);

            p.Add(f);

            //35
            f = new Pravilo();
            f.leftSide ="op-otn";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","="));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","<>"));
            f.rightSide.Add(alternative);


            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","<"));
            f.rightSide.Add(alternative);


            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","<="));
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("",">"));
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("",">="));
            f.rightSide.Add(alternative);

            p.Add(f);


            //36     
            f = new Pravilo();
            f.leftSide ="prost-virag";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("sign1",""));
            alternative.Add(new Pravilo.structRightSide("term",""));
            alternative.Add(new Pravilo.structRightSide("additional-term",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);


            p.Add(f);

            //37
            f = new Pravilo();
            f.leftSide ="additional-term";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("additional-op",""));
            alternative.Add(new Pravilo.structRightSide("term",""));
            alternative.Add(new Pravilo.structRightSide("additional-term",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("", "lambda"));
            f.rightSide.Add(alternative);

            p.Add(f);

            //38
            f = new Pravilo();
            f.leftSide ="term";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("factor",""));
            alternative.Add(new Pravilo.structRightSide("mult-factor",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            p.Add(f);

            //39
            f = new Pravilo();
            f.leftSide ="mult-factor";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("mult-op",""));
            alternative.Add(new Pravilo.structRightSide("factor",""));
            alternative.Add(new Pravilo.structRightSide("mult-factor",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","lambda"));
            f.rightSide.Add(alternative);

            p.Add(f);


            //40
            f = new Pravilo();
            f.leftSide ="factor";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("perem",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("chislo",""));
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("string",""));
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("id-const",""));
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","("));
            alternative.Add(new Pravilo.structRightSide("virag",""));
            alternative.Add(new Pravilo.structRightSide("",")"));
            f.rightSide.Add(alternative);


            p.Add(f);


            //41
            f = new Pravilo();
            f.leftSide ="additional-op";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","+"));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","-"));
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","or"));
            f.rightSide.Add(alternative);

            p.Add(f);

            //42             
            f = new Pravilo();
            f.leftSide ="mult-op";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","*"));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","/"));
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","div"));
            f.rightSide.Add(alternative);


            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","mod"));
            f.rightSide.Add(alternative);


            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","and"));
            f.rightSide.Add(alternative);

            p.Add(f);


            //43
            f = new Pravilo();
            f.leftSide ="perem";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("id", ""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("id", ""));
            alternative.Add(new Pravilo.structRightSide("", "["));
            alternative.Add(new Pravilo.structRightSide("chislo-sign", ""));
            alternative.Add(new Pravilo.structRightSide("", "]"));
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("id", ""));
            alternative.Add(new Pravilo.structRightSide("", "["));
            alternative.Add(new Pravilo.structRightSide("id", ""));
            alternative.Add(new Pravilo.structRightSide("", "]"));
            f.rightSide.Add(alternative);

            p.Add(f);

            //44
            f = new Pravilo();
            f.leftSide ="in-out";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("in",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("out",""));
            f.rightSide.Add(alternative);

            p.Add(f);


            //45
            f = new Pravilo();
            f.leftSide ="in";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","read"));
            alternative.Add(new Pravilo.structRightSide("", "("));
            alternative.Add(new Pravilo.structRightSide("perem2",""));
            alternative.Add(new Pravilo.structRightSide("",")"));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","readln"));
            alternative.Add(new Pravilo.structRightSide("", "("));
            alternative.Add(new Pravilo.structRightSide("perem2",""));
            alternative.Add(new Pravilo.structRightSide("",")"));
            f.rightSide.Add(alternative);

            p.Add(f);



            //46
            f = new Pravilo();
            f.leftSide ="perem2";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("perem",""));
            alternative.Add(new Pravilo.structRightSide("perem1",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","lambda"));
            f.rightSide.Add(alternative);

            p.Add(f);


            //47
            f = new Pravilo();
            f.leftSide ="perem1";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("",","));
            alternative.Add(new Pravilo.structRightSide("perem",""));
            alternative.Add(new Pravilo.structRightSide("perem1",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","lambda"));
            f.rightSide.Add(alternative);

            p.Add(f);



            //48
            f = new Pravilo();
            f.leftSide ="out";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","write"));
            alternative.Add(new Pravilo.structRightSide("", "("));
            alternative.Add(new Pravilo.structRightSide("virag2",""));
            alternative.Add(new Pravilo.structRightSide("",")"));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","writeln"));
            alternative.Add(new Pravilo.structRightSide("", "("));
            alternative.Add(new Pravilo.structRightSide("virag2",""));
            alternative.Add(new Pravilo.structRightSide("",")"));
            f.rightSide.Add(alternative);

            p.Add(f);



            //49
            f = new Pravilo();
            f.leftSide ="virag2";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("virag",""));
            alternative.Add(new Pravilo.structRightSide("virag1",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","lambda"));
            f.rightSide.Add(alternative);

            p.Add(f);


            //50
            f = new Pravilo();
            f.leftSide ="virag1";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("",","));
            alternative.Add(new Pravilo.structRightSide("virag",""));
            alternative.Add(new Pravilo.structRightSide("virag1",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","lambda"));
            f.rightSide.Add(alternative);

            p.Add(f);


            //51
            f = new Pravilo();
            f.leftSide ="chislo";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("integer",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("real",""));
            f.rightSide.Add(alternative);

            p.Add(f);


            //52
            f = new Pravilo();
            f.leftSide ="integer";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("chislo-sign",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            p.Add(f);


            //53
            f = new Pravilo();
            f.leftSide ="real";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("chislo-sign",""));
            alternative.Add(new Pravilo.structRightSide("chislo-wout-sign1",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            p.Add(f);



            //54
            f = new Pravilo();
            f.leftSide ="chislo-wout-sign1";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","."));
            alternative.Add(new Pravilo.structRightSide("chislo-wout-sign",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","lambda"));
            f.rightSide.Add(alternative);

            p.Add(f);



            //55
            f = new Pravilo();
            f.leftSide ="chislo-sign";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("sign1",""));
            alternative.Add(new Pravilo.structRightSide("chislo-wout-sign",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            p.Add(f);



            //56
            f = new Pravilo();
            f.leftSide ="chislo-wout-sign";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("digit",""));
            alternative.Add(new Pravilo.structRightSide("digit1",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            p.Add(f);


            //57
            f = new Pravilo();
            f.leftSide ="digit1";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("digit",""));
            alternative.Add(new Pravilo.structRightSide("digit1",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","lambda"));
            f.rightSide.Add(alternative);

            p.Add(f);



            //58
            f = new Pravilo();
            f.leftSide ="string";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","'"));
            alternative.Add(new Pravilo.structRightSide("char",""));
            alternative.Add(new Pravilo.structRightSide("char1",""));
            alternative.Add(new Pravilo.structRightSide("","'"));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            p.Add(f);


            //59
            f = new Pravilo();
            f.leftSide ="char1";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("char",""));
            alternative.Add(new Pravilo.structRightSide("char1",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","lambda"));
            f.rightSide.Add(alternative);

            p.Add(f);


            //60
            f = new Pravilo();
            f.leftSide ="char";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","''"));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","lambda")); //после пост обработки будут выбраны все строковые символы символы
            f.rightSide.Add(alternative);

            p.Add(f);


            //61
            f = new Pravilo();
            f.leftSide ="id";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("letter",""));
            alternative.Add(new Pravilo.structRightSide("letter-or-digit",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            p.Add(f);


            //62
            f = new Pravilo();
            f.leftSide ="letter-or-digit";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("letter",""));
            alternative.Add(new Pravilo.structRightSide("letter-or-digit",""));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("digit",""));
            alternative.Add(new Pravilo.structRightSide("letter-or-digit",""));
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","lambda"));
            f.rightSide.Add(alternative);

            p.Add(f);

            //63
            f = new Pravilo();
            f.leftSide ="sign";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","+"));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("","-"));
            f.rightSide.Add(alternative);

            p.Add(f);

            //64
            f = new Pravilo();
            f.leftSide ="digit";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("", Convert.ToChar(48).ToString()));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);
            for (int i = 49; i <= 57; i++)
            {
                alternative = new List<Pravilo.structRightSide>();
                alternative.Add(new Pravilo.structRightSide("", Convert.ToChar(i).ToString()));
                f.rightSide.Add(alternative);
            }
            p.Add(f);

            //65
            f = new Pravilo();
            f.leftSide ="letter";

            alternative = new List<Pravilo.structRightSide>();
            alternative.Add(new Pravilo.structRightSide("", Convert.ToChar(65).ToString()));
            f.rightSide = new List<List<Pravilo.structRightSide>>();
            f.rightSide.Add(alternative);
            for (int i = 66; i <= 90; i++)
            {
                alternative = new List<Pravilo.structRightSide>();
                alternative.Add(new Pravilo.structRightSide("", Convert.ToChar(i).ToString()));
                f.rightSide.Add(alternative);
            }
            for (int i = 97; i <= 122; i++)
            {
                alternative = new List<Pravilo.structRightSide>();
                alternative.Add(new Pravilo.structRightSide("", Convert.ToChar(i).ToString()));
                f.rightSide.Add(alternative);
            }
            for (int i = 1040; i <= 1103; i++)
            {
                alternative = new List<Pravilo.structRightSide>();
                alternative.Add(new Pravilo.structRightSide("", Convert.ToChar(i).ToString()));
                f.rightSide.Add(alternative);
            }
            p.Add(f);
        }




    }
}
