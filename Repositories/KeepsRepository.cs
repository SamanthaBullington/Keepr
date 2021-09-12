using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Keepr.Models;

namespace Keepr.Repositories
{
  public class KeepsRepository
  {
    private readonly IDbConnection _db;

    public KeepsRepository(IDbConnection db)
    {
      _db = db;
    }

     public List<Keep> GetAll()
    {
      string sql = @"
                SELECT 
                    k.*,
                    a.*
                FROM keeps k
                JOIN accounts a ON k.creatorId = a.id;
            ";
      return _db.Query<Keep, Profile, Keep>(sql, (k, p) =>
      {
        k.Creator = p;
        return k;
      }, splitOn: "id").ToList();
    }

    public Keep GetById(int id)
    {
      string sql = @"
                SELECT 
                    k.*,
                    a.*
                FROM keeps k
                JOIN accounts a ON k.creatorId = a.id
                WHERE k.id = @id;
            ";

      return _db.Query<Keep, Profile, Keep>(sql, (k, p) =>
        {
          k.Creator = p;
          return k;
        }, new { id }).FirstOrDefault();
    }
   internal List<VaultKeepViewModel> GetAllByAccountId(string accountId)
    {
      string sql = @"
      SELECT 
        k.*,
        v.*
      FROM keeps k
      JOIN vaults v ON k.vaultId = v.id
      WHERE k.creatorId = @accountId;";
      return _db.Query<VaultKeepViewModel, Vault, VaultKeepViewModel>(sql, (keep, vault) =>
      {
        keep.VaultKeepId = vault.Id;
        return keep;
      }, new { accountId }, splitOn: "id").ToList();
    }

       internal IEnumerable<VaultKeepViewModel> GetKeepByVaultId(int id)
        {
            string sql = @"SELECT 
            k.*,
            vk.id AS VaultKeepId
            FROM vaultkeeps vk
            JOIN keeps k ON k.id = vk.keepId
            WHERE vaultId = @id;";
            return _db.Query<VaultKeepViewModel>(sql, new { id });
        }

    public Keep Create(Keep newKeep)
    {
      var sql = @"
            INSERT INTO keeps(name, description, img, creatorId)
            VALUES(@Name, @Description, @Img, @CreatorId);
            SELECT LAST_INSERT_ID();
            ";
     newKeep.Id = _db.ExecuteScalar<int>(sql, newKeep);
      return GetById(newKeep.Id);
    }

    public Keep Edit(Keep updatedKeep)
    {
      string sql = @"
      UPDATE keeps
      SET
        name = @Name,
        description = @Description,
        img = @Img,
        keeps = @Keeps,
        views = @Views,
        shares = @Shares
      WHERE id = @Id
      ;";
      _db.Execute(sql, updatedKeep);
      return updatedKeep;
    }

    public void Delete(int id)
    {
      string sql = "DELETE FROM keeps WHERE id = @id LIMIT 1;";
      _db.Execute(sql, new { id });

    }
  }
}