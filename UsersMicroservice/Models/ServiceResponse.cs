using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsersMicroservice.Models
{
    public class ServiceResponse
    {
        public bool IsError { get; set; }
        public ServiceResponseType ResponseType { get; set; }
        public string ErrorMessage { get; set; }
        public string CreatedId { get; set; }
    }

    public enum ServiceResponseType
    {
        NotFound,
        Conflict,
        BadRequest,
        InternalError,
        Success
    }
}
