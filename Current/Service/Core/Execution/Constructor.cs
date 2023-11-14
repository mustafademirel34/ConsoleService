using ConsoleService.Service.Core.Data.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleService.Service.Core.Execution
{
    internal class Constructor
    {
        public static Result Run(Type classType)
        {
            Result result = new Result();

            ConstructorInfo[] constructors = classType.GetConstructors();
            string returnValue = string.Empty;

            if (constructors.Count() == 0)
            {
                result.Success = false;
                result.Message = "Class constructor is not found";
            }
            else
            {
                foreach (ConstructorInfo constructor in constructors)
                {
                    ParameterInfo[] parameters = constructor.GetParameters();
                    if (parameters.Length > 0)
                    {
                        object[] parameterValues = new object[parameters.Length];
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            Console.Write($"> Please enter parameter of '{parameters[i].Name}': ");
                            string inputValue = Console.ReadLine();
                            try
                            {
                                parameterValues[i] = Convert.ChangeType(inputValue, parameters[i].ParameterType);
                            }
                            catch
                            {
                                result.Success = false;
                                result.Message = "Parametre uyumsuz!";
                                return result;
                            }
                        }
                        try
                        {
                            object instance = constructor.Invoke(parameterValues);
                        }
                        catch
                        {
                            result.Success = false;
                            result.Message = "Constructor çağrısı başarısız!";
                            return result;
                        }
                    }
                    else
                    {
                        object instance = constructor.Invoke(null);
                    }
                }
             
            }
            return result;
        }
    }
}
