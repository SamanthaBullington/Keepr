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