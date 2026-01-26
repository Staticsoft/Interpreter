using FluentAssertions.Collections;
using FluentAssertions.Execution;

namespace Staticsoft.Interpreter.Server.Tests;

public static class CollectionExtensions
{
	public static AndConstraint<GenericCollectionAssertions<T>> BeSimilarTo<T>(
		this GenericCollectionAssertions<T> assertions,
		params object[] expectedItems
	)
	{
		var subject = assertions.Subject;

		Execute.Assertion
			.ForCondition(subject != null)
			.FailWith("Expected a collection, but found <null>.");

		var actualList = new List<T>(subject!);

		Execute.Assertion
			.ForCondition(actualList.Count == expectedItems.Length)
			.FailWith(
				"Expected {context:collection} to contain {0} items, but found {1}.",
				expectedItems.Length,
				actualList.Count
			);

		for (int i = 0; i < expectedItems.Length; i++)
		{
			actualList[i].Should().BeEquivalentTo(
				expectedItems[i],
				options => options.WithStrictOrdering(),
				$"item at index {i} does not match"
			);
		}

		return new AndConstraint<GenericCollectionAssertions<T>>(assertions);
	}
}