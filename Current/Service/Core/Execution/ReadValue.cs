using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleService.Service.Core.Execution
{
    internal class ReadValue
    {
        public static dynamic Run(string arg, Type classType)
        {
            // Türün bir örneğini oluştur (instance)
            var instance = Activator.CreateInstance(classType);

            // İstenen özellik (property) veya alan (field) adını kullanarak özelliği/alanı al
            MemberInfo member = classType.GetMember(arg).FirstOrDefault();

            object value = null;

            // Üye bir özellik (property) ise
            if (member is PropertyInfo property)
            {
                // Özelliğin değerini al
                value = property.GetValue(instance);
            }
            // Üye bir alan (field) ise
            else if (member is FieldInfo field)
            {
                // Alanın değerini al
                value = field.GetValue(instance);
            }

            // Eğer özellik veya alan değeri null değilse, değeri ekrana yazdır
            if (value != null)
            {
                return value.ToString();
            }
            else
            {
                return null;
            }
        }
    }
}
