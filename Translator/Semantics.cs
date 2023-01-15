using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator
{
    class Semantics
    {
        public struct Ident
        {
            public String key;
            public String type;
            public bool array;
            public bool constant;
            public Ident(String k = "", String t = "", bool a = false, bool c = false)
            {
                key = k;
                type = t;
                array = a;
                constant = c;

            }
        }

        public static List<Ident> ids = new List<Ident>();
        public bool GoAnalazy()
        {




            //Собираем идентификаторы и их типы
            int indexBegin = Code.Tokens.IndexOf(Code.Tokens.Find(x=> x.value=="begin"));

            //Оптимизация - удаление лишних идентификаторов
            for (int i = 1; i < Code.Idents.Count; i++)
            {
                if (Code.Tokens.FindIndex(indexBegin, Code.Tokens.Count - indexBegin, x => x.value == Code.Idents[i]) == -1)
                {
                    int start =Code.Tokens.FindIndex(x => x.value == Code.Idents[i]);
                    int length = Code.Tokens.FindIndex(start,x=>x.value==","||x.value==";")-start+1;
                    Code.Tokens.RemoveRange(start, length);
                    Code.Idents.RemoveAt(i);
                    i--;
                }

            }

            indexBegin = Code.Tokens.IndexOf(Code.Tokens.Find(x => x.value == "begin"));

            foreach (var item in Code.Tokens)
            {
                int index = Code.Tokens.IndexOf(item);
                if ((index < indexBegin) && (item.klass == "идентификатор"))
                {
                    Code.Idents.Remove(item.value);
                    if (index != 1)
                    {
                        //Проверка на константу
                        if (Code.Tokens.FindIndex(x => x.value == "var") >= index)
                        {
                            ids.Add(new Ident(item.value, Code.Tokens[index + 2].type, false, true));
                        }
                        else
                        {
                            //Берём токен следующий за ограничителем : после текущего идентфикатора
                            String tmp = Code.Tokens[Code.Tokens.IndexOf(Code.Tokens.Find(x => x.value == ":" && Code.Tokens.IndexOf(x)>index), index, indexBegin - index)+1].value;
                            if (tmp != "array")
                                ids.Add(new Ident(item.value, tmp));
                            else
                            {
                                //Если встретили массив обрабатываем глубже
                                tmp = Code.Tokens[Code.Tokens.IndexOf(Code.Tokens.Find(x => x.value == "of" && Code.Tokens.IndexOf(x) > index), index, indexBegin - index) + 1].value;
                                ids.Add(new Ident(item.value, tmp, true));
                            }
                        }
                    }
                    else
                        //обозначаем тип названия программы
                        ids.Add(new Ident(item.value, "main"));

                }
            }

            if (Code.Idents.Count != 0)
            {
                int indexToken = Code.Tokens.IndexOf(Code.Tokens.Find(x=> x.value==Code.Idents[0]&& Code.Tokens.IndexOf(x)>indexBegin), indexBegin);
                Code.SyntError = "В строке " + Code.Tokens[indexToken].str_num + " столбце " + Code.Tokens[indexToken].pos_num + " встречен не объявленный идентификатор \"" + Code.Tokens[indexToken].value + "\" ";
                return false;
            }

            //Проверяем преобразование типов
            int indexT = indexBegin+1;
            while (indexT < Code.Tokens.Count && indexT > indexBegin)
            {
                if (Code.Tokens[indexT].klass != "идентификатор")
                    indexT = Code.Tokens.IndexOf(Code.Tokens.Find(x => x.klass == "идентификатор" && Code.Tokens.IndexOf(x) > indexT), indexT);

                if ((Code.Tokens[indexT + 1].value == ":=")
                 || (Code.Tokens[indexT + 1].value == "=")
                 || (Code.Tokens[indexT + 1].value == "<>")
                 || (Code.Tokens[indexT + 1].value == "<")
                 || (Code.Tokens[indexT + 1].value == "<=")
                 || (Code.Tokens[indexT + 1].value == ">")
                 || (Code.Tokens[indexT + 1].value == ">="))
                {
                    int indexLast;
                    if (Code.Tokens[indexT + 1].value == ":=")
                        indexLast = Code.Tokens.IndexOf(Code.Tokens.Find(x => (x.value == ";" || x.value == "begin") && Code.Tokens.IndexOf(x) > indexT), indexT);
                    else
                        indexLast = Code.Tokens.IndexOf(Code.Tokens.Find(x => (x.value == ")" || x.value == "then" || x.value == "begin") && Code.Tokens.IndexOf(x) > indexT), indexT);
                    for (int i = indexT + 2; i < indexLast; i++)
                    {
                        if ((Code.Tokens[i].klass == "строка") || (Code.Tokens[i].klass == "число   "))
                        {
                            if (Code.Tokens[i].type != ids.Find(x => x.key == Code.Tokens[indexT].value).type)
                            {
                                if (!((Code.Tokens[i].type == "integer") && ids.Find(x => x.key == Code.Tokens[indexT].value).type == "real"))
                                {
                                    Code.SyntError = "В строке " + Code.Tokens[indexT].str_num + " столбце " + Code.Tokens[indexT].pos_num + " правая часть выражения должна иметь тип \"" + ids.Find(x => x.key == Code.Tokens[indexT].value).type + "\" ";
                                    return false;
                                }

                            }

                        }
                        else
                        {
                            if (Code.Tokens[i].klass == "идентификатор")
                            {
                                if (ids.Find(x => x.key == Code.Tokens[indexT].value).type != ids.Find(x => x.key == Code.Tokens[i].value).type)
                                {
                                    if (!((Code.Tokens[i].type == "integer") && ids.Find(x => x.key == Code.Tokens[indexT].value).type == "real"))
                                    {
                                        Code.SyntError = "В строке " + Code.Tokens[indexT].str_num + " столбце " + Code.Tokens[indexT].pos_num + " правая часть выражения должна иметь тип \"" + ids.Find(x => x.key == Code.Tokens[indexT].value).type + "\", а встречен " + ids.Find(x => x.key == Code.Tokens[i].value).key + " с типом \"" + ids.Find(x => x.key == Code.Tokens[i].value).type + "\"";
                                        return false;
                                    }
                                }
                            }
                            if ((Code.Tokens[i].klass == "оператор" || Code.Tokens[i].klass == "ключевое слово") && (ids.Find(x => x.key == Code.Tokens[indexT].value).type == "string"))
                            {
                                if ((Code.Tokens[i].value != "+")
                                    && (Code.Tokens[i].value != "=")
                                    && (Code.Tokens[i].value != "<>")
                                    && (Code.Tokens[i].value != "<")
                                    && (Code.Tokens[i].value != "<=")
                                    && (Code.Tokens[i].value != ">")
                                    && (Code.Tokens[i].value != ">="))
                                {
                                    Code.SyntError = "В строке " + Code.Tokens[indexT].str_num + " строки можно только складывать";
                                    return false;
                                }
                            }
                            if (Code.Tokens[i].value == "[")
                            {
                                int p = Code.Tokens.IndexOf(Code.Tokens.Find(x => (x.value == "]") && Code.Tokens.IndexOf(x) > i), i);
                                for (; i < p; i++)
                                {
                                    if ((Code.Tokens[i].klass == "строка") || (Code.Tokens[i].klass == "число   "))
                                    {
                                        if (Code.Tokens[i].type != "integer")
                                        {
                                            Code.SyntError = "В строке " + Code.Tokens[indexT].str_num + " индекс массива должен иметь тип \"integer\" ";
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (Code.Tokens[i].klass == "идентификатор")
                                        {
                                            if (ids.Find(x => x.key == Code.Tokens[i].value).type != "integer")
                                            {
                                                Code.SyntError = "В строке " + Code.Tokens[indexT].str_num + " индекс массива должен иметь тип \"integer\" ";
                                                return false;
                                            }
                                        }
                                    }
                                }

                            }

                        }

                    }
                }

                indexT++;
            }
             

            return true;
        }
    }
}