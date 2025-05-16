using BLL.DTOs.AccountDTOs;
using BLL.Exceptions;
using BLL.Managers.AccountManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace E_Learning_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountManager _account;

        public AccountController(IAccountManager account)
        {
            _account = account;
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginDto logInDto)
        {
            if (!ModelState.IsValid)
            {
                // Returns 400 with validation error messages
                return BadRequest(new
                {
                    errors = ModelState,
                    code = 400,
                    message = "you have made a bad request!",
                    date = (string)null
                });

            }
            try
            {
                var result = await _account.Login(logInDto);
                return Ok(result);
            }
            catch(CustomException ex)
            {
                return BadRequest(new
                {
                    errors = ex.Errors,
                    code = 400,
                    message = "you have made a bad request!",
                    data = (string)null
                });
            }
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegisterDto registerInDto)
        {
            if (!ModelState.IsValid)
            {
                // Returns 400 with validation error messages
                return BadRequest(new
                {
                    errors = ModelState,
                    code = 400,
                    message = "you have made a bad request!",
                    date = (string)null
                });

            }
            try
            {
                var result = await _account.Register(registerInDto);
                if(result == null)
                {
                    return NotFound("Registeration Failed");
                }
                return Ok(new
                {
                    code = 200,
                    message = "تم تسجيل الحساب بنجاح وإرسال رمز التفعيل للبريد الإلكتروني.",
                    token = result
                });
            }
            catch(CustomException ex)
            {
                return BadRequest(new
                {
                    errors = ex.Errors,
                    code = 400,
                    message = "you have made a bad request!",
                    date = (string)null
                });
            }
        }
    }
}
