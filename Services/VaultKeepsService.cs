using System;
using Keepr.Models;
using Keepr.Repositories;

namespace Keepr.Services
{
  public class VaultKeepsService
  {
    private readonly VaultKeepsRepository _vkr;
    private readonly KeepsRepository _kr;

    public VaultKeepsService(VaultKeepsRepository vkr, KeepsRepository kr)
    {
      _vkr = vkr;
      _kr = kr;
    }

    //     internal List<VaultKeep> Get()
    // {
    //   return _vkr.GetAll();
    // }

    // internal List<VaultKeep> GetByVaultId(int vaultId)
    // {
    //   return _vkr.GetAll(vaultId);
    // }

    // internal VaultKeep Get(int id)
    // {
    //   VaultKeep found = _vkr.GetById(id);
    //   if (found == null)
    //   {
    //     throw new Exception("Invalid Id");
    //   }
    //   return found;
    // }

    internal VaultKeep Create(VaultKeep newVaultKeep)
    {
      return _vkr.Create(newVaultKeep);
    }

    private VaultKeep GetById(int id)
    {
      VaultKeep found = _vkr.GetById(id);
      if (found == null)
      {
        throw new Exception("Invalid Id");
      }
      return found;
    }

   internal string Delete(int vaultKeepId, string userId)
    {
      VaultKeep toDelete = GetById(vaultKeepId);
      if (toDelete.CreatorId != userId)
      {
        throw new Exception("Thats not your restaurant");
      }
      _vkr.Delete(vaultKeepId);
      return "Deleted";
    }
  }
}