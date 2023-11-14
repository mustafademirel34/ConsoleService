using ConsoleService.Service.Core.Data.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleService.Service.Core.Execution
{
    internal class Method
    {
        public static Result Run(Type classType, MethodInfo method, dynamic[] paramsOfMethod, dynamic[] paramsOfClass, bool runToWhichHaveParamsCtor)
        {
            Result result = new Result();

            object instance = null;
            // İlgili türden bir örnek (instance) oluşturur.

            object[] parameterst = new object[] { };
            //////////////////////
            //instance = Activator.CreateInstance(type, paramsOfClass);

            // Parametreli constructor'ı çağırmak için kullanılacak parametreler
            // Parametreli constructor'ı çağırın ve sonucu alın //parametresiz conclar içim //System.MissingMethodException: 

            //Utilities.PrintColoredLine("> " + e.Message + "\n", ConsoleColor.Red);

            if (runToWhichHaveParamsCtor) // true ise parametreli git//(Options.Getbool("ConstructorPrefence")
            {

                ConstructorInfo[] constructors = classType.GetConstructors();


                for (int i = 0; i < constructors.Length; i++)
                {
                    ParameterInfo[] uparams = constructors[i].GetParameters();

                    if (uparams.Length > 0)
                    {
                        if (paramsOfClass.Length >= uparams.Length)
                        {
                            object[] adjustedParameters = new object[uparams.Length]; // Dönüştürülen parametreleri saklamak için yeni bir dizi oluşturun

                            for (int j = 0; j < uparams.Length; j++)
                            {
                                try
                                {
                                    adjustedParameters[j] = Convert.ChangeType(paramsOfClass[j], uparams[j].ParameterType);
                                }
                                catch (Exception e)
                                {
                                    result.Success = false;
                                    result.Message = "There was a problem matching the constructor parameters";
                                    return result;
                                }

                            }

                            paramsOfClass = adjustedParameters;

                            // Constructor'ı çağırmak için bu ayarlanmış parametreleri kullanı
                            //instance = constructors[i].Invoke(adjustedParameters);
                            //----her ctor için adjustedParameters ayarlar
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = $"Constructor on type {classType} parameters mismatch";
                            return result;
                        }
                    }
                }

                instance = Activator.CreateInstance(classType, paramsOfClass);
            }
            else
            {
                try
                {
                    BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;
                    instance = Activator.CreateInstance(classType, flags, null, null, null);
                }
                catch (Exception e)
                {
                    result.Success = false;
                    result.Message = e.Message + " ? Try using the parameterized constructor path";
                    return result;
                }
            }


            ////////////
            //instance = Activator.CreateInstance(type,); //METOTLA BİRLİKTE CTOR ÇALIŞIYOR, PARAMETRE OLMADIĞI İÇİN HATA VERİYOR


            //MissingMethodException
            // Metot adını kullanarak ilgili metodu alır.

            try
            {
                // Metodun parametrelerini alır.
                var methodParameters = method.GetParameters();
                // Dönen değeri yakalar
                object returnValue = null;

                if (paramsOfMethod.Length == methodParameters.Length)
                {
                    // Uygun tiplerde yeni bir parametre dizisi oluşturur.
                    var adjustedParameters = new object[paramsOfMethod.Length];
                    for (int i = 0; i < paramsOfMethod.Length; i++)
                    {
                        // Parametrelerin türünü alır.
                        var expectedType = methodParameters[i].ParameterType;
                        // Parametreyi uygun tipe dönüştürüp yeni dizide saklar.
                        adjustedParameters[i] = Convert.ChangeType(paramsOfMethod[i], expectedType);
                    }

                    // Metodu çağırır ve uyarlanmış parametreleri kullanır.
                    if (method.IsStatic)
                    {
                        returnValue = method.Invoke(null, adjustedParameters);
                    }
                    else
                    {
                        if (instance != null)
                            returnValue = method.Invoke(instance, adjustedParameters);//coc olmadan olacak ?, BindingFlags.c
                    }

                }
                else
                {
                    result.Success = false;
                    result.Message ="Method parameters incompatibility exist: ";   

                    // MethodInfo'nun parametrelerini alın
                    ParameterInfo[] parameters = method.GetParameters();

                    foreach (var parameter in parameters)
                    {
                        result.Message += $"{parameter.Name.ToString()}({parameter.ParameterType.ToString().Replace("System.", "")}) ";
                    }

                    //Utilities.PrintColoredLine("> Method parameters incompatibility exist", ConsoleColor.Red, true);
                    return result;

                }
                // Eğer metottan bir değer döndüyse, bu değeri ekrana yazdır
                result.Success = true;
                result.Value = returnValue;
                return result;
            }
            catch (Exception e)
            {
                //Utilities.PrintColoredLine("> " + e.Message, ConsoleColor.Red);
                result.Success = false;
                result.Message = e.Message;
                return result;
            }

        }
    }
}
