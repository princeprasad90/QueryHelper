using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryHelper
{
    public static class QueryBuilder
    {
        public static string Insert(string Table, List<QueryModel> DictVal)
        {
            string SubKey = "";
            string SubVal = "";

            foreach (var entry in DictVal)
            {
                SubKey += $"{entry.Key},";
                SubVal += FormatSQLValue(entry.Value, entry.SQLType) + ',';
            }
            return $"INSERT INTO {Table} ({SubKey.TrimEnd(',')}) VALUES ({SubVal.TrimEnd(',')})  ";
        }
        public static string Select(string Table, List<QueryModel> DictVal, List<CondModel> DictCond, LogicOperator logic = LogicOperator.AND)
        {
            string selectVal = "";
            string SubCond = "";
            foreach (var entry in DictVal)
            {
                selectVal += $"{entry.Key},";
            }
            foreach (var cond in DictCond)
            {
                string logicc = logic.ToString();
                if (DictCond.LastOrDefault().Equals(cond))
                {
                    logicc = "";
                }
                SubCond += CreateComparisonClause(cond.Key, cond.Operator, cond.Value, cond.SQLType) + $" {logicc} ";
            }
            return $"SELECT {selectVal.TrimEnd(',')} FROM {Table} WHERE {SubCond.TrimEnd(',')}";
        }
        public static string Update(string Table, List<QueryModel> DictVal, List<CondModel> DictCond, LogicOperator logic = LogicOperator.AND)
        {
            string SubVal = "";
            string SubCond = "";
            foreach (var entry in DictVal)
            {
                SubVal += $" {entry.Key}=";
                SubVal += FormatSQLValue(entry.Value) + ',';
            }
            foreach (var cond in DictCond)
            {
                string logicc = logic.ToString();
                if (DictCond.LastOrDefault().Equals(cond))
                {
                    logicc = "";
                }
                SubCond += CreateComparisonClause(cond.Key, cond.Operator, cond.Value) + $" {logicc} ";
            }
            return $"UPDATE {Table} SET {SubVal.TrimEnd(',')} WHERE {SubCond.TrimEnd(',')}";
        }
        public static string UpdateWithoutCondition(string Table, List<QueryModel> DictVal)
        {
            string SubVal = "";
            foreach (var entry in DictVal)
            {
                SubVal += $" {entry.Key}=";
                SubVal += FormatSQLValue(entry.Value) + ',';
            }
            return $"UPDATE {Table} SET {SubVal.TrimEnd(',')} ";
        }


        internal static string GenCondition(List<ConditionList> condlist, LogicOperator logic)
        {
            string Cond = "";
            string logiccHead = logic.ToString();
            foreach (var item in condlist)
            {
                var SubCond = "";
                foreach (var item1 in item.CondList)
                {
                    string logicc = item.Logic.ToString();
                    if (item.CondList.LastOrDefault().Equals(item1))
                    {
                        logicc = "";
                    }
                    SubCond += CreateComparisonClause(item1.Key, item1.Operator, item1.Value) + $" {logicc} ";
                }
                if (condlist.LastOrDefault().Equals(item))
                {
                    logiccHead = "";
                }
                Cond += "(" + SubCond + ")" + logiccHead.ToString();
            }
            return Cond;
        }
        internal static string CreateComparisonClause(string fieldName, Comparison comparisonOperator, object value, SQLType type = SQLType.NON_SQL)
        {
            string Output = "";
            if (value != null && value != DBNull.Value)
            {
                switch (comparisonOperator)
                {
                    case Comparison.Equals:
                        Output = fieldName + " = " + FormatSQLValue(value, type); break;
                    case Comparison.NotEquals:
                        Output = fieldName + " <> " + FormatSQLValue(value, type); break;
                    case Comparison.GreaterThan:
                        Output = fieldName + " > " + FormatSQLValue(value, type); break;
                    case Comparison.GreaterOrEquals:
                        Output = fieldName + " >= " + FormatSQLValue(value, type); break;
                    case Comparison.LessThan:
                        Output = fieldName + " < " + FormatSQLValue(value, type); break;
                    case Comparison.LessOrEquals:
                        Output = fieldName + " <= " + FormatSQLValue(value, type); break;
                    case Comparison.Like:
                        Output = fieldName + " LIKE " + FormatSQLValue(value, type); break;
                    case Comparison.NotLike:
                        Output = "NOT " + fieldName + " LIKE " + FormatSQLValue(value, type); break;
                    case Comparison.In:
                        Output = fieldName + " IN (" + FormatSQLValue(value, type) + ")"; break;
                }
            }
            return Output;
        }
        internal static string FormatSQLValue(object someValue, SQLType type = SQLType.NON_SQL)
        {
            string FormattedValue = "";
            if (someValue == null)
            {
                FormattedValue = "NULL";
            }
            if (type == SQLType.SQL)
            {
                FormattedValue = someValue.ToString();
                return FormattedValue;
            }
            else
            {
                switch (someValue.GetType().Name)
                {
                    case "String": FormattedValue = "'" + ((string)someValue).Replace("'", "''") + "'"; break;
                    //   case "DateTime": FormattedValue = "'" + ((DateTime)someValue).ToString("yyyy/MM/dd hh:mm:ss") + "'"; break;
                    case "DBNull": FormattedValue = "NULL"; break;
                    case "Boolean": FormattedValue = (bool)someValue ? "1" : "0"; break;
                    //case "SqlLiteral": FormattedValue = ((SqlLiteral)someValue).Value; break;
                    default: FormattedValue = someValue.ToString(); break;
                }
            }
            return FormattedValue;
        }
    }
}
