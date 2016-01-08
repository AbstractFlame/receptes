﻿using System;
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

        public static string TranslateFromDictionary(string dictionary_id)
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
            string sql = "SELECT " + expectedLang + " FROM dictionary WHERE dic_id='" + dictionary_id + "'";
            DataSet dataSet = DBManager.QueryDataSet(sql);
            return dataSet.Tables[0].Rows[0][0].ToString();
        }

        public static string TranslateCarton(string CartonId)
        {
            string expectedLang;
            switch (currentLang)
            {
                case LanguageManager.Lang.HUNGARIAN:
                    expectedLang = "KartonNev";
                    break;
                case LanguageManager.Lang.ENGLISH:
                    expectedLang = "KartonNev_en";
                    break;
                case LanguageManager.Lang.GERMAN:
                    expectedLang = "KartonNev_de";
                    break;
                case LanguageManager.Lang.SLOVENSKY:
                    expectedLang = "KartonNev_sk";
                    break;
                default:
                    expectedLang = "KartonNev";
                    break;
            }
            string sql = "SELECT " + expectedLang + " FROM T_Karton_UJ WHERE Karton_ID='" + CartonId + "'";
            DataSet dataSet = DBManager.QueryDataSet(sql);
            return dataSet.Tables[0].Rows[0][0].ToString();
        }

        public static string TranslateGroup(string groupId)
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
            string sql = "SELECT " + expectedLang + " FROM T_Group WHERE CS_ID='" + groupId + "'";
            DataSet dataSet = DBManager.QueryDataSet(sql);
            return dataSet.Tables[0].Rows[0][0].ToString();
        }

        public static string TranslateSubGroup(string subGroupId)
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
            string sql = "SELECT " + expectedLang + " FROM T_Subgroup WHERE SCS_ID='" + subGroupId + "'";
            DataSet dataSet = DBManager.QueryDataSet(sql);
            return dataSet.Tables[0].Rows[0][0].ToString();
        }

        public static string TranslateIngredient(string ingredientId)
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
            string sql = "SELECT " + expectedLang + " FROM T_Ingredient WHERE SCS_ID='" + ingredientId + "'";
            DataSet dataSet = DBManager.QueryDataSet(sql);
            return dataSet.Tables[0].Rows[0][0].ToString();
        }
    }
}
