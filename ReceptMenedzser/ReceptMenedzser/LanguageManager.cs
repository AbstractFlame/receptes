using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ReceptMenedzser
{

    public class LanguageManager
    {
        public enum Lang {HUNGARIAN, ENGLISH, GERMAN, SLOVENSKY};
        public static Lang schosenLang;

        public static string TranslateFromDictionary(string english, Lang lang)
        {
            string expectedLang;
            switch (lang)
            {
                case LanguageManager.Lang.HUNGARIAN:
                    expectedLang = "magyar";
                    break;
                case LanguageManager.Lang.ENGLISH:
                    expectedLang = "english";

                    break;
                case LanguageManager.Lang.GERMAN:
                    expectedLang = "deutsch";
                    break;
                case LanguageManager.Lang.SLOVENSKY:
                    expectedLang = "slovensky";
                    break;
                default:
                    expectedLang = "magyar";
                    break;
            }
            string sql = "SELECT " + expectedLang + " FROM dictionary WHERE english='" + english + "'";
            DataSet dataSet = DBManager.QueryDataSet(sql);
            return dataSet.Tables[0].Rows[0][0].ToString();
        }
    }
}
