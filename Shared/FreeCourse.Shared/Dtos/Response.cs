using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FreeCourse.Shared.Dtos
{
    public class Response<T>
    {
        public T Data { get; private set; }

        [JsonIgnore]
        public HttpStatusCode StatusCode { get; private set; }

        [JsonIgnore]
        public bool IsSuccessful { get; private set; }

        public List<string> Errors { get; set; }

        // Static Factory Method
        public static Response<T> Success(T data, HttpStatusCode statusCode)
        {
            return new Response<T>() { Data = data, StatusCode = statusCode, IsSuccessful = true };
        }

        public static Response<T> Success(HttpStatusCode statusCode)
        {
            return new Response<T>() { Data = default(T), StatusCode = statusCode, IsSuccessful = true };
        }

        public static Response<T> Fail(List<string> errors, HttpStatusCode statusCode)
        {
            return new Response<T>()
            {
                Errors = errors,
                StatusCode = statusCode,
                IsSuccessful = false
            };
        }

        public static Response<T> Fail(string error, HttpStatusCode statusCode)
        {
            return new Response<T>()
            {
                Errors = new List<string>() { error },
                StatusCode = statusCode,
                IsSuccessful = false
            };
        }
    }
}