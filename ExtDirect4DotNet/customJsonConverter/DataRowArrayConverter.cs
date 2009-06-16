using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Data;

namespace ExtDirect4DotNet.customJsonConverter
{
    class DataRowArrayConverter : JsonConverter
    {
        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        public override void WriteJson(JsonWriter writer, object dataRow)
        {
            DataRowCollection rows = dataRow as DataRowCollection;

            

            // *** HACK: need to use root serializer to write the column value
            //     should be fixed in next ver of JSON.NET with writer.Serialize(object)
            JsonSerializer ser = new JsonSerializer();
            writer.WriteStartArray();
            for (int i = 0; i < rows.Count; i++)
            {
               

                DataRow row = (DataRow)rows[i];
                

                writer.WriteStartObject();
                foreach (DataColumn column in row.Table.Columns)
                {
                    writer.WritePropertyName(column.ColumnName);
                    ser.Serialize(writer, row[column]);
                }
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
        }

        

        /// <summary>
        /// Determines whether this instance can convert the specified value type.
        /// </summary>
        /// <param name="valueType">Type of the value.</param>
        /// <returns>
        ///     <c>true</c> if this instance can convert the specified value type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type valueType)
        {
            return typeof(DataRowCollection).IsAssignableFrom(valueType);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType)
        {
            throw new NotImplementedException();
        }
    }

    
}
