using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Translator
{
    public class Token
    {
        public Token(string kl = "",string va= "",string ty = "",int ad=0,int st=0,int po=0)
        {
            klass = kl;
            value =va;
            type=ty;
            adress =ad;
            str_num= st;
            pos_num = po;
            count++;
        }
        public static List<string> iden = new List <string>(); //список идентификаторов
        public static List<string> keys = new List <string>(); // список ключевых слов
        public static int count=0;
        public static int NUM_STR = 1;
        public static int NUM_POS = 0; 
        public string klass; // класс лексемы
        public string value; // значение лексемы
        public string type; // тип лексемы
        public int adress; //  адрес лексемы в таблице имён или таблице ключей
        public int str_num; // номер строки
        public int pos_num; // номер позиции в строке

       
        public void conclude()
        {

            string res = klass + '\t' + value + '\t' + type + '\t' + adress.ToString() + '\t' + str_num.ToString() + '\t' + pos_num.ToString() + "\r\n";
            File.AppendAllText("result.txt", res);
            
        }
        public void define(string lex, int id, ref string text)
        {
            int boo=0;
            int j = 0;
            int p = 0;
            int k = 0;
            int t = 0;
            if (id != 1)
            {
                k = text.IndexOf(lex);
                t = text.IndexOf(lex);
            }
            else
            {
                k = text.IndexOf('\'');
                t = k;
            }
                while (j<t)
                {
                    if (text[j]=='\r')
                    {
                        boo++;
                        NUM_STR++;
                        NUM_POS = 0;
                        k = k-p-2;
                        p = 0;
                        j++;
                    }
                    p++;
                    j++;
                }
                bool log = true;
                int a = 0;
                int st = 0;
                int len = lex.Length;
                if (id==1)
                {
                    int i=k;
                    while (i<lex.Length)
                    {
                        if (text[i]=='\r')
                        {
                            st++;
                            log = false;
                            a = i + 2;
                            len = len + 2;
                        }
                        i++;
                    }
                }
                if (boo != 0) boo--;
                pos_num = k+1+NUM_POS+boo;
                str_num = NUM_STR;
                if (log) NUM_POS = NUM_POS + k + lex.Length+boo;
                else
                {
                    NUM_POS = len - a;
                    NUM_STR = NUM_STR + st;
                }
                text = text.Substring(j+lex.Length);
                
                
           
    

            lex = lex.ToLower();
            value = lex;
            switch(id)
            {
                case 1:
                    klass = "строка";
                    type = "string";
                    break;
                case 2:
                    klass = "число   ";
                    if (lex.IndexOf('.') > 0) type = "real";
                    else type = "integer";
                    break;
                case 3:
                    klass = "оператор";
                    break;
                case 4:
                    klass = "ограничитель";
                    break;
                case 5:
                    if (keys.Contains(lex))
                    {
                        klass = "ключевое слово";
                        adress = keys.IndexOf(lex);
                    }
                    else
                    {
                        klass = "идентификатор";
                        if (iden.Contains(lex)) adress = iden.IndexOf(lex);
                        else
                        {
                            iden.Add(lex);
                            adress = iden.IndexOf(lex);
                        }
                    }
                    if (lex == "true" || lex == "false") type = "boolean";
                    break; 
            }
        }
       
    }
}
