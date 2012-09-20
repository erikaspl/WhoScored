using System;
using System.Data;
using NHibernate;
using NHibernate.Driver;
using NHibernate.SqlTypes;
using Npgsql;
using NpgsqlTypes;

namespace WhoScored.Db.NHibernate.Extensions
{
    public class NpgsqlDriverExtended : NpgsqlDriver
    {
        protected override void InitializeParameter(IDbDataParameter dbParam, string name, SqlType sqlType)
        {
            if (sqlType is NpgsqlExtendedSqlType && dbParam is NpgsqlParameter)
            {
                this.InitializeParameter(dbParam as NpgsqlParameter, name, sqlType as NpgsqlExtendedSqlType);
            }
            else
            {
                base.InitializeParameter(dbParam, name, sqlType);
            }
        }

        protected virtual void InitializeParameter(NpgsqlParameter dbParam, string name, NpgsqlExtendedSqlType sqlType)
        {
            if (sqlType == null)
            {
                throw new QueryException(String.Format("No type assigned to parameter '{0}'", name));
            }

            dbParam.ParameterName = FormatNameForParameter(name);
            dbParam.DbType = sqlType.DbType;
            dbParam.NpgsqlDbType = sqlType.NpgDbType;

        }
    }

    public class NpgsqlExtendedSqlType : SqlType
    {

        public NpgsqlExtendedSqlType(DbType dbType, NpgsqlDbType npgDbType)
            : base(dbType)
        {
            this.npgDbType = npgDbType;
        }

        public NpgsqlExtendedSqlType(DbType dbType, NpgsqlDbType npgDbType, int length)
            : base(dbType, length)
        {
            this.npgDbType = npgDbType;
        }

        public NpgsqlExtendedSqlType(DbType dbType, NpgsqlDbType npgDbType, byte precision, byte scale)
            : base(dbType, precision, scale)
        {
            this.npgDbType = npgDbType;
        }

        private readonly NpgsqlDbType npgDbType;
        public NpgsqlDbType NpgDbType
        {
            get
            {
                return this.npgDbType;
            }
        }
    }
}
