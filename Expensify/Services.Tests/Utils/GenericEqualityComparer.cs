using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Services.Tests.Utils
{
    /// <summary>
    /// Generic object comparer
    /// </summary>
    /// <typeparam name="T">object type </typeparam>
    public class GenericEqualityComparer<T> : IEqualityComparer<T>
    {
        /// <summary>
        /// Gets or sets properties to compare
        /// </summary>
        private readonly string[] props;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericEqualityComparer{T}"/> class.
        /// </summary>
        public GenericEqualityComparer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericEqualityComparer{T}"/> class.
        /// </summary>
        /// <param name="props">Properties to compare </param>
        public GenericEqualityComparer(params string[] props) => this.props = props;

        /// <summary>
        /// Compares two objects
        /// </summary>
        /// <param name="x">Expected result </param>
        /// <param name="y">actual result </param>
        /// <returns>1 if true and 0 if false </returns>
        public bool Equals(T x, T y)
        {
            bool result = true;
            Type type = x.GetType();
            PropertyInfo[] propertyInfos = type.GetProperties();

            if (props != null && props.Count() > 0)
            {
                foreach (var item in props)
                {
                    var check = propertyInfos.SingleOrDefault(t => t.Name == item);
                    if (check.GetValue(x) != null && check.GetValue(y) != null && !check.GetValue(x).Equals(check.GetValue(y)))
                    {
                        result = false;
                        break;
                    }
                }
            }
            else
            {
                foreach (var item in propertyInfos)
                {
                    var check = propertyInfos.SingleOrDefault(t => t.Name == item.Name);
                    if (check.GetValue(x) != null && check.GetValue(y) != null && !check.GetValue(x).Equals(check.GetValue(y)))
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Not required for now
        /// </summary>
        /// <param name="obj">object to </param>
        /// <returns>asd</returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetHashCode([DisallowNull] T obj)
        {
            throw new NotImplementedException();
        }
    }
}
