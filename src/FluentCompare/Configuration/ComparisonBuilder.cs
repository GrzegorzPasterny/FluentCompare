using FluentCompare.Configuration.Models;
using FluentCompare.Execution;
using FluentCompare.ResultObjects;

namespace FluentCompare.Configuration
{
	public class ComparisonBuilder
	{
		private ComparisonConfiguration _configuration = new();

		/// <summary>
		/// Builds and executes the comparison for <paramref name="t"/>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="t"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public ComparisonResult Compare<T>(params T[] t)
		{
			if (typeof(T) == typeof(object))
			{
				// TODO: Consider adding Build method to ComparisonBuilder
				// TODO: Consider creating ObjectsComparisonByReferenceEquality
				// and ObjectsComparisonByPropertyEquality classes
				return new ObjectsComparison(_configuration.ComplexTypesComparisonMode)
					.Compare(t);
			}

			throw new NotImplementedException();
		}

		public ComparisonBuilder UsePropertyEquality()
		{
			_configuration.ComplexTypesComparisonMode = ComplexTypesComparisonMode.PropertyEquality;
			return this;
		}

		public ComparisonBuilder UseReferenceEquality()
		{
			_configuration.ComplexTypesComparisonMode = ComplexTypesComparisonMode.ReferenceEquality;
			return this;
		}
	}
}
