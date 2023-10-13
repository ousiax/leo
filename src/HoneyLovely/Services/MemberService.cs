﻿using Alyio.Extensions;
using HoneyLovely.Models;
using System.Data;
using System.Data.SQLite;

namespace HoneyLovely.Services
{
    internal sealed class MemberService : IMemberService
    {
        private readonly IDbConnectionManager _dbConnectionManager;

        public MemberService(IDbConnectionManager dbConnectionManager)
        {
            _dbConnectionManager = dbConnectionManager;
        }

        public async Task<List<Member>> GetAsync()
        {
            var members = new List<Member>();
            using var conn = await _dbConnectionManager.OpenAsync().ConfigureAwait(false);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM member";
            using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
            {
                while (reader.Read())
                {
                    members.Add(new Member
                    {
                        Id = Guid.Parse(reader["id"].ToString()),
                        Name = reader["name"].ToString(),
                        Birthday = reader["birthday"].ToDateTime() ?? DateTime.Now,
                        CardNo = reader["cardno"].ToString(),
                        Gender = reader["gender"].ToString(),
                        Phone = reader["phone"].ToString()
                    });
                }
            }

            cmd.CommandText = "SELECT * FROM member_detail WHERE id = @id";
            foreach (var mem in members)
            {
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SQLiteParameter("@id") { DbType = DbType.String, Value = mem.Id });
                using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (reader.Read())
                    {
                        mem.Details.Add(new MemberDetail
                        {
                            Id = Guid.Parse(reader["id"].ToString()),
                            Date = reader["date"].ToDateTime().Value,
                            Item = reader["item"].ToString(),
                            Count = reader["count"].ToInt32() ?? 0,
                            Height = reader["height"].ToDouble() ?? 0.0d,
                            Weight = reader["weight"].ToDouble() ?? 0.0d
                        });
                    }
                }
            }

            return members;
        }

        public async Task<int> CreateAsync(Member member)
        {
            using var conn = await _dbConnectionManager.OpenAsync().ConfigureAwait(false);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO member (id, name , phone , gender , birthday , cardno) "
                + "VALUES (@id, @name , @phone , @gender , @birthday , @cardno)";
            cmd.Parameters.Add(new SQLiteParameter("@id") { DbType = DbType.String, Value = Guid.NewGuid() });
            cmd.Parameters.Add(new SQLiteParameter("@name") { DbType = DbType.String, Value = member.Name });
            cmd.Parameters.Add(new SQLiteParameter("@phone") { DbType = DbType.String, Value = member.Phone });
            cmd.Parameters.Add(new SQLiteParameter("@gender") { DbType = DbType.String, Value = member.Gender });
            cmd.Parameters.Add(new SQLiteParameter("@birthday") { DbType = DbType.String, Value = member.Birthday });
            cmd.Parameters.Add(new SQLiteParameter("@cardno") { DbType = DbType.String, Value = member.CardNo });
            return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }

        public async Task<int> UpdateAsync(Member member)
        {
            using var conn = await _dbConnectionManager.OpenAsync().ConfigureAwait(false);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE member "
                + "SET name=@name , phone=@phone , gender=@gender , birthday=@birthday , cardno=@cardno "
                + "WHERE id=@id";
            cmd.Parameters.Add(new SQLiteParameter("@id") { DbType = DbType.String, Value = member.Id });
            cmd.Parameters.Add(new SQLiteParameter("@name") { DbType = DbType.String, Value = member.Name });
            cmd.Parameters.Add(new SQLiteParameter("@phone") { DbType = DbType.String, Value = member.Phone });
            cmd.Parameters.Add(new SQLiteParameter("@gender") { DbType = DbType.String, Value = member.Gender });
            cmd.Parameters.Add(new SQLiteParameter("@birthday") { DbType = DbType.String, Value = member.Birthday });
            cmd.Parameters.Add(new SQLiteParameter("@cardno") { DbType = DbType.String, Value = member.CardNo });
            return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
    }
}
