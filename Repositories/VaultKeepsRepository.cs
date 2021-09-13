using System.Data;
using System.Linq;
using Dapper;
using Keepr.Models;

namespace Keepr.Repositories
{
  public class VaultKeepsRepository
  {
    private readonly IDbConnection _db;

    public VaultKeepsRepository(IDbConnection db)
    {
      _db = db;
    }

    //     public List<VaultKeep> GetAll()
    // {
    //   string sql = @"
    //   SELECT
    //    a.*,
    //    vk.*
    //   FROM vaultkeeps vk
    //   JOIN accounts a ON a.id = vk.creatorId
    //   ;";
    //   return _db.Query<Profile, VaultKeep, VaultKeep>(sql, (prof, vaultkeep) =>
    //   {
    //     vaultkeep.Creator = prof;
    //     return vaultkeep;
    //   }, splitOn: "id").ToList();
    // }

    // internal List<VaultKeep> GetAll(int vaultId)
    // {
    //   string sql = @"
    //   SELECT 
    //     a.*,
    //     vk.*
    //   FROM vaultkeeps vk
    //   JOIN accounts a ON a.id = vk.creatorId
    //   WHERE vk.vaultId = @vaultId;";
    //   return _db.Query<Profile, VaultKeep, VaultKeep>(sql, (prof, vaultkeep) =>
    //   {
    //     vaultkeep.Creator = prof;
    //     return vaultkeep;
    //   }, new { vaultId }, splitOn: "id").ToList();
    // }

    //   internal List<VaultKeep> GetAllByAccountId(string accountId)
    // {
    //   string sql = @"
    //   SELECT 
    //     a.*,
    //     vk.*,
    //     v.*,
    //     k.*
    //   FROM vaultkeeps vk
    //   JOIN accounts a ON a.id = vk.creatorId,
    //   JOIN vaults v ON vk.vaultId = v.id,
    //   JOIN keeps k ON vk.keepId = k.id
    //   WHERE vk.creatorId = @accountId;";
    //   return _db.Query<Profile, VaultKeep, Vault, Keep, VaultKeep>(sql, (prof, vaultkeep, keepId) =>
    //   {
    //     vaultkeep.Creator = prof;
    //     vaultkeep.KeepId = KeepId;
    //     return vaultkeep;
    //   }, new { accountId }, splitOn: "id").ToList();
    // }

    internal VaultKeep Create(VaultKeep newVaultKeep)
    {
      string sql = @"
            INSERT INTO vaultkeeps(vaultId, keepId, creatorId)
            VALUES(@VaultId, @KeepId, @CreatorId);
            SELECT LAST_INSERT_ID();
            ";
      newVaultKeep.Id = _db.ExecuteScalar<int>(sql, newVaultKeep);
      return GetById(newVaultKeep.Id);
    }

    internal VaultKeep GetById(int id)
    {
      string sql = @"
                SELECT 
                    vk.*,
                    a.*
                FROM vaultkeeps vk
                JOIN accounts a ON vk.creatorId = a.id
                WHERE vk.id = @id;
            ";

      return _db.Query<VaultKeep, Profile, VaultKeep>(sql, (vk, p) =>
        {
          vk.Creator = p;
          return vk;
        }, new { id }).FirstOrDefault();
    }

    internal string Delete(int id)
    {
      string sql = "DELETE FROM vaultkeeps WHERE id = @id LIMIT 1;";
      _db.Execute(sql, new { id });
      return "Deleted";
    }
  }
}