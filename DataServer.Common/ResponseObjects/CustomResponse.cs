using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServer.Common.ResponseObjects
{
    public class CustomResponse<T> where T : class
    {
        public int StatusCode { get; set; }
        public bool HasError { get; set; }
        public T Data { get; set; }
        public CustomError Error { get; set; }

        public class CustomError
        {
            public string Message { get; set; }
        }
    }

    public class CustomResponses
    {
        public static CustomResponse<T> Ok<T>(T data) where T : class
        {
            return new CustomResponse<T>()
            {
                StatusCode = 200,
                HasError = false,
                Data = data,
                Error = null
            };
        }

        //    public static CustomResponse<T> Unauthorized<T>(string message) where T : class
        //    {

        //        return new CustomResponse<T>()
        //        {
        //            StatusCode = 401,
        //            HasError = true,
        //            Data = null,
        //            Error = new CustomResponse<T>.CustomError()
        //            {
        //                Message = message
        //            }
        //        };
        //    }

        //    public static CustomResponse<T> ServerError<T>(string message) where T : class
        //    {
        //        return new CustomResponse<T>()
        //        {
        //            StatusCode = 500,
        //            HasError = true,
        //            Data = null,
        //            Error = new CustomResponse<T>.CustomError()
        //            {
        //                Message = message
        //            }
        //        };
        //    }
    }
}
