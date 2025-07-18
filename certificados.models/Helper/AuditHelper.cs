using certificados.services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace certificados.models.Helper
{
    // Generic method convert any Entity do Audit Entity 
    public class AuditHelper
    {
        /*
         * Method return a Audit Entity from dbo Entity
         */
        public static TAuditoria ConvertToAudit<T, TAuditoria>(T sourceEntity)
            where TAuditoria : new()
        {
            var auditEntity = new TAuditoria();
            var sourceProps = typeof(T).GetProperties();
            var auditProps = typeof(TAuditoria).GetProperties();

            foreach (var prop in sourceProps)
            {
                var auditProp = auditProps.FirstOrDefault(p =>
                    p.Name == prop.Name &&
                    p.PropertyType == prop.PropertyType
                );

                auditProp?.SetValue(auditEntity, prop.GetValue(sourceEntity));
            }
             
            var fhistoriaProp = auditProps.FirstOrDefault(p => p.Name == "FHISTORIA");
            fhistoriaProp?.SetValue(auditEntity, Utils.timeParsed(DateTime.Now));

            return auditEntity;
        }

    }
}
