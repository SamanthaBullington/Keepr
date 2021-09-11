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
  public class KeepsController : ControllerBase
  {
    private readonly KeepsService _ks;
    private readonly AccountService _acts;

    public KeepsController(KeepsService ks, AccountService acts)
    {
      _ks = ks;
      _acts = acts;
    }

        [HttpGet]
    public ActionResult<List<Keep>> Get()
    {
      try
      {
        List<Keep> keeps = _ks.GetKeeps();
        return Ok(keeps);
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }

        [HttpGet("{id}")]
    public ActionResult<Keep> GetOne(int id)
    {
      try
      {
        Keep keep = _ks.GetKeep(id);
        return Ok(keep);
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Keep>> Create([FromBody] Keep newKeep)
    {
      try
      {
        // what does this do?????
        // gets the bearer token from the request headers
        // asks auth0 if the bearer token is valid
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        // NEVER EVER TRUST THE CLIENT
        newKeep.CreatorId = userInfo.Id;
        Keep keep = _ks.CreateKeep(newKeep);
        return Ok(keep);

      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<Keep>> Edit([FromBody] Keep updatedKeep, int id)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        updatedKeep.CreatorId = userInfo.Id;
        updatedKeep.Id = id;
        Keep keep = _ks.Edit(updatedKeep);
        return Ok(keep);
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
        _ks.Delete(id, userInfo.Id);
        return Ok("Keep has been deleted");
      }
      catch (Exception err)
      {
        return BadRequest(err.Message);
      }
    }
  }
}