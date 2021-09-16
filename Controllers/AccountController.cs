using System;
using System.Threading.Tasks;
using Keepr.Models;
using Keepr.Services;
using CodeWorks.Auth0Provider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Keepr.Controllers
{
  [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;

        private readonly KeepsService _ks;
        private readonly VaultsService _vs;

        public AccountController(AccountService accountService, VaultsService vs, KeepsService ks)
        {
            _accountService = accountService;
            _ks = ks;
            _vs = vs;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<Account>> Get()
        {
            try
            {
                Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                return Ok(_accountService.GetOrCreateProfile(userInfo));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    [HttpGet("/vaults")]
    [Authorize]
    public async Task<ActionResult<List<Vault>>> GetVaultsByAccount(string id)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        List<Vault> vaults = _vs.GetVaultsByAccountId(id);
        return Ok(vaults);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpGet("/keeps")]
    public async Task<ActionResult<List<Keep>>> GetKeepsByAccount(string id)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        List<Keep> keeps = _ks.GetKeepsByAccountId(id);
        return Ok(keeps);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    }
}