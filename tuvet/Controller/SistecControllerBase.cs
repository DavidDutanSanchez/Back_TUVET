using Microsoft.AspNetCore.Mvc;
using tu_vet_back.tuvet.Model;

namespace tu_vet_back.tuvet.Controller
{
    public class SistecControllerBase : ControllerBase
    {

        // Override Ok with object
        [NonAction]
        public override OkObjectResult Ok(object? value = null)
        {
            string message = value as string ?? "OK";
            object? result = value is not string ? value : null;
            return Ok(message, result);
        }

        // Ok with message and result
        [NonAction]
        public OkObjectResult Ok<T>(string message, T? result = default)
        {
            return base.Ok(new DefaultResponseDto<T>
            {
                success = true,
                message = message,
                result = result
            });
        }

        // Override Created with string uri and object
        [NonAction]
        public override CreatedResult Created(string uri, object? obj)
        {
            return Created(new Uri(uri, UriKind.RelativeOrAbsolute), obj);
        }

        // Override Created with Uri uri and object
        [NonAction]
        public override CreatedResult Created(Uri uri, object? obj)
        {
            string message = obj as string ?? "CREATED";

            object? result = obj is not string ? obj : default;

            return Created(uri, message, result);
        }

        // Created with Uri uri, message and result
        [NonAction]
        public CreatedResult Created<T>(Uri uri, string message, T? result)
        {
            return base.Created(uri, new DefaultResponseDto<T>
            {
                success = true,
                message = message,
                result = result
            });
        }

        // Override BadRequest with object
        [NonAction]
        public override BadRequestObjectResult BadRequest(object obj)
        {
            string message = obj as string ?? "FAILURE";
            object? result = obj is not string ? obj : null;
            return BadRequest(message, result);
        }

        // BadRequest with message and result
        [NonAction]
        public BadRequestObjectResult BadRequest<T>(string message, T? result = default)
        {
            return base.BadRequest(new DefaultResponseDto<T?>
            {
                success = false,
                message = message,
                result = result
            });
        }

        // Override NotFound with object
        [NonAction]
        public override NotFoundObjectResult NotFound(object obj)
        {
            string message = obj as string ?? "NOT FOUND";
            object? result = obj is not string ? obj : null;
            return NotFound(message, result);
        }

        // NotFound with message and result
        [NonAction]
        public NotFoundObjectResult NotFound<T>(string message, T? result = default)
        {
            return base.NotFound(new DefaultResponseDto<T?>
            {
                success = false,
                message = message,
                result = result
            });
        }

        // Override Unauthorized with object
        [NonAction]
        public override UnauthorizedObjectResult Unauthorized(object obj)
        {
            string message = obj as string ?? "UNAUTHORIZED";
            object? result = obj is not string ? obj : null;
            return Unauthorized(message, result);
        }

        // Unauthorized with message and result
        [NonAction]
        public UnauthorizedObjectResult Unauthorized<T>(string message, T? result = default)
        {
            return base.Unauthorized(new DefaultResponseDto<T?>
            {
                success = false,
                message = message,
                result = result
            });
        }

        // Forbidden with message
        [NonAction]
        public ObjectResult Forbidden(string message)
        {
            return base.StatusCode(403, new DefaultResponseDto<object>
            {
                success = false,
                message = message,
                result = null
            });
        }

        // InternalServerError with message
        [NonAction]
        public ObjectResult InternalServerError(string message)
        {
            return base.StatusCode(500, new DefaultResponseDto<object>
            {
                success = false,
                message = message,
                result = null
            });
        }
    }
}
