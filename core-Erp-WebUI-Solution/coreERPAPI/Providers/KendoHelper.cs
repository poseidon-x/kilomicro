using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace coreERP
{
    public static class KendoHelper
    {
        public static void getSortOrder(KendoRequest req, ref string order)
        {
            // order the results
            if (req.sort != null && req.sort.Count > 0)
            {
                List<string> sorts = new List<string>();
                req.sort.ForEach(x =>
                {
                    sorts.Add(string.Format("{0} {1}", x.field, x.dir));
                });

                order = string.Join(",", sorts.ToArray());
            } 
        }

        public static async Task<string> getSortOrderAsync(KendoRequest req, string order)
        {
            // order the results
            if (req.sort != null && req.sort.Count > 0)
            {
                List<string> sorts = new List<string>();
                req.sort.ForEach(x =>
                {
                    sorts.Add(string.Format("{0} {1}", x.field, x.dir));
                });

                order = string.Join(",", sorts.ToArray());
            }

            return order;
        }

        public static string getWhereClause<T>(KendoRequest req, List<object> parameters)
        {
            string whereClause = null;

            if (req.filter != null && (req.filter.filters != null && req.filter.filters.Count > 0))
            {
                var filters = req.filter.filters;

                for (var i = 0; i < filters.Count; i++)
                {
                    if (i == 0)
                        whereClause += string.Format(" {0}",
                            BuildWhereClause<T>(i, req.filter.logic, filters[i],
                            parameters));
                    else
                        whereClause += string.Format(" {0} {1}",
                            ToLinqOperator(req.filter.logic),
                            BuildWhereClause<T>(i, req.filter.logic, filters[i],
                            parameters));
                }

            }

            return whereClause;
        }

        public static async Task<string> getWhereClauseAsync<T>(KendoRequest req, List<object> parameters)
        {
            string whereClause = null;

            if (req.filter != null && (req.filter.filters != null && req.filter.filters.Count > 0))
            {
                var filters = req.filter.filters;

                for (var i = 0; i < filters.Count; i++)
                {
                    if (i == 0)
                        whereClause += string.Format(" {0}",
                            BuildWhereClauseAsync<T>(i, req.filter.logic, filters[i],
                            parameters));
                    else
                        whereClause +=  string.Format(" {0} {1}",
                            await ToLinqOperatorAsync(req.filter.logic),
                            await BuildWhereClauseAsync<T>(i, req.filter.logic, filters[i],
                            parameters));
                }

            }

            return whereClause;
        }

        public static string ToLinqOperator(string @operator)
        {
            switch (@operator.ToLower())
            {
                case "eq": return " == ";
                case "neq": return " != ";
                case "gte": return " >= ";
                case "gt": return " > ";
                case "lte": return " <= ";
                case "lt": return " < ";
                case "or": return " || ";
                case "and": return " && ";
                default: return null;
            }
        }

        public static async Task<string> ToLinqOperatorAsync(string @operator)
        {
            switch (@operator.ToLower())
            {
                case "eq": return " == ";
                case "neq": return " != ";
                case "gte": return " >= ";
                case "gt": return " > ";
                case "lte": return " <= ";
                case "lt": return " < ";
                case "or": return " || ";
                case "and": return " && ";
                default: return null;
            }
        }

        public static PropertyInfo GetNestedProp<T>(String name)
        {
            PropertyInfo info = null;
            var type = (typeof(T));
            foreach (var prop in name.Split('.'))
            {
                info = type.GetProperty(prop);
                type = info.PropertyType;
            }
            return info;
        }

        public static string BuildWhereClause<T>(int index, string logic,
            KendoFilter filter, List<object> parameters)
        {
            var entityType = (typeof(T));
            var property = entityType.GetProperty(filter.Field);
            var relFieldName = filter.Field; 
            switch (filter.Operator.ToLower())
            {
                case "eq":
                case "neq":
                case "gte":
                case "gt":
                case "lte":
                case "lt":
                    if (typeof(DateTime).IsAssignableFrom(property.PropertyType))
                    {
                        parameters.Add(DateTime.Parse(filter.Value).Date);
                        return string.Format("EntityFunctions.TruncateTime({0}){1}@{2}",
                            filter.Field,
                            ToLinqOperator(filter.Operator),
                            index);
                    }
                    if (typeof(int).IsAssignableFrom(property.PropertyType))
                    {
                        parameters.Add(int.Parse(filter.Value));
                        return string.Format("{0}{1}@{2}",
                            filter.Field,
                            ToLinqOperator(filter.Operator),
                            index);
                    }
                    parameters.Add(filter.Value);
                    return string.Format("{0}{1}@{2}",
                        relFieldName,
                        ToLinqOperator(filter.Operator),
                        index);
                case "startswith":
                    parameters.Add(filter.Value);
                    return string.Format("{0}.StartsWith(" + "@{1})",
                        relFieldName,
                        index);
                case "endswith":
                    parameters.Add(filter.Value);
                    return string.Format("{0}.EndsWith(" + "@{1})",
                        relFieldName,
                        index);
                case "contains":
                    parameters.Add(filter.Value);
                    return string.Format("{0}.Contains(" + "@{1})",
                        relFieldName,
                        index);
                default:
                    throw new ArgumentException(
                        "This operator is not yet supported for this Grid",
                        filter.Operator);
            }
        }

        public static async Task<string> BuildWhereClauseAsync<T>(int index, string logic,
            KendoFilter filter, List<object> parameters)
        {
            var entityType = (typeof(T));
            var property = entityType.GetProperty(filter.Field);
            var relFieldName = filter.Field;
            switch (filter.Operator.ToLower())
            {
                case "eq":
                case "neq":
                case "gte":
                case "gt":
                case "lte":
                case "lt":
                    if (typeof(DateTime).IsAssignableFrom(property.PropertyType))
                    {
                        parameters.Add(DateTime.Parse(filter.Value).Date);
                        return string.Format("EntityFunctions.TruncateTime({0}){1}@{2}",
                            filter.Field,
                            ToLinqOperator(filter.Operator),
                            index);
                    }
                    if (typeof(int).IsAssignableFrom(property.PropertyType))
                    {
                        parameters.Add(int.Parse(filter.Value));
                        return string.Format("{0}{1}@{2}",
                            filter.Field,
                            ToLinqOperator(filter.Operator),
                            index);
                    }
                    parameters.Add(filter.Value);
                    return string.Format("{0}{1}@{2}",
                        relFieldName,
                        ToLinqOperator(filter.Operator),
                        index);
                case "startswith":
                    parameters.Add(filter.Value);
                    return string.Format("{0}.StartsWith(" + "@{1})",
                        relFieldName,
                        index);
                case "endswith":
                    parameters.Add(filter.Value);
                    return string.Format("{0}.EndsWith(" + "@{1})",
                        relFieldName,
                        index);
                case "contains":
                    parameters.Add(filter.Value);
                    return string.Format("{0}.Contains(" + "@{1})",
                        relFieldName,
                        index);
                default:
                    throw new ArgumentException(
                        "This operator is not yet supported for this Grid",
                        filter.Operator);
            }
        }


    }
}