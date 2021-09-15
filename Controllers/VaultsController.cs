using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeWorks.Auth0Provider;
using Keepr.Models;
using Keepr.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Keepr.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class VaultsController : ControllerBase
  {
    private readonly VaultsService _vs;
    private readonly AccountService _acts;
    private readonly KeepsService _ks;

    public VaultsController(VaultsService vs, AccountService acts, KeepsService ks)
    {
      _vs = vs;
      _acts = acts;
      _ks = ks;
    }

        [HttpGet]
    public ActionResult<List<Vault>> Get()
    {
      try
      {
        List<Vault> vaults = _vs.GetVaults();
        return Ok(vaults);
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Vault>> GetOneAsync(int id)
    {
      try
      {
       Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        Vault vault = _vs.GetOneVault(id, userInfo?.Id);
        return Ok(vault);
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    
        [HttpGet("{id}/keeps")]
        public async Task<ActionResult<List<VaultKeepViewModel>>> GetKeepByVaultIdAsync(int id)
        {
            try
            {
              Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                return Ok(_ks.GetKeepByVaultId(id, userInfo?.Id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Vault>> Create([FromBody] Vault newVault)
    {
      try
      {
        // what does this do?????
        // gets the bearer token from the request headers
        // asks auth0 if the bearer token is valid
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        // NEVER EVER TRUST THE CLIENT
        newVault.CreatorId = userInfo.Id;
        newVault.Creator = userInfo;
        Vault vault = _vs.CreateVault(newVault);
        return Ok(vault);

      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<Vault>> Edit([FromBody] Vault updatedVault, int id)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        updatedVault.CreatorId = userInfo.Id;
        updatedVault.Creator = userInfo;
        updatedVault.Id = id;
        Vault vault = _vs.Edit(updatedVault);
        return Ok(vault);
      }
      catch (Exception err)
      {
        return BadRequest(err.Message);
      }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult<String>> Delete(int id)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        _vs.Delete(id, userInfo.Id);
        return Ok("Vault has been deleted");
      }
      catch (Exception err)
      {
        return BadRequest(err.Message);
      }
    }
  }
}