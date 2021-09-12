using Keepr.Repositories;
using Keepr.Models;
using System.Collections.Generic;
using System;

namespace Keepr.Services
{
  public class KeepsService
  {
    private readonly KeepsRepository _keepRepo;

    public KeepsService(KeepsRepository keepRepo)
    {
      _keepRepo = keepRepo;
    }

//GetAll
     internal List<Keep> GetKeeps()
    {
      return _keepRepo.GetAll();
    }
//GetOneById
      internal Keep GetKeep(int id)
    {
      return _keepRepo.GetById(id);
    }
    internal Keep CreateKeep(Keep newKeep)
    {
      return _keepRepo.Create(newKeep);
    }

    internal Keep Edit(Keep updatedKeep)
    {
   Keep original = GetKeep(updatedKeep.Id);
      if (original.CreatorId != updatedKeep.CreatorId)
      {
        throw new Exception("Not your keep to change");
      }
      // NOTE these are the same kind of evaluation ('??' Null Coalesing Operator)
      //ASK MARK WHY USING THE SAME ON BOTH DOESNT WORK
      original.Name = updatedKeep.Name != null ? updatedKeep.Name : original.Name;
      original.Description = updatedKeep.Description ?? original.Description;
      original.Img = updatedKeep.Img ?? original.Img;
      _keepRepo.Edit(original);
      return original;
    }
    internal void Delete(int keepId, string userId)
    {
      Keep toDelete = GetKeep(keepId);
      if (toDelete.CreatorId != userId)
      {
        throw new Exception("This is not your keep");
      }
      _keepRepo.Delete(keepId);
    }

    // internal List<VaultKeepsExtended> GetKeepsForAccount(string id)
    // {
    //   return _keepRepo.GetByAccountId(id);
    // }
  }
}