using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace Elephanel;

public class SqlParameters
{
	private List<(string id, object? value)> parameters;

	public SqlParameters()
	{
		parameters = new List<(string id, object? value)>();
	}

	public static SqlParameters AddInit(string id, object? value)
	{
		SqlParameters param = new SqlParameters();
		return param.Add(id, value);
	}
	public SqlParameters Add(string id, object? value)
	{
		parameters.Add((id, value is ulong ? (long)(ulong)value : value));
		return this;
	}

	public void Apply(NpgsqlCommand cmd)
	{
		foreach(var parameter in parameters)
		{
			cmd.Parameters.AddWithValue(parameter.id.StartsWith("@") ? parameter.id : "@" + parameter.id, parameter.value ?? DBNull.Value);
		}
	}
}
