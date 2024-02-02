using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace Elephanel;

public class SqlTable<T>
{
	private IEnumerable<(PropertyInfo property, SqlColumnAttribute attribute)> Properties;

	private string TableName;
	private PostgresConnection Connection;

	public SqlTable(string name)
	{
		TableName = name;
		Connection = PostgresConnection.Create();

		var type = typeof(T);
		var allProps = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		var props = allProps.Select(prop => (property: prop, attribute: prop.GetCustomAttribute<SqlColumnAttribute>()))
							.Where(pair => pair.attribute is not null);

		if (props.Count() <= 0) throw new Exception();

		Properties = props;
	}

	public async Task<int> Insert(T insertedValue, bool returnIdentity = false)
	{
		await Connection.OpenAsync();
		var values = Properties.Where(prop => !prop.attribute.Readonly).Select(prop =>
			(id: prop.attribute.Default ? prop.property.Name : prop.attribute.Id,
			 value: prop.property.GetValue(insertedValue)));
		var parameters = string.Join(", ", values.Select(v => "@" + v.id));

		string sql = $"INSERT INTO {TableName} VALUES ({parameters});";
		if (returnIdentity) sql += "SELECT SCOPE_IDENTITY();";
		var cmd = Connection.CreateCommand(sql);

		foreach (var value in values)
		{
			cmd.Parameters.AddWithValue("@" + value.id, value.value ?? DBNull.Value);
		}

		return Convert.ToInt32(await cmd.ExecuteScalarAsync() ?? 0);
	}

	// Research showed that this doesn't cause performance issues, but if (for some godforsaken reason) they do arise, simply copy the SelectWhere(string, SqlParameters) and remove the WHERE clause
	public Task<List<T>> SelectAll() => SelectWhere("1=1");
	public Task<List<T>> SelectWhere(string where) => SelectWhere(where, new SqlParameters());
	public async Task<List<T>> SelectWhereOrCreate(string where, SqlParameters whereParams, T insertIfNone)
	{
		var select = await SelectWhere(where, whereParams);
		if(select.Count <= 0) { await Insert(insertIfNone); return new List<T>{ insertIfNone }; };
		return select;
	}
	public async Task<List<T>> SelectWhere(string where, SqlParameters whereParams)
	{
		await Connection.OpenAsync();
		List<T> list = new List<T>();
		var type = typeof(T);

		string sql = $"SELECT * FROM {TableName} WHERE {where}";
		var cmd = Connection.CreateCommand(sql);
		whereParams.Apply(cmd);
		using var reader = await cmd.ExecuteReaderAsync();

		while (reader.Read())
		{
			T element = (T)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(type);
			foreach (var prop in Properties)
			{
				if (prop.property.SetMethod is null) continue;

				var paramName = prop.attribute.Default ? prop.property.Name : prop.attribute.Id;
				var value = reader[paramName.ToLower()];

				if(value is DBNull) prop.property.SetValue(element, null);
				else if(
					prop.property.PropertyType.IsGenericType && 
					prop.property.PropertyType.GetGenericTypeDefinition() == typeof(List<>)) 
					{
						prop.property.SetValue(element, prop.property.PropertyType.GetConstructor([]).Invoke([]));
						prop.property.PropertyType.GetMethod("AddRange").Invoke(prop.property.GetValue(element), [value]);
					}
				else prop.property.SetValue(element, value);
			}
			list.Add(element);
		}

		return list;
	}

	public Task<int> DeleteAll() => DeleteWhere("1=1");
	public Task<int> DeleteWhere(string where) => DeleteWhere(where, new SqlParameters());
	public async Task<int> DeleteWhere(string where, SqlParameters whereParams)
	{
		await Connection.OpenAsync();
		string sql = $"DELETE FROM {TableName} WHERE {where}";
		var cmd = Connection.CreateCommand(sql);
		whereParams.Apply(cmd);

		return (int)(await cmd.ExecuteScalarAsync() ?? 0);
	}

	public Task<int> UpdateAll(string update, SqlParameters parameters) => UpdateWhere(update, "1=1", parameters);
	public async Task<int> UpdateWhere(string update, string where, SqlParameters parameters)
	{
		await Connection.OpenAsync();
		string sql = $"UPDATE {TableName} SET {update} WHERE {where}";
		var cmd = Connection.CreateCommand(sql);
		parameters.Apply(cmd);

		return (int)(await cmd.ExecuteScalarAsync() ?? 0);
	}
}
