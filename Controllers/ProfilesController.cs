using System;
using Microsoft.AspNetCore.Mvc;
using Keepr.Models;
using Keepr.Services;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using CodeWorks.Auth0Provider;

namespace Keepr.Controllers
{
  [ApiController]
    [Route("api/[controller]")]
    public class ProfilesController : ControllerBase
    {
        private readonly ProfilesService _profilesService;
        private readonly KeepsService _ks;
        private readonly VaultsService _vs;

        public ProfilesController(ProfilesService service, KeepsService ks,  VaultsService vs)
        {
            _profilesService = service;
            _ks = ks;
            _vs = vs;
        }

        [HttpGet("{id}")]
        public ActionResult<Profile> Get(string id)
        {
            try
            {
                Profile profile = _profilesService.GetProfileById(id);
                return Ok(profile);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    [HttpGet("{id}/keeps")]
    public async Task<ActionResult<Account>> GetKeepsByProfile(string id)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        List<Keep> keeps = _ks.GetKeepsByProfileId(id);
        return Ok(keeps);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

     [HttpGet("{id}/vaults")]
    [Authorize]
    public async Task<ActionResult<Account>> GetVaultsByProfile(string id)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        List<Vault> vaults = _vs.GetVaultsByProfileId(id);
        return Ok(vaults);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    
    
    }
}