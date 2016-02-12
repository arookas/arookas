using System;
using System.Collections.Generic;

namespace arookas.Collections {
	public static class aCollection {
		public static IEnumerable<T> DistinctBy<T>(this IEnumerable<T> collection) {
			return new HashSet<T>(collection);
		}
		public static IEnumerable<TKey> DistinctBy<TSource, TKey>(this IEnumerable<TSource> collection, Func<TSource, TKey> selector) {
			var keys = new HashSet<TKey>();
			foreach (var i in collection) {
				keys.Add(selector(i));
			}
			return keys;
		}

		public static T[] Duplicate<T>(this T[] array) 	{
			aError.CheckNull(array, "array");
			return Duplicate(array, 0, array.Length);
		}
		public static T[] Duplicate<T>(this T[] array, int count) {
			return Duplicate(array, 0, count);
		}
		public static T[] Duplicate<T>(this T[] array, int offset, int count) {
			aError.CheckNull(array, "array");
			if (count == 0) {
				return new T[0];
			}
			aError.CheckArrayRegion(array, "array", offset, count);
			var copy = new T[count];
			Array.Copy(array, offset, copy, 0, count);
			return copy;
		}

		public static IEnumerator<T> GetArrayEnumerator<T>(this T[] array) {
			aError.CheckNull(array, "array");
			return ((IEnumerable<T>)array).GetEnumerator();
		}

		public static int IndexOfFirst<T>(this IEnumerable<T> collection, Predicate<T> predicate) {
			var i = 0;
			foreach (var item in collection) {
				if (predicate(item)) {
					return i;
				}
				++i;
			}
			return -1;
		}
		public static int IndexOfLast<T>(this IEnumerable<T> collection, Predicate<T> predicate) {
			var index = 0;
			var result = -1;
			foreach (var item in collection) {
				if (predicate(item)) {
					result = index;
				}
				++index;
			}
			return result;
		}
		public static int IndexOfSingle<T>(this IEnumerable<T> collection, Predicate<T> predicate) {
			var index = 0;
			var result = -1;
			foreach (var item in collection) {
				if (predicate(item)) {
#if DEBUG
					aError.Check<InvalidOperationException>(result < 0, "There was not a single element in the collection which matched the predicate.");
#endif
					result = index;
				}
				++index;
			}
			return result;
		}
		public static IEnumerable<int> IndexOfWhere<T>(this IEnumerable<T> collection, Predicate<T> predicate) {
			var index = 0;
			foreach (T item in collection) {
				if (predicate(item)) {
					yield return index;
				}
				++index;
			}
		}

		public static T[] Initialize<T>(int count, Func<T> predicate) {
			aError.CheckMin(count, "count", 0);
			aError.CheckNull(predicate, "predicate");
			var array = new T[count];
			for (var i = 0; i < count; i++) {
				array[i] = predicate();
			}
			return array;
		}
		public static T[] Initialize<T>(int count, Func<int, T> predicate) {
			aError.CheckMin(count, "count", 0);
			aError.CheckNull(predicate, "predicate");
			var array = new T[count];
			for (var i = 0; i < count; i++) {
				array[i] = predicate(i);
			}
			return array;
		}

		public static void Transform<T>(this T[] collection, Func<T, T> action) {
			for (var i = 0; i < collection.Length; ++i) {
				collection[i] = action(collection[i]);
			}
		}
		public static void Transform<T>(this T[] collection, Func<int, T, T> action) {
			for (var i = 0; i < collection.Length; i++) {
				collection[i] = action(i, collection[i]);
			}
		}
		public static void Transform<T>(this IList<T> collection, Func<T, T> action) {
			for (var i = 0; i < collection.Count; i++) {
				collection[i] = action(collection[i]);
			}
		}
		public static void Transform<T>(this IList<T> collection, Func<int, T, T> action) {
			for (var i = 0; i < collection.Count; i++) {
				collection[i] = action(i, collection[i]);
			}
		}

		public static bool Unique<TSource, TKey>(this IEnumerable<TSource> collection, Func<TSource, TKey> comparer) {
			var keys = new HashSet<TKey>();
			foreach (var item in collection) {
				if (!keys.Add(comparer(item))) {
					return false;
				}
			}
			return true;
		}
	}
}
