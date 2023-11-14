﻿using Alyio.Extensions;
using Leo.Data.Domain.Entities;
using System.Data;
using System.Data.SQLite;

namespace Leo.Web.Data.Services
{
    internal sealed class MemberService : IMemberService
    {
        private readonly IDbConnectionManager _dbConnectionManager;

        public MemberService(IDbConnectionManager dbConnectionManager)
        {
            _dbConnectionManager = dbConnectionManager;
        }

        public async Task<Member?> GetAsync(Guid id)
        {
            var member = null as Member;
            using var conn = await _dbConnectionManager.OpenAsync().ConfigureAwait(false);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM member "
                + "WHERE id = @id";
            cmd.Parameters.Add(new SQLiteParameter("@id") { DbType = DbType.String, Value = id });
            using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            if (reader.Read())
            {
                member = new Member
                {
                    Id = Guid.Parse(reader["id"].ToString()),
                    Name = reader["name"].ToString(),
                    Birthday = reader["birthday"].ToDateTime(),
                    CardNo = reader["cardno"].ToString(),
                    Gender = reader["gender"].ToEnum<Gender>(),
                    Phone = reader["phone"].ToString()
                };
            }
            return member;
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
                        Gender = reader["gender"].ToEnum<Gender>(),
                        Phone = reader["phone"].ToString()
                    });
                }
            }

            return members;
        }

        public async Task<Guid> CreateAsync(Member member)
        {
            if (member.Id == Guid.Empty)
            {
                member.Id = Guid.NewGuid();
            }

            using var conn = await _dbConnectionManager.OpenAsync().ConfigureAwait(false);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO member (id, name, phone, gender, birthday, cardno,"
                + "created_at, created_by) "
                + "VALUES (@id, @name, @phone, @gender, @birthday, @cardno, "
                + "@created_at, @created_by)";
            cmd.Parameters.Add(new SQLiteParameter("@id") { DbType = DbType.String, Value = member.Id });
            cmd.Parameters.Add(new SQLiteParameter("@name") { DbType = DbType.String, Value = member.Name });
            cmd.Parameters.Add(new SQLiteParameter("@phone") { DbType = DbType.String, Value = member.Phone });
            cmd.Parameters.Add(new SQLiteParameter("@gender") { DbType = DbType.String, Value = member.Gender });
            cmd.Parameters.Add(new SQLiteParameter("@birthday") { DbType = DbType.DateTime, Value = member.Birthday });
            cmd.Parameters.Add(new SQLiteParameter("@cardno") { DbType = DbType.String, Value = member.CardNo });
            cmd.Parameters.Add(new SQLiteParameter("@created_at") { DbType = DbType.DateTime, Value = member.CreatedAt });
            cmd.Parameters.Add(new SQLiteParameter("@created_by") { DbType = DbType.String, Value = member.CreatedBy });
            await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            return member.Id;
        }

        public async Task UpdateAsync(Member member)
        {
            using var conn = await _dbConnectionManager.OpenAsync().ConfigureAwait(false);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE member "
                + "SET name=@name, phone=@phone, gender=@gender, birthday=@birthday, cardno=@cardno, "
                + "updated_at = @updated_at, updated_by = @updated_by "
                + "WHERE id=@id";
            cmd.Parameters.Add(new SQLiteParameter("@id") { DbType = DbType.String, Value = member.Id });
            cmd.Parameters.Add(new SQLiteParameter("@name") { DbType = DbType.String, Value = member.Name });
            cmd.Parameters.Add(new SQLiteParameter("@phone") { DbType = DbType.String, Value = member.Phone });
            cmd.Parameters.Add(new SQLiteParameter("@gender") { DbType = DbType.String, Value = member.Gender });
            cmd.Parameters.Add(new SQLiteParameter("@birthday") { DbType = DbType.DateTime, Value = member.Birthday });
            cmd.Parameters.Add(new SQLiteParameter("@cardno") { DbType = DbType.String, Value = member.CardNo });
            cmd.Parameters.Add(new SQLiteParameter("@updated_at") { DbType = DbType.DateTime, Value = member.CreatedAt });
            cmd.Parameters.Add(new SQLiteParameter("@updated_by") { DbType = DbType.String, Value = member.CreatedBy });
            await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
    }
}
