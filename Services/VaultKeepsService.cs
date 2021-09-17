using System;
using Keepr.Models;
using Keepr.Repositories;

namespace Keepr.Services
{
  public class VaultKeepsService
  {
    private readonly VaultKeepsRepository _vkr;
    private readonly KeepsRepository _kr;

    private readonly VaultsRepository _vr;

    public VaultKeepsService(VaultKeepsRepository vkr, KeepsRepository kr, VaultsRepository vr)
    {
      _vkr = vkr;
      _kr = kr;
      _vr = vr;
    }

    internal VaultKeep Create(VaultKeep newVaultKeep)
    {
      Vault found = _vr.GetById(newVaultKeep.VaultId);
      if (found.CreatorId != newVaultKeep.CreatorId){
        throw new Exception("Not your vault");
      }
      _kr.UpdateKeepCount(newVaultKeep.KeepId, 1);
      // call to keeps repo and SET the keep.keeps += 1
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
      _kr.UpdateKeepCount(toDelete.KeepId, -1);
      return "Deleted";
    }
  }
}