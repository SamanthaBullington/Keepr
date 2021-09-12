using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Keepr.Models;

namespace Keepr.Repositories
{
  public class VaultsRepository
  {
    private readonly IDbConnection _db;

    public VaultsRepository(IDbConnection db)
    {
      _db = db;
    }

     public List<Vault> GetAll()
    {
      string sql = @"
                SELECT 
                    v.*,
                    a.*
                FROM vaults v
                JOIN accounts a ON v.creatorId = a.id;
                WHERE v.isPrivate = false;";
      return _db.Query<Vault, Profile, Vault>(sql, (v, p) =>
      {
        v.Creator = p;
        return v;
      }, splitOn: "id").ToList();
    }

    public Vault GetById(int id)
    {
      string sql = @"
                SELECT 
                    v.*,
                    a.*
                FROM vaults v
                JOIN accounts a ON v.creatorId = a.id
                WHERE v.id = @id;
            ";

      return _db.Query<Vault, Profile, Vault>(sql, (v, p) =>
        {
          v.Creator = p;
          return v;
        }, new { id }, splitOn:"id").FirstOrDefault();
    }

    public Vault Create(Vault newVault)
    {
      var sql = @"
            INSERT INTO vaults(name, description, isPrivate, creatorId)
            VALUES(@Name, @Description, @IsPrivate, @CreatorId);
            SELECT LAST_INSERT_ID();
            ";
     newVault.Id = _db.ExecuteScalar<int>(sql, newVault);
      return GetById(newVault.Id);
    }

    public Vault Edit(Vault updatedVault)
    {
      string sql = @"
      UPDATE vaults
      SET
        name = @Name,
        description = @Description,
        isPrivate = @IsPrivate
      WHERE id = @Id
      ;";
      _db.Execute(sql, updatedVault);
      return updatedVault;
    }

    public void Delete(int id)
    {
      string sql = "DELETE FROM vaults WHERE id = @id LIMIT 1;";
      _db.Execute(sql, new { id });

    }
  }
}