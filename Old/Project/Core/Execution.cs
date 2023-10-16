using ConsoleTesting.Core;
using ConsoleTesting.Test;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTesting.Project.Core
{
    public class Execution : ISway
    {
        public dynamic ReadPropertyOrField(string nameSpace, string propertyOrFieldName)
        {
            // İsim alanına (namespace) ait türü al
            var type = Type.GetType(nameSpace);

            // Türün bir örneğini oluştur (instance)
            var instance = Activator.CreateInstance(type);

            // İstenen özellik (property) veya alan (field) adını kullanarak özelliği/alanı al
            MemberInfo member = type.GetMember(propertyOrFieldName).FirstOrDefault();

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

        public void RunConstructor(Type classType)
        {
            ConstructorInfo[] constructors = classType.GetConstructors();

            if (constructors.Count() == 0)
            {
                Console.WriteLine("Constructor yok");
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
                                Console.WriteLine("\nParametre uyumsuz!");
                                break;
                            }
                        }

                        try
                        {
                            object instance = constructor.Invoke(parameterValues);
                        }
                        catch
                        {
                            Console.WriteLine("Constructor çağrısı başarısız!");
                        }
                    }
                    else
                    {
                        object instance = constructor.Invoke(null);
                    }
                }
            }
        }

        public dynamic RunMethod(string nameSpace, string methodName, dynamic[] parameters1, dynamic[] parameters2)
        {
            // Verilen namespace ve sınıf adıyla ilgili türü alır.
            Type type = Type.GetType(nameSpace);

            object instance = null;
            // İlgili türden bir örnek (instance) oluşturur.

            object[] parameterst = new object[] { };
            //////////////////////
            //instance = Activator.CreateInstance(type, parameters2);

            // Parametreli constructor'ı çağırmak için kullanılacak parametreler




            // Parametreli constructor'ı çağırın ve sonucu alın //parametresiz conclar içim //System.MissingMethodException: 

            //Utilities.PrintColoredLine("> " + e.Message + "\n", ConsoleColor.Red);


            if (Options.Getbool("ConstructorPrefence")) // true ise parametreli git
            {

                ConstructorInfo[] constructors = type.GetConstructors();


                for (int i = 0; i < constructors.Length; i++)
                {
                    ParameterInfo[] uparams = constructors[i].GetParameters();

                    if (uparams.Length > 0)
                    {
                        if (parameters2.Length >= uparams.Length)
                        {
                            object[] adjustedParameters = new object[uparams.Length]; // Dönüştürülen parametreleri saklamak için yeni bir dizi oluşturun

                            for (int j = 0; j < uparams.Length; j++)
                            {
                                try
                                {
                                    adjustedParameters[j] = Convert.ChangeType(parameters2[j], uparams[j].ParameterType);
                                }
                                catch (Exception e)
                                {

                                    return $"XThere was a problem matching the constructor parameters.";
                                }

                            }

                            parameters2 = adjustedParameters;


                            // Constructor'ı çağırmak için bu ayarlanmış parametreleri kullanın
                            //instance = constructors[i].Invoke(adjustedParameters);


                            //----her ctor için adjustedParameters ayarlar

                        }
                        else
                        {
                            return $"XConstructor on type {nameSpace} parameters mismatch.";
                        }
                    }
                }

                instance = Activator.CreateInstance(type, parameters2);
            }
            else
            {
                try
                {
                    BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;
                    instance = Activator.CreateInstance(type, flags, null, null, null);
                }
                catch (Exception e)
                {
                    return "X" + e.Message + " Try using the parameterized constructor path.";
                }
            }





            ////////////
            //instance = Activator.CreateInstance(type,); //METOTLA BİRLİKTE CTOR ÇALIŞIYOR, PARAMETRE OLMADIĞI İÇİN HATA VERİYOR


            //MissingMethodException
            // Metot adını kullanarak ilgili metodu alır.
            var method = type.GetMethod(methodName);

            try
            {
                // Metodun parametrelerini alır.
                var methodParameters = method.GetParameters();
                // Dönen değeri yakalar
                object returnValue = null;

                if (parameters1.Length == methodParameters.Length)
                {
                    // Uygun tiplerde yeni bir parametre dizisi oluşturur.
                    var adjustedParameters = new object[parameters1.Length];
                    for (int i = 0; i < parameters1.Length; i++)
                    {
                        // Parametrelerin türünü alır.
                        var expectedType = methodParameters[i].ParameterType;
                        // Parametreyi uygun tipe dönüştürüp yeni dizide saklar.
                        adjustedParameters[i] = Convert.ChangeType(parameters1[i], expectedType);
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

                    string returnText = "XMethod parameters incompatibility exist: ";

                    // MethodInfo'nun parametrelerini alın
                    ParameterInfo[] parameters = method.GetParameters();

                    foreach (var parameter in parameters)
                    {
                        returnText += $"{parameter.Name.ToString()}({parameter.ParameterType.ToString().Replace("System.", "")}) ";
                    }

                    //Utilities.PrintColoredLine("> Method parameters incompatibility exist", ConsoleColor.Red, true);
                    return returnText;

                }
                // Eğer metottan bir değer döndüyse, bu değeri ekrana yazdır
                return returnValue;
            }
            catch (Exception e)
            {
                //Utilities.PrintColoredLine("> " + e.Message, ConsoleColor.Red);
                return e.Message;
            }

        }
    }

    public interface ISway
    {
        public void RunConstructor(Type classType);
        public dynamic ReadPropertyOrField(string nameSpace, string propertyOrFieldName);
        public dynamic RunMethod(string nameSpace, string methodName, dynamic[] parameters1, dynamic[] parameters2);
    }
}
