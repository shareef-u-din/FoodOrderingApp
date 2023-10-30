using Application.Common;
using AutoMapper;
using MediatR;
using WebAPI.Errors;
using Serilog;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using System.Text;

namespace WebAPI.Controllers.V1
{
    public class BaseController : ControllerBase
    {
        private IMediator _mediatorInstance;
        private IMapper _mapperInstance;
        private Serilog.ILogger _loggerInstance;
        protected IMediator _mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();
        protected IMapper _mapper => _mapperInstance ??= HttpContext.RequestServices.GetService<IMapper>();
        protected Serilog.ILogger _logger => _loggerInstance ??= HttpContext.RequestServices.GetService<Serilog.ILogger>();

        protected IActionResult HandleErrorResponse(List<Error> errors)
        {
            var apiError = new ErrorResponse();

            if (errors.Any(e => e.Code == ErrorCode.NotFound))
            {
                var error = errors.FirstOrDefault(e => e.Code == ErrorCode.NotFound);

                apiError.StatusCode = 404;
                apiError.StatusPhrase = "Not Found";
                apiError.Timestamp = DateTime.Now;
                apiError.Errors.Add(error.Message);
                _logger.Error(error.Message, apiError);

                return NotFound(apiError);
            }
            else if (errors.Any(e => e.Code == ErrorCode.ServerError))
            {
                var error = errors.FirstOrDefault(e => e.Code == ErrorCode.ServerError);

                apiError.StatusCode = 500;
                apiError.StatusPhrase = "Server Error";
                apiError.Timestamp = DateTime.Now;
                apiError.Errors.Add(error.Message);
                _logger.Error(error.Message, apiError);
                return StatusCode(500, apiError);
            }

            apiError.StatusCode = 400;
            apiError.StatusPhrase = "Bad request";
            apiError.Timestamp = DateTime.Now;
            errors.ForEach(e => apiError.Errors.Add(e.Message));
            _logger.Error(apiError.StatusPhrase, apiError);
            return StatusCode(400, apiError);
        }

        protected IActionResult HandleValidationErrors(List<ValidationFailure> errors)
        {
            var apiError = new ErrorResponse();
            var message = new StringBuilder();
            foreach (var error in errors)
            {
                message.Append(error.PropertyName +", "+ error.ErrorMessage);
                message.AppendLine();
            }

            apiError.StatusCode = 400;
            apiError.StatusPhrase = "Bad request";
            apiError.Timestamp = DateTime.Now;
            errors.ForEach(e => apiError.Errors.Add(e.ErrorMessage));
            _logger.Error(apiError.StatusPhrase + ", Validation Errors:" + message.ToString());
            return BadRequest(apiError);
        }
    }
}