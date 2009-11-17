using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;

namespace ExtDirect4DotNet.helper
{
    public class FieldDescriptionHelper
    {
        /// <summary>
        /// adds the Meta-Data to this StoreLoadResponseWrapper
        /// </summary>
        /// <param name="dataTable">The DataTable to generate the metaData from</param>
        public Hashtable[] getFieldDescription(DataTable dataTable)
        {

            List<Hashtable> fields = new List<Hashtable>();
            foreach (DataColumn dc in dataTable.Columns)
            {
                Hashtable fieldDescription = generateFieldDescription(dc);

                if (fieldDescription != null)
                {
                    fields.Add(fieldDescription);
                }
                
            }
            return fields.ToArray();

        }

        /// <summary>
        /// Generates a Hashtable that represents a Fielddescription.
        /// 
        /// return null to not add this column to the metadata.
        /// </summary>
        /// <param name="dc">DataColumn to generate Fielddescription for</param>
        /// <returns>A Hashtable representation of the field or null if this should not become part of the Ext.data.Record</returns>
        protected Hashtable generateFieldDescription(DataColumn dc)
        {
            Hashtable fieldDescription = new Hashtable();
            fieldDescription.Add("name", dc.ColumnName);

            mapType(dc, ref fieldDescription);

            return fieldDescription;
        }

        protected void mapType(DataColumn dc, ref Hashtable ht)
        {
            if (dc.DataType == typeof(System.DateTime))
                ht.Add("type", "date");

        }
    }
}
