using Keepr.Models;
using System.Collections.Generic;
using System;
using Keepr.Repositories;

namespace Keepr.Services
{
  public class VaultsService
  {
    private readonly VaultsRepository _vaultRepo;

    public VaultsService(VaultsRepository vaultRepo)
    {
      _vaultRepo = vaultRepo;
    }

//GetAll
     internal List<Vault> GetVaults()
    {
      return _vaultRepo.GetAll();
    }
//GetOneById
      internal Vault GetOneVault(int id)
    {
      return _vaultRepo.GetById(id);
    }
    internal Vault CreateVault(Vault newVault)
    {
      return _vaultRepo.Create(newVault);
    }

    internal Vault Edit(Vault updatedVault)
    {
   Vault original = GetOneVault(updatedVault.Id);
      if (original.CreatorId != updatedVault.CreatorId)
      {
        throw new Exception("Not your vault to change");
      }
      // NOTE these are the same kind of evaluation ('??' Null Coalesing Operator)
      //ASK MARK WHY USING THE SAME ON BOTH DOESNT WORK
      original.Name = updatedVault.Name != null ? updatedVault.Name : original.Name;
      original.Description = updatedVault.Description ?? original.Description;
       original.IsPrivate = updatedVault.IsPrivate ?? original.IsPrivate;
      _vaultRepo.Edit(original);
      return original;
    }
    internal void Delete(int vaultId, string userId)
    {
      Vault toDelete = GetOneVault(vaultId);
      if (toDelete.CreatorId != userId)
      {
        throw new Exception("This is not your vault");
      }
      _vaultRepo.Delete(vaultId);
    }
  }
}