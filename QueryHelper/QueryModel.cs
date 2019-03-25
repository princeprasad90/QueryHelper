using System.Collections.Generic;

namespace QueryHelper
{
    public class ConditionList
    {
        public List<CondModel> CondList { get; set; }
        public LogicOperator Logic { get; set; }
    }
    public class QueryModel
    {
        public string Key { get; set; }
        public dynamic Value { get; set; }
        public SQLType SQLType { get; set; }

    }
    public class CondModel
    {
        public string Key { get; set; }
        public Comparison Operator { get; set; }
        public dynamic Value { get; set; }
        public SQLType SQLType { get; set; }
    }
    public enum Comparison
    {
        Equals,
        NotEquals,
        Like,
        NotLike,
        GreaterThan,
        GreaterOrEquals,
        LessThan,
        LessOrEquals,
        In
    }
    public enum LogicOperator
    {
        AND,
        OR,
        NOT
    }
    public enum SQLType
    {
        SQL = 1,
        NON_SQL = 0,
    }
}
