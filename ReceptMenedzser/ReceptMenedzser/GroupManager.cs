using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Data;

namespace ReceptMenedzser
{
    public class GroupManager
    {
        public enum AlabontasType { NINCS, CSAK_FOCSOPORT, CSAK_ALCSOPORT, FO_ES_AL, OSSZETEVOK };

        private class TreeItem : TreeViewItem
        {
            public string Id;
            public string SQLSubGroupQuery;
            public int level;
            public TreeItem parentItem;
            public string pictureName;
            public TreeItem(string Id, string SQLSubGroupQuery, int level, string picturePath, TreeItem parentItem = null)
            {
                this.Id = Id;
                this.SQLSubGroupQuery = SQLSubGroupQuery;
                this.level = level;
                this.pictureName = picturePath;
                this.parentItem = parentItem;
            }
        }

        public static string GetSQLQuery(TreeViewItem selectedItem)
        {
            TreeItem selectedTreeItem = (TreeItem)selectedItem;
            if (selectedTreeItem.parentItem != null)
                return GetSQLQuery(selectedTreeItem.parentItem) + " INTERSECT " + selectedTreeItem.SQLSubGroupQuery;
            else
                return selectedTreeItem.SQLSubGroupQuery;
        }

        public static string getPicturePath(TreeViewItem selectedItem)
        {
            TreeItem selectedTreeItem = (TreeItem)selectedItem;
            if(selectedTreeItem.pictureName != "")
                switch(selectedTreeItem.level)
                {
                    case 1:
                        return System.AppDomain.CurrentDomain.BaseDirectory + "images\\Pictures\\" + selectedTreeItem.pictureName;
                    case 2:
                        return System.AppDomain.CurrentDomain.BaseDirectory + "images\\Pictures\\" + selectedTreeItem.pictureName;
                    case 3:
                        return System.AppDomain.CurrentDomain.BaseDirectory + "images\\Pictures\\" + selectedTreeItem.pictureName;
                    default:
                        return System.AppDomain.CurrentDomain.BaseDirectory + "images\\Pictures\\Cook.jpg";
                }
            else if (selectedTreeItem.parentItem != null)
                return getPicturePath(selectedTreeItem.parentItem);
            else
                return "";
        }

        public static void FillTreeView(TreeView treeView)
        {
            // fILL First item: "All"
            string SQLSubGroupQuery = "select * from recept";
            TreeItem treeItemLevel1 = new TreeItem("-1", SQLSubGroupQuery, 1, "Cook.jpg");
            treeItemLevel1.Header = LanguageManager.TranslateFromDictionary("24"); // all
            treeView.Items.Add(treeItemLevel1);

            string cartonCloumnName = LanguageManager.GetCartonColumnName();
            DataSet cartonDataSet = DBManager.QueryDataSet("SELECT * FROM T_Karton_UJ ORDER BY " + cartonCloumnName + " ASC");
            foreach (DataRow drLevel1 in cartonDataSet.Tables[0].Rows)
            {
                string id = drLevel1[0].ToString();
                string kartonName = LanguageManager.TranslateCarton(id);
                string imagePath = drLevel1["KartonKep"].ToString();
                AlabontasType type = (AlabontasType)Convert.ToInt32(drLevel1[8]); // alábontás típusa (felsorlás számként tárolva)
                SQLSubGroupQuery = drLevel1[6].ToString();
                string SQLMasodikAlabontas = drLevel1[9].ToString();
                treeItemLevel1 = new TreeItem(id, SQLSubGroupQuery, 1, imagePath);
                treeItemLevel1.Header = kartonName;
                switch (type)
                {
                    // 0 - nincs alábontás
                    case AlabontasType.NINCS:
                        // nothing to do
                        break;
                    case AlabontasType.CSAK_FOCSOPORT:
                        CsakFo(treeItemLevel1, SQLSubGroupQuery, SQLMasodikAlabontas);
                        break;
                    case AlabontasType.CSAK_ALCSOPORT:
                        CsakAl(treeItemLevel1, SQLSubGroupQuery, SQLMasodikAlabontas);
                        break;
                    case AlabontasType.FO_ES_AL:
                        FoEsAl(treeItemLevel1, SQLSubGroupQuery, SQLMasodikAlabontas);
                        break;
                    case AlabontasType.OSSZETEVOK:
                        Osszetevok(treeItemLevel1, SQLSubGroupQuery, SQLMasodikAlabontas);
                        break;
                }
                treeView.Items.Add(treeItemLevel1);
            }
        }

        private static void CsakFo(TreeItem treeItemLevel1, string SQLSubGroupQuery, string SQLMasodikAlabontas)
        {
            string groupCloumnName = LanguageManager.GetGroupColumnName();
            DataSet level2Dataset = DBManager.QueryDataSet("SELECT T_Group.Pic as Pic, T_Group.CS_ID as CS_ID FROM (" + SQLSubGroupQuery + ") AS recept_filtered JOIN T_Group WHERE recept_filtered.group_id = T_Group.CS_ID GROUP BY T_Group.CS_ID ORDER BY T_Group." + groupCloumnName + " ASC");
            foreach (DataRow drLevel2 in level2Dataset.Tables[0].Rows)
            {
                string idLevel2 = drLevel2["CS_ID"].ToString();
                string imagePath = drLevel2["Pic"].ToString();
                string nameLevel2 = LanguageManager.TranslateGroup(idLevel2);
                string SQLSubGroupQueryLevel2 = "SELECT * FROM recept WHERE recept.group_id = " + idLevel2;
                TreeItem treeItemLevel2 = new TreeItem(idLevel2, SQLSubGroupQueryLevel2, 2, imagePath, treeItemLevel1);
                treeItemLevel2.Header = nameLevel2;
                if (IsLevel3(SQLMasodikAlabontas, idLevel2))
                    BuildLevel3(treeItemLevel2, SQLSubGroupQueryLevel2);
                treeItemLevel1.Items.Add(treeItemLevel2);
            }
        }

        private static void CsakAl(TreeItem treeItemLevel1, string SQLSubGroupQuery, string SQLMasodikAlabontas)
        {
            string groupCloumnName = LanguageManager.GetGroupColumnName();
            DataSet level2Dataset = DBManager.QueryDataSet("SELECT T_Subgroup.Pic as Pic, T_Subgroup.SCS_ID as SCS_ID FROM (" + SQLSubGroupQuery + ") AS recept_filtered JOIN T_Subgroup WHERE recept_filtered.subgroup_id = T_Subgroup.SCS_ID GROUP BY T_Subgroup.SCS_ID ORDER BY T_Subgroup." + groupCloumnName + " ASC");
            foreach (DataRow drLevel2 in level2Dataset.Tables[0].Rows)
            {
                string idLevel2 = drLevel2["SCS_ID"].ToString();
                string imagePath = drLevel2["Pic"].ToString();
                string nameLevel2 = LanguageManager.TranslateSubGroup(idLevel2);
                string SQLSubGroupQueryLevel2 = "SELECT * FROM recept WHERE recept.subgroup_id = " + idLevel2;
                TreeItem treeItemLevel2 = new TreeItem(idLevel2, SQLSubGroupQueryLevel2, 2, imagePath, treeItemLevel1);
                treeItemLevel2.Header = nameLevel2;
                if (IsLevel3(SQLMasodikAlabontas, idLevel2))
                    BuildLevel3(treeItemLevel2, SQLSubGroupQueryLevel2);
                treeItemLevel1.Items.Add(treeItemLevel2);
            }
        }

        private static void FoEsAl(TreeItem treeItemLevel1, string SQLSubGroupQuery, string SQLMasodikAlabontas)
        {
            string groupCloumnName = LanguageManager.GetGroupColumnName();
            DataSet level2Dataset = DBManager.QueryDataSet("SELECT T_Group.Pic as Pic, T_Group.CS_ID as CS_ID FROM (" + SQLSubGroupQuery + ") AS recept_filtered JOIN T_Group WHERE recept_filtered.group_id = T_Group.CS_ID GROUP BY T_Group.CS_ID ORDER BY T_Group." + groupCloumnName + " ASC");
            foreach (DataRow drLevel2 in level2Dataset.Tables[0].Rows)
            {
                string idLevel2 = drLevel2["CS_ID"].ToString();
                string imagePath = drLevel2["Pic"].ToString();
                string nameLevel2 = LanguageManager.TranslateGroup(idLevel2);
                string SQLSubGroupQueryLevel2 = "SELECT * FROM recept WHERE recept.group_id = " + idLevel2;
                TreeItem treeItemLevel2 = new TreeItem(idLevel2, SQLSubGroupQueryLevel2, 2, imagePath, treeItemLevel1);
                treeItemLevel2.Header = nameLevel2;
                if (IsLevel3(SQLMasodikAlabontas, idLevel2))
                    BuildLevel3(treeItemLevel2, SQLSubGroupQueryLevel2);
                treeItemLevel1.Items.Add(treeItemLevel2);
            }

            level2Dataset = DBManager.QueryDataSet("SELECT T_Subgroup.Pic as Pic, T_Subgroup.SCS_ID as SCS_ID FROM (" + SQLSubGroupQuery + ") AS recept_filtered JOIN T_Subgroup WHERE recept_filtered.group_id = T_Subgroup.SCS_ID GROUP BY T_Subgroup.SCS_ID ORDER BY T_Subgroup." + groupCloumnName + " ASC");
            foreach (DataRow drLevel2 in level2Dataset.Tables[0].Rows)
            {
                string idLevel2 = drLevel2["SCS_ID"].ToString();
                string imagePath = drLevel2["Pic"].ToString();
                string nameLevel2 = LanguageManager.TranslateSubGroup(idLevel2);
                string SQLSubGroupQueryLevel2 = "SELECT * FROM recept WHERE recept.subgroup_id = " + idLevel2;
                TreeItem treeItemLevel2 = new TreeItem(idLevel2, SQLSubGroupQueryLevel2, 2, imagePath, treeItemLevel1);
                treeItemLevel2.Header = nameLevel2;
                if (IsLevel3(SQLMasodikAlabontas, idLevel2))
                    BuildLevel3(treeItemLevel2, SQLSubGroupQueryLevel2);
                treeItemLevel1.Items.Add(treeItemLevel2);
            }
        }

        private static void Osszetevok(TreeItem treeItemLevel1, string SQLSubGroupQuery, string SQLMasodikAlabontas)
        {
            string groupCloumnName = LanguageManager.GetGroupColumnName();
            DataSet level2Dataset = DBManager.QueryDataSet("SELECT T_Ingredient.Pic as Pic, T_Ingredient.SCS_ID as SCS_ID FROM (" + SQLSubGroupQuery + ") AS recept_filtered JOIN T_Ingredient WHERE recept_filtered.fo_osszetevo_id = T_Ingredient.SCS_ID GROUP BY T_Ingredient.SCS_ID ORDER BY T_Ingredient." + groupCloumnName + " ASC");
            foreach (DataRow drLevel2 in level2Dataset.Tables[0].Rows)
            {
                string idLevel2 = drLevel2["SCS_ID"].ToString();
                string imagePath = drLevel2["Pic"].ToString();
                string nameLevel2 = LanguageManager.TranslateIngredient(idLevel2);
                string SQLSubGroupQueryLevel2 = "SELECT * FROM recept WHERE recept.fo_osszetevo_id = " + idLevel2;
                TreeItem treeItemLevel2 = new TreeItem(idLevel2, SQLSubGroupQueryLevel2, 2, imagePath, treeItemLevel1);
                treeItemLevel2.Header = nameLevel2;
                if (IsLevel3(SQLMasodikAlabontas, idLevel2))
                    BuildLevel3(treeItemLevel2, SQLSubGroupQueryLevel2);
                treeItemLevel1.Items.Add(treeItemLevel2);
            }
        }

        // csak subgroup esetén működik helyesen!
        private static bool IsLevel3(string SQLMasodikAlabontas, string subgroupId)
        {
            if (SQLMasodikAlabontas == "")
                return false;

            DataSet dataSet = DBManager.QueryDataSet("SELECT * FROM (" + SQLMasodikAlabontas + ") AS recept_filtered JOIN T_Subgroup WHERE recept_filtered.group_id = " + subgroupId + " GROUP BY T_Subgroup.SCS_ID");
            return dataSet.Tables[0].Rows.Count > 0;
        }

        // csak subgroup esetén működik helyesen!
        private static void BuildLevel3(TreeItem treeItemLevel2, string SQLSubGroupQueryLevel2)
        {
            string groupCloumnName = LanguageManager.GetGroupColumnName();
            DataSet level3Dataset = DBManager.QueryDataSet("SELECT T_Ingredient.Pic as Pic, T_Ingredient.SCS_ID as SCS_ID FROM (" + SQLSubGroupQueryLevel2 + ") AS recept_filtered JOIN T_Ingredient WHERE recept_filtered.fo_osszetevo_id = T_Ingredient.SCS_ID GROUP BY T_Ingredient.SCS_ID ORDER BY T_Ingredient." + groupCloumnName + " ASC");
            foreach (DataRow drLevel3 in level3Dataset.Tables[0].Rows)
            {
                string idLevel3 = drLevel3["SCS_ID"].ToString();
                string imagePath = drLevel3["Pic"].ToString();
                string nameLevel3 = LanguageManager.TranslateIngredient(idLevel3);
                string SQLSubGroupQueryLevel3 = "SELECT * FROM recept WHERE recept.fo_osszetevo_id = " + idLevel3;
                TreeItem treeItemLevel3 = new TreeItem(idLevel3, SQLSubGroupQueryLevel3, 3, imagePath, treeItemLevel2);
                treeItemLevel3.Header = nameLevel3;
                treeItemLevel2.Items.Add(treeItemLevel3);
            }
        }
    }
}
