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
  [Route("/api/[controller]")]
  [Authorize]
  public class VaultKeepsController : ControllerBase
  {
    private readonly VaultKeepsService _vks;

    public VaultKeepsController(VaultKeepsService vks)
    {
      _vks = vks;
    }

//  [HttpGet]
//     public ActionResult<List<VaultKeep>> Get()
//     {
//       try
//       {
//         List<VaultKeep> vaultkeeps = _vks.Get();
//         return Ok(vaultkeeps);
//       }
//       catch (Exception err)
//       {
//         return BadRequest(err.Message);
//       }
//     }

//     [HttpGet("{id}")]
//     public ActionResult<VaultKeep> Get(int id)
//     {
//       try
//       {
//         VaultKeep vaultkeeps = _vks.Get(id);
//         return Ok(vaultkeeps);
//       }
//       catch (Exception err)
//       {
//         return BadRequest(err.Message);
//       }
//     }



    [HttpPost]
    public async Task<ActionResult<VaultKeep>> Create([FromBody] VaultKeep newVaultKeep)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        newVaultKeep.CreatorId = userInfo.Id;
        VaultKeep vk = _vks.Create(newVaultKeep);
        return Ok(vk);
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
        _vks.Delete(id, userInfo.Id);
        return Ok("deleted");
      }
      catch (Exception err)
      {
        return BadRequest(err.Message);
      }
    }
  }
}