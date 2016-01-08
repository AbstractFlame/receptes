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
        public static Lang currentLang;

        public static string TranslateFromDictionary(string english)
        {
            string expectedLang;
            switch (currentLang)
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

        public static string TranslateGroup(string english)
        {
            string expectedLang;
            switch (currentLang)
            {
                case LanguageManager.Lang.HUNGARIAN:
                    expectedLang = "Desc";
                    break;
                case LanguageManager.Lang.ENGLISH:
                    expectedLang = "Desc_en";

                    break;
                case LanguageManager.Lang.GERMAN:
                    expectedLang = "Desc_de";
                    break;
                case LanguageManager.Lang.SLOVENSKY:
                    expectedLang = "Desc_sk";
                    break;
                default:
                    expectedLang = "Desc";
                    break;
            }
            string sql = "SELECT " + expectedLang + " FROM T_Group WHERE Desc_en='" + english + "'";
            DataSet dataSet = DBManager.QueryDataSet(sql);
            return dataSet.Tables[0].Rows[0][0].ToString();
        }

        public static string TranslateSubGroup(string english)
        {
            string expectedLang;
            switch (currentLang)
            {
                case LanguageManager.Lang.HUNGARIAN:
                    expectedLang = "Desc";
                    break;
                case LanguageManager.Lang.ENGLISH:
                    expectedLang = "Desc_en";

                    break;
                case LanguageManager.Lang.GERMAN:
                    expectedLang = "Desc_de";
                    break;
                case LanguageManager.Lang.SLOVENSKY:
                    expectedLang = "Desc_sk";
                    break;
                default:
                    expectedLang = "Desc";
                    break;
            }
            string sql = "SELECT " + expectedLang + " FROM T_Subgroup WHERE Desc_en='" + english + "'";
            DataSet dataSet = DBManager.QueryDataSet(sql);
            return dataSet.Tables[0].Rows[0][0].ToString();
        }

        public static string TranslateIngredient(string english)
        {
            string expectedLang;
            switch (currentLang)
            {
                case LanguageManager.Lang.HUNGARIAN:
                    expectedLang = "Desc";
                    break;
                case LanguageManager.Lang.ENGLISH:
                    expectedLang = "Desc_en";

                    break;
                case LanguageManager.Lang.GERMAN:
                    expectedLang = "Desc_de";
                    break;
                case LanguageManager.Lang.SLOVENSKY:
                    expectedLang = "Desc_sk";
                    break;
                default:
                    expectedLang = "Desc";
                    break;
            }
            string sql = "SELECT " + expectedLang + " FROM T_Ingredient WHERE Desc_en='" + english + "'";
            DataSet dataSet = DBManager.QueryDataSet(sql);
            return dataSet.Tables[0].Rows[0][0].ToString();
        }
    }
}
