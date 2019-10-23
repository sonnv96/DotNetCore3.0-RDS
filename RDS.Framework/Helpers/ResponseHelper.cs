using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RDS.Framework.Helpers
{
    public class SuccessJson
    {
        public object data { get; set; }

        public string message { get; set; }
    }

    public class FailJson
    {
        public IEnumerable<ErrorItem> errors { get; set; }

        public class ErrorItem
        {
            public string errorCode { get; set; }

            public string errorMessage { get; set; }
        }
    }

    public static class ResponseHelper
    {
        #region private methods

        private static ObjectResult SuccessObjectResult(int httpStatusCode, object data, string message)
        {
            data = data ?? new { };
            message = message ?? "success";
            return new ObjectResult(SuccessJson(data, message))
            {
                StatusCode = httpStatusCode
            };
        }

        private static ObjectResult FailObjectResult(int httpStatusCode, IEnumerable<(string errorCode, string errorMsg)> errors)
        {
            return new ObjectResult(FailJson(errors))
            {
                StatusCode = httpStatusCode
            };
        }

        #endregion

        #region ObjectResult methods

        public static ObjectResult Ok(object data = null, string message = null)
        {
            return SuccessObjectResult(StatusCodes.Status200OK, data, message);
        }

        public static ObjectResult Created(object data = null, string message = null)
        {
            return SuccessObjectResult(StatusCodes.Status201Created, data, message);
        }

        public static ObjectResult BadRequest(string errorMsg, string errorCode = null)
        {
            return BadRequest(new (string errorCode, string errorMsg)[] { (errorCode, errorMsg) });
        }

        public static ObjectResult BadRequest(IEnumerable<string> errorMsgs, string errorCode = null)
        {
            return BadRequest(errorMsgs.Select(x => (errorCode, x)));
        }

        public static ObjectResult BadRequest(IEnumerable<(string errorCode, string errorMsg)> errors)
        {
            errors = errors.Select(x => (x.errorCode ?? StatusCodes.Status400BadRequest.ToString(), x.errorMsg));
            return FailObjectResult(StatusCodes.Status400BadRequest, errors);
        }

        public static ObjectResult NotFound(string errorMsg = "NotFound")
        {
            var errors = new (string errorCode, string errorMsg)[] { (StatusCodes.Status404NotFound.ToString(), errorMsg) };
            return FailObjectResult(StatusCodes.Status404NotFound, errors);
        }

        public static ObjectResult InternalServerError(string errorMsg = "InternalServerError")
        {
            var errors = new (string errorCode, string errorMsg)[] { (StatusCodes.Status500InternalServerError.ToString(), errorMsg) };
            return FailObjectResult(StatusCodes.Status500InternalServerError, errors);
        }

        public static ObjectResult Unauthorized(string errorMsg = "Unauthorized")
        {
            var errors = new (string errorCode, string errorMsg)[] { (StatusCodes.Status401Unauthorized.ToString(), errorMsg) };
            return FailObjectResult(StatusCodes.Status401Unauthorized, errors);
        }

        #endregion

        #region Json methods

        public static SuccessJson SuccessJson(object data = null, string msg = null)
        {
            return new SuccessJson { data = data, message = msg };
        }

        public static FailJson FailJson(string errorCode, string errorMsg)
        {
            return FailJson(new (string errorCode, string errorMsg)[] { (errorCode, errorMsg) });
        }

        public static FailJson FailJson(IEnumerable<(string errorCode, string errorMsg)> errors)
        {
            return new FailJson { errors = errors.Select(x => new FailJson.ErrorItem { errorCode = x.errorCode, errorMessage = x.errorMsg }) };
        }

        #endregion
    }
}
