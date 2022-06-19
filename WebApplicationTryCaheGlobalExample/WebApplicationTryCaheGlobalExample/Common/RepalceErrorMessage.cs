using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using WebApplicationTryCaheGlobalExample.Mapper;
using WebApplicationTryCaheGlobalExample.MapperErrorMessage;
using WebApplicationTryCaheGlobalExample.Service;

namespace WebApplicationTryCaheGlobalExample.Common
{
    /// <summary>
    /// 替換全域的錯誤訊息
    /// </summary>
    public class RepalceErrorMessage : SingleTon<RepalceErrorMessage>
    {
        /// <summary>
        /// 註冊對應字典表
        /// </summary>
        private readonly Dictionary<string, Func<string, string>> _registErrorMessageMappingDictionary = new Dictionary<string, Func<string, string>>
        {
            {typeof(TestErrorFirstService).FullName + nameof(TestErrorFirstService.TestErrorFirst_MethodA), (string message) => { return ReplaceTestErrorFirstService.Instance.Value.TestErrorSecond_MethodA(message); } },
            {typeof(TestErrorFirstService).FullName + nameof(TestErrorFirstService.TestErrorFirst_MethodB), (string message) => { return ReplaceTestErrorFirstService.Instance.Value.TestErrorSecond_MethodB(message); } },
            {typeof(TestErrorSecondService).FullName + nameof(TestErrorSecondService.TestErrorSecond_MethodC), (string message) => { return ReplaceTestErrorSecondService.Instance.Value.TestErrorSecond_MethodC(message); } },
            {typeof(TestErrorSecondService).FullName + nameof(TestErrorSecondService.TestErrorSecond_MethodD), (string message) => { return ReplaceTestErrorSecondService.Instance.Value.TestErrorSecond_MethodD(message); } },
        };

        /// <summary>
        /// 全域替換錯誤訊息
        /// </summary>
        public string GlobalReplaceErrorMessage(ExceptionContext filterContext)
        {
            try
            {
                //保留未用
                var controllerName = filterContext.RouteData.Values["controller"];
                var actionName = filterContext.RouteData.Values["action"];
                
                //ExceptionContext 紀錄的Method Name
                var methodName = filterContext.Exception.TargetSite.Name;
                //呼叫SingleTon的Class全名(命名空間 + ClassName)
                var serviceFullName = filterContext.Exception.TargetSite.DeclaringType.FullName;
                
                //找出真實的Service全名位址，做為檢索條件
                var realMethodName = ParseCurrentMethodName(methodName);
                var searchName = serviceFullName + realMethodName;
                if (_registErrorMessageMappingDictionary.ContainsKey(searchName))
                {
                    var GetMessage = _registErrorMessageMappingDictionary[searchName].Invoke(filterContext.Exception.Message);
                    if (!string.IsNullOrEmpty(GetMessage))
                    {
                        return GetMessage;
                    }
                }
            }
            catch (Exception ex)
            { 
                //獨立紀錄Log事件，不影響主流程程式
            }
            return filterContext.Exception.Message;

            //Regular Expression 找出當前查詢的真實的Business MethodName
            string ParseCurrentMethodName(string input)
            {
                var result = string.Empty;
                var regexPattern = @"(<.+>)";
                var coll = Regex.Match(input, regexPattern);
                if (coll.Groups.Count > 0)
                {
                    input = coll.Groups[0].Value;
                    var regexPattern2 = "\\w+";
                    var coll2 = Regex.Match(input, regexPattern2);
                    if (coll2.Groups.Count > 0)
                    {
                        result = coll2.Groups[0].Value;
                    }
                }
                return result;
            }
        }

        public string MappingFullName<T>(string methodName)
        {
            return typeof(T).FullName + methodName;
        }
    }
}