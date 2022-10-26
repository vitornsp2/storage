using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace findox.Domain.Models.Service
{
    public class ControllerResponse
    {
        public string? Status { get; set; }

        public string? Message { get; set; }

        public string? Code { get; set; }

        public object? Data { get; set; }
        public void Error()
        {
            Status = "error";
            Message = "An error occurred while processing your request.";
        }
        public void Fail(string? message)
        {
            Status = "fail";
            Message = message;
        }

        public void Success(object? data)
        {
            Status = "success";
            Data = data;
        }
    }
}