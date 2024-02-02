using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elephanel;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class SqlColumnAttribute : Attribute
{
	public string Id { get; private set; }
	public bool Default { get; private set; }
	public bool Readonly { get; private set; }

	private SqlColumnAttribute(string id, bool _default, bool _readonly)
	{
		Id = id;
		Default = _default;
		Readonly = _readonly;
	}

	public SqlColumnAttribute(string id, bool _readonly = false) : this(id, false, _readonly) { }
	public SqlColumnAttribute(bool _readonly = false) : this("", true, _readonly) { }
}
