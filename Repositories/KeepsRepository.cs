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
                UPDATE keeps
                SET views = views + 1
                WHERE id = @id;
            ";

      return _db.Query<Keep, Profile, Keep>(sql, (k, p) =>
        {
          k.Creator = p;
          return k;
        }, new { id }).FirstOrDefault();
    }

    internal List<Keep> GetKeepsByProfileId(string id)
    {
      string sql = @"
      SELECT 
      k.*,
      a.*
      FROM keeps k
      JOIN accounts a ON k.creatorId = a.id
      WHERE k.creatorId = @id;";
      return _db.Query<Keep, Profile, Keep>(sql, (keep, profile) =>
      {
        keep.Creator = profile;
        return keep;
      }, new { id }, splitOn: "id").ToList();
    }

    internal List<Keep> GetKeepsByAccountId(string id)
    {
      string sql = @"
      SELECT 
      k.*,
      a.*
      FROM keeps k
      JOIN accounts a ON k.creatorId = a.id
      WHERE k.creatorId = @id;";
      return _db.Query<Keep, Profile, Keep>(sql, (keep, profile) =>
      {
        keep.Creator = profile;
        return keep;
      }, new { id }, splitOn: "id").ToList();
    }

       internal List<VaultKeepViewModel> GetKeepByVaultId(int id)
        {
            string sql = @"SELECT
            a.*, 
            k.*,
            vk.id AS VaultKeepId
            FROM vaultkeeps vk
            JOIN keeps k ON vk.keepId = k.id
            JOIN accounts a ON k.creatorId = a.id
            WHERE vaultId = @id";
            return _db.Query<Profile, VaultKeepViewModel, VaultKeepViewModel>(sql, (p, vkvm) =>
        {
           vkvm.Creator = p;
          return vkvm;
        }, new { id },splitOn:"id").ToList();
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

    public void UpdateKeepCount(int id, int val){
      string sql = @"
       UPDATE keeps
       SET 
        keeps = keeps + @val
      WHERE id = @id
      ";
      _db.Execute(sql, new{id, val});
    }

    public void UpdateViewCount(int id){
      string sql = @"
       UPDATE keeps
       SET 
        views = views + 1
      WHERE id = @id
      ";
      _db.Execute(sql, new{id});
    }

    public void UpdateShareCount(int id){
      string sql = @"
       UPDATE keeps
       SET 
      shares = shares + 1
      WHERE id = @id
      ";
      _db.Execute(sql, new{id});
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