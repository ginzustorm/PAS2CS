using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Translator
{
    public static class Code
    {
        //Расположение .pas файла    
        public static String PasPath = "";
        //Расположение будущего .cs файла    
        public static String CsPath = "";
        //.pas файл
        public static String AllCode = "";
        //Ошибка лексического анализатора
        public static String LexError = "";
        //Ошибка синтаксического анализатора
        public static String SyntError = "";
        //Ошибка генератора кода
        public static String GenerationError;
        public static List<Token> Tokens = new List<Token>();
        public static List<String> Idents = new List<String>();
        

        //Вывод
        public static string conclusion;

    }


    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FirstForm());
        }
    }
}
