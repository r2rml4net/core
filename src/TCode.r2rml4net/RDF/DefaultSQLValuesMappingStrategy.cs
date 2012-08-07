using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using TCode.r2rml4net.TriplesGeneration;

namespace TCode.r2rml4net.RDF
{
    public class DefaultSQLValuesMappingStrategy : ISQLValuesMappingStrategy
    {
        private readonly IDictionary<Type, string> _datatypeMappings = new Dictionary<Type, string>();

        public DefaultSQLValuesMappingStrategy()
        {
            FillDefaultDatatypeMappings();
        }

        #region Implementation of ISQLValuesMappingStrategy

        public string GetLexicalForm(int columnIndex, IDataRecord logicalRow, out Uri naturalRdfDatatype)
        {
            Type type = logicalRow.GetFieldType(columnIndex);
            naturalRdfDatatype = GetXsdUriForType(type, logicalRow.GetDataTypeName(columnIndex));

            if (logicalRow.IsDBNull(columnIndex))
                return null;

            return GetMappedValue(columnIndex, logicalRow, naturalRdfDatatype);
        }

        #endregion

        protected internal virtual string GetMappedValue(int columnIndex, IDataRecord logicalRow, Uri dataType)
        {
            if (dataType != null)
            {
                string uriString = dataType.ToString();

                switch (uriString)
                {
                    case XsdDatatypes.Boolean:
                        return logicalRow.GetValue(columnIndex).ToString().ToLower();
                    case XsdDatatypes.Decimal:
                        return logicalRow.GetDecimal(columnIndex).ToString(CultureInfo.InvariantCulture);
                    case XsdDatatypes.Double:
                        var format = "0.0" + new string('#', 29) + "E-0";

                        return logicalRow.GetFieldType(columnIndex) == typeof(float) 
                            ? logicalRow.GetFloat(columnIndex).ToString(format, CultureInfo.InvariantCulture) 
                            : logicalRow.GetDouble(columnIndex).ToString(format, CultureInfo.InvariantCulture);

                    case XsdDatatypes.DateTime:
                        return GetUtcTime(columnIndex, logicalRow).ToString("u").Replace(' ', 'T').TrimEnd('Z');
                    case XsdDatatypes.Time:
                        return GetUtcTime(columnIndex, logicalRow).ToString("u").Split(' ')[1];
                    case XsdDatatypes.Date:
                        return GetUtcTime(columnIndex, logicalRow).ToString("u").Split(' ')[0];
                    case XsdDatatypes.Binary:
                        return ByteArrayToString((byte[]) logicalRow.GetValue(columnIndex));
                }
            }

            return logicalRow.GetValue(columnIndex).ToString();
        }

        protected virtual Uri GetXsdUriForType(Type type, string sqlTypeName)
        {
            if (type == typeof(DateTime) && !string.IsNullOrWhiteSpace(sqlTypeName))
            {
                var typeNamelowered = sqlTypeName.ToLower();
                if (typeNamelowered == "time")
                {
                    return new Uri(XsdDatatypes.Time);
                }
                if (typeNamelowered == "date")
                {
                    return new Uri(XsdDatatypes.Date);
                }
            }
            if (_datatypeMappings.ContainsKey(type))
            {
                return new Uri(_datatypeMappings[type]);
            }

            return null;
        }

        private void FillDefaultDatatypeMappings()
        {
            _datatypeMappings.Add(typeof(int), XsdDatatypes.Integer);
            _datatypeMappings.Add(typeof(short), XsdDatatypes.Integer);
            _datatypeMappings.Add(typeof(long), XsdDatatypes.Integer);
            _datatypeMappings.Add(typeof(byte[]), XsdDatatypes.Binary);
            _datatypeMappings.Add(typeof(decimal), XsdDatatypes.Decimal);
            _datatypeMappings.Add(typeof(bool), XsdDatatypes.Boolean);
            _datatypeMappings.Add(typeof(float), XsdDatatypes.Double);
            _datatypeMappings.Add(typeof(double), XsdDatatypes.Double);
            _datatypeMappings.Add(typeof(DateTime), XsdDatatypes.DateTime);
            _datatypeMappings.Add(typeof(TimeSpan), XsdDatatypes.DateTime);
        }

        static DateTime GetUtcTime(int columnIndex, IDataRecord logicalRow)
        {
            return TimeZoneInfo.ConvertTimeToUtc(logicalRow.GetDateTime(columnIndex), TimeZoneInfo.Utc);
        }

        static string ByteArrayToString(byte[] bytes)
        {
            char[] c = new char[bytes.Length * 2];
            byte b;
            for (int i = 0; i < bytes.Length; i++)
            {
                b = ((byte)(bytes[i] >> 4));
                c[i * 2] = (char)(b > 9 ? b + 0x37 : b + 0x30);
                b = ((byte)(bytes[i] & 0xF));
                c[i * 2 + 1] = (char)(b > 9 ? b + 0x37 : b + 0x30);
            }
            return new string(c);
        }
    }
}