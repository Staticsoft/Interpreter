using Staticsoft.Interpreter.Server;
using System.Globalization;

namespace Staticsoft.TestServer;

public class TestTableConverter : TableConverter<TestTable, TestView>
{
	public TestView Convert(TestTable data)
		=> new()
		{
			Id = data.Id,
			Name = $"{data.FirstName} {data.LastName}",
			Salary = data.Salary,
			HireDate = data.HireDate.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture)
		};
}