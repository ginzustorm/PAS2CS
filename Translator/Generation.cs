using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator
{
    class Generation
    {

        String sharpCode = "", specEndings = "";
        int deepCount=0;
        String deep = "      ";

        public Generation()
        {
        }
        public bool GoGenerate()
        {
            StreamWriter csCode;
            try
            {
                csCode = new StreamWriter(Path.GetFullPath(Code.PasPath).Replace(".pas", ".cs"), false, Encoding.ASCII);
            }
            catch (Exception e)
            {
                Code.GenerationError = "Ошибка при создании файла: " + e.Message;
                return false;
            }
            sharpCode = "using System;\nclass ";
            sharpCode += Code.Tokens[1].value + "\n{\n" + "   static void Main()\n   {\n";
            specEndings += "   }\n}";

            //Обьявляем переменные первая начинается с Code.Tokens[4]
            for (int index = 1; index < Semantics.ids.Count; index++)
            {
                String type;
                switch (Semantics.ids[index].type)
                {
                    case "boolean":
                        type = "bool";
                        break;
                    case "real":
                        type = "double";
                        break;
                    case "integer":
                        type = "int";
                        break;
                    case "string":
                        type = "string";
                        break;
                    default:
                        Code.GenerationError = "Замечен недопустимый тип переменной";
                        return false;
                }
                if (!Semantics.ids[index].array)
                {
                    if (!Semantics.ids[index].constant)
                    {
                        if (type == "int" || type == "double")
                        {
                            sharpCode += deep + type + " " + Semantics.ids[index].key + " = 0;\n";
                        }
                        else
                        {
                            sharpCode += deep + type + " " + Semantics.ids[index].key + " = \"\";\n";
                        }

                    }
                    else
                        sharpCode += deep + "const " + type + " " + Semantics.ids[index].key + " = " + Code.Tokens[Code.Tokens.FindIndex(x => x.value == Semantics.ids[index].key) + 2].value + ";\n";


                }
                else
                {
                    String arrayBegining, arrayEnding;
                    int indexCountOFEleemnts = Code.Tokens.FindIndex(Code.Tokens.IndexOf(Code.Tokens.Find(x=>x.value==Semantics.ids[index].key&&x.klass=="идентификатор")),x => x.value == "," || x.value == "]")-1;
                    //Берём верхнюю границу массива, увеличиваем её на 1 и получаем количество элементов в массиве
                    Int32 aCount = Int32.Parse(Code.Tokens[indexCountOFEleemnts].value)+1;
                    arrayBegining = deep + type + "[]";
                    arrayEnding = " = new " + type + "[" + aCount.ToString() + "]";
                    //проверка количества размерностей у массива
                    indexCountOFEleemnts++;
                    while (Code.Tokens[indexCountOFEleemnts].value == ",")
                    {
                        arrayBegining += "[]";
                        indexCountOFEleemnts = Code.Tokens.FindIndex(indexCountOFEleemnts + 1, x => x.value == "," || x.value == "]") - 1;
                        aCount = Int32.Parse(Code.Tokens[indexCountOFEleemnts].value) + 1;
                        arrayEnding += "[" + aCount.ToString() + "]";
                        indexCountOFEleemnts++;
                    }
                    sharpCode += arrayBegining + Semantics.ids[index].key + arrayEnding + ";\n";
                }
            }


            //Основная часть программы
            int indexT = Code.Tokens.FindIndex(x=>x.value=="begin")+1;



            while (indexT < Code.Tokens.Count - 2)
            {
                switch (Code.Tokens[indexT].value)
                {
                    case ":=":
                        indexT++;
                        sharpCode += " = ";
                        break;
                    case "begin":
                        sharpCode += "\n" + deep + "{\n";
                        deep += "      ";
                        indexT++;
                        break;
                    case "end":
                        deep = deep.Remove(0, 6);
                        sharpCode += deep + "}\n";
                        indexT++;
                        break;
                    case "read":
                        {
                            indexT++;
                            bool read=false;
                            String tmp = " = Console.Read();\n";
                            while (Code.Tokens[++indexT].value != ")")
                            {
                                if (Code.Tokens[indexT].value != ",")
                                {
                                    sharpCode += deep + Code.Tokens[indexT].value + tmp;
                                    read = true;
                                }
                            }
                            if (!read)
                                sharpCode +=deep+ "Console.ReadLine();\n";
                            indexT += 2;
                        }
                        break;
                    case "readln":
                        {
                            indexT++;
                            bool read=false;
                            String tmp = " = Console.ReadLine();\n";
                            while (Code.Tokens[++indexT].value != ")")
                            {
                                if (Code.Tokens[indexT].value != ",")
                                {
                                    sharpCode += deep + Code.Tokens[indexT].value + tmp;
                                    read = true;
                                }
                            }
                            if (!read)
                                sharpCode += deep+ "Console.ReadLine();\n";
                            indexT += 2;
                        }
                        break;
                    case "write":
                        {
                            indexT++;
                            sharpCode += deep + "Console.Write(";
                            while (Code.Tokens[++indexT].value != ")")
                            {
                                if (Code.Tokens[indexT].value == ",")
                                    sharpCode += "+";
                                else
                                    sharpCode += Code.Tokens[indexT].value.Replace("'", "\"");
                            }
                            sharpCode += ");\n";
                            indexT += 2;
                        }
                        break;
                    case "writeln":
                        {
                            indexT++;
                            sharpCode += deep + "Console.WriteLine(";
                            while (Code.Tokens[++indexT].value != ")")
                            {
                                if (Code.Tokens[indexT].value == ",")
                                    sharpCode += "+";
                                else
                                    sharpCode += Code.Tokens[indexT].value.Replace("'", "\"");
                            }
                            sharpCode += ");\n";
                            indexT += 2;
                        }
                        break;

                    case "if":
                        {
                            indexT++;
                            sharpCode += deep + "if (";
                            while (Code.Tokens[indexT].value != "then")
                            {
                                switch (Code.Tokens[indexT].value)
                                {
                                    case "and":
                                        sharpCode += "&&";
                                        break;
                                    case "or":
                                        sharpCode += "||";
                                        break;
                                    case "<>":
                                        sharpCode += "!=";
                                        break;
                                    case "mod":
                                        sharpCode += "%";
                                        break;
                                    case "div":
                                        sharpCode += "/";
                                        break;
                                    case "=":
                                        sharpCode += "==";
                                        break;
                                    default:
                                        sharpCode += Code.Tokens[indexT].value.Replace("'", "\"");
                                        break;
                                }
                                indexT++;
                            }
                            sharpCode += ") ";
                        }
                        break;
                    case "while":
                        {
                            indexT++;
                            sharpCode += deep + "while (";
                            while (Code.Tokens[indexT].value != "do")
                            {
                                switch (Code.Tokens[indexT].value)
                                {
                                    case "and":
                                        sharpCode += "&&";
                                        break;
                                    case "or":
                                        sharpCode += "||";
                                        break;
                                    case "=":
                                        sharpCode += "==";
                                        break;
                                    case "mod":
                                        sharpCode += "%";
                                        break;
                                    case "div":
                                        sharpCode += "/";
                                        break;
                                    default:
                                        sharpCode += Code.Tokens[indexT].value.Replace("'", "\"");
                                        break;
                                }
                                indexT++;
                            }
                            sharpCode += ") ";
                        }
                        break;
                    case "mod":
                        {
                            indexT++;
                            sharpCode += "%";
                        }
                        break;
                    case "div":
                        {
                            indexT++;
                            sharpCode += "/";
                        }
                        break;
                    case "for":
                        {
                            indexT++;
                            bool to=true;
                            String id = Code.Tokens[indexT].value;
                            sharpCode += deep + "for (";
                            while (Code.Tokens[indexT].value != "do")
                            {
                                switch (Code.Tokens[indexT].value)
                                {
                                    case "mod":
                                        sharpCode += "%";
                                        break;
                                    case "div":
                                        sharpCode += "/";
                                        break;
                                    case ":=":
                                        sharpCode += "=";
                                        break;
                                    case "downto":
                                        {
                                            to = false;
                                            sharpCode += "; " + id + ">=";
                                        }
                                        break;
                                    case "to":
                                        {
                                            to = true;
                                            sharpCode += "; " + id + "<=";
                                        }
                                        break;
                                    default:
                                        sharpCode += Code.Tokens[indexT].value.Replace("'", "\"");
                                        break;
                                }
                                indexT++;
                            }
                            if (to)
                                sharpCode += "; " + id + "++";
                            else
                                sharpCode += "; " + id + "--";
                            sharpCode += ") ";
                        }
                        break;
                    case "else":
                        {
                            indexT++;
                            sharpCode += deep + "else";
                        }
                        break;

                    case ";":
                        {
                            indexT++;
                            if (sharpCode[sharpCode.Length - 2] != '}')
                                sharpCode += ";\n";
                        }
                        break;
                    case "[":
                        indexT++;
                        sharpCode += "[";
                        break;
                    case "]":
                        indexT++;
                        sharpCode += "]";
                        break;
                    case "+":
                        indexT++;
                        sharpCode += "+";

                        if (Code.Tokens[indexT].value == "1")
                        {
                            int specIndexR = Code.Tokens.FindLastIndex(indexT, x => x.value == ":=")+1;
                            Token cheker = Code.Tokens[specIndexR-1];
                            int specIndexL = Code.Tokens.FindLastIndex(indexT, x=> x.value==";"||x.value=="begin")+1;
                            if (specIndexL<specIndexR)
                            {
                                while (Code.Tokens[specIndexL].value==Code.Tokens[specIndexR].value)
                                {
                                    specIndexR++;
                                    specIndexL++;
                                }
                                if (Code.Tokens[specIndexL]==cheker)
                                {
                                    int lastIndexDel = sharpCode.LastIndexOf("=")+2;
                                    int indexDel = sharpCode.LastIndexOf(";");
                                    if (indexDel < sharpCode.LastIndexOf("}"))
                                        indexDel = sharpCode.LastIndexOf("}");
                                    if (indexDel < sharpCode.LastIndexOf("{"))
                                        indexDel = sharpCode.LastIndexOf("{");
                                    indexDel += 2 + deep.Length;
                                    sharpCode =sharpCode.Remove(indexDel, lastIndexDel-indexDel);                                
                                    sharpCode += "+";
                                    indexT++;
                                }
                            } 
                        }    

                        break;
                    case "-":
                        indexT++;
                        sharpCode += "-";
 
                        if (Code.Tokens[indexT].value == "1")
                        {
                            int specIndexR = Code.Tokens.FindLastIndex(indexT, x => x.value == ":=")+1;
                            Token cheker = Code.Tokens[specIndexR-1];
                            int specIndexL = Code.Tokens.FindLastIndex(indexT, x=> x.value==";"||x.value=="begin")+1;
                            if (specIndexL < specIndexR)
                            {
                                while (Code.Tokens[specIndexL].value == Code.Tokens[specIndexR].value)
                                {
                                    specIndexR++;
                                    specIndexL++;
                                }
                                if (Code.Tokens[specIndexL] == cheker)
                                {
                                    int lastIndexDel = sharpCode.LastIndexOf("=")+2;
                                    int indexDel = sharpCode.LastIndexOf(";");
                                    if (indexDel < sharpCode.LastIndexOf("}"))
                                        indexDel = sharpCode.LastIndexOf("}");
                                    if (indexDel < sharpCode.LastIndexOf("{"))
                                        indexDel = sharpCode.LastIndexOf("{");
                                    indexDel += 2 + deep.Length;
                                    sharpCode = sharpCode.Remove(indexDel, lastIndexDel - indexDel);
                                    sharpCode += "-";
                                    indexT++;
                                }
                            }
                        }
                        break;
                    case "*":
                        indexT++;
                        sharpCode += "*";
                        break;
                    case "/":
                        indexT++;
                        sharpCode += "/";
                        break;
                    case "(":
                        indexT++;
                        sharpCode += "(";
                        break;
                    case ")":
                        indexT++;
                        sharpCode += ")";
                        break;
                    default:
                        if ((Code.Tokens[indexT].klass == "идентификатор") || (Code.Tokens[indexT].klass == "строка") || (Code.Tokens[indexT].klass == "число   "))
                            if (sharpCode[sharpCode.Length - 1] == '\n')
                                sharpCode += deep + Code.Tokens[indexT].value.Replace("'", "\"");
                            else
                                sharpCode += Code.Tokens[indexT].value.Replace("'", "\"");
                        indexT++;
                        break;
                }
            }


            csCode.WriteLine(sharpCode + specEndings);
            csCode.Close();
            return true;
        }
    }
}
