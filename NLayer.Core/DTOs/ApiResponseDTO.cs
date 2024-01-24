using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NLayer.Core.DTOs
{
    public class ApiResponseDTO<T>
    {
        //Dataya null yazmaktansa yeni Datasız bir dto oluşturcam. Dönecek property nameler aynı olsun ki clientlaar yeni model oluşturmak zorunda klamayacak.
        public T Data { get; set; }

        [JsonIgnore]//jsona dönüştütürken ignore edicek cevap olarak clienta dönmeyecek ama kodun içinde kullanabilirim.
        public int StatusCode { get; set; }
        public List<String> Errors { get; set; }

        public static ApiResponseDTO<T>Success(int  statusCode, T data)
        {
            return new ApiResponseDTO<T> { Data = data, StatusCode = statusCode};
        }

        public static ApiResponseDTO<T> Success(int statusCode)
        {
            return new ApiResponseDTO<T> {StatusCode = statusCode};
        }

        public static ApiResponseDTO<T> Fail( int statusCode,List<string> errors)
        {
            return new ApiResponseDTO<T> { StatusCode = statusCode, Errors=errors };
        }

        public static ApiResponseDTO<T> Fail(int statusCode, string error)
        {
            return new ApiResponseDTO<T> { StatusCode = statusCode, Errors = new List<string> { error }};
        }

    }
}
